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
            string xString = typeof(GraphData).GetField("elementName_Xaxis").GetValue(null).ToString();

            graphData.RedrawAxisX();

            Assert.IsTrue(graphData.canvasElementList.Contains(xString));
            Assert.AreEqual(1, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void RedrawAxisX_XaxisAlreadyExists_DoesntAddNewXaxisToElementList()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            string xString = typeof(GraphData).GetField("elementName_Xaxis").GetValue(null).ToString();
            graphData.RedrawAxisX();

            graphData.RedrawAxisX();

            Assert.IsTrue(graphData.canvasElementList.Contains(xString));
            Assert.AreEqual(1, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void RedrawAxisY_YaxisDoesntExist_DoesAddNewYaxisToElementList()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            string yString = typeof(GraphData).GetField("elementName_Yaxis").GetValue(null).ToString();

            graphData.RedrawAxisY();

            Assert.IsTrue(graphData.canvasElementList.Contains(yString));
            Assert.AreEqual(1, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void RedrawAxisY_YaxisAlreadyExists_DoesntAddNewYaxisToElementList()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            string yString = typeof(GraphData).GetField("elementName_Yaxis").GetValue(null).ToString();
            graphData.RedrawAxisY();

            graphData.RedrawAxisY();

            Assert.IsTrue(graphData.canvasElementList.Contains(yString));
            Assert.AreEqual(1, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void RedrawAxes_AxesDontExist_DoesAddNewAxesToElementList()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            string xString = typeof(GraphData).GetField("elementName_Xaxis").GetValue(null).ToString();
            string yString = typeof(GraphData).GetField("elementName_Yaxis").GetValue(null).ToString();

            graphData.RedrawAxes();

            Assert.IsTrue(graphData.canvasElementList.Contains(xString));
            Assert.IsTrue(graphData.canvasElementList.Contains(yString));
            Assert.AreEqual(2, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void RedrawAxes_AxesAlreadyExist_DoesntAddNewAxesToElementList()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            string xString = typeof(GraphData).GetField("elementName_Xaxis").GetValue(null).ToString();
            string yString = typeof(GraphData).GetField("elementName_Yaxis").GetValue(null).ToString();
            graphData.RedrawAxes();

            graphData.RedrawAxes();

            Assert.IsTrue(graphData.canvasElementList.Contains(xString));
            Assert.IsTrue(graphData.canvasElementList.Contains(yString));
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

        // Test if CreateDatasets creates one averageline for one dataset
        // Test if CreateDatasets creates one datapoint for one dataset with one point
        // Test if CreateDatasets creates two datapoint for one dataset with two points
        // Test if CreateDatasets results in a canvasElementList List and canvasElementNames Dictionary of the same size
    }
}
