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

        //int currentElementSelectedIndex = 0;
        List<string> canvasElementList = new List<string>();
        Dictionary<string, int> canvasElementNames = new Dictionary<string, int>();

        //public class GraphDataContainer
        //{
        //    Canvas graphCanvas;

        //    public int maxValueX { get; private set; } = 5;
        //    public double maxValueY { get; private set; } = 10.0;

        //    public double graphWidth { get; private set; } = 0.0;
        //    public double graphHeight { get; private set; } = 0.0;

        //    public double margin { get; private set; } = 10;
        //    public double xmin { get; private set; } = 0.0;
        //    public double xmax { get; private set; } = 0.0;
        //    public double ymin { get; private set; } = 0.0;
        //    public double ymax { get; private set; } = 0.0;

        //    public double xStep { get; private set; } = 0.0;
        //    public double yStep { get; private set; } = 0.0;
        //    public double yScale { get; private set; } = 0.0;

        //    public List<double[]> dataSets { get; private set; } = new List<double[]>();

        //    public Brush[] brushes { get; private set; } = { Brushes.Red, Brushes.Green, Brushes.Blue };
        //    public Brush[] brushesAverage { get; private set; } = { Brushes.Crimson, Brushes.DarkOliveGreen, Brushes.Navy };

        //    public GraphDataContainer(Canvas _canvas, double _graphWidth, double _graphHeight, double _margin, int _amountOfMeasurements, double _maxYValue)
        //    {
        //        graphCanvas = _canvas;
        //        graphWidth = _graphWidth;
        //        graphHeight = _graphHeight;
        //        margin = _margin;
        //        //maxValueX = _amountOfMeasurements;
        //        maxValueY = _maxYValue;

        //        //Recalculate();
        //    }

        //    public void Recalculate()
        //    {
        //        if(dataSets.Count < 1)
        //        {
        //            Console.WriteLine("No datasets added!");
        //            return;
        //        }

        //        maxValueX = dataSets[0].Length;

        //        xmax = graphWidth - margin;
        //        ymax = graphHeight - margin;

        //        xmin = margin;
        //        ymin = margin;

        //        xStep = xmax / (maxValueX - 1);
        //        yStep = ymax / 5;
        //        yScale = ymax / maxValueY;
        //    }

        //    public void SetMargin(double _margin)
        //    {
        //        margin = _margin;
        //    }

        //    public void SetMaxX(int _x)
        //    {
        //        maxValueX = _x;
        //    }

        //    public void SetMaxY(double _y)
        //    {
        //        maxValueY = _y;
        //    }

        //    public void AddDataSet(double[] set)
        //    {
        //        dataSets.Add(set);
        //    }

        //    public void ResetDataSets()
        //    {
        //        dataSets.Clear();
        //    }

        //    public void PrintDataSetValues()
        //    {
        //        for(int d=0; d< dataSets.Count; d++)
        //        {
        //            for(int i=0; i< dataSets[d].Length; i++)
        //            {
        //                Console.WriteLine("dataSets["+d+"]["+i+"] = " + dataSets[d][i]);
        //            }
        //        }
        //    }
        //}

        //public GraphDataContainer graphData;
        public GraphData graphData;

        const string elementName_Xaxis = "X-Axis";
        const string elementName_Yaxis = "Y-Axis";
        const string elementName_Graph = "Graph ";
        const string elementName_Average = "Average ";

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
            DrawCustomGraph();
        }

        private void DrawDemoGraph()
        {
            const double margin = 10;
            double xmin = margin;
            double xmax = canGraph.Width - margin;
            double ymin = margin;
            double ymax = canGraph.Height - margin;
            const double step = 10;

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new Point(0, ymax), new Point(canGraph.Width, ymax)));
            for (double x = xmin + step;
                x <= canGraph.Width - step; x += step)
            {
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, ymax - margin / 2),
                    new Point(x, ymax + margin / 2)));
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);

            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, 0), new Point(xmin, canGraph.Height)));
            for (double y = step; y <= canGraph.Height - step; y += step)
            {
                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(xmin - margin / 2, y),
                    new Point(xmin + margin / 2, y)));
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canGraph.Children.Add(yaxis_path);

            // Make some data sets.
            Brush[] brushes = { Brushes.Red, Brushes.Green, Brushes.Blue };
            Random rand = new Random();
            for (int data_set = 0; data_set < 3; data_set++)
            {
                int last_y = rand.Next((int)ymin, (int)ymax);

                PointCollection points = new PointCollection();
                for (double x = xmin; x <= xmax; x += step)
                {
                    last_y = rand.Next(last_y - 10, last_y + 10);
                    if (last_y < ymin) last_y = (int)ymin;
                    if (last_y > ymax) last_y = (int)ymax;
                    points.Add(new Point(x, last_y));
                }

                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 1;
                polyline.Stroke = brushes[data_set];
                polyline.Points = points;

                canGraph.Children.Add(polyline);
            }
        }

        private void DrawCustomGraph()
        {
            int measurementsAmount = 5;
            double maxYVal = 10.0;
            double marginVal = 10;

            //graphData = new GraphDataContainer(canGraph, canGraph.Width, canGraph.Height, marginVal, measurementsAmount, maxYVal);
            graphData = new GraphData(canGraph, canGraph.Width, canGraph.Height, marginVal, measurementsAmount, maxYVal);

            double[] dataSet1 = new double[5] { 1, 2, 3, 2, 1 };
            graphData.AddDataSet(dataSet1);

            double[] dataSet2 = new double[5] { 4, 4, 1, 4, 4 };
            graphData.AddDataSet(dataSet2);

            double[] dataSet3 = new double[5] { 0, 4, 8, 8, 2 };
            graphData.AddDataSet(dataSet3);

            graphData.Recalculate();

            RedrawAxes();
            CreateDataSets();
        }

        public void RedrawAxes()
        {
            RedrawAxisX();
            RedrawAxisY();
        }

        public void RedrawAxisY()
        {
            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(graphData.xmin, 0), new Point(graphData.xmin, graphData.graphHeight)));//new Point(graphData.xmin, 0), new Point(graphData.xmin, canGraph.Height)));
            for (double y = 0; y <= graphData.graphHeight; y += graphData.yStep)//for (double y = yStep; y <= canGraph.Height - yStep; y += yStep)
            {
                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(graphData.xmin - graphData.margin / 2, y),
                    new Point(graphData.xmin + graphData.margin / 2, y)));
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            if (canvasElementList.Contains(elementName_Yaxis))
            {
                int elementIndex = canvasElementNames[elementName_Yaxis];
                canGraph.Children.RemoveAt(elementIndex);
                canGraph.Children.Insert(elementIndex, yaxis_path);
            }
            else
            {
                canGraph.Children.Add(yaxis_path);
                canvasElementList.Add(elementName_Yaxis);
            }
        }

        public void RedrawAxisX()
        {
            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new Point(0, graphData.ymax), new Point(graphData.graphWidth, graphData.ymax)));//new Point(0, graphData.ymax), new Point(canGraph.Width, graphData.ymax)));
            for (double x = graphData.xmin + graphData.xStep;
                x <= graphData.graphWidth; x += graphData.xStep)//x <= canGraph.Width - xStep; x += xStep)
            {
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, graphData.ymax - graphData.margin / 2),
                    new Point(x, graphData.ymax + graphData.margin / 2)));
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            if (canvasElementList.Contains(elementName_Xaxis))
            {
                int elementIndex = canvasElementNames[elementName_Xaxis];
                canGraph.Children.RemoveAt(elementIndex);
                canGraph.Children.Insert(elementIndex, xaxis_path);
            }
            else
            {
                canGraph.Children.Add(xaxis_path);
                canvasElementList.Add(elementName_Xaxis);
            }
        }

        public void CreateDataSets()
        {
            // Create dataset
            for (int d = 0; d < graphData.dataSets.Count; d++)
            {
                if (d >= graphData.brushes.Length) { break; }
                PointCollection points = new PointCollection();
                for (int i = 0; i < graphData.maxValueX; i++)
                {
                    double xCoord = graphData.xmin + (i * graphData.xStep);
                    points.Add(new Point(xCoord, graphData.ymax - (graphData.dataSets[d][i] * graphData.yScale)));
                }

                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 1;
                polyline.Stroke = graphData.brushes[d];
                polyline.Points = points;

                if (canvasElementList.Contains(elementName_Graph + d))
                {
                    int elementIndex = canvasElementNames[elementName_Graph + d];
                    canGraph.Children.RemoveAt(elementIndex);
                    canGraph.Children.Insert(elementIndex, polyline);
                }
                else
                {
                    canGraph.Children.Add(polyline);
                    canvasElementList.Add(elementName_Graph + d);
                }
            }

            // Calculate averages
            for (int d = 0; d < graphData.dataSets.Count; d++)
            {
                if (d >= graphData.brushesAverage.Length) { break; }
                PointCollection pointsAverage = new PointCollection();
                double sumValue = 0;
                for (int i = 0; i < graphData.maxValueX; i++)
                {
                    sumValue += graphData.dataSets[d][i];
                }

                double averageValue = sumValue / graphData.maxValueX;
                for (int i = 0; i < graphData.maxValueX; i++)
                {
                    double xCoord = graphData.xmin + (i * graphData.xStep);
                    pointsAverage.Add(new Point(xCoord, graphData.ymax - (averageValue * graphData.yScale)));
                }

                Polyline polylineAverage = new Polyline();
                polylineAverage.StrokeThickness = 2;
                polylineAverage.Stroke = graphData.brushesAverage[d];
                polylineAverage.Points = pointsAverage;

                if (canvasElementList.Contains(elementName_Average + d))
                {
                    int elementIndex = canvasElementNames[elementName_Average + d];
                    canGraph.Children.RemoveAt(elementIndex);
                    canGraph.Children.Insert(elementIndex, polylineAverage);
                }
                else
                {
                    canGraph.Children.Add(polylineAverage);
                    canvasElementList.Add(elementName_Average + d);
                }
            }

            if(canvasElementNames.Count != canvasElementList.Count)
            {            
                canvasElementNames.Clear();
                for (int i = 0; i < canvasElementList.Count; i++)
                {
                    int index = canvasElementNames.Count;
                    canvasElementNames.Add(canvasElementList[i], index);
                }
            }
        }

        public void RecalculateDataSets()
        {
            for (int d = 0; d < graphData.dataSets.Count; d++)
            {
                if (d >= graphData.brushes.Length) { break; }
                PointCollection points = ((Polyline)canGraph.Children[canvasElementNames[elementName_Graph + d]]).Points;
                ((Polyline)canGraph.Children[canvasElementNames[elementName_Graph + d]]).Points.Clear();
                for (int i = 0; i < graphData.maxValueX; i++)
                {
                    double xCoord = graphData.xmin + (i * graphData.xStep);
                    Point newPoint = new Point(xCoord, graphData.ymax - (graphData.dataSets[d][i] * graphData.yScale));

                    if (i < points.Count - 1)
                    {
                        ((Polyline)canGraph.Children[canvasElementNames[elementName_Graph + d]]).Points[i] = newPoint;
                    }
                    else ((Polyline)canGraph.Children[canvasElementNames[elementName_Graph + d]]).Points.Add(newPoint);
                }
            }

            for (int d = 0; d < graphData.dataSets.Count; d++)
            {
                if (d >= graphData.brushesAverage.Length) { break; }
                PointCollection pointsAverage = ((Polyline)canGraph.Children[canvasElementNames[elementName_Average + d]]).Points;
                ((Polyline)canGraph.Children[canvasElementNames[elementName_Average + d]]).Points.Clear();
                double sumValue = 0;
                for (int i = 0; i < graphData.maxValueX; i++)
                {
                    sumValue += graphData.dataSets[d][i];
                }

                double averageValue = sumValue / graphData.maxValueX;
                for (int i = 0; i < graphData.maxValueX; i++)
                {
                    double xCoord = graphData.xmin + (i * graphData.xStep);
                    Point newPoint = new Point(xCoord, graphData.ymax - (averageValue * graphData.yScale));
                    
                    if (i < pointsAverage.Count - 1)
                    {
                        ((Polyline)canGraph.Children[canvasElementNames[elementName_Average + d]]).Points[i] = newPoint;
                    }
                    else ((Polyline)canGraph.Children[canvasElementNames[elementName_Average + d]]).Points.Add(newPoint);
                }
            }
        }

        //private void DrawCustomGraph()
        //{
        //    int measurements = 5;
        //    int maxVal = 10;

        //    const double margin = 10;
        //    double xmin = margin;
        //    double xmax = canGraph.Width - margin;
        //    double ymin = margin;
        //    double ymax = canGraph.Height - margin;
        //    const double step = 10;

        //    double xStep = xmax / (measurements - 1);
        //    double yStep = ymax / 5;
        //    double yScale = ymax / maxVal;

        //    //graphData = new GraphDataContainer(canGraph.Width, canGraph.Height, margin, measurements, maxVal);

        //    List<double[]> dataSets = new List<double[]>();

        //    double[] dataSet1 = new double[5] { 1, 2, 3, 2, 1 };
        //    dataSets.Add(dataSet1);

        //    double[] dataSet2 = new double[5] { 4, 4, 1, 4, 4 };
        //    dataSets.Add(dataSet2);

        //    double[] dataSet3 = new double[5] { 0, 4, 8, 8, 2 };
        //    dataSets.Add(dataSet3);

        //    // Make the X axis.
        //    GeometryGroup xaxis_geom = new GeometryGroup();
        //    xaxis_geom.Children.Add(new LineGeometry(
        //        new Point(0, ymax), new Point(canGraph.Width, ymax)));
        //    for (double x = xmin + xStep;
        //        x <= canGraph.Width; x += xStep)//x <= canGraph.Width - xStep; x += xStep)
        //    {
        //        xaxis_geom.Children.Add(new LineGeometry(
        //            new Point(x, ymax - margin / 2),
        //            new Point(x, ymax + margin / 2)));
        //    }

        //    Path xaxis_path = new Path();
        //    xaxis_path.StrokeThickness = 1;
        //    xaxis_path.Stroke = Brushes.Black;
        //    xaxis_path.Data = xaxis_geom;

        //    canGraph.Children.Add(xaxis_path);
        //    canvasElementList.Add(elementName_Xaxis);

        //    // Make the Y ayis.
        //    GeometryGroup yaxis_geom = new GeometryGroup();
        //    yaxis_geom.Children.Add(new LineGeometry(
        //        new Point(xmin, 0), new Point(xmin, canGraph.Height)));
        //    for (double y = 0; y <= canGraph.Height; y += yStep)//for (double y = yStep; y <= canGraph.Height - yStep; y += yStep)
        //    {
        //        yaxis_geom.Children.Add(new LineGeometry(
        //            new Point(xmin - margin / 2, y),
        //            new Point(xmin + margin / 2, y)));
        //    }

        //    Path yaxis_path = new Path();
        //    yaxis_path.StrokeThickness = 1;
        //    yaxis_path.Stroke = Brushes.Black;
        //    yaxis_path.Data = yaxis_geom;

        //    canGraph.Children.Add(yaxis_path);
        //    canvasElementList.Add(elementName_Yaxis);

        //    // Make some data sets.
        //    Brush[] brushes = { Brushes.Red, Brushes.Green, Brushes.Blue };
        //    Brush[] brushesAverage = { Brushes.Crimson, Brushes.DarkOliveGreen, Brushes.Navy };

        //    for (int d = 0; d < dataSets.Count; d++)
        //    {
        //        if (d >= brushes.Length) { break; }
        //        PointCollection points = new PointCollection();
        //        for (int i = 0; i < measurements; i++)
        //        {
        //            double xCoord = xmin + (i * xStep);
        //            points.Add(new Point(xCoord, ymax - (dataSets[d][i] * yScale)));
        //        }

        //        Polyline polyline = new Polyline();
        //        polyline.StrokeThickness = 1;
        //        polyline.Stroke = brushes[d];
        //        polyline.Points = points;

        //        canGraph.Children.Add(polyline);
        //        canvasElementList.Add(elementName_Graph + d);
        //    }

        //    // Calculate averages
        //    for (int d = 0; d < dataSets.Count; d++)
        //    {
        //        if (d >= brushesAverage.Length) { break; }
        //        PointCollection pointsAverage = new PointCollection();
        //        double sumValue = 0;
        //        for (int i = 0; i < measurements; i++)
        //        {
        //            sumValue += dataSets[d][i];
        //        }

        //        double averageValue = sumValue / measurements;
        //        for (int i = 0; i < measurements; i++)
        //        {
        //            double xCoord = xmin + (i * xStep);
        //            pointsAverage.Add(new Point(xCoord, ymax - (averageValue * yScale)));
        //        }

        //        Polyline polylineAverage = new Polyline();
        //        polylineAverage.StrokeThickness = 2;
        //        polylineAverage.Stroke = brushesAverage[d];
        //        polylineAverage.Points = pointsAverage;

        //        canGraph.Children.Add(polylineAverage);
        //        canvasElementList.Add(elementName_Average + d);
        //    }

        //    for (int i = 0; i < canvasElementList.Count; i++)
        //    {
        //        int index = canvasElementNames.Count;
        //        canvasElementNames.Add(canvasElementList[i], index);
        //    }
        //}

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            //if (currentElementSelectedIndex < canGraph.Children.Count)
            //{
            //    canGraph.Children[currentElementSelectedIndex].Visibility = Visibility.Visible;
            //}

            //currentElementSelectedIndex++;
            //if(currentElementSelectedIndex > canGraph.Children.Count - 1)
            //{
            //    currentElementSelectedIndex = 0;
            //}

            //if (currentElementSelectedIndex < canGraph.Children.Count)
            //{
            //    Console.WriteLine("Hiding element " + currentElementSelectedIndex + ": " + canvasElementList[currentElementSelectedIndex]);
            //    Console.WriteLine("Dictionary says the index of this element is " + canvasElementNames[canvasElementList[currentElementSelectedIndex]]);
            //    canGraph.Children[currentElementSelectedIndex].Visibility = Visibility.Hidden;
            //}            
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
            //int measurements = ArrayCompare.algorithmPerformances.Count;
            int algorithmCount = ArrayCompare.algorithmNames.Count;
            
            graphData.ResetDataSets();

            // Get data of every algorithm for a single array
            for(int i=0; i< algorithmCount; i++)
            {
                if (i > 2) { break; }
                graphData.AddDataSet(ArrayCompare.GetResultArrayDouble(i, 1));
            }

            double maxY = 0.0;
            for (int i = 0; i < graphData.dataSets.Count; i++)
            {
                for(int j=0; j< graphData.dataSets[i].Length; j++)
                {
                    double curY = graphData.dataSets[i][j];
                    Console.WriteLine("Calculated value from graphPoints[" + j + "] of dataset " + i + " = " + curY);
                    if (curY > maxY)
                    {
                        maxY = curY;
                    }
                }                
            }

            Console.WriteLine("Calculated maxY=" + maxY);

            graphData.SetMaxY(maxY);
            graphData.Recalculate();
            RedrawAxisX();

            // Recalculate and redraw graph
            RecalculateDataSets();

            graphData.PrintDataSetValues();
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
            HandleCheckBox(0, (checkBoxOne.IsChecked == true));
        }

        private void CheckBoxTwo_Click(object sender, RoutedEventArgs e)
        {
            HandleCheckBox(1, (checkBoxTwo.IsChecked == true));
        }

        private void CheckBoxThree_Click(object sender, RoutedEventArgs e)
        {
            HandleCheckBox(2, (checkBoxThree.IsChecked == true));
        }

        private void HandleCheckBox(int checkBoxNumber, bool visible)
        {
            string graphName = elementName_Graph + checkBoxNumber;
            string averageName = elementName_Average + checkBoxNumber;

            if (visible)
            {
                if (canvasElementNames.Keys.Contains(graphName))
                {
                    canGraph.Children[canvasElementNames[graphName]].Visibility = Visibility.Visible;
                }
                if (canvasElementNames.Keys.Contains(averageName))
                {
                    canGraph.Children[canvasElementNames[averageName]].Visibility = Visibility.Visible;
                }                
            }
            else
            {
                if (canvasElementNames.Keys.Contains(graphName))
                {
                    canGraph.Children[canvasElementNames[graphName]].Visibility = Visibility.Hidden;
                }
                if (canvasElementNames.Keys.Contains(averageName))
                {
                    canGraph.Children[canvasElementNames[averageName]].Visibility = Visibility.Hidden;
                }
            }

            if (checkBoxAutoResize.IsChecked == true)
            {
                RescaleCanvas();
            }
        }

        private void CheckBoxAutoResize_Click(object sender, RoutedEventArgs e)
        {
            if(checkBoxAutoResize.IsChecked == true)
            {
                RescaleCanvas();
            }            
        }

        private void RescaleCanvas()
        {
            double maxY;
            double minY = graphData.ymax;
            PointCollection graphPoints;
            
            // Calculate smallest value of the datasets with visible graphs (-> closest to 0, which is at the top of the canvas!)
            if (checkBoxOne.IsChecked == true)
            {
                graphPoints = ((Polyline)canGraph.Children[canvasElementNames[elementName_Graph + 0]]).Points;
                for (int i = 0; i < graphPoints.Count; i++)
                {
                    double curY = graphPoints[i].Y;
                    Console.WriteLine("Calculated value from graphPoints[" + i + "] of dataset 1 = " + curY);
                    if (curY < minY)
                    {
                        minY = (int)curY;
                    }
                }
            }
            if (checkBoxTwo.IsChecked == true)
            {
                graphPoints = ((Polyline)canGraph.Children[canvasElementNames[elementName_Graph + 1]]).Points;
                for (int i = 0; i < graphPoints.Count; i++)
                {
                    double curY = graphPoints[i].Y;
                    Console.WriteLine("Calculated value from graphPoints[" + i + "] of dataset 2 = " + curY);
                    if (curY < minY)
                    {
                        minY = (int)curY;
                    }
                }
            }
            if (checkBoxThree.IsChecked == true)
            {
                graphPoints = ((Polyline)canGraph.Children[canvasElementNames[elementName_Graph + 2]]).Points;
                for (int i = 0; i < graphPoints.Count; i++)
                {
                    double curY = graphPoints[i].Y;
                    Console.WriteLine("Calculated value from graphPoints[" + i + "] of dataset 3 = " + curY);
                    if (curY < minY)
                    {
                        minY = (int)curY;
                    }
                }
            }

            Console.WriteLine("Calculated minY = " + minY);
            maxY = (minY - graphData.ymax) / (-graphData.yScale);
            Console.WriteLine("Calculated maxY = " + maxY);

            if (maxY != 0)
            {
                // Set max Y value
                graphData.SetMaxY(maxY);
                graphData.Recalculate();
                RedrawAxisY();

                // Recalculate and redraw graph
                RecalculateDataSets();
            }
        }
    }    
}
