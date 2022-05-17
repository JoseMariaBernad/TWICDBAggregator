using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using Ionic.Zip;

namespace TWICDBAggregator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int FirstKnownPGN = 920;
        DateTime FirstKnownDate = new DateTime(2012, 6, 25);

        List<string> statuses = new List<string>();

        bool running = false; // is a build in progress?
        bool wasCanceled = false; // background worker got shutdown or not?

        BackgroundWorker bw = new System.ComponentModel.BackgroundWorker(); // this thread builds the db.

        public MainWindow()
        {
            InitializeComponent();

            //set calendars and default values

            calendarStart.DisplayDateStart = FirstKnownDate;
            calendarStart.DisplayDateEnd = DateTime.Now;
            calendarStart.DisplayDate = FirstKnownDate;
       
            calendarEnd.DisplayDateStart = FirstKnownDate;
            calendarEnd.DisplayDateEnd = DateTime.Now;
            calendarEnd.DisplayDate = DateTime.Now;
           
            status.ItemsSource = statuses;
            statuses.Add("Written by Ross Hytnen");

            bw.DoWork += BuildThread;
            bw.RunWorkerCompleted += RunWorkerCompleted;
            bw.WorkerSupportsCancellation = true;

            ValidateData();
        }

        /// <summary>
        /// Given a selected date, calculates the number of weeks since the start of the TWIC records.
        /// 1 week = 1 pgn and so a simple addition is sufficient to find us the name.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private int GetPGNFromDate(DateTime date)
        {

            TimeSpan ts = date - FirstKnownDate;
            double week = System.Math.Floor(ts.TotalDays / 7);

            return (int)(FirstKnownPGN + week);
        }
        

        /// <summary>
        /// Don't allow them to try and build a database until all necesary info is available
        /// </summary>
        /// <returns></returns>
        bool ValidateData()
        {
            bool ret = true;

            if (textBoxFileName.GetLineLength(0) < 1)
                ret = false;
            else if (calendarEnd.SelectedDate == null)
                ret = false;
            else if (calendarStart.SelectedDate == null)
                ret = false;

            if (ret == false)
                buttonBuild.IsEnabled = false;
            else
                buttonBuild.IsEnabled = true;

            return ret;
        }

        void Log(string log)
        {
            statuses[0] = log;
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                status.Items.Refresh();    
            }));
            
        }

        /// <summary>
        /// Set buttons active, clean up remaining files, etc ...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                buttonBuild.Content = "Build Database";
                running = false;
                if (wasCanceled == true)
                    Log("Build Canceled ...");
                else
                    Log("Build Complete ...");

                wasCanceled = false;
                PurgeAppData();
                buttonBuild.IsEnabled = true;
                textBoxFileName.IsEnabled = true;
                buttonFileChooser.IsEnabled = true;
                rbAppend.IsEnabled = true;
                rbCreateNew.IsEnabled = true;
                calendarEnd.IsEnabled = true;
                calendarStart.IsEnabled = true;

            }));
        }


        /// <summary>
        /// Cleans out any left over files.
        /// </summary>
        void PurgeAppData()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path += "\\TWICDBAggregator\\";

            string [] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// This is the function that does the downloading, unziping and merging of the database.
        /// </summary>
        void BuildThread(object sender, DoWorkEventArgs e)
        {
            Log("Starting database build.");

            int first = 0;
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                first = GetPGNFromDate((DateTime)calendarStart.SelectedDate);
            }));

            int second = 0;
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                second = GetPGNFromDate((DateTime)calendarEnd.SelectedDate);
            }));

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebClient webclient = new WebClient();
            webclient.Headers.Add("User-Agent: Other");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path += "\\TWICDBAggregator\\";

            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                Log("Unable to create application data path:" + ex.Message);
                return;
            }

            FileMode fm = FileMode.Create;

            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                if (rbCreateNew.IsChecked == true)
                    fm = FileMode.Create;
                else
                    fm = FileMode.Append;
            }));

            FileStream output = null;
            try
            {
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    output = new System.IO.FileStream(textBoxFileName.Text, fm);
                }));
            }
            catch (Exception ex)
            {
                Log("Can't open database for writing: " + ex.Message);
            }
            for (int i = first; i <= second; i++)
            {
                if (!running)
                {
                    output.Close();
                    return;
                }
              
                string pgnFilename = "twic" + i.ToString() + ".pgn";
                string zipFilename = "twic" + i.ToString() + "g.zip";
                string url = "https://www.theweekinchess.com/zips/" + zipFilename;

                Log("Downloading " + url);
                try
                {
                    webclient.DownloadFile(url, path + zipFilename);
                }
                catch (Exception ex)
                {
                    Log("Failed to download " + url + ": " + ex.Message);
                    continue;
                }

                try
                {
                    Log("Unzipping " + zipFilename);
                    ZipFile zip = ZipFile.Read(path + zipFilename);
                    System.IO.FileStream stream = new System.IO.FileStream(path + pgnFilename, System.IO.FileMode.CreateNew);
                    zip[0].Extract(stream);
                    stream.Close();
                    zip.Dispose();
                }
                catch (Exception ex)
                {
                    Log("Failed to extract " + zipFilename + ":" + ex.Message);
                    continue;
                }

                try
                {
                    Log("Merging " + pgnFilename);
                    byte[] bytes = File.ReadAllBytes(path + pgnFilename);
                    output.Write(bytes, 0, bytes.Length);
                }
                catch (Exception ex)
                {
                    Log("Failed to write to database:" + ex.Message);
                    continue;
                }

                try
                {
                    File.Delete(path + pgnFilename);
                    File.Delete(path + zipFilename);
                }
                catch
                {
                    continue;
                }
            }
            output.Close();
            Log("Database build complete.");


        }

        private void calendarStart_Loaded(object sender, RoutedEventArgs e)
        {
            calendarStart.SelectedDate = FirstKnownDate;
        }

        private void calendarEnd_Loaded(object sender, RoutedEventArgs e)
        {
            calendarEnd.SelectedDate = DateTime.Now;
        }

    }
}
