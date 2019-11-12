using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmTests.UnitTests
{
    [TestClass]
    public class ArrayCompareTests
    {
        [TestMethod]
        public void CalculateAlgorithmAveragePerformances_AfterRunTestbench_CalculatesRightAverage()
        {
            ArrayCompare.Init();
            ArrayCompare.RunTestbench();

            long expectedResult = 0;
            int measurements = ArrayCompare.algorithmPerformances.Count;

            for (int i=0; i< measurements; i++)
            {
                expectedResult += ArrayCompare.algorithmPerformances[i][0].ticksElapsed[0];
            }
            expectedResult /= measurements;

            Assert.AreEqual(expectedResult, ArrayCompare.algorithmPerformancesAverage[0].ticksElapsed[0]);
        }

        [TestMethod]
        public void AddAlgorithmToQueue_NewAlgorithm_DoesAddToQueue()
        {
            int expectedResult = 1;
            ArrayCompare.ClearAlgorithmQueue();

            ArrayCompare.AddAlgorithmToQueue("Bubble Sort", ArraySortingAlgorithms.BubbleSort);

            Assert.IsTrue(ArrayCompare.algorithmNames.Contains("Bubble Sort"));
            Assert.AreEqual(expectedResult, ArrayCompare.algorithmNames.Count);
            Assert.IsTrue(ArrayCompare.QueueListLengthsAreEqual());
        }

        [TestMethod]
        public void AddAlgorithmToQueue_DuplicateAlgorithm_DoesNotAddToQueue()
        {
            int expectedResult = 1;
            ArrayCompare.ClearAlgorithmQueue();

            ArrayCompare.AddAlgorithmToQueue("Bubble Sort", ArraySortingAlgorithms.BubbleSort);
            ArrayCompare.AddAlgorithmToQueue("Bubble Sort", ArraySortingAlgorithms.BubbleSort);

            Assert.IsTrue(ArrayCompare.algorithmNames.Contains("Bubble Sort"));
            Assert.AreEqual(expectedResult, ArrayCompare.algorithmNames.Count);
            Assert.IsTrue(ArrayCompare.QueueListLengthsAreEqual());
        }

        [TestMethod]
        public void AddAlgorithmToQueue_MultipleAlgorithms_DoAddToQueue()
        {
            int expectedResult = 2;
            ArrayCompare.ClearAlgorithmQueue();

            ArrayCompare.AddAlgorithmToQueue("Bubble Sort", ArraySortingAlgorithms.BubbleSort);
            ArrayCompare.AddAlgorithmToQueue("Selection Sort", ArraySortingAlgorithms.SelectionSort);

            Assert.IsTrue(ArrayCompare.algorithmNames.Contains("Bubble Sort"));
            Assert.IsTrue(ArrayCompare.algorithmNames.Contains("Selection Sort"));
            Assert.AreEqual(expectedResult, ArrayCompare.algorithmNames.Count);
            Assert.IsTrue(ArrayCompare.QueueListLengthsAreEqual());
        }

        [TestMethod]
        public void GetAlgorithmIndex_ValidIndex_ReturnsRightIndexZero()
        {
            int expectedResult = 0;
            ArrayCompare.ClearAlgorithmQueue();

            ArrayCompare.AddAlgorithmToQueue("Bubble Sort", ArraySortingAlgorithms.BubbleSort);
            ArrayCompare.AddAlgorithmToQueue("Selection Sort", ArraySortingAlgorithms.SelectionSort);                   
            int result = ArrayCompare.GetAlgorithmIndex("Bubble Sort");

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetAlgorithmIndex_ValidIndex_ReturnsRightIndexOne()
        {
            int expectedResult = 1;
            ArrayCompare.ClearAlgorithmQueue();

            ArrayCompare.AddAlgorithmToQueue("Selection Sort", ArraySortingAlgorithms.SelectionSort);
            ArrayCompare.AddAlgorithmToQueue("Bubble Sort", ArraySortingAlgorithms.BubbleSort);
            int result = ArrayCompare.GetAlgorithmIndex("Bubble Sort");

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void QueueListLengthsAreEqual_EqualLengths_ReturnsTrue()
        {
            ArrayCompare.ClearAlgorithmQueue();

            ArrayCompare.AddAlgorithmToQueue("Selection Sort", ArraySortingAlgorithms.SelectionSort);
            ArrayCompare.AddAlgorithmToQueue("Bubble Sort", ArraySortingAlgorithms.BubbleSort);

            Assert.IsTrue(ArrayCompare.QueueListLengthsAreEqual());
        }

        [TestMethod]
        public void QueueListLengthsAreEqual_InEqualLengths_ReturnsFalse()
        {
            ArrayCompare.ClearAlgorithmQueue();

            ArrayCompare.algorithmNames.Add("Test");

            Assert.IsFalse(ArrayCompare.QueueListLengthsAreEqual());
        }

        [TestMethod]
        public void QueueListLengthsAreEqual_EmptyLists_ReturnsTrue()
        {
            ArrayCompare.ClearAlgorithmQueue();

            Assert.IsTrue(ArrayCompare.QueueListLengthsAreEqual());
        }        

        [TestMethod]
        public void ClearAlgorithmQueue_ClearsQueue()
        {
            int expectedResult = 0;
            ArrayCompare.ClearAlgorithmQueue();

            ArrayCompare.AddAlgorithmToQueue("Selection Sort", ArraySortingAlgorithms.SelectionSort);
            ArrayCompare.AddAlgorithmToQueue("Bubble Sort", ArraySortingAlgorithms.BubbleSort);
            ArrayCompare.ClearAlgorithmQueue();

            Assert.AreEqual(expectedResult, ArrayCompare.algorithmNames.Count);
            Assert.IsTrue(ArrayCompare.QueueListLengthsAreEqual());
        }

        [TestMethod]
        public void AddToBaseArrays_ValidArrayLength_DoesAddToBaseArray()
        {
            int expectedResult;
            ArrayCompare.ClearArrayQueue();
            int[] testArray = ArrayCompare.CreateArray_InOrder();

            ArrayCompare.AddToBaseArrays(testArray);

            Assert.IsNotNull(ArrayCompare.baseArrays[0]);
            Assert.AreEqual(ArrayCompare.baseArrays.Count, 1);
            for (int i = 0; i < testArray.Length; i++)
            {
                expectedResult = testArray[i];
                Assert.AreEqual(expectedResult, ArrayCompare.baseArrays[0][i]);
            }
        }

        [TestMethod]
        public void AddToBaseArrays_InvalidArrayLength_DoesNotAddToBaseArray()
        {
            int expectedResult = 0;
            int a = 10;
            if(a == ArrayCompare.arraySize)
            {
                a++;
            }
            int[] testArray = new int[a];
            ArrayCompare.ClearArrayQueue();

            ArrayCompare.AddToBaseArrays(testArray);
            Assert.AreEqual(expectedResult, ArrayCompare.baseArrays.Count);
        }

        [TestMethod]
        public void ClearArrayQueue_ClearsQueue()
        {
            ArrayCompare.ClearArrayQueue();            
            int[] testArray = ArrayCompare.CreateArray_InOrder();
            ArrayCompare.AddToBaseArrays(testArray);

            ArrayCompare.ClearArrayQueue();

            Assert.AreEqual(0, ArrayCompare.baseArrays.Count);
            Assert.AreEqual(ArrayCompare.sortedArrays.Count, ArrayCompare.baseArrays.Count);
        }
                
        [TestMethod]
        public void CreateArray_InOrder_CreatesArrayOfRightSize()
        {
            int[] testArray;
            int expectedResult = ArrayCompare.arraySize;

            testArray = ArrayCompare.CreateArray_InOrder();

            Assert.AreEqual(expectedResult, testArray.Length);
        }

        [TestMethod]
        public void CreateArray_InOrder_CreatesOrderedArray()
        {
            int[] testArray;

            testArray = ArrayCompare.CreateArray_InOrder();

            for (int i = 0; i < testArray.Length - 1; i++)
            {
                Assert.IsTrue(testArray[i] <= testArray[i + 1]);
            }
        }

        [TestMethod]
        public void ResetSortedArrays_ResetsSortedToBaseValues()
        {
            int[] testArray;
            int expectedResult;
            ArrayCompare.ClearArrayQueue();
            testArray = ArrayCompare.CreateArray_InOrder();

            ArrayCompare.AddToBaseArrays(testArray);
            ArrayCompare.ResetSortedArrays();

            Assert.AreEqual(ArrayCompare.baseArrays[0].Length, ArrayCompare.sortedArrays[0].Length);

            for (int i = 0; i < ArrayCompare.baseArrays[0].Length; i++)
            {
                expectedResult = ArrayCompare.baseArrays[0][i];
                Assert.AreEqual(expectedResult, ArrayCompare.sortedArrays[0][i]);
            }
        }

        [TestMethod]
        public void RunTestbench_NoAlgorithms_DoesntReturnError()
        {
            ArrayCompare.ClearAlgorithmQueue();

            ArrayCompare.RunTestbench();

            //If there is no error, this test is a success
        }

        [TestMethod]
        public void RunTestbench_NoParams_DoesOneRun()
        {
            int expectedResult = 1;
            ArrayCompare.Init();

            ArrayCompare.RunTestbench();
            int measurements = ArrayCompare.algorithmPerformances.Count;

            Assert.AreEqual(expectedResult, measurements);
        }

        //public static List<AlgorithmPerformance> RunAllAlgorithms(bool log = false)

        //public static AlgorithmPerformance RunAlgorithm(int algorithmIndex, bool log = false)

        //public static AlgorithmPerformance RunAlgorithmForAllArrays(int algorithmIndex, bool log = false)

        //public static long RunAlgorithmForArray(int algorithmIndex, int arrayIndex)


        [TestMethod]
        public void FormatTime_500ns_Returns500ns()
        {
            double ns = 500;
            string expectedResult = "500 ns";

            string result = ArrayCompare.FormatTimeFromNanoseconds(ns);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void FormatTime_5000ns_Returns5us()
        {
            double ns = 5000;
            string expectedResult = "5 us";

            string result = ArrayCompare.FormatTimeFromNanoseconds(ns);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void FormatTime_5000000ns_Returns5ms()
        {
            double ns = 5000000;
            string expectedResult = "5 ms";

            string result = ArrayCompare.FormatTimeFromNanoseconds(ns);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
