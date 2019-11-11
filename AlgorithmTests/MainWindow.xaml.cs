using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlgorithmTests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool toUpdate = true;

        public MainWindow()
        {
            InitializeComponent();
            ComponentDispatcher.ThreadIdle += new EventHandler(UpdateDataGridBackgroundColor);

            ArrayCompare.Init();
            arrayData.ItemsSource = ArrayCompare.algorithmPerformancesAverage;
            for (int i = 0; i < ArrayCompare.numberOfArrays; i++)
            {
                arrayData.Columns.Add(new DataGridTextColumn
                {
                    Binding = new Binding(string.Format("ticksElapsed[{0}]", i)),
                    MinWidth = 50
                });
            }
            ICollectionView cvAlgorithmPerformances = CollectionViewSource.GetDefaultView(arrayData.ItemsSource);
            if (cvAlgorithmPerformances != null && cvAlgorithmPerformances.CanGroup == true)
            {
                cvAlgorithmPerformances.GroupDescriptions.Clear();
                cvAlgorithmPerformances.GroupDescriptions.Add(new PropertyGroupDescription("algorithmName"));
            }
            //UpdateDataGridBackgroundColor();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            ArrayCompare.PrintArrays(null);

            Console.WriteLine("Running testbench forwards");
            ArrayCompare.RunTestbench(2);
            toUpdate = true;
            //arrayData.Items.Refresh();

            //Console.WriteLine("Running testbench backwards");
            //ArrayCompare.RunTestbenchReverse();

            //Console.WriteLine("Running testbench forwards again");
            //ArrayCompare.RunTestbench();

            arrayData.Items.Refresh();
            //UpdateDataGridBackgroundColor();
        }

        //private void UpdateDataGridBackgroundColor()
        //{
        //    foreach (ArrayCompare.AlgorithmPerformance perf in arrayData.ItemsSource)
        //    {
        //        Console.WriteLine(perf.algorithmName);
        //        var row = arrayData.ItemContainerGenerator.ContainerFromItem(perf) as DataGridRow;

        //        if (row == null)
        //        {
        //            return;
        //        }
        //        if (ArrayCompare.GetAlgorithmIndex(perf.algorithmName) % 2 == 0)
        //        {
        //            row.Background = Brushes.Aqua;
        //        }
        //        else row.Background = Brushes.Lime;
        //    }
        //}

        private void UpdateDataGridBackgroundColor(object sender, EventArgs e)
        {
            if (toUpdate)
            {
                toUpdate = false;
                foreach (ArrayCompare.AlgorithmPerformance perf in arrayData.ItemsSource)
                {
                    var row = arrayData.ItemContainerGenerator.ContainerFromItem(perf) as DataGridRow;

                    if (row == null)
                    {
                        return;
                    }
                    if (ArrayCompare.GetAlgorithmIndex(perf.algorithmName) % 2 == 0)
                    {
                        row.Background = Brushes.Aqua;
                    }
                    else row.Background = Brushes.Lime;
                }
            }
        }
    }    
}
