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
        public void GraphData_CreatesAxes()
        {
            Canvas canvas = new Canvas();

            GraphData graphData = new GraphData(canvas);

            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Xaxis)));
            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Yaxis)));
            //Axes and Label 0
            Assert.AreEqual(3, graphData.canvasElementList.Count);
        }

        //[TestMethod]
        //public void RedrawAxisX_XaxisDoesntExist_DoesAddNewXaxisToElementList()
        //{
        //    Canvas canvas = new Canvas();
        //    GraphData graphData = new GraphData(canvas);

        //    graphData.RedrawAxisX();

        //    Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Xaxis)));
        //    //Axes and Label 0
        //    Assert.AreEqual(3, graphData.canvasElementList.Count);
        //}

        [TestMethod]
        public void RedrawAxisX_XaxisAlreadyExists_DoesntAddNewXaxisToElementList()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);

            graphData.RedrawAxisX();
            for (int i = 0; i < graphData.canvasElementList.Count; i++)
            {
                Console.WriteLine(graphData.canvasElementList[i]);
            }
            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Xaxis)));
            //Axes and Label 0
            Assert.AreEqual(3, graphData.canvasElementList.Count);
        }

        //[TestMethod]
        //public void RedrawAxisY_YaxisDoesntExist_DoesAddNewYaxisToElementList()
        //{
        //    Canvas canvas = new Canvas();
        //    GraphData graphData = new GraphData(canvas);

        //    graphData.RedrawAxisY();

        //    Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Yaxis)));
        //    //Axes and Label 0
        //    Assert.AreEqual(3, graphData.canvasElementList.Count);
        //}

        [TestMethod]
        public void RedrawAxisY_YaxisAlreadyExists_DoesntAddNewYaxisToElementList()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            graphData.RedrawAxisY();

            graphData.RedrawAxisY();

            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Yaxis)));
            //Axes and Label 0
            Assert.AreEqual(3, graphData.canvasElementList.Count);
        }

        //[TestMethod]
        //public void RedrawAxes_AxesDontExist_DoesAddNewAxesToElementList()
        //{
        //    Canvas canvas = new Canvas();
        //    GraphData graphData = new GraphData(canvas);

        //    graphData.RedrawAxes();

        //    Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Xaxis)));
        //    Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Yaxis)));
        //    //Axes and Label 0
        //    Assert.AreEqual(3, graphData.canvasElementList.Count);
        //}

        [TestMethod]
        public void RedrawAxes_AxesAlreadyExist_DoesntAddNewAxesToElementList()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            graphData.RedrawAxes();

            graphData.RedrawAxes();

            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Xaxis)));
            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Yaxis)));
            //Axes and Label 0
            Assert.AreEqual(3, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void CreateDatasets_NoDatasetsFound_DoesntReturnError()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);

            graphData.CreateDatasets(0);

            //If there is no error, this test is a success
        }

        [TestMethod]
        public void CreateDatasets_OneDatasetFound_PlotsOneAverageLine()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);      
            graphData.canvasData.AddDataset(0, new double[] { 0.0, 1.0, 2.0 });
            graphData.canvasData.Recalculate(0);

            graphData.CreateDatasets(0);

            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Average, 0)));
        }

        [TestMethod]
        public void CreateDatasets_OneDatasetFound_SyncsListAndDictionary()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            graphData.canvasData.AddDataset(0, new double[] { 0.0, 1.0, 2.0 });
            graphData.canvasData.Recalculate(0);

            graphData.CreateDatasets(0);

            Assert.AreEqual(graphData.canvasElementList.Count, graphData.canvasElementNames.Count);
        }

        [TestMethod]
        public void CreateDatasets_OneDatasetOneElement_PlotsOnePoint()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            graphData.canvasData.AddDataset(0, new double[] { 0.0 });
            graphData.canvasData.Recalculate(0);

            graphData.CreateDatasets(0);

            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Point, 0, 0)));
            // X-axis, Y-axis, Label 0, one polyline for the dataset, one polyline for the average, one datapoint            
            Assert.AreEqual(6, graphData.canvasElementList.Count); 
        }

        [TestMethod]
        public void CreateDatasets_TwoDatasetsTwoElements_PlotsFourPoints()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            graphData.canvasData.AddDataset(0, new double[] { 0.0 , 1.0});
            graphData.canvasData.AddDataset(0, new double[] { 0.0 , 1.0 });
            graphData.canvasData.Recalculate(0);

            graphData.CreateDatasets(0);

            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Point, 0, 0)));
            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Point, 0, 1)));
            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Point, 1, 0)));
            Assert.IsTrue(graphData.canvasElementList.Contains(graphData.GetFormattedString(GraphData.GraphElementTypes.Point, 1, 1)));

            // X-axis, Y-axis, Label 0, two polylines for the dataset, two polylines for the averages, four datapoint
            Assert.AreEqual(11, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void AddIndividualDatapoints_ValidDatasetAndTwoPoints_AddsTwoDatapoints()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            PointCollection points = new PointCollection();
            for (int i = 0; i < 2; i++) { points.Add(new Point(i, 0)); }

            graphData.AddIndividualDatapoints(0, points);

            // X-axis, Y-axis, one polyline for the dataset, one polyline for the average, one datapoint
            Assert.AreEqual(5, graphData.canvasElementList.Count);
        }

        [TestMethod]
        public void AddAlgorithmsDataToGraph_TwoMeasurementsTwoAlgorithms_AddsTwoDatasets()
        {
            Canvas canvas = new Canvas();
            GraphData graphData = new GraphData(canvas);
            List<double[]> performances = new List<double[]>();
            performances.Add(new double[] { 1, 2 });
            performances.Add(new double[] { 3, 4 });

            graphData.AddAlgorithmsDataToGraph(0, performances);

            Assert.AreEqual(2, graphData.canvasData.arrayDatasets[0].dataSets.Count);
            Assert.AreEqual(2, graphData.canvasData.arrayDatasets[0].dataSets[0].Length);
            Assert.AreEqual(2, graphData.canvasData.arrayDatasets[0].dataSets[1].Length);
        }

        [TestMethod]
        public void RecalculateDataplot_OneDataset_UpdatesAllValues()
        {
            Canvas canvas = new Canvas {Width = 100.0, Height = 100.0};
            GraphData graphData = new GraphData(canvas);
            graphData.canvasData.AddDataset(0, new double[] { 10.0 , 20.0, 30.0 });
            graphData.canvasData.Recalculate(0);
            graphData.CreateDatasets(0);
            PointCollection oldPoints = ((Polyline)graphData.canvasData.canvas.Children[graphData.canvasElementNames[graphData.GetFormattedString(GraphData.GraphElementTypes.Graph, 0)]]).Points;
            List<double> oldVals = new List<double>();
            for (int i = 0; i < oldPoints.Count; i++) { oldVals.Add(oldPoints[i].Y); }
            graphData.canvasData.ResetDatasets(0);
            graphData.canvasData.AddDataset(0, new double[] { 20.0, 10.0, 30.0 });

            graphData.RecalculateDataplot(0);
            PointCollection newPoints = ((Polyline)graphData.canvasData.canvas.Children[graphData.canvasElementNames[graphData.GetFormattedString(GraphData.GraphElementTypes.Graph, 0)]]).Points;
            List<double> newVals = new List<double>();
            for (int i=0; i<newPoints.Count; i++){ newVals.Add(newPoints[i].Y); }

            // The y-axis is in the top left of the canvas, so closer to the "normal" x-axis actually means a bigger y-value
            Assert.IsTrue(newVals[0] < oldVals[0]);
            Assert.IsTrue(newVals[1] > oldVals[1]);
            Assert.IsTrue(newVals[2] == oldVals[2]);
        }

        [TestMethod]
        public void ToggleVisible_ValidDatasetIndex_SetsVisibilityToFalse()
        {
            Canvas canvas = new Canvas();// { Width = 100.0, Height = 100.0 };
            GraphData graphData = new GraphData(canvas);
            graphData.canvasData.AddDataset(0, new double[] { 10.0, 20.0, 30.0 });
            graphData.datasetVisible.Add(true);
            graphData.canvasData.Recalculate(0);
            graphData.CreateDatasets(0);

            graphData.ToggleVisible(0, 0, false);
            Visibility pointsVisibility = ((Polyline)graphData.canvasData.canvas.Children[graphData.canvasElementNames[graphData.GetFormattedString(GraphData.GraphElementTypes.Graph, 0)]]).Visibility;

            // The y-axis is in the top left of the canvas, so closer to the "normal" x-axis actually means a bigger y-value
            Assert.AreEqual(Visibility.Hidden, pointsVisibility);
        }
    }
}
