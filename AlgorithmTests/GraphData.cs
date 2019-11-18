using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AlgorithmTests
{
    public class GraphData
    {     
        public enum GraphElementTypes { Xaxis, Yaxis, Graph, Average, Point, Label}

        public List<string> canvasElementList { get; private set; } = new List<string>();
        public Dictionary<string, int> canvasElementNames { get; private set; } = new Dictionary<string, int>();

        public CanvasData canvasData;

        public bool autoResize = true;
        public bool plotPolyline = true;
        public List<bool> datasetVisible { get; private set; } = new List<bool>();

        public class DatasetContainer
        {
            public List<double[]> dataSets = new List<double[]>();
            public double maxValueY = 10.0;
            public double yScale  = 0.0;
            public double whitespaceLeft = 10.0f;
        }

        public class CanvasData{

            public Canvas canvas { get; private set; }

            public int maxValueX { get; private set; } = 5;
            //public double maxValueY { get; private set; } = 10.0;

            public double graphWidth { get; private set; } = 0.0;
            public double graphHeight { get; private set; } = 0.0;

            public double margin { get; private set; } = 15;
            public double xmin { get; private set; } = 0.0;
            public double xmax { get; private set; } = 0.0;
            public double ymin { get; private set; } = 0.0;
            public double ymax { get; private set; } = 0.0;

            public double xStep { get; private set; } = 0.0;
            public double yStep { get; private set; } = 0.0;
            //public double yScale { get; private set; } = 0.0;
            public int amountYsteps { get; private set; } = 5;

            public List<DatasetContainer> arrayDatasets = new List<DatasetContainer>();
            public int selectedArray = 0;
            //public List<double[]> dataSets { get; private set; } = new List<double[]>();

            public Brush[] brushes { get; private set; } = 
                { Brushes.Red, Brushes.Green, Brushes.Indigo, Brushes.Orange, Brushes.Blue, Brushes.Violet, Brushes.Yellow };
            public Brush[] brushesAverage { get; private set; } = 
                { Brushes.Crimson, Brushes.DarkOliveGreen, Brushes.Purple, Brushes.DarkOrange, Brushes.Navy, Brushes.DeepPink, Brushes.Goldenrod };

            public CanvasData(Canvas _canvas)
            {
                canvas = _canvas;
                graphWidth = canvas.Width;
                graphHeight = canvas.Height;
            }

            public void Recalculate(int array)
            {
                if (arrayDatasets.Count < array) { Console.WriteLine("Array not found"); return; }

                if (arrayDatasets[array].dataSets.Count < 1)
                {
                    Console.WriteLine("No datasets added!");
                    maxValueX = 5;
                }
                else maxValueX = arrayDatasets[array].dataSets[0].Length;

                xmax = graphWidth - margin;
                ymax = graphHeight - margin;

                xmin = margin;
                ymin = margin;

                xStep = xmax / (maxValueX - 1);
                yStep = ymax / amountYsteps;
                arrayDatasets[array].yScale = ymax / arrayDatasets[array].maxValueY;
            }

            public void SetMargin(double _margin)
            {
                margin = _margin;
            }

            public void SetMaxX(int _x)
            {
                maxValueX = _x;
            }

            public void SetMaxY(int array, double _y)
            {
                arrayDatasets[array].maxValueY = _y;
            }

            public void AddDataset(int array, double[] set)
            {
                if(array< arrayDatasets.Count)
                {
                    arrayDatasets[array].dataSets.Add(set);
                }                
            }

            public void ResetDatasets(int array)
            {
                arrayDatasets[array].dataSets.Clear();
            }

            public void PrintDatasetValues(int array)
            {
                for (int d = 0; d < arrayDatasets[array].dataSets.Count; d++)
                {
                    for (int i = 0; i < arrayDatasets[array].dataSets[d].Length; i++)
                    {
                        Console.WriteLine("dataSets[" + d + "][" + i + "] = " + arrayDatasets[array].dataSets[d][i]);
                    }
                }
            }
        }

        public GraphData(Canvas _canvas)
        {
            canvasData = new CanvasData(_canvas);
            InitZero();
        }

        public string GetFormattedString(GraphElementTypes elementType, int datasetIndex=-1, int pointIndex=-1)
        {
            switch (elementType)
            {
                case GraphElementTypes.Xaxis:
                    return "X-Axis";
                case GraphElementTypes.Yaxis:
                    return "Y-Axis";
                case GraphElementTypes.Graph:
                    if(datasetIndex != -1)
                    {
                        return "Graph " + datasetIndex;
                    }
                    return null;
                case GraphElementTypes.Average:
                    if (datasetIndex != -1)
                    {
                        return "Average " + datasetIndex;
                    }
                    return null;                    
                case GraphElementTypes.Point:
                    if (datasetIndex != -1 && pointIndex != -1)
                    {
                        return "Point " + datasetIndex + " " + pointIndex;
                    }
                    return null;
                case GraphElementTypes.Label:
                    if (datasetIndex != -1)
                    {
                        return "Label " + datasetIndex;
                    }
                    return null;
            }
            return null;
        }
        
        public void DrawCustomGraph(int array)
        {
            canvasData.Recalculate(array);

            RedrawAxes();
        }        

        public void RedrawAxisX()
        {
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(new Point(0, canvasData.ymax), new Point(canvasData.graphWidth, canvasData.ymax)));

            for (double x = canvasData.xmin + canvasData.xStep; x <= canvasData.graphWidth; x += canvasData.xStep)
            {
                xaxis_geom.Children.Add(new LineGeometry(new Point(x, canvasData.ymax - canvasData.margin / 2), new Point(x, canvasData.ymax + canvasData.margin / 2)));
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            string xName = GetFormattedString(GraphElementTypes.Xaxis);

            if (canvasElementList.Contains(xName))
            {
                if (canvasElementNames.Keys.Contains(xName))
                {
                    int elementIndex = canvasElementNames[xName];
                    if (canvasData.canvas.Children.Count > elementIndex)
                    {
                        ReplaceElement(elementIndex, xaxis_path);
                    }
                    else AddElement(xName, xaxis_path);
                }
            }
            else AddElement(xName, xaxis_path);
        }

        public void RedrawAxisY()
        {
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(new Point(canvasData.xmin, 0), new Point(canvasData.xmin, canvasData.graphHeight)));

            for (double y = 0; y <= canvasData.graphHeight; y += canvasData.yStep)
            {
                yaxis_geom.Children.Add(new LineGeometry(new Point(canvasData.xmin - canvasData.margin / 2, y), new Point(canvasData.xmin + canvasData.margin / 2, y)));
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            string yName = GetFormattedString(GraphElementTypes.Yaxis);

            if (canvasElementList.Contains(yName))
            {
                if (canvasElementNames.Keys.Contains(yName)) {
                    int elementIndex = canvasElementNames[yName];
                    if(canvasData.canvas.Children.Count > elementIndex)
                    {
                        ReplaceElement(elementIndex, yaxis_path);
                    }
                    else AddElement(yName, yaxis_path);
                }
            }
            else AddElement(yName, yaxis_path);

            RedrawLabelsY();
        }

        public void RedrawLabelsY()
        {
            if (canvasData.arrayDatasets.Count < canvasData.selectedArray) { return; }
            double maxY = canvasData.arrayDatasets[canvasData.selectedArray].maxValueY;
            List<UIElement> yaxis_labels = new List<UIElement>();

            double whiteSpace = 0.0f;
            if (canvasData.arrayDatasets.Count > canvasData.selectedArray)
            {
                whiteSpace = canvasData.arrayDatasets[canvasData.selectedArray].whitespaceLeft;
            }

            int step = 0;
            int division = 1;
            int smallest = (int)(maxY / canvasData.amountYsteps);
            while (smallest >= 10)
            {
                smallest /= 10;
                division *= 10;
            }
            //if (maxY > 1000)
            //{
            //    division = 1000;
            //}

            for (double y = 0; y <= canvasData.graphHeight- canvasData.yStep; y += canvasData.yStep)
            {
                double yCoord;
                if (y == 0) { yCoord = canvasData.graphHeight-20; }
                else yCoord = (canvasData.graphHeight - y) - 27.5;

                int content = (int)((maxY / canvasData.amountYsteps) * step)/division;

                yaxis_labels.Add(new Label { Content = content, HorizontalContentAlignment = HorizontalAlignment.Right, Margin = new Thickness(-5-whiteSpace, yCoord, 0, 0) });

                step++;
            }
            yaxis_labels.Add(new Label { Content = "x" + division, Margin = new Thickness(-5, -25, 0, 0)});

            for (int i = 0; i < yaxis_labels.Count; i++)
            {
                string yLabelName = GetFormattedString(GraphElementTypes.Label, i);
                if (canvasElementList.Contains(yLabelName))
                {
                    if (canvasElementNames.Keys.Contains(yLabelName))
                    {
                        int elementIndex = canvasElementNames[yLabelName];
                        if (canvasData.canvas.Children.Count > elementIndex)
                        {
                            ReplaceElement(elementIndex, yaxis_labels[i]);
                        }
                        else AddElement(yLabelName, yaxis_labels[i]);
                    }
                }
                else AddElement(yLabelName, yaxis_labels[i]);
            }
        }

        public void ReplaceElement(int elementIndex, UIElement element)
        {
            canvasData.canvas.Children.RemoveAt(elementIndex);
            canvasData.canvas.Children.Insert(elementIndex, element);
        }

        public void AddElement(string elementName, UIElement element)
        {
            canvasElementList.Add(elementName);
            canvasData.canvas.Children.Add(element);
        }

        public void RedrawAxes()
        {
            RedrawAxisX();
            RedrawAxisY();
        }

        public void CreateDatasets(int array)
        {
            canvasElementList.Clear();
            canvasElementNames.Clear();
            canvasData.canvas.Children.Clear();

            RedrawAxes();

            // Create dataset
            for (int d = 0; d < canvasData.arrayDatasets[array].dataSets.Count; d++)
            {
                if (d >= canvasData.brushes.Length) { break; }

                PointCollection points = new PointCollection();
                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    double xCoord = canvasData.xmin + (i * canvasData.xStep);
                    double yCoord = canvasData.ymax - (canvasData.arrayDatasets[array].dataSets[d][i] * canvasData.arrayDatasets[array].yScale);

                    points.Add(new Point(xCoord, yCoord));
                }

                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 1;
                polyline.Stroke = canvasData.brushes[d];
                polyline.Points = points;
                if (datasetVisible.Count > d)
                {
                    if (datasetVisible[d] == false || !plotPolyline)
                    {
                        polyline.Visibility = Visibility.Hidden;
                    }
                }

                string graphName = GetFormattedString(GraphElementTypes.Graph, d);

                if (canvasElementList.Contains(graphName))
                {
                    ReplaceElement(canvasElementNames[graphName], polyline);
                }
                else AddElement(graphName, polyline);

                AddIndividualDatapoints(d, points);
            }
            
            // Calculate averages
            for (int d = 0; d < canvasData.arrayDatasets[array].dataSets.Count; d++)
            {
                if (d >= canvasData.brushesAverage.Length) { break; }

                PointCollection pointsAverage = new PointCollection();
                double sumValue = 0;
                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    sumValue += canvasData.arrayDatasets[array].dataSets[d][i];
                }

                double averageValue = sumValue / canvasData.maxValueX;
                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    double xCoord = canvasData.xmin + (i * canvasData.xStep);
                    pointsAverage.Add(new Point(xCoord, canvasData.ymax - (averageValue * canvasData.arrayDatasets[array].yScale)));
                }

                Polyline polylineAverage = new Polyline();
                polylineAverage.StrokeThickness = 2;
                polylineAverage.Stroke = canvasData.brushesAverage[d];
                polylineAverage.Points = pointsAverage;
                if (datasetVisible.Count > d)
                {
                    if (datasetVisible[d] == false)
                    {
                        polylineAverage.Visibility = Visibility.Hidden;
                    }
                }

                string averageName = GetFormattedString(GraphElementTypes.Average, d);

                if (canvasElementList.Contains(averageName))
                {
                    ReplaceElement(canvasElementNames[averageName], polylineAverage);
                }
                else AddElement(averageName, polylineAverage);
            }

            for (int i = 0; i < canvasElementList.Count; i++)
            {
                if (canvasElementNames.Keys.Contains(canvasElementList[i]))
                {
                    canvasElementNames[canvasElementList[i]] = i;
                }
                else canvasElementNames.Add(canvasElementList[i], i);
            }
        }

        public void AddIndividualDatapoints(int datasetIndex, PointCollection points)
        {
            for(int p=0; p<points.Count; p++)
            {
                Ellipse ellipse = CreateEllipse(datasetIndex, points[p].X, points[p].Y);
                if (datasetVisible.Count > datasetIndex)
                {
                    if (datasetVisible[datasetIndex] == false)
                    {
                        ellipse.Visibility = Visibility.Hidden;
                    }
                }

                string name = GetFormattedString(GraphElementTypes.Point, datasetIndex, p);

                if (canvasElementList.Contains(name))
                {
                    ReplaceElement(canvasElementNames[name], ellipse);
                }
                else AddElement(name, ellipse);
            }
        }

        public Ellipse CreateEllipse(int datasetIndex, double X, double Y)
        {
            const float width = 4;
            const float radius = width / 2;

            Ellipse ellipse = new Ellipse();
            ellipse.SetValue(Canvas.LeftProperty, X - radius);
            ellipse.SetValue(Canvas.TopProperty, Y - radius);
            ellipse.Fill = canvasData.brushes[datasetIndex];
            ellipse.Stroke = canvasData.brushes[datasetIndex];
            ellipse.StrokeThickness = 1;
            ellipse.Width = width;
            ellipse.Height = width;

            return ellipse;
        }

        public void InitArrayDatasets(int arrayAmount)
        {
            canvasData.arrayDatasets.Clear();
            for (int a=0; a< arrayAmount; a++)
            {
                canvasData.arrayDatasets.Add(new DatasetContainer());
            }
            Console.WriteLine("Initialized " + arrayAmount + " arrays");
        }

        public void InitZero()
        {            
            datasetVisible.Clear();

            InitArrayDatasets(canvasData.selectedArray + 1);
            ResetGraph();
        }

        public void AddAlgorithmsDataToGraph(int array, List<double[]> algorithmPerformances)
        {
            int algorithmCount = algorithmPerformances.Count;

            canvasData.ResetDatasets(array);
            datasetVisible.Clear();

            for (int i = 0; i < algorithmCount; i++)
            {
                //if (i > 2) { break; }
                canvasData.AddDataset(array, algorithmPerformances[i]);
                datasetVisible.Add(true);
            }

            double maxY = 0.0;
            for (int i = 0; i < canvasData.arrayDatasets[array].dataSets.Count; i++)
            {
                if (datasetVisible[i] == true)
                {
                    for (int j = 0; j < canvasData.arrayDatasets[array].dataSets[i].Length; j++)
                    {
                        double curY = canvasData.arrayDatasets[array].dataSets[i][j];

                        if (curY > maxY) { maxY = curY; }
                    }
                }
            }

            canvasData.canvas.Children.Clear();

            canvasData.SetMaxY(array, maxY);
            canvasData.Recalculate(array);

            RedrawAxes();
            CreateDatasets(array);

            //canvasData.PrintDatasetValues(array);
        }

        public void DisplayArrayData(int arrayIndex)
        {
            Console.WriteLine("Displaying data of array " + arrayIndex);
            canvasData.selectedArray = arrayIndex;
            RecalculateDataplot(arrayIndex);
        }

        // Recalculates the dataplot, note this function should not be used after modifying the dataset, but only when scaling the graph
        public void RecalculateDataplot(int array)
        {
            for (int d = 0; d < canvasData.arrayDatasets[array].dataSets.Count; d++)
            {
                if (d >= canvasData.brushes.Length) { break; }

                string graphName = GetFormattedString(GraphElementTypes.Graph, d);
                if (!canvasElementNames.ContainsKey(graphName)) { break; }
                
                PointCollection points = ((Polyline)canvasData.canvas.Children[canvasElementNames[graphName]]).Points;

                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    double xCoord = canvasData.xmin + (i * canvasData.xStep);
                    double yCoord = canvasData.ymax - (canvasData.arrayDatasets[array].dataSets[d][i] * canvasData.arrayDatasets[array].yScale);

                    Point newPoint = new Point(xCoord, yCoord);
                    
                    if (i < points.Count)
                    {
                        ((Polyline)canvasData.canvas.Children[canvasElementNames[graphName]]).Points[i] = newPoint;
                    }
                }
            }

            for (int d = 0; d < canvasData.arrayDatasets[array].dataSets.Count; d++)
            {
                if (d >= canvasData.brushes.Length) { break; }

                for (int p = 0; p < canvasData.maxValueX; p++)
                {
                    string name = GetFormattedString(GraphElementTypes.Point, d, p);
                    if (!canvasElementNames.ContainsKey(name)) { break; }
                    
                    double xCoord = canvasData.xmin + (p * canvasData.xStep);
                    double yCoord = canvasData.ymax - (canvasData.arrayDatasets[array].dataSets[d][p] * canvasData.arrayDatasets[array].yScale);

                    Ellipse pointEllipse = CreateEllipse(d, xCoord, yCoord);

                    Visibility visibility = canvasData.canvas.Children[canvasElementNames[name]].Visibility;

                    ReplaceElement(canvasElementNames[name], pointEllipse);
                    canvasData.canvas.Children[canvasElementNames[name]].Visibility = visibility;
                }
            }

            for (int d = 0; d < canvasData.arrayDatasets[array].dataSets.Count; d++)
            {
                if (d >= canvasData.brushesAverage.Length) { break; }

                string averageName = GetFormattedString(GraphElementTypes.Average, d);
                if (!canvasElementNames.ContainsKey(averageName)) { break; }

                PointCollection pointsAverage = ((Polyline)canvasData.canvas.Children[canvasElementNames[averageName]]).Points;

                double sumValue = 0;
                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    sumValue += canvasData.arrayDatasets[array].dataSets[d][i];
                }

                double averageValue = sumValue / canvasData.maxValueX;
                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    double xCoord = canvasData.xmin + (i * canvasData.xStep);
                    double yCoord = canvasData.ymax - (averageValue * canvasData.arrayDatasets[array].yScale);

                    Point newPoint = new Point(xCoord, yCoord);

                    if (i < pointsAverage.Count)
                    {
                        ((Polyline)canvasData.canvas.Children[canvasElementNames[averageName]]).Points[i] = newPoint;
                    }
                }
            }
        }

        public void ToggleVisible(int array, int dataSetIndex, bool visible)
        {
            string graphName = GetFormattedString(GraphElementTypes.Graph, dataSetIndex);
            string averageName = GetFormattedString(GraphElementTypes.Average, dataSetIndex);
            Visibility visibility = Visibility.Visible;

            if (dataSetIndex >= datasetVisible.Count) { return; }
            datasetVisible[dataSetIndex] = visible;

            if (!visible) { visibility = Visibility.Hidden; }

            // Polylines
            if (canvasElementNames.Keys.Contains(graphName) && plotPolyline)
            {
                canvasData.canvas.Children[canvasElementNames[graphName]].Visibility = visibility;
            }

            // Average lines
            if (canvasElementNames.Keys.Contains(averageName))
            {
                canvasData.canvas.Children[canvasElementNames[averageName]].Visibility = visibility;
            }

            // Points            
            for (int p = 0; p < canvasData.arrayDatasets[array].dataSets[dataSetIndex].Length; p++)
            {
                string ellipseName = GetFormattedString(GraphElementTypes.Point, dataSetIndex, p);

                if (canvasElementNames.Keys.Contains(ellipseName))
                {
                    canvasData.canvas.Children[canvasElementNames[ellipseName]].Visibility = visibility;
                }
            }

            if (autoResize) { RescaleCanvas(array); }
        }

        public void ToggleAutoResize(int array, bool resize)
        {
            autoResize = resize;
            if (autoResize) { RescaleCanvas(array); }
        }

        public void PlotPolyline(int array, bool plot)
        {
            plotPolyline = plot;
            Visibility visibility = Visibility.Visible;

            if(plot == false)
            {
                visibility = Visibility.Hidden;
            }

            for (int d = 0; d < canvasData.arrayDatasets[array].dataSets.Count; d++)
            {
                if (datasetVisible[d])
                {
                    string graphName = GetFormattedString(GraphElementTypes.Graph, d);

                    if (canvasElementList.Contains(graphName))
                    {
                        ((Polyline)canvasData.canvas.Children[canvasElementNames[graphName]]).Visibility = visibility;
                    }
                }
            }
        }

        public void ResetGraph()
        {
            canvasData.canvas.Children.Clear();
            for (int i=0; i< canvasData.arrayDatasets.Count; i++)
            {
                canvasData.ResetDatasets(i);
                canvasData.SetMaxY(i, 10.0);
                canvasData.Recalculate(i);
            }
            RedrawAxes();
        }

        public void RescaleCanvas(int array)
        {
            double maxY = 0.0;

            if(canvasData.arrayDatasets[array].dataSets.Count < 1) { return; }

            for (int i = 0; i < canvasData.arrayDatasets[array].dataSets.Count; i++)
            {
                if (datasetVisible[i])
                {
                    for (int j = 0; j < canvasData.arrayDatasets[array].dataSets[i].Length; j++)
                    {
                        double curY = canvasData.arrayDatasets[array].dataSets[i][j];

                        if (curY > maxY)
                        {
                            maxY = curY;
                        }
                    }
                }
            }

            if (maxY != 0)
            {
                // Set max Y value
                canvasData.SetMaxY(array, maxY);
                canvasData.Recalculate(array);
                RedrawAxisY();

                //Print everything to check if they are in sync
                //for (int i = 0; i < canvasData.canvas.Children.Count; i++)
                //{
                //    Console.WriteLine(i + ") " + canvasData.canvas.Children[i].GetType());
                //    if (canvasElementList.Count > i)
                //    {
                //        string n = canvasElementList[i];
                //        Console.WriteLine("canvasElementList[" + i + "] = " + n);
                //        if (canvasElementNames.Keys.Contains(n))
                //        {
                //            Console.WriteLine("canvasElementNames[" + n + "] = " + canvasElementNames[n]);
                //        }
                //        else Console.WriteLine("canvasElementNames.Keys.Contains(" + n + ") returned false");
                //    }
                //    else Console.WriteLine("canvasElementList.Count < " + i);
                //    Console.WriteLine("");
                //}

                // Recalculate plot values
                RecalculateDataplot(array);
            }
        }

    }
}
