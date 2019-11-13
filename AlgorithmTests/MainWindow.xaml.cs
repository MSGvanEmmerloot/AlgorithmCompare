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
            //DrawDemoGraph();
            //DrawCustomGraph();

            graphData = new GraphData(canGraph);

            graphData.checkBoxOneChecked = (checkBoxOne.IsChecked == true);
            graphData.checkBoxTwoChecked = (checkBoxTwo.IsChecked == true);
            graphData.checkBoxThreeChecked = (checkBoxThree.IsChecked == true);
            graphData.checkBoxAutoResizeChecked = (checkBoxAutoResize.IsChecked == true);

            graphData.DrawCustomGraph();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            AddAlgorithmsDataToGraph();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            //ArrayCompare.PrintArrays(null);

            Console.WriteLine("Running testbench forwards");
            ArrayCompare.RunTestbench(2);
            toUpdate = true;
            //Console.WriteLine("Running testbench backwards");
            //ArrayCompare.RunTestbenchReverse();

            arrayData.Items.Refresh();
            //UpdateDataGridBackgroundColor();
        }

        public void AddAlgorithmsDataToGraph()
        {
            List<double[]> algorithmPerformanceList = new List<double[]>();            

            // Get data of every algorithm for a single array
            for(int i=0; i< ArrayCompare.algorithmNames.Count; i++)
            {
                algorithmPerformanceList.Add(ArrayCompare.GetResultArrayDouble(i, 1));
            }

            graphData.AddAlgorithmsDataToGraph(algorithmPerformanceList);
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

        private void CheckBoxOne_Click(object sender, RoutedEventArgs e)
        {
            graphData.checkBoxOneChecked = (checkBoxOne.IsChecked == true);
            graphData.HandleCheckBox(0, (checkBoxOne.IsChecked == true));
        }

        private void CheckBoxTwo_Click(object sender, RoutedEventArgs e)
        {
            graphData.checkBoxTwoChecked = (checkBoxTwo.IsChecked == true);
            graphData.HandleCheckBox(1, (checkBoxTwo.IsChecked == true));
        }

        private void CheckBoxThree_Click(object sender, RoutedEventArgs e)
        {
            graphData.checkBoxThreeChecked = (checkBoxThree.IsChecked == true);
            graphData.HandleCheckBox(2, (checkBoxThree.IsChecked == true));
        }

        //private void HandleCheckBox(int checkBoxNumber, bool visible)
        //{
        //    string graphName = elementName_Graph + checkBoxNumber;
        //    string averageName = elementName_Average + checkBoxNumber;

        //    if (visible)
        //    {
        //        if (canvasElementNames.Keys.Contains(graphName))
        //        {
        //            canGraph.Children[canvasElementNames[graphName]].Visibility = Visibility.Visible;
        //        }
        //        if (canvasElementNames.Keys.Contains(averageName))
        //        {
        //            canGraph.Children[canvasElementNames[averageName]].Visibility = Visibility.Visible;
        //        }                
        //    }
        //    else
        //    {
        //        if (canvasElementNames.Keys.Contains(graphName))
        //        {
        //            canGraph.Children[canvasElementNames[graphName]].Visibility = Visibility.Hidden;
        //        }
        //        if (canvasElementNames.Keys.Contains(averageName))
        //        {
        //            canGraph.Children[canvasElementNames[averageName]].Visibility = Visibility.Hidden;
        //        }
        //    }

        //    if (checkBoxAutoResize.IsChecked == true)
        //    {
        //        RescaleCanvas();
        //    }
        //}

        private void CheckBoxAutoResize_Click(object sender, RoutedEventArgs e)
        {
            graphData.checkBoxAutoResizeChecked = (checkBoxAutoResize.IsChecked == true);
            if (checkBoxAutoResize.IsChecked == true)
            {
                graphData.RescaleCanvas();
            }            
        }
    }    
}
