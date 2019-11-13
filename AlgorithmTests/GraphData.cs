using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace AlgorithmTests
{
    public class GraphData
    {
        Canvas graphCanvas;

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

        public GraphData(Canvas _canvas, double _graphWidth, double _graphHeight, double _margin, int _amountOfMeasurements, double _maxYValue)
        {
            graphCanvas = _canvas;
            graphWidth = _graphWidth;
            graphHeight = _graphHeight;
            margin = _margin;
            //maxValueX = _amountOfMeasurements;
            maxValueY = _maxYValue;

            //Recalculate();
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
}
