   M S B A M L   `   `   `  ����  TWICDBAggregator!    TWICDBAggregator.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  TWIC DB Aggregtor��5   	   $	��387��6#   $	��431��60    � 
ResizeMode$ NoResize=�6<   .��5      � -   5        $��grid1��5      $
��Auto��6   $
��Auto��6&   .+�5   
   �� -   5        $��border2��5   f   
�������$��1q�6&   $
��Auto��6:   $��Stretch=�6H   $��Stretch=�6u   $
��Auto��6�   ._�5      �� -   5        $��status��5   $   $��23��6   $
��Auto��62   $��Bottom=�6?   6\   5      �� 5	   
   -   5        $��label1��5	   _   $��
Start Date��6   $��28��6%   $
��Left=�61   $��	12,12,0,0q�6L   $	��Top=�6m   �� 5
   
   -   5        $��label2��5
   ^   $��End Date��6   $��28��6#   $
��Left=�6/   $��
218,12,0,0q�6J   $	��Top=�6l   &   System.Windows.Controls.Calendar  5   
   -   5        $��calendarStart��5   N   $	��170��6   $
��Left=�6    $��	12,36,0,0q�6;   $	��Top=�6c   $	��180��6{      SelectionMode$ 
SingleDate=�6�      DisplayDate$ 
11/16/1998��6�      DisplayDateStart$ 
11/16/1998X�6�   6    5   
   -   5        $��calendarEnd��5   O   $	��170��6   $
��Left=�6    $��
218,36,0,0q�6;   $	��Top=�6b   $	��180��6z   6�   	� 5   
   -   5        $��rbCreateNew��5   f   $��
Create New��6   $��16��6+   $
��Left=�67   $��
12,212,0,0q�6R   $	��Top=�6y    d� 	IsChecked$
 TrueY�6�    	� 	GroupName$ 	Overwrite��6�   	� 5   
   -	   5        $��rbAppend��5   b   $��Append��6   $��16��6'   $
��Left=�63   $��
12,234,0,0q�6N   $	��Top=�6r   $ 	Overwrite��6�   �� 5   
   -
   5        $��textBoxFileName��5   N   $��23��6   $
��Left=�6   $��218,212,0,0q�69   $	��Top=�6e   $	��150��6}   6�   �� 5   
   -   5        $��buttonFileChooser��5   [   $	��...��6   $��23��6   $
��Left=�6+   $��374,212,0,0q�6F   $	��Top=�6t   $��24��6�   6�   �� 5   
   -   5        $��buttonBuild��5   f   $��Build Database��6   $��31��6*   $
��Left=�66   $��116,259,0,0q�6Q   $	��Top=�6y   $	��180��6�   6�   �� 5   
   -   5        $��border1��5   w   
�������$��1q�6&   $	��100��6:   $
��Left=�6G   $��500,127,0,0q�6b   $	��Top=�6�   $	��200��6�   5      5      