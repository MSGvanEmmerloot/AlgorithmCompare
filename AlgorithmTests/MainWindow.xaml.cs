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
        
        public GraphData graphData;

        public MainWindow()
        {
            InitializeComponent();

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

            graphData = new GraphData(canGraph);

            graphData.DrawCustomGraph();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            AddAlgorithmsDataToGraph();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Running testbench forwards");
            ArrayCompare.RunTestbench(5);
            toUpdate = true;

            arrayData.Items.Refresh();
        }

        public void AddAlgorithmsDataToGraph()
        {
            List<double[]> algorithmPerformanceList = new List<double[]>();            

            // Get data of every algorithm for a single array
            for(int i=0; i< ArrayCompare.algorithmNames.Count; i++)
            {
                algorithmPerformanceList.Add(ArrayCompare.GetResultArrayDouble(i, 1));
            }

            checkBoxOne.Content = ArrayCompare.algorithmNames[0];
            checkBoxOne.Background = graphData.canvasData.brushes[0];

            checkBoxTwo.Content = ArrayCompare.algorithmNames[1];
            checkBoxTwo.Background = graphData.canvasData.brushes[1];

            checkBoxThree.Content = ArrayCompare.algorithmNames[2];
            checkBoxThree.Background = graphData.canvasData.brushes[2];

            graphData.AddAlgorithmsDataToGraph(algorithmPerformanceList);
        }
        
        //private void UpdateDataGridBackgroundColor(object sender, EventArgs e)
        //{
        //    if (toUpdate)
        //    {
        //        toUpdate = false;
        //        foreach (ArrayCompare.AlgorithmPerformance perf in arrayData.ItemsSource)
        //        {
        //            var row = arrayData.ItemContainerGenerator.ContainerFromItem(perf) as DataGridRow;

        //            if (row == null)
        //            {
        //                return;
        //            }
        //            if (ArrayCompare.GetAlgorithmIndex(perf.algorithmName) % 2 == 0)
        //            {
        //                row.Background = Brushes.Aqua;
        //            }
        //            else row.Background = Brushes.Lime;
        //        }

        //        checkBoxOne.Content = ArrayCompare.algorithmNames[0];
        //        checkBoxOne.Background = graphData.canvasData.brushes[0];

        //        checkBoxTwo.Content = ArrayCompare.algorithmNames[1];
        //        checkBoxTwo.Background = graphData.canvasData.brushes[1];

        //        checkBoxThree.Content = ArrayCompare.algorithmNames[2];
        //        checkBoxThree.Background = graphData.canvasData.brushes[2];
        //    }
        //}

        private void CheckBoxOne_Click(object sender, RoutedEventArgs e)
        {
            graphData.ToggleVisible(0, (checkBoxOne.IsChecked == true));
        }

        private void CheckBoxTwo_Click(object sender, RoutedEventArgs e)
        {
            graphData.ToggleVisible(1, (checkBoxTwo.IsChecked == true));
        }

        private void CheckBoxThree_Click(object sender, RoutedEventArgs e)
        {
            graphData.ToggleVisible(2, (checkBoxThree.IsChecked == true));
        }

        private void CheckBoxFour_Click(object sender, RoutedEventArgs e)
        {
            //graphData.ToggleVisible(2, (checkBoxFour.IsChecked == true));
        }

        private void CheckBoxFive_Click(object sender, RoutedEventArgs e)
        {
            //graphData.ToggleVisible(2, (checkBoxFive.IsChecked == true));
        }

        private void CheckBoxAutoResize_Click(object sender, RoutedEventArgs e)
        {
            graphData.ToggleAutoResize((checkBoxAutoResize.IsChecked == true));
        }

        private void CheckBoxPlotPolyline_Click(object sender, RoutedEventArgs e)
        {
            graphData.PlotPolyline((checkBoxPolyLine.IsChecked == true));
        }
    }    
}
