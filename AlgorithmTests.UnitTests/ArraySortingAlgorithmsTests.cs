using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgorithmTests;

namespace AlgorithmTests.UnitTests
{
    [TestClass]
    public class ArraySortingAlgorithmsTests
    {
        [TestMethod]
        public void CheckArraySorted_SortedArrayNoDuplicates_ReturnsTrue()
        {
            int[] testArray = new int[] { 0, 1, 2, 3, 4, 5 };

            bool result = ArraySortingAlgorithms.CheckArraySorted(testArray);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckArraySorted_SortedArrayWithDuplicates_ReturnsTrue()
        {
            int[] testArray = new int[] { 0, 0, 2, 2, 4, 5 };

            bool result = ArraySortingAlgorithms.CheckArraySorted(testArray);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckArraySorted_UnsortedArrayNoDuplicates_ReturnsFalse()
        {
            int[] testArray = new int[] { 0, 1, 2, 3, 5, 4 };

            bool result = ArraySortingAlgorithms.CheckArraySorted(testArray);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckArraySorted_UnsortedArrayWithDuplicates_ReturnsFalse()
        {
            int[] testArray = new int[] { 0, 1, 3, 3, 2, 4 };

            bool result = ArraySortingAlgorithms.CheckArraySorted(testArray);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void BubbleSort_ReturnsSortedArray()
        {
            int[] testArray = new int[] { 0, 5, 1, 4, 3, 2 };

            ArraySortingAlgorithms.BubbleSort(testArray);
            bool result = ArraySortingAlgorithms.CheckArraySorted(testArray);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SelectionSort_ReturnsSortedArray()
        {
            int[] testArray = new int[] { 0, 5, 1, 4, 3, 2 };

            ArraySortingAlgorithms.SelectionSort(testArray);
            bool result = ArraySortingAlgorithms.CheckArraySorted(testArray);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void InsertionSort_ReturnsSortedArray()
        {
            int[] testArray = new int[] { 0, 5, 1, 4, 3, 2 };

            ArraySortingAlgorithms.InsertionSort(testArray);
            bool result = ArraySortingAlgorithms.CheckArraySorted(testArray);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MergeSort_ReturnsSortedArray()
        {
            int[] testArray = new int[] { 0, 5, 1, 4, 3, 2 };

            ArraySortingAlgorithms.MergeSort(testArray);
            bool result = ArraySortingAlgorithms.CheckArraySorted(testArray);

            Assert.IsTrue(result);
        }
    }
}
