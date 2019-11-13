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
        const string elementName_Point = "Point ";

        public bool autoResize = true;
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
        
        public void DrawCustomGraph()
        {
            //dataSetVisible.Add(true);
            //dataSetVisible.Add(true);
            //dataSetVisible.Add(true);

            canvasData.Recalculate();

            RedrawAxes();
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

            if (canvasElementList.Contains(elementName_Xaxis) && canvasElementNames.Keys.Contains(elementName_Xaxis))
            {
                int elementIndex = canvasElementNames[elementName_Xaxis];
                if (canvasData.canvas.Children.Count > elementIndex)
                {
                    canvasData.canvas.Children.RemoveAt(elementIndex);
                    canvasData.canvas.Children.Insert(elementIndex, xaxis_path);
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

            if (canvasElementList.Contains(elementName_Yaxis) && canvasElementNames.Keys.Contains(elementName_Yaxis))
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
            canvasElementList.Clear();
            canvasElementNames.Clear();

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

            //for(int i=0; i< canvasElementList.Count; i++)
            //{
            //    Console.WriteLine("element " + i + ": " + canvasElementList[i]);
            //}
        }

        public void AddIndividualDataPoints(int datasetIndex, PointCollection points)
        {
            for(int p=0; p<points.Count; p++)
            {
                Ellipse ellipse = CreateEllipse(datasetIndex, points[p].X, points[p].Y);

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

            canvasData.ResetDataSets();

            for (int i = 0; i < algorithmCount; i++)
            {
                if (i > 2) { break; }
                canvasData.AddDataSet(algorithmPerformances[i]);
                dataSetVisible.Add(true);
            }

            double maxY = 0.0;
            for (int i = 0; i < canvasData.dataSets.Count; i++)
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

            canvasData.canvas.Children.Clear();

            canvasData.SetMaxY(maxY);
            canvasData.Recalculate();

            RedrawAxes();
            CreateDataSets();

            canvasData.PrintDataSetValues();
        }

        // Recalculates the dataplot, note this function should not be used after modifying the dataset, but only when scaling the graph
        public void RecalculateDataplot()
        {
            for (int d = 0; d < canvasData.dataSets.Count; d++)
            {
                if (d >= canvasData.brushes.Length) { break; }
                if(!canvasElementNames.ContainsKey(elementName_Graph + d)) { break; }

                PointCollection points = ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Graph + d]]).Points;
                //((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Graph + d]]).Points.Clear();
                for (int i = 0; i < canvasData.maxValueX; i++)
                {
                    double xCoord = canvasData.xmin + (i * canvasData.xStep);
                    Point newPoint = new Point(xCoord, canvasData.ymax - (canvasData.dataSets[d][i] * canvasData.yScale));

                    if (i < points.Count - 1)
                    {
                        ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Graph + d]]).Points[i] = newPoint;
                        //Console.WriteLine("Modified " + (elementName_Graph + d) + " point " + i + " (element " + canvasElementNames[elementName_Graph + d] + ")");
                    }
                    //else
                    //{
                    //    ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Graph + d]]).Points.Add(newPoint);
                    //    Console.WriteLine("Added " + (elementName_Graph + d) +  " point " + i + " (element " + canvasElementNames[elementName_Graph + d] + ")");
                    //}
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

                    Ellipse pointEllipse = CreateEllipse(d, xCoord, (canvasData.ymax - (canvasData.dataSets[d][p] * canvasData.yScale)));

                    canvasData.canvas.Children.RemoveAt(canvasElementNames[name]);
                    canvasData.canvas.Children.Insert(canvasElementNames[name], pointEllipse);

                    //Console.WriteLine("Replaced " + name + " (element " + canvasElementNames[name] + ")");                    
                }

                //for (int p = 0; p < canvasData.maxValueX; p++)
                //{
                //    string name = elementName_Point + d + " " + p;
                //    if (!canvasElementNames.ContainsKey(name))
                //    {
                //        double xCoord = canvasData.xmin + (p * canvasData.xStep);

                //        Ellipse pointEllipse = CreateEllipse(d, xCoord, (canvasData.ymax - (canvasData.dataSets[d][p] * canvasData.yScale)));

                //        canvasData.canvas.Children.Add(pointEllipse);
                //        canvasElementList.Add(name);

                //        Console.WriteLine("Added " + name + " (element " + (canvasElementList.Count-1) + ")");
                //    }
                //    else
                //    {
                //        double xCoord = canvasData.xmin + (p * canvasData.xStep);

                //        Ellipse pointEllipse = CreateEllipse(d, xCoord, (canvasData.ymax - (canvasData.dataSets[d][p] * canvasData.yScale)));

                //        canvasData.canvas.Children.RemoveAt(canvasElementNames[name]);
                //        canvasData.canvas.Children.Insert(canvasElementNames[name], pointEllipse);

                //        Console.WriteLine("Replaced " + name + " (element " + canvasElementNames[name] + ")");
                //    }
                //}
            }

            for (int d = 0; d < canvasData.dataSets.Count; d++)
            {
                if (d >= canvasData.brushesAverage.Length) { break; }
                if (!canvasElementNames.ContainsKey(elementName_Average + d)) { break; }

                PointCollection pointsAverage = ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Average + d]]).Points;
                //((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Average + d]]).Points.Clear();
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
                    //else ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Average + d]]).Points.Add(newPoint);
                }
            }

            //foreach(string s in canvasElementNames.Keys)
            //{
            //    Console.WriteLine("Key: " + s);
            //}

            //canvasElementNames.Clear();
            //for (int i = 0; i < canvasElementList.Count; i++)
            //{
            //    int index = canvasElementNames.Count;
            //    canvasElementNames.Add(canvasElementList[i], index);
            //    Console.WriteLine("Added " + canvasElementList[i] + " with index " + index + " (element " + i + ")");
            //}
        }

        public void ToggleVisible(int dataSetIndex, bool visible)
        {
            string graphName = elementName_Graph + dataSetIndex;
            string averageName = elementName_Average + dataSetIndex;

            if (dataSetIndex >= dataSetVisible.Count) { return; }
            dataSetVisible[dataSetIndex] = visible;

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

        public void RescaleCanvas()
        {
            double maxY;
            double minY = canvasData.ymax;
            PointCollection graphPoints;

            if(canvasData.dataSets.Count < 1) { return; }

            // Calculate smallest value of the datasets with visible graphs (-> closest to 0, which is at the top of the canvas!)
            for (int d=0; d < canvasData.dataSets[0].Length; d++)
            {
                if (dataSetVisible[d])
                {
                    graphPoints = ((Polyline)canvasData.canvas.Children[canvasElementNames[elementName_Graph + d]]).Points;
                    for (int i = 0; i < graphPoints.Count; i++)
                    {
                        double curY = graphPoints[i].Y;
                        //Console.WriteLine("Calculated value from graphPoints[" + i + "] of dataset " + d + " = " + curY);
                        if (curY < minY)
                        {
                            minY = (int)curY;
                        }
                    }
                }
            }
            
            //Console.WriteLine("Calculated minY = " + minY);
            maxY = (minY - canvasData.ymax) / (-canvasData.yScale);
            //Console.WriteLine("Calculated maxY = " + maxY);

            if (maxY != 0)
            {
                // Set max Y value
                canvasData.SetMaxY(maxY);
                canvasData.Recalculate();
                RedrawAxisY();

                // Recalculate plot values
                RecalculateDataplot();
            }
        }

    }
}
