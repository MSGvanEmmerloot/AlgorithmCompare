using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AlgorithmTests
{
    public class GraphData
    {     
        List<string> canvasElementList = new List<string>();
        Dictionary<string, int> canvasElementNames = new Dictionary<string, int>();

        public CanvasData canvasData;

        const string elementName_Xaxis = "X-Axis";
        const string elementName_Yaxis = "Y-Axis";
        const string elementName_Graph = "Graph ";
        const string elementName_Average = "Average ";

        public bool checkBoxOneChecked = true;
        public bool checkBoxTwoChecked = true;
        public bool checkBoxThreeChecked = true;
        public bool checkBoxAutoResizeChecked = true;

        public class CanvasData{

            public Canvas canvas { get; private set; }

            public int maxValueX { get; private set; } = 5;
            public double maxValueY { get; private set; } = 10.0;

            public double graphWidth { get; private set; } = 0.0;
            public double graphHeight { get; private set; } = 0.0;

            public double margin { get; private set; } = 10;
            public double xmin { get; private set; } = 0.0;
            public double xmax { get; private set; } = 0.0;
            public double ymin { get; private set; } = 0.0;
            public double ymax { get; private set; } = 0.0;

            public double xStep { get; private set; } = 0.0;
            public double yStep { get; private set; } = 0.0;
            public double yScale { get; private set; } = 0.0;

            public List<double[]> dataSets { get; private set; } = new List<double[]>();

            public Brush[] brushes { get; private set; } = { Brushes.Red, Brushes.Green, Brushes.Blue };
            public Brush[] brushesAverage { get; private set; } = { Brushes.Crimson, Brushes.DarkOliveGreen, Brushes.Navy };

            //public CanvasData(Canvas _canvas, double _graphWidth, double _graphHeight, double _margin, int _amountOfMeasurements, double _maxYValue)
            public CanvasData(Canvas _canvas)
            {
                canvas = _canvas;
                graphWidth = canvas.Width;
                graphHeight = canvas.Height;
            }

            public void Recalculate()
            {
                if (dataSets.Count < 1)
                {
                    Console.WriteLine("No datasets added!");
                    return;
                }

                maxValueX = dataSets[0].Length;

                xmax = graphWidth - margin;
                ymax = graphHeight - margin;

                xmin = margin;
                ymin = margin;

                xStep = xmax / (maxValueX - 1);
                yStep = ymax / 5;
                yScale = ymax / maxValueY;
            }

            public void SetMargin(double _margin)
            {
                margin = _margin;
            }

            public void SetMaxX(int _x)
            {
                maxValueX = _x;
            }

            public void SetMaxY(double _y)
            {
                maxValueY = _y;
            }

            public void AddDataSet(double[] set)
            {
                dataSets.Add(set);
            }

            public void ResetDataSets()
            {
                dataSets.Clear();
            }

            public void PrintDataSetValues()
            {
                for (int d = 0; d < dataSets.Count; d++)
                {
                    for (int i = 0; i < dataSets[d].Length; i++)
                    {
                        Console.WriteLine("dataSets[" + d + "][" + i + "] = " + dataSets[d][i]);
                    }
                }
            }
        }

        public GraphData(Canvas _canvas)
        {
            canvasData = new CanvasData(_canvas);
        }

        private void DrawDemoGraph()
        {            
            const double margin = 10;
            double xmin = margin;
            double xmax = canvasData.canvas.Width - margin;
            double ymin = margin;
            double ymax = canvasData.canvas.Height - margin;
            const double step = 10;

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new Point(0, ymax), new Point(canvasData.canvas.Width, ymax)));
            for (double x = xmin + step;
                x <= canvasData.canvas.Width - step; x += step)
            {
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, ymax - margin / 2),
                    new Point(x, ymax + margin / 2)));
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canvasData.canvas.Children.Add(xaxis_path);

            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, 0), new Point(xmin, canvasData.canvas.Height)));
            for (double y = step; y <= canvasData.canvas.Height - step; y += step)
            {
                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(xmin - margin / 2, y),
                    new Point(xmin + margin / 2, y)));
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canvasData.canvas.Children.Add(yaxis_path);

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

                canvasData.canvas.Children.Add(polyline);
            }
        }

        public void DrawCustomGraph()
        {
            double[] dataSet1 = new double[5] { 1, 2, 3, 2, 1 };
            canvasData.AddDataSet(dataSet1);

            double[] dataSet2 = new double[5] { 4, 4, 1, 4, 4 };
            canvasData.AddDataSet(dataSet2);

            double[] dataSet3 = new double[5] { 0, 4, 8, 8, 2 };
            canvasData.AddDataSet(dataSet3);

            canvasData.Recalculate();

            RedrawAxes();
            CreateDataSets();
        }

        public void RedrawAxes()
        {
            RedrawAxisX();
            RedrawAxisY();
        }

        public void RedrawAxisX()
        {
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new Point(0, canvasData.ymax), new Point(canvasData.graphWidth, canvasData.ymax)));
            for (double x = canvasData.xmin + canvasData.xStep;
                x <= canvasData.graphWidth; x += canvasData.xStep)
            {
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, canvasData.ymax - canvasData.margin / 2),
                    new Point(x, canvasData.ymax + canvasData.margin / 2)));
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            if (canvasElementList.Contains(elementName_Xaxis))
            {
                int elementIndex = canvasElementNames[elementName_Xaxis];
                canvasData.canvas.Children.RemoveAt(elementIndex);
                canvasData.canvas.Children.Insert(elementIndex, xaxis_path);
            }
            else
            {
                canvasData.canvas.Children.Add(xaxis_path);
                canvasElementList.Add(elementName_Xaxis);
            }
        }

        public void RedrawAxisY()
        {
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(canvasData.xmin, 0), new Point(canvasData.xmin, canvasData.graphHeight)));
            for (double y = 0; y <= canvasData.graphHeight; y += canvasData.yStep)
            {
                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(canvasData.xmin - canvasData.margin / 2, y),
                    new Point(canvasData.xmin + canvasData.margin / 2, y)));
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            if (canvasElementList.Contains(elementName_Yaxis))
            {
                int elementIndex = canvasElementNames[elementName_Yaxis];
                canvasData.canvas.Children.RemoveAt(elementIndex);
                canvasData.canvas.Children.Insert(elementIndex, yaxis_path);
            }
            else
            {
                canvasData.canvas.Children.Add(yaxis_path);
                canvasElementList.Add(elementName_Yaxis);
            }
        }

        

        public void CreateDataSets()
        {
            // Create dataset
            for (int d = 0; d < canvasData.dataSets.Count; d++)
            {
                if (d >= canvasData.brushes.Length) { break; }
                PointCollection points = new PointCollection();
                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    double xCoord = canvasData.xmin + (i * canvasData.xStep);
                    points.Add(new Point(xCoord, canvasData.ymax - (canvasData.dataSets[d][i] * canvasData.yScale)));
                }

                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 1;
                polyline.Stroke = canvasData.brushes[d];
                polyline.Points = points;

                if (canvasElementList.Contains(elementName_Graph + d))
                {
                    int elementIndex = canvasElementNames[elementName_Graph + d];
                    canvasData.canvas.Children.RemoveAt(elementIndex);
                    canvasData.canvas.Children.Insert(elementIndex, polyline);
                }
                else
                {
                    canvasData.canvas.Children.Add(polyline);
                    canvasElementList.Add(elementName_Graph + d);
                }
            }

            // Calculate averages
            for (int d = 0; d < canvasData.dataSets.Count; d++)
            {
                if (d >= canvasData.brushesAverage.Length) { break; }
                PointCollection pointsAverage = new PointCollection();
                double sumValue = 0;
                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    sumValue += canvasData.dataSets[d][i];
                }

                double averageValue = sumValue / canvasData.maxValueX;
                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    double xCoord = canvasData.xmin + (i * canvasData.xStep);
                    pointsAverage.Add(new Point(xCoord, canvasData.ymax - (averageValue * canvasData.yScale)));
                }

                Polyline polylineAverage = new Polyline();
                polylineAverage.StrokeThickness = 2;
                polylineAverage.Stroke = canvasData.brushesAverage[d];
                polylineAverage.Points = pointsAverage;

                if (canvasElementList.Contains(elementName_Average + d))
                {
                    int elementIndex = canvasElementNames[elementName_Average + d];
                    canvasData.canvas.Children.RemoveAt(elementIndex);
                    canvasData.canvas.Children.Insert(elementIndex, polylineAverage);
                }
                else
                {
                    canvasData.canvas.Children.Add(polylineAverage);
                    canvasElementList.Add(elementName_Average + d);
                }
            }

            if (canvasElementNames.Count != canvasElementList.Count)
            {
                canvasElementNames.Clear();
                for (int i = 0; i < canvasElementList.Count; i++)
                {
                    int index = canvasElementNames.Count;
                    canvasElementNames.Add(canvasElementList[i], index);
                }
            }
        }

        public void AddAlgorithmsDataToGraph(List<double[]> algorithmPerformances)
        {
            int algorithmCount = ArrayCompare.algorithmNames.Count;

            canvasData.ResetDataSets();

            for (int i = 0; i < algorithmCount; i++)
            {
                if (i > 2) { break; }
                canvasData.AddDataSet(algorithmPerformances[i]);
            }

            double maxY = 0.0;
            for (int i = 0; i < canvasData.dataSets.Count; i++)
            {
                for (int j = 0; j < canvasData.dataSets[i].Length; j++)
                {
                    double curY = canvasData.dataSets[i][j];
                    Console.WriteLine("Calculated value from graphPoints[" + j + "] of dataset " + i + " = " + curY);
                    if (curY > maxY)
                    {
                        maxY = curY;
                    }
                }
            }

            Console.WriteLine("Calculated maxY=" + maxY);

            canvasData.SetMaxY(maxY);
            canvasData.Recalculate();
            RedrawAxisX();

            // Recalculate and redraw graph
            RecalculateDataSets();

            canvasData.PrintDataSetValues();
        }

        public void RecalculateDataSets()
        {
            for (int d = 0; d < canvasData.dataSets.Count; d++)
            {
                if (d >= canvasData.brushes.Length) { break; }
                PointCollection points = ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Graph + d]]).Points;
                ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Graph + d]]).Points.Clear();
                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    double xCoord = canvasData.xmin + (i * canvasData.xStep);
                    Point newPoint = new Point(xCoord, canvasData.ymax - (canvasData.dataSets[d][i] * canvasData.yScale));

                    if (i < points.Count - 1)
                    {
                        ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Graph + d]]).Points[i] = newPoint;
                    }
                    else ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Graph + d]]).Points.Add(newPoint);
                }
            }

            for (int d = 0; d < canvasData.dataSets.Count; d++)
            {
                if (d >= canvasData.brushesAverage.Length) { break; }
                PointCollection pointsAverage = ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Average + d]]).Points;
                ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Average + d]]).Points.Clear();
                double sumValue = 0;
                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    sumValue += canvasData.dataSets[d][i];
                }

                double averageValue = sumValue / canvasData.maxValueX;
                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    double xCoord = canvasData.xmin + (i * canvasData.xStep);
                    Point newPoint = new Point(xCoord, canvasData.ymax - (averageValue * canvasData.yScale));

                    if (i < pointsAverage.Count - 1)
                    {
                        ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Average + d]]).Points[i] = newPoint;
                    }
                    else ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Average + d]]).Points.Add(newPoint);
                }
            }
        }

        public void HandleCheckBox(int checkBoxNumber, bool visible)
        {
            string graphName = elementName_Graph + checkBoxNumber;
            string averageName = elementName_Average + checkBoxNumber;

            if (visible)
            {
                if (canvasElementNames.Keys.Contains(graphName))
                {
                    canvasData.canvas.Children[canvasElementNames[graphName]].Visibility = Visibility.Visible;
                }
                if (canvasElementNames.Keys.Contains(averageName))
                {
                    canvasData.canvas.Children[canvasElementNames[averageName]].Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (canvasElementNames.Keys.Contains(graphName))
                {
                    canvasData.canvas.Children[canvasElementNames[graphName]].Visibility = Visibility.Hidden;
                }
                if (canvasElementNames.Keys.Contains(averageName))
                {
                    canvasData.canvas.Children[canvasElementNames[averageName]].Visibility = Visibility.Hidden;
                }
            }

            if (checkBoxAutoResizeChecked)
            {
                RescaleCanvas();
            }
        }

        public void RescaleCanvas()
        {
            double maxY;
            double minY = canvasData.ymax;
            PointCollection graphPoints;

            // Calculate smallest value of the datasets with visible graphs (-> closest to 0, which is at the top of the canvas!)
            if (checkBoxOneChecked)
            {
                graphPoints = ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Graph + 0]]).Points;
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
            if (checkBoxTwoChecked)
            {
                graphPoints = ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Graph + 1]]).Points;
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
            if (checkBoxThreeChecked)
            {
                graphPoints = ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Graph + 2]]).Points;
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
            maxY = (minY - canvasData.ymax) / (-canvasData.yScale);
            Console.WriteLine("Calculated maxY = " + maxY);

            if (maxY != 0)
            {
                // Set max Y value
                canvasData.SetMaxY(maxY);
                canvasData.Recalculate();
                RedrawAxisY();

                // Recalculate and redraw graph
                RecalculateDataSets();
            }
        }

    }
}
