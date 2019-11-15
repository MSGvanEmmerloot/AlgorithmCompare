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
        public List<CheckBox> algorithmSelectCheckBoxes = new List<CheckBox>();
        public List<string> arrayNames = new List<string>();

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

            AddAlgorithmCheckBoxesToList();

            graphData = new GraphData(canGraph);
            graphData.DrawCustomGraph();
        }

        private void AddAlgorithmCheckBoxesToList()
        {
            algorithmSelectCheckBoxes.Add(checkBoxOne);
            algorithmSelectCheckBoxes.Add(checkBoxTwo);
            algorithmSelectCheckBoxes.Add(checkBoxThree);
            algorithmSelectCheckBoxes.Add(checkBoxFour);
            algorithmSelectCheckBoxes.Add(checkBoxFive);
            algorithmSelectCheckBoxes.Add(checkBoxSix);
            algorithmSelectCheckBoxes.Add(checkBoxSeven);
            algorithmSelectCheckBoxes.Add(checkBoxEight);
            algorithmSelectCheckBoxes.Add(checkBoxNine);
        }

        private void RefreshArraySelectionList(int arrayAmount)
        {
            comboBoxArraySelect.Items.Clear();
            arrayNames.Clear();
            for (int i=0; i< arrayAmount; i++)
            {
                string arrayName = "Array " + i;
                arrayNames.Add(arrayName);
                ComboBoxItem newComboBox = new ComboBoxItem { Content = arrayName };
                newComboBox.Selected += ComboBoxArraySelectItem_Selected;
                comboBoxArraySelect.Items.Add(newComboBox);
            }
            comboBoxArraySelect.SelectedIndex = 0;
        }

        public void AddAlgorithmsDataToGraph()
        {
            List<double[]> algorithmPerformanceList = new List<double[]>();

            if (ArrayCompare.algorithmPerformances.Count < 1) { return; }
            if (ArrayCompare.algorithmPerformances[0].Count < 1) { return; }
            if (ArrayCompare.algorithmPerformances[0][0].ticksElapsed.Length < 1) { return; }

            int len = ArrayCompare.algorithmPerformances[0][0].ticksElapsed.Length;

            for (int c = 0; c< algorithmSelectCheckBoxes.Count; c++)
            {
                algorithmSelectCheckBoxes[c].Visibility = Visibility.Hidden;
            }

            // Get data of every algorithm for a single array
            for(int i=0; i< ArrayCompare.algorithmNames.Count; i++)
            {
                algorithmPerformanceList.Add(ArrayCompare.GetResultArrayDouble(i, 1));

                if (algorithmSelectCheckBoxes.Count > i)
                {
                    algorithmSelectCheckBoxes[i].Content = ArrayCompare.algorithmNames[i];
                    if (graphData.canvasData.brushes.Length > i)
                    {
                        algorithmSelectCheckBoxes[i].Background = graphData.canvasData.brushes[i];
                    }
                    algorithmSelectCheckBoxes[i].Visibility = Visibility.Visible;
                }
            }

            RefreshArraySelectionList(len);


            graphData.AddAlgorithmsDataToGraph(algorithmPerformanceList);
        }
        
        private void CheckBoxAlgorithmSelect_Click(object sender, RoutedEventArgs e)
        {
            int index = algorithmSelectCheckBoxes.FindIndex(x => x.Equals(sender));

            if(index == -1) { return; }
            
            graphData.ToggleVisible(index, ((CheckBox)sender).IsChecked == true);
        }

        //private void CheckBoxOne_Click(object sender, RoutedEventArgs e)
        //{
        //    graphData.ToggleVisible(0, (checkBoxOne.IsChecked == true));
        //}

        private void CheckBoxAutoResize_Click(object sender, RoutedEventArgs e)
        {
            graphData.ToggleAutoResize((checkBoxAutoResize.IsChecked == true));
        }

        private void CheckBoxPlotPolyline_Click(object sender, RoutedEventArgs e)
        {
            graphData.PlotPolyline((checkBoxPolyLine.IsChecked == true));
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

        private void ComboBoxArraySelectItem_Selected(object sender, RoutedEventArgs e)
        {
            string content = ((ComboBoxItem)sender).Content.ToString();
            //Console.WriteLine(content);
            if (arrayNames.Contains(content))
            {
                //Console.WriteLine(arrayNames.IndexOf(content));
                graphData.DisplayArrayData(arrayNames.IndexOf(content));
            }            
        }
    }    
}
