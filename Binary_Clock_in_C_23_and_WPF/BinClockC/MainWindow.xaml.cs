using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BinClockC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Sets all rectangles opacity to 0.35
            foreach (var r in LogicalTreeHelper.GetChildren(MainGrid))
            {
                if (r is Rectangle) (r as Rectangle).Fill.Opacity = 0.35; 
            }

            // Start a continuous thread
            Task.Factory.StartNew(() =>
            {
                try
                {
                    // while the thread is running...
                    while (true)
                    {
                        // ...get the current system time
                        DateTime _now = System.DateTime.Now;

                        // Convert each part of the system time (i.e.: hour, minutes, seconds) to binary, filling with 0s up to a length of 6 char each
                        String _binHour = Convert.ToString(_now.Hour, 2).PadLeft(6, '0');
                        String _binMinute = Convert.ToString(_now.Minute, 2).PadLeft(6, '0');
                        String _binSeconds = Convert.ToString(_now.Second, 2).PadLeft(6, '0');

                        // For each digit of the binary hour representation
                        for (int i = 0; i <= _binHour.Length - 1; i++)
                        {

                            // Dispatcher invoke to refresh the UI, which belongs to the main thread
                            H0.Dispatcher.Invoke(() =>
                            {
                            // Update the contents of the labels which use decimal h/m/s representation
                            lbHour.Content = _now.Hour.ToString("00");
                                lbMinute.Content = _now.Minute.ToString("00");
                                lbSeconds.Content = _now.Second.ToString("00");

                            // Search for a rectangle which name corresponds to the _binHour current char index.
                            // Then, set its opacity to 1 if the current _binHour digit is 1, or to 0.35 otherwise
                            (MainGrid.FindName("H" + i.ToString()) as Rectangle).Fill.Opacity = _binHour.Substring(i, 1).CompareTo("1") == 0 ? 1 : 0.35;
                                (MainGrid.FindName("M" + i.ToString()) as Rectangle).Fill.Opacity = _binMinute.Substring(i, 1).CompareTo("1") == 0 ? 1 : 0.35;
                                (MainGrid.FindName("S" + i.ToString()) as Rectangle).Fill.Opacity = _binSeconds.Substring(i, 1).CompareTo("1") == 0 ? 1 : 0.35;
                            });

                        }

                    }
                }
                catch (TaskCanceledException)
                {

                    //throw;
                }
                    
            });

        }
    }
}
