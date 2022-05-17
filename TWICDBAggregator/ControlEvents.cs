using System;
using System.Windows;
using System.Windows.Controls;

namespace TWICDBAggregator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Find first pgn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calendarStart_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Log("Pressed ...");
            if (calendarEnd.SelectedDate == null)
                calendarEnd.SelectedDate = calendarStart.SelectedDate;

            DateTime d1 = (DateTime)calendarStart.SelectedDate;
            DateTime d2 = (DateTime)calendarEnd.SelectedDate;
            TimeSpan ts = d2 - d1;

            if (ts.TotalDays < 0)
            {
                calendarEnd.SelectedDate = calendarStart.SelectedDate;
                calendarEnd.DisplayDate = calendarStart.DisplayDate;
            }
            d2 = (DateTime)calendarEnd.SelectedDate;
            Log("Date Range: " + d1.ToShortDateString() + " to " + d2.ToShortDateString());
            ValidateData();
        }

        /// <summary>
        /// Find last pgn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calendarEnd_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calendarStart.SelectedDate == null)
                calendarStart.SelectedDate = calendarEnd.SelectedDate;

            DateTime d1 = (DateTime)calendarStart.SelectedDate;
            DateTime d2 = (DateTime)calendarEnd.SelectedDate;
            TimeSpan ts = d2 - d1;

            if (ts.TotalDays < 0)
            {
                calendarStart.SelectedDate = calendarEnd.SelectedDate;
                calendarStart.DisplayDate = calendarEnd.DisplayDate;
            }
            d1 = (DateTime)calendarStart.SelectedDate;
            Log("Date Range: " + d1.ToShortDateString() + " to " + d2.ToShortDateString());
            ValidateData();
        }


        /// <summary>
        /// Pick a file for the big database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFileChooser_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "FullTWIC";
            dlg.DefaultExt = ".pgn";
            dlg.Filter = "PGN files (.pgn)|*.pgn";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                textBoxFileName.Text = dlg.FileName;
            }

            ValidateData();
        }

        /// <summary>
        /// Validate data if they enter the filename by hand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxFileName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ValidateData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBuild_Click(object sender, RoutedEventArgs e)
        {
            if (running == false)
            {
                running = true;
                buttonBuild.Content = "Cancel";   
                bw.RunWorkerAsync();
                textBoxFileName.IsEnabled = false;
                buttonFileChooser.IsEnabled = false;
                rbAppend.IsEnabled = false;
                rbCreateNew.IsEnabled = false;
                calendarEnd.IsEnabled = false;
                calendarStart.IsEnabled = false;
            }

            else
            {
                running = false;
                buttonBuild.Content = "Cancelling ...";
                buttonBuild.IsEnabled = false;
                bw.CancelAsync();
                wasCanceled = true;
                Log("Build cancelled.");
                // should be some file clean up code here.
            }
        }
        

          



    }

}
