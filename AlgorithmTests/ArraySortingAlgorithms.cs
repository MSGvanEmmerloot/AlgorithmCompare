using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmTests
{
    public static class ArraySortingAlgorithms
    {
        // For every element in the array, check if it is not bigger than the next element
        public static bool CheckArraySorted(int[] arr)
        {
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                if(arr[i] > arr[i + 1]) { return false; }
            }

            return true;
        }


        // Use the Bubble Sort algorithm to sort the array arr
        // Continuously swaps adjacent elements if they are in the wrong order, until it makes a pass whithout making a swap
        public static void BubbleSort(int[] arr)
        {
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        // swap temp and arr[i] 
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
        }

        // Use the Selection Sort algorithm to sort the array arr
        // Finds the minimum element from the unsorted part of the array, puts it at the beginning and continues with the next element
        public static void SelectionSort(int[] arr)
        {
            int n = arr.Length;

            // One by one move boundary of unsorted subarray 
            for (int i = 0; i < n - 1; i++)
            {
                // Find the minimum element in unsorted array 
                int min_idx = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (arr[j] < arr[min_idx]) { min_idx = j; }
                }

                // Swap the found minimum element with the first element 
                int temp = arr[min_idx];
                arr[min_idx] = arr[i];
                arr[i] = temp;
            }
        }

        // Use the Insertion Sort algorithm to sort the array arr
        // Loops over all elements and puts them either in front or behind the subarray thats' already sorted
        public static void InsertionSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = 1; i < n; ++i)
            {
                int key = arr[i];
                int j = i - 1;

                // Move elements of arr[0..i-1], that are greater than key,  to one position ahead of their current position 
                while (j >= 0 && arr[j] > key)
                {
                    arr[j + 1] = arr[j];
                    j = j - 1;
                }
                arr[j + 1] = key;
            }
        }

        // Use the Merge Sort algorithm to sort the array arr
        public static void MergeSort(int[] arr)
        {
            MergeSort(arr, 0, arr.Length - 1);
        }
        // Divides arr into two arrays, calls itself for those halves and merges the sorted halves. l is for left index and r is right index of the sub-array to be sorted
        private static void MergeSort(int[] arr, int l, int r)
        {
            if (l < r)
            {
                // Same as (l+r)/2, but avoids overflow for 
                // large l and h 
                int m = l + (r - l) / 2;

                // Sort first and second halves 
                MergeSort(arr, l, m);
                MergeSort(arr, m + 1, r);

                Merge(arr, l, m, r);
            }
        }
        // Merges two subarrays of arr. First subarray is arr[l..m]. Second subarray is arr[m+1..r] 
        private static void Merge(int[] arr, int l, int m, int r)
        {
            int i, j, k;
            int n1 = m - l + 1;
            int n2 = r - m;

            int[] L = new int[n1];
            int[] R = new int[n2];

            // Copy data to temp arrays L[] and R[]
            for (i = 0; i < n1; i++) { L[i] = arr[l + i]; }                
            for (j = 0; j < n2; j++) { R[j] = arr[m + 1 + j]; }                

            // Merge the temp arrays back into arr[l..r]
            i = 0; // Initial index of first subarray 
            j = 0; // Initial index of second subarray 
            k = l; // Initial index of merged subarray 
            while (i < n1 && j < n2)
            {
                if (L[i] <= R[j])
                {
                    arr[k] = L[i];
                    i++;
                }
                else
                {
                    arr[k] = R[j];
                    j++;
                }
                k++;
            }

            // Copy the remaining elements of L[], if there are any
            while (i < n1)
            {
                arr[k] = L[i];
                i++;
                k++;
            }

            // Copy the remaining elements of R[], if there are any
            while (j < n2)
            {
                arr[k] = R[j];
                j++;
                k++;
            }
        }

        // Use the Quick Sort algorithm to sort the array arr
        public static void QuickSort(int[] arr)
        {
            QuickSort(arr, 0, arr.Length - 1);
        }
        /* The main function that implements QuickSort(). arr[] --> Array to be sorted, 
        low --> Starting index, high --> Ending index */
        static void QuickSort(int[] arr, int low, int high)
        {
            if (low < high)
            {

                // pi is partitioning index, arr[pi] is now at right place
                int pi = Partition(arr, low, high);

                // Recursively sort elements before partition and after partition 
                QuickSort(arr, low, pi - 1);
                QuickSort(arr, pi + 1, high);
            }
        }
        /* This function takes last element as pivot, places the pivot element at its correct position in sorted array, and places all 
        smaller (smaller than pivot) to left of pivot and all greater elements to right of pivot */
        static int Partition(int[] arr, int low,
                                       int high)
        {
            int pivot = arr[high];

            // index of smaller element 
            int i = (low - 1);
            for (int j = low; j < high; j++)
            {
                // If current element is smaller than the pivot 
                if (arr[j] < pivot)
                {
                    i++;

                    // swap arr[i] and arr[j] 
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }

            // swap arr[i+1] and arr[high] (or pivot) 
            int temp1 = arr[i + 1];
            arr[i + 1] = arr[high];
            arr[high] = temp1;

            return i + 1;
        }

        // Use the Radix Sort algorithm to sort the array arr
        public static void RadixSort(int[] arr)
        {
            RadixSort(arr, arr.Length);
        }
        // The main function to that sorts arr[] of size n using Radix Sort  
        public static void RadixSort(int[] arr, int n)
        {
            // Find the maximum number to know number of digits  
            int m = GetMax(arr, n);

            // Do counting sort for every digit. Note that instead of passing digit number, exp is passed. exp is 10^i where i is current digit number  
            for (int exp = 1; m / exp > 0; exp *= 10)
                CountSort(arr, n, exp);
        }
        // A function to do counting sort of arr[] according to the digit represented by exp.  
        public static void CountSort(int[] arr, int n, int exp)
        {
            int[] output = new int[n]; // output array  
            int i;
            int[] count = new int[10];

            //initializing all elements of count to 0 
            for (i = 0; i < 10; i++)
                count[i] = 0;

            // Store count of occurrences in count[]  
            for (i = 0; i < n; i++)
                count[(arr[i] / exp) % 10]++;

            // Change count[i] so that count[i] now contains actual position of this digit in output[]  
            for (i = 1; i < 10; i++)
                count[i] += count[i - 1];

            // Build the output array  
            for (i = n - 1; i >= 0; i--)
            {
                output[count[(arr[i] / exp) % 10] - 1] = arr[i];
                count[(arr[i] / exp) % 10]--;
            }

            // Copy the output array to arr[], so that arr[] now contains sorted numbers according to current digit  
            for (i = 0; i < n; i++)
                arr[i] = output[i];
        }
        public static int GetMax(int[] arr, int n)
        {
            int mx = arr[0];
            for (int i = 1; i < n; i++)
                if (arr[i] > mx)
                    mx = arr[i];
            return mx;
        }
    }
}
