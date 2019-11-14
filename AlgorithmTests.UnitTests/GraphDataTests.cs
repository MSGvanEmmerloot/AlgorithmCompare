using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AlgorithmTests.UnitTests
{
    [TestClass]
    public class GraphDataTests
    {
        [TestMethod]
        public void GraphData_CreatesCanvas()
        {
            Canvas canvas = new Canvas();

            GraphData graphData = new GraphData(canvas);
            bool hasCanvas = (graphData.canvasData.canvas != null);

            Assert.IsTrue(hasCanvas);
        }

        [TestMethod]
        public void RedrawAxisX_XaxisDoesntExist_DoesAddNewXaxisToElementList()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);

            graphData.RedrawAxisX();

            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Xaxis)));
            Assert.AreEqual(1, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void RedrawAxisX_XaxisAlreadyExists_DoesntAddNewXaxisToElementList()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            graphData.RedrawAxisX();

            graphData.RedrawAxisX();

            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Xaxis)));
            Assert.AreEqual(1, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void RedrawAxisY_YaxisDoesntExist_DoesAddNewYaxisToElementList()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);

            graphData.RedrawAxisY();

            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Yaxis)));
            Assert.AreEqual(1, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void RedrawAxisY_YaxisAlreadyExists_DoesntAddNewYaxisToElementList()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            graphData.RedrawAxisY();

            graphData.RedrawAxisY();

            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Yaxis)));
            Assert.AreEqual(1, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void RedrawAxes_AxesDontExist_DoesAddNewAxesToElementList()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);

            graphData.RedrawAxes();

            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Xaxis)));
            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Yaxis)));
            Assert.AreEqual(2, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void RedrawAxes_AxesAlreadyExist_DoesntAddNewAxesToElementList()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            graphData.RedrawAxes();

            graphData.RedrawAxes();

            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Xaxis)));
            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Yaxis)));
            Assert.AreEqual(2, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void CreateDatasets_NoDatasetsFound_DoesntReturnError()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);

            graphData.CreateDatasets();

            //If there is no error, this test is a success
        }

        [TestMethod]
        public void CreateDatasets_OneDatasetFound_PlotsOneAverageLine()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);      
            graphData.canvasData.AddDataset(new double[] { 0.0, 1.0, 2.0 });
            graphData.canvasData.Recalculate();

            graphData.CreateDatasets();

            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Average, 0)));
        }

        [TestMethod]
        public void CreateDatasets_OneDatasetFound_SyncsListAndDictionary()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            graphData.canvasData.AddDataset(new double[] { 0.0, 1.0, 2.0 });
            graphData.canvasData.Recalculate();

            graphData.CreateDatasets();

            Assert.AreEqual(graphData.canvasElementList.Count, graphData.canvasElementNames.Count);
        }

        [TestMethod]
        public void CreateDatasets_OneDatasetOneElement_PlotsOnePoint()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            graphData.canvasData.AddDataset(new double[] { 0.0 });
            graphData.canvasData.Recalculate();

            graphData.CreateDatasets();

            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Point, 0, 0)));
            // X-axis, Y-axis, one polyline for the dataset, one polyline for the average, one datapoint
            Assert.AreEqual(5, graphData.canvasElementList.Count); 
        }

        [TestMethod]
        public void CreateDatasets_TwoDatasetsTwoElements_PlotsFourPoints()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            graphData.canvasData.AddDataset(new double[] { 0.0 , 1.0});
            graphData.canvasData.AddDataset(new double[] { 0.0 , 1.0 });
            graphData.canvasData.Recalculate();

            graphData.CreateDatasets();

            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Point, 0, 0)));
            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Point, 0, 1)));
            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Point, 1, 0)));
            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Point, 1, 1)));

            // X-axis, Y-axis, two polylines for the dataset, two polylines for the averages, four datapoint
            Assert.AreEqual(10, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void AddIndividualDatapoints_ValidDatasetAndTwoPoints_AddsTwoDatapoints()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            PointCollection points = new PointCollection();
            for (int i = 0; i < 2; i++) { points.Add(new Point(i, 0)); }

            graphData.AddIndividualDatapoints(0, points);

            Assert.AreEqual(2, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void AddAlgorithmsDataToGraph_TwoMeasurementsTwoAlgorithms_AddsTwoDatasets()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            List<double[]> performances = new List<double[]>();
            performances.Add(new double[] { 1, 2 });
            performances.Add(new double[] { 3, 4 });

            graphData.AddAlgorithmsDataToGraph(performances);

            Assert.AreEqual(2, graphData.canvasData.dataSets.Count);
            Assert.AreEqual(2, graphData.canvasData.dataSets[0].Length);
            Assert.AreEqual(2, graphData.canvasData.dataSets[1].Length);
        }

        [TestMethod]
        public void RecalculateDataplot_OneDataset_UpdatesAllValues()
        {
            Canvas canvas = new Canvas {Width = 100.0, Height = 100.0};
            GraphData graphData = new GraphData(canvas);
            graphData.canvasData.AddDataset(new double[] { 10.0 , 20.0, 30.0 });
            graphData.canvasData.Recalculate();
            graphData.CreateDatasets();
            PointCollection oldPoints = ((Polyline)graphData.canvasData.canvas.Children[graphData.canvasElementNames[graphData.GetFormattedString(GraphData.GraphElementTypes.Graph, 0)]]).Points;
            List<double> oldVals = new List<double>();
            for (int i = 0; i < oldPoints.Count; i++) { oldVals.Add(oldPoints[i].Y); }
            graphData.canvasData.ResetDatasets();
            graphData.canvasData.AddDataset(new double[] { 20.0, 10.0, 30.0 });

            graphData.RecalculateDataplot();
            PointCollection newPoints = ((Polyline)graphData.canvasData.canvas.Children[graphData.canvasElementNames[graphData.GetFormattedString(GraphData.GraphElementTypes.Graph, 0)]]).Points;
            List<double> newVals = new List<double>();
            for (int i=0; i<newPoints.Count; i++){ newVals.Add(newPoints[i].Y); }

            // The y-axis is in the top left of the canvas, so closer to the "normal" x-axis actually means a bigger y-value
            Assert.IsTrue(newVals[0] < oldVals[0]);
            Assert.IsTrue(newVals[1] > oldVals[1]);
            Assert.IsTrue(newVals[2] == oldVals[2]);
        }
    }
}
