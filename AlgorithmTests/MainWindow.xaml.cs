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
        private List<int> arrayElementAmounts = new List<int>();
        private List<int> measurementAmounts = new List<int>();

        private int selectedArray = 0;
        private int arraySizeCurrent = 20;
        private int arraySizeNew = 20;
        private int measurements = 10;

        public MainWindow()
        {
            InitializeComponent();

            ArrayCompare.Init();
            arrayData.ItemsSource = ArrayCompare.algorithmPerformancesAverage;
            for (int i = 0; i < ArrayCompare.numberOfArrays; i++)
            {
                arrayData.Columns.Add(new DataGridTextColumn
                {
                    Header="Array " + i,
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
            for (int c = 0; c < algorithmSelectCheckBoxes.Count; c++)
            {
                algorithmSelectCheckBoxes[c].Visibility = Visibility.Hidden;
            }

            graphData = new GraphData(canGraph);

            ArrayCompare.UpdateArraySize(arraySizeNew);
            RefreshArraySizeSelectionList();
            RefreshMeasurementAmountSelectionList();
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
            comboBoxArraySelect.SelectedIndex = selectedArray;
        }

        private void RefreshArraySizeSelectionList()
        {
            comboBoxArraySizeSelect.Items.Clear();
            arrayElementAmounts.Clear();

            AddArraySelectItem(20);
            AddArraySelectItem(50);
            AddArraySelectItem(100);
            AddArraySelectItem(200);

            comboBoxArraySizeSelect.SelectedIndex = 0;
        }

        private void AddArraySelectItem(int elementSize)
        {
            arrayElementAmounts.Add(elementSize);
            ComboBoxItem newComboBox = new ComboBoxItem { Content = elementSize };
            newComboBox.Selected += ComboBoxArraySizeSelectItem_Selected;
            comboBoxArraySizeSelect.Items.Add(newComboBox);
        }

        private void RefreshMeasurementAmountSelectionList()
        {
            comboBoxMeasurementAmountSelect.Items.Clear();
            measurementAmounts.Clear();

            AddMeasurementSelectItem(1);
            AddMeasurementSelectItem(5);
            AddMeasurementSelectItem(10);
            AddMeasurementSelectItem(25);
            AddMeasurementSelectItem(100);

            measurements = measurementAmounts[2];
            comboBoxMeasurementAmountSelect.SelectedIndex = 2;
        }

        private void AddMeasurementSelectItem(int runs)
        {
            measurementAmounts.Add(runs);
            ComboBoxItem newComboBox = new ComboBoxItem { Content = runs };
            newComboBox.Selected += ComboBoxMeasurementAmountSelectItem_Selected;
            comboBoxMeasurementAmountSelect.Items.Add(newComboBox);
        }

        public void RunTests()
        {
            ArrayCompare.RunTestbench(measurements);
            toUpdate = true;

            arrayData.Items.Refresh();
            AddAlgorithmsDataToGraph();
        }

        public void AddAlgorithmsDataToGraph()
        {
            if (ArrayCompare.algorithmPerformances.Count < 1) { return; }
            if (ArrayCompare.algorithmPerformances[0].Count < 1) { return; }
            if (ArrayCompare.algorithmPerformances[0][0].ticksElapsed.Length < 1) { return; }

            int len = ArrayCompare.algorithmPerformances[0][0].ticksElapsed.Length;

            for (int c = 0; c< algorithmSelectCheckBoxes.Count; c++)
            {
                algorithmSelectCheckBoxes[c].Visibility = Visibility.Hidden;
            }

            graphData.InitArrayDatasets(len);
            RefreshArraySelectionList(len);

            for (int a = 0; a < len; a++)
            {
                if (a == selectedArray) { continue; }
                AddSingleArrayDataToGraph(a);
            }
            AddSingleArrayDataToGraph(selectedArray);

            measurementAmountLabel.Content = "Average values based on " + ArrayCompare.algorithmPerformances.Count + 
                                             " measurements,\narray size = " + ArrayCompare.arraySize;
        }

        public void AddSingleArrayDataToGraph(int arrayIndex)
        {
            List<double[]> algorithmPerformanceList = new List<double[]>();

            // Get data of every algorithm for a single array
            for (int i = 0; i < ArrayCompare.algorithmNames.Count; i++)
            {
                algorithmPerformanceList.Add(ArrayCompare.GetResultArrayDouble(i, arrayIndex));

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

            graphData.AddAlgorithmsDataToGraph(arrayIndex, algorithmPerformanceList);
        }


        private void CheckBoxAlgorithmSelect_Click(object sender, RoutedEventArgs e)
        {
            int index = algorithmSelectCheckBoxes.FindIndex(x => x.Equals(sender));

            if(index == -1) { return; }
            
            graphData.ToggleVisible(selectedArray, index, ((CheckBox)sender).IsChecked == true);
        }
        
        private void CheckBoxAutoResize_Click(object sender, RoutedEventArgs e)
        {
            graphData.ToggleAutoResize(selectedArray, (checkBoxAutoResize.IsChecked == true));
        }

        private void CheckBoxPlotDatapoints_Click(object sender, RoutedEventArgs e)
        {
            graphData.PlotDatapoints(selectedArray, (checkBoxDatapoints.IsChecked == true));
        }

        private void CheckBoxPlotPolyline_Click(object sender, RoutedEventArgs e)
        {
            graphData.PlotPolyline(selectedArray, (checkBoxPolyLine.IsChecked == true));
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            AddAlgorithmsDataToGraph();
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            RunTests();                      
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            if(arraySizeNew != arraySizeCurrent)
            {
                ArrayCompare.UpdateArraySize(arraySizeNew);
                arraySizeCurrent = arraySizeNew;
            }
            ArrayCompare.ClearAlgorithmPerformances();
            graphData.ResetGraph();
            arrayData.Items.Refresh();
            //measurementAmountLabel.Content = "Average values based on " + ArrayCompare.algorithmPerformances.Count + " measurements";
            measurementAmountLabel.Content = "Average values based on " + ArrayCompare.algorithmPerformances.Count +
                                             " measurements,\narray size = " + ArrayCompare.arraySize;
        }

        private void ComboBoxArraySelectItem_Selected(object sender, RoutedEventArgs e)
        {
            string content = ((ComboBoxItem)sender).Content.ToString();

            if (arrayNames.Contains(content))
            {
                selectedArray = arrayNames.IndexOf(content);
                graphData.DisplayArrayData(selectedArray);
            }            
        }

        private void ComboBoxArraySizeSelectItem_Selected(object sender, RoutedEventArgs e)
        {
            string content = ((ComboBoxItem)sender).Content.ToString();
            int contentInt = int.Parse(content);

            if (arrayElementAmounts.Contains(contentInt))
            {
                int index= arrayElementAmounts.IndexOf(contentInt);
                //Console.WriteLine("index=" + index + ",size=" + arrayElementAmounts.Count + ", arrayElementAmounts[" + index + "]=" + arrayElementAmounts[index]);
                if (arrayElementAmounts.Count > index)
                {
                    arraySizeNew = arrayElementAmounts[index];
                }
            }      
        }

        private void ComboBoxMeasurementAmountSelectItem_Selected(object sender, RoutedEventArgs e)
        {
            string content = ((ComboBoxItem)sender).Content.ToString();
            int contentInt = int.Parse(content);

            if (measurementAmounts.Contains(contentInt))
            {
                int index = measurementAmounts.IndexOf(contentInt);
                Console.WriteLine("index=" + index + ",size=" + measurementAmounts.Count + ", measurementAmounts[" + index + "]=" + measurementAmounts[index]);
                if (measurementAmounts.Count > index)
                {
                    measurements = measurementAmounts[index];
                }
            }
        }
    }    
}
