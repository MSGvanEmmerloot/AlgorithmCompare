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

        // Test if CreateDatasets results in a canvasElementList List and canvasElementNames Dictionary of the same size
    }
}
