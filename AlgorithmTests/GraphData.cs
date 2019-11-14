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
        public List<string> canvasElementList { get; private set; } = new List<string>();
        public Dictionary<string, int> canvasElementNames { get; private set; } = new Dictionary<string, int>();

        public CanvasData canvasData;

        public const string elementName_Xaxis = "X-Axis";
        public const string elementName_Yaxis = "Y-Axis";
        public const string elementName_Graph = "Graph ";
        public const string elementName_Average = "Average ";
        public const string elementName_Point = "Point ";

        public bool autoResize = true;
        public bool plotPolyline = true;
        public List<bool> dataSetVisible { get; private set; } = new List<bool>();

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
                    maxValueX = 5;
                }
                else maxValueX = dataSets[0].Length;

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

            public void AddDataset(double[] set)
            {
                dataSets.Add(set);
            }

            public void ResetDatasets()
            {
                dataSets.Clear();
            }

            public void PrintDatasetValues()
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
        
        public void DrawCustomGraph()
        {
            canvasData.Recalculate();

            RedrawAxes();
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
                if (canvasElementNames.Keys.Contains(elementName_Xaxis))
                {
                    int elementIndex = canvasElementNames[elementName_Xaxis];
                    if (canvasData.canvas.Children.Count > elementIndex)
                    {
                        canvasData.canvas.Children.RemoveAt(elementIndex);
                        canvasData.canvas.Children.Insert(elementIndex, xaxis_path);
                    }
                    else
                    {
                        canvasData.canvas.Children.Add(xaxis_path);
                        canvasElementList.Add(elementName_Xaxis);
                    }
                }
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
                if (canvasElementNames.Keys.Contains(elementName_Yaxis)) {
                    int elementIndex = canvasElementNames[elementName_Yaxis];
                    if(canvasData.canvas.Children.Count> elementIndex)
                    {
                        canvasData.canvas.Children.RemoveAt(elementIndex);
                        canvasData.canvas.Children.Insert(elementIndex, yaxis_path);
                    }
                    else
                    {
                        canvasData.canvas.Children.Add(yaxis_path);
                        canvasElementList.Add(elementName_Yaxis);
                    }
                }
            }
            else
            {
                canvasData.canvas.Children.Add(yaxis_path);
                canvasElementList.Add(elementName_Yaxis);
            }
        }

        public void RedrawAxes()
        {
            RedrawAxisX();
            RedrawAxisY();
        }

        public void CreateDatasets()
        {
            canvasElementList.Clear();
            canvasElementNames.Clear();
            canvasData.canvas.Children.Clear();

            RedrawAxes();

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
                if (dataSetVisible[d] == false || !plotPolyline)
                {
                    polyline.Visibility = Visibility.Hidden;
                }

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

                AddIndividualDataPoints(d, points);
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
                if (dataSetVisible[d] == false)
                {
                    polylineAverage.Visibility = Visibility.Hidden;
                }

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

            for (int i = 0; i < canvasElementList.Count; i++)
            {
                if (canvasElementNames.Keys.Contains(canvasElementList[i]))
                {
                    canvasElementNames[canvasElementList[i]] = i;
                }
                else canvasElementNames.Add(canvasElementList[i], i);
            }
        }

        public void AddIndividualDataPoints(int datasetIndex, PointCollection points)
        {
            for(int p=0; p<points.Count; p++)
            {
                Ellipse ellipse = CreateEllipse(datasetIndex, points[p].X, points[p].Y);
                if (dataSetVisible[datasetIndex] == false)
                {
                    ellipse.Visibility = Visibility.Hidden;
                }

                string name = elementName_Point + datasetIndex + " " + p;

                if (canvasElementList.Contains(name))
                {
                    int elementIndex = canvasElementNames[name];
                    canvasData.canvas.Children.RemoveAt(elementIndex);
                    canvasData.canvas.Children.Insert(elementIndex, ellipse);
                }
                else
                {
                    canvasData.canvas.Children.Add(ellipse);
                    canvasElementList.Add(name);
                }
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

        public void AddAlgorithmsDataToGraph(List<double[]> algorithmPerformances)
        {
            int algorithmCount = ArrayCompare.algorithmNames.Count;

            canvasData.ResetDatasets();

            for (int i = 0; i < algorithmCount; i++)
            {
                if (i > 2) { break; }
                canvasData.AddDataset(algorithmPerformances[i]);
                dataSetVisible.Add(true);
            }

            double maxY = 0.0;
            for (int i = 0; i < canvasData.dataSets.Count; i++)
            {
                if (dataSetVisible[i] == true)
                {
                    for (int j = 0; j < canvasData.dataSets[i].Length; j++)
                    {
                        double curY = canvasData.dataSets[i][j];
                        //Console.WriteLine("Calculated value from graphPoints[" + j + "] of dataset " + i + " = " + curY);
                        if (curY > maxY)
                        {
                            maxY = curY;
                        }
                    }
                }
            }

            canvasData.canvas.Children.Clear();

            canvasData.SetMaxY(maxY);
            canvasData.Recalculate();

            RedrawAxes();
            CreateDatasets();

            canvasData.PrintDatasetValues();
        }

        // Recalculates the dataplot, note this function should not be used after modifying the dataset, but only when scaling the graph
        public void RecalculateDataplot()
        {
            for (int d = 0; d < canvasData.dataSets.Count; d++)
            {
                if (d >= canvasData.brushes.Length) { break; }
                if(!canvasElementNames.ContainsKey(elementName_Graph + d)) { break; }

                //Console.WriteLine("canvasElementNames[" + elementName_Graph + " + " + d + "] = " + canvasElementNames[elementName_Graph + d]);
                PointCollection points = ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Graph + d]]).Points;

                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    double xCoord = canvasData.xmin + (i * canvasData.xStep);
                    double yCoord = canvasData.ymax - (canvasData.dataSets[d][i] * canvasData.yScale);
                    Console.WriteLine("New point " + d + " + " + i + " coords: (" + xCoord + " , " + yCoord + ")");

                    Point newPoint = new Point(xCoord, yCoord);

                    if (i < points.Count)//if (i < points.Count - 1)
                    {
                        ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Graph + d]]).Points[i] = newPoint;
                        //Console.WriteLine("Modified " + (elementName_Graph + d) + " point " + i + " (element " + canvasElementNames[elementName_Graph + d] + ")");
                    }
                }
            }

            for (int d = 0; d < canvasData.dataSets.Count; d++)
            {
                if (d >= canvasData.brushes.Length) { break; }

                for (int p = 0; p < canvasData.maxValueX; p++)
                {
                    string name = elementName_Point + d + " " + p;
                    if (!canvasElementNames.ContainsKey(name)) { break; }
                    
                    double xCoord = canvasData.xmin + (p * canvasData.xStep);
                    double yCoord = canvasData.ymax - (canvasData.dataSets[d][p] * canvasData.yScale);
                    Console.WriteLine("New ellipse " + d + " + " + p + " coords: (" + xCoord + " , " + yCoord + ")");

                    Ellipse pointEllipse = CreateEllipse(d, xCoord, yCoord);

                    Visibility visibility = canvasData.canvas.Children[canvasElementNames[name]].Visibility;

                    canvasData.canvas.Children.RemoveAt(canvasElementNames[name]);
                    canvasData.canvas.Children.Insert(canvasElementNames[name], pointEllipse);
                    canvasData.canvas.Children[canvasElementNames[name]].Visibility = visibility;
                }
            }

            for (int d = 0; d < canvasData.dataSets.Count; d++)
            {
                if (d >= canvasData.brushesAverage.Length) { break; }
                if (!canvasElementNames.ContainsKey(elementName_Average + d)) { break; }

                PointCollection pointsAverage = ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Average + d]]).Points;

                double sumValue = 0;
                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    sumValue += canvasData.dataSets[d][i];
                }

                double averageValue = sumValue / canvasData.maxValueX;
                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    double xCoord = canvasData.xmin + (i * canvasData.xStep);
                    double yCoord = canvasData.ymax - (averageValue * canvasData.yScale);
                    //Console.WriteLine("New average point " + d + " + " + i + " coords: (" + xCoord + " , " + yCoord + ")");

                    Point newPoint = new Point(xCoord, yCoord);

                    if (i < pointsAverage.Count)//if (i < pointsAverage.Count - 1)
                    {
                        ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Average + d]]).Points[i] = newPoint;
                    }
                }
            }
        }

        public void ToggleVisible(int dataSetIndex, bool visible)
        {
            string graphName = elementName_Graph + dataSetIndex;
            string averageName = elementName_Average + dataSetIndex;
            Visibility visibility = Visibility.Visible;

            if (dataSetIndex >= dataSetVisible.Count) { return; }
            dataSetVisible[dataSetIndex] = visible;

            if (!visible)
            {
                visibility = Visibility.Hidden;
            }

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
            for (int p = 0; p < canvasData.dataSets[dataSetIndex].Length; p++)
            {
                string ellipseName = elementName_Point + dataSetIndex + " " + p;

                if (canvasElementNames.Keys.Contains(ellipseName))
                {
                    Console.WriteLine("canvasElementNames.Keys.Contains(" + ellipseName + ") returned true");
                    canvasData.canvas.Children[canvasElementNames[ellipseName]].Visibility = visibility;
                    Console.WriteLine("Child " + canvasElementNames[ellipseName] + " visibility = " + canvasData.canvas.Children[canvasElementNames[ellipseName]].Visibility);
                }
                else Console.WriteLine("canvasElementNames.Keys.Contains("+ellipseName+") returned false");
            }

            if (autoResize)
            {
                RescaleCanvas();
            }
        }

        public void ToggleAutoResize(bool resize)
        {
            autoResize = resize;
            if (autoResize)
            {
                RescaleCanvas();
            }
        }

        public void PlotPolyline(bool plot)
        {
            plotPolyline = plot;
            Visibility visibility = Visibility.Visible;

            if(plot == false)
            {
                visibility = Visibility.Hidden;
            }

            for (int d = 0; d < canvasData.dataSets.Count; d++)
            {
                if (dataSetVisible[d])
                {
                    if (canvasElementList.Contains(elementName_Graph + d))
                    {
                        ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Graph + d]]).Visibility = visibility;
                    }
                }
            }
        }

        public void RescaleCanvas()
        {
            double maxY = 0.0;

            if(canvasData.dataSets.Count < 1) { return; }

            for (int i = 0; i < canvasData.dataSets.Count; i++)
            {
                if (dataSetVisible[i])
                {
                    for (int j = 0; j < canvasData.dataSets[i].Length; j++)
                    {
                        double curY = canvasData.dataSets[i][j];
                        //Console.WriteLine("Calculated value from graphPoints[" + j + "] of dataset " + i + " = " + curY);
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
                canvasData.SetMaxY(maxY);
                canvasData.Recalculate();
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
                RecalculateDataplot();
            }
        }

    }
}
