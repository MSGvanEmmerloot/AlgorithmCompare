using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmTests
{
    public static class ArrayCompare
    {
        //public class AlgorithmPerformance
        //{
        //    public string algorithmName { get; private set; }
        //    public long ticksElapsed { get; private set; }

        //    public AlgorithmPerformance(string name, long ticks)
        //    {
        //        algorithmName = name;
        //        ticksElapsed = ticks;
        //    }
        //}

        //public class ArrayPerformance
        //{
        //    public List<AlgorithmPerformance> algorithmPerformances;

        //    public ArrayPerformance()
        //    {
        //        algorithmPerformances = new List<AlgorithmPerformance>();
        //    }

        //    public void AddAlgorithmPerformance(string name, long ticks)
        //    {
        //        algorithmPerformances.Add(new AlgorithmPerformance(name, ticks));
        //    }
        //}

        //public static List<ArrayPerformance> arrayPerformances = new List<ArrayPerformance>();
        public class AlgorithmPerformance
        {
            public string algorithmName { get; private set; }
            public long[] ticksElapsed { get; private set; }

            public AlgorithmPerformance(string name)
            {
                algorithmName = name;
                ticksElapsed = new long[numberOfArrays];
            }

            public void AddResult(int index, long ticks)
            {
                if (index < ticksElapsed.Length)
                {
                    ticksElapsed[index] = ticks;
                }
            }

            public void SetResults(long[] results)
            {
                for (int i = 0; i < results.Length; i++)
                {
                    if (ticksElapsed.Length > i)
                    {
                        ticksElapsed[i] = results[i];
                    }
                }
            }
        }

        //public static List<AlgorithmPerformance> algorithmPerformances = new List<AlgorithmPerformance>();
        public static List<List<AlgorithmPerformance>> algorithmPerformances = new List<List<AlgorithmPerformance>>();
        public static ObservableCollection<AlgorithmPerformance> algorithmPerformancesAverage = new ObservableCollection<AlgorithmPerformance>();

        public static int arraySize { get; private set; } = 20;
        public static int numberOfArrays { get; private set; } = 0;

        //For debugging
        public static string runCode = "";
        public static bool doneCalculating = false;

        //static string testString = "Hi";
        private static bool initialized = false;
        private static Stopwatch stopWatch = new Stopwatch();
        private static long nsPerTick = 0;

        public static List<int[]> baseArrays { get; private set; } = new List<int[]>();
        public static List<int[]> sortedArrays { get; private set; } = new List<int[]>();

        private static List<Action<int[]>> algorithms = new List<Action<int[]>>();
        public static List<string> algorithmNames { get; private set; } = new List<string>();

        public static void Init()
        {
            if (!initialized)
            {
                InitArrays();
                initialized = true;
                nsPerTick = (1000 * 1000 * 1000) / Stopwatch.Frequency;
                
                numberOfArrays = baseArrays.Count;

                AddAlgorithmToQueue("Bubble Sort", ArraySortingAlgorithms.BubbleSort);
                AddAlgorithmToQueue("Selection Sort", ArraySortingAlgorithms.SelectionSort);
                AddAlgorithmToQueue("Insertion Sort", ArraySortingAlgorithms.InsertionSort);
                AddAlgorithmToQueue("Merge Sort", ArraySortingAlgorithms.MergeSort);

                for (int i = 0; i < algorithmNames.Count; i++)
                {
                    algorithmPerformancesAverage.Add(new AlgorithmPerformance(algorithmNames[i]));
                }
            }
        }

        private static void CalculateAlgorithmAveragePerformances()
        {
            if(algorithmPerformances == null) { return; }
            if(algorithmPerformances.Count == 0) { return; }

            int measurements = algorithmPerformances.Count;
            int numberOfAlgorithms = algorithmPerformances[0].Count;

            long[,] results = new long[numberOfAlgorithms,numberOfArrays];

            // For every set of measurements..
            for (int i = 0; i < measurements; i++)
            {
                // For every algorithm in the set..
                for(int j=0; j< numberOfAlgorithms; j++)
                {
                    // For every array in the algorithm class..
                    for (int k = 0; k < numberOfArrays; k++)
                    {
                        results[j,k] += algorithmPerformances[i][j].ticksElapsed[k];
                    }
                }                
            }

            // For every algorithm
            for(int a=0; a<algorithmPerformancesAverage.Count; a++)
            {
                // Calculate the average duration of every array
                for (int r = 0; r < numberOfArrays; r++)
                {
                    long average = results[a,r] /= measurements;
                    algorithmPerformancesAverage[a].ticksElapsed[r] = average;
                }
            }

            Console.WriteLine("Calculated the averages for " + numberOfAlgorithms + " algorithms with " + numberOfArrays + " arrays each, based on " + measurements + " measurements");
            printAlgorithmResults();
        }

        private static void printAlgorithmResults()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < algorithmPerformancesAverage.Count; i++)
            {
                sb.Append(algorithmPerformancesAverage[i].algorithmName + ") [");
                for (int j = 0; j < algorithmPerformancesAverage[i].ticksElapsed.Length; j++)
                {
                    sb.Append(" ").Append(algorithmPerformancesAverage[i].ticksElapsed[j]);
                }
                sb.Append("]");
                Console.WriteLine(sb);
                sb.Clear();
            }
        }        

        //private static void AddAlgorithmToQueue(string name, Action<int[]> algorithm)
        //{
        //    algorithmNames.Add(name);
        //    algorithms.Add(algorithm);
        //}
        public static void AddAlgorithmToQueue(string name, Action<int[]> algorithm)
        {
            if (algorithmNames.Contains(name)){
                return;
            }

            algorithmNames.Add(name);
            algorithms.Add(algorithm);
        }

        public static int GetAlgorithmIndex(string algorithmName)
        {
            return algorithmNames.FindIndex(x => x.Equals(algorithmName));
        }

        public static bool QueueListLengthsAreEqual()
        {
            return (algorithmNames.Count == algorithms.Count);
        }

        public static void ClearAlgorithmQueue()
        {
            algorithms.Clear();
            algorithmNames.Clear();
        }

        private static void InitArrays()
        {
            //baseArrays.Add(new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            //baseArrays.Add(new int[10] { 0, 0, 1, 1, -1, -1, 0, 1, -1, 0 });
            //baseArrays.Add(new int[10] { 0, 9, 1, 8, 2, 7, 3, 6, 4, 5 });
            baseArrays.Add(CreateArray_InOrder());
            baseArrays.Add(CreateArray_SemiInOrder(10));
            baseArrays.Add(CreateArray_MixedUp());
            ResetSortedArrays();
        }

        public static void AddToBaseArrays(int[] newArray)
        {
            if(newArray.Length != arraySize){
                return;
            }

            baseArrays.Add(newArray);

            ResetSortedArray(baseArrays.Count-1);
        }

        public static void ClearArrayQueue()
        {
            baseArrays.Clear();
            sortedArrays.Clear();
        }

        // Create an array that's already ordered (0 1 2 3 4)
        public static int[] CreateArray_InOrder()
        {
            int[] tempArray = new int[arraySize];

            for(int i=0; i<arraySize; i++)
            {
                tempArray[i] = i;
            }

            return tempArray;
        }

        // Create an array that has a pattern (0 1 2 0 1 2)
        private static int[] CreateArray_SemiInOrder(int numberOfValues)
        {
            int[] tempArray = new int[arraySize];
            int curVal = 0;

            for (int i = 0; i < arraySize; i++)
            {
                if(curVal == numberOfValues)
                {
                    curVal = 0;
                }
                tempArray[i] = curVal;
                curVal++;
            }

            return tempArray;
        }

        // Create an array that has mixed high and low values (0 5 1 4 2 3)
        private static int[] CreateArray_MixedUp()
        {
            int[] tempArray = new int[arraySize];
            int min = 0;
            int max = arraySize - 1;

            for (int i = 0; i < arraySize; i++)
            {
                tempArray[i] = i % 2 == 0 ? min++ : max--;
            }

            return tempArray;
        }

        // Reset one element of sortedArray
        private static void ResetSortedArray(int index)
        {
            if(baseArrays.Count < index)
            {
                return;
            }

            if (sortedArrays.Count<index || index == 0)            
            {
                sortedArrays.Add(new int[baseArrays[index].Length]);
            }

            if (baseArrays[index] != null && sortedArrays[index] != null)
            {
                Array.Copy(baseArrays[index], sortedArrays[index], baseArrays[index].Length);
            }
        }

        // Reset all sortedArray elements
        public static void ResetSortedArrays()
        {
            for (int i = 0; i < baseArrays.Count; i++)
            {
                if (sortedArrays.Count < i + 1)
                {
                    sortedArrays.Add(new int[baseArrays[i].Length]);
                }

                Array.Copy(baseArrays[i], sortedArrays[i], baseArrays[i].Length);
            }
        }

        // Run all sorting algorithms
        public static void RunTestbench(int runAmount = 1)
        {
            if (algorithms.Count <= 0)
            {
                return;
            }

            // Dummy measurement to reduce overhead with the "real" measurements
            ResetStopWatch();

            for (int r = 0; r < runAmount; r++)
            {
                // Run real measurements
                algorithmPerformances.Add(RunAllAlgorithms());
            }

            // Calculate the average of all measurements per array per algorithm
            CalculateAlgorithmAveragePerformances();
        }

        private static void ResetStopWatch()
        {
            stopWatch.Restart();
            algorithms[0].Invoke(sortedArrays[0]);
            stopWatch.Stop();
        }

        public static List<AlgorithmPerformance> RunAllAlgorithms(bool log = false)
        {
            List<AlgorithmPerformance> currentTestRun = new List<AlgorithmPerformance>();

            for (int j = 0; j < algorithms.Count; j++)
            {
                currentTestRun.Add(RunAlgorithm(j, log));
            }

            return currentTestRun;
        }

        public static AlgorithmPerformance RunAlgorithm(int algorithmIndex, bool log = false)
        {
            if (log)
            {            
                Console.WriteLine("Calling algorithm \"" + algorithmNames[algorithmIndex] + "\"");
            }

            stopWatch.Reset();
            ResetSortedArrays();

            AlgorithmPerformance currentPerformance = RunAlgorithmForAllArrays(algorithmIndex);

            if (log)
            {            
                Console.WriteLine("Before reset");
                PrintArrays(sortedArrays);
                ResetSortedArrays();
                Console.WriteLine("After reset");
                PrintArrays(sortedArrays);
            }

            return currentPerformance;
        }

        public static AlgorithmPerformance RunAlgorithmForAllArrays(int algorithmIndex, bool log = false)
        {
            AlgorithmPerformance currentPerformance = new AlgorithmPerformance(algorithmNames[algorithmIndex]);

            for (int i = 0; i < sortedArrays.Count; i++)
            {
                long elapsedTicks = RunAlgorithmForArray(algorithmIndex, i);
                currentPerformance.AddResult(i, elapsedTicks);
                if (log)
                {
                    Console.WriteLine("Sorting array " + i + " took " + FormatTimeFromTicks(elapsedTicks) + " (" + elapsedTicks + " ticks)");
                }                
            }

            return currentPerformance;
        }

        public static long RunAlgorithmForArray(int algorithmIndex, int arrayIndex)
        {
            stopWatch.Restart();
            algorithms[algorithmIndex].Invoke(sortedArrays[arrayIndex]);
            stopWatch.Stop();
            return stopWatch.ElapsedTicks;
        }

        //public static void RunTestbench()
        //{
        //    if (algorithms.Count <= 0)
        //    {
        //        return;
        //    }

        //    // Dummy measurement to reduce overhead with the "real" measurements
        //    stopWatch.Restart();
        //    algorithms[0].Invoke(sortedArrays[0]);
        //    stopWatch.Stop();

        //    List<AlgorithmPerformance> currentTestRun = new List<AlgorithmPerformance>();
        //    for (int j = 0; j < algorithms.Count; j++)
        //    {
        //        Console.WriteLine("Calling algorithm \"" + algorithmNames[j] + "\"");
        //        stopWatch.Reset();
        //        ResetSortedArrays();

        //        AlgorithmPerformance currentPerformance = new AlgorithmPerformance(algorithmNames[j]);

        //        for (int i = 0; i < sortedArrays.Count; i++)
        //        {
        //            stopWatch.Restart();
        //            algorithms[j].Invoke(sortedArrays[i]);
        //            stopWatch.Stop();
        //            long elapsedTicks = stopWatch.ElapsedTicks;
        //            currentPerformance.AddResult(i, elapsedTicks);
        //            Console.WriteLine("Sorting array " + i + " took " + FormatTimeFromTicks(elapsedTicks) + " (" + elapsedTicks + " ticks)");
        //        }
        //        currentTestRun.Add(currentPerformance);

        //        Console.WriteLine("Before reset");
        //        PrintArrays(sortedArrays);
        //        ResetSortedArrays();
        //        Console.WriteLine("After reset");
        //        PrintArrays(sortedArrays);
        //    }

        //    algorithmPerformances.Add(currentTestRun);
        //    CalculateAlgorithmAveragePerformances();
        //}

        // Run all sorting algorithms, but in the other direction (TBD)
        //public static void RunTestbenchReverse()
        //{
        //    if (algorithms.Count <= 0)
        //    {
        //        return;
        //    }

        //    List<AlgorithmPerformance> currentTestRun = new List<AlgorithmPerformance>();
        //    for (int j = algorithms.Count - 1; j > -1; j--)
        //    {
        //        Console.WriteLine("Calling algorithm \"" + algorithmNames[j] + "\"");
        //        stopWatch.Reset();
        //        ResetSortedArrays();

        //        AlgorithmPerformance currentPerformance = new AlgorithmPerformance(algorithmNames[j]);

        //        for (int i = sortedArrays.Count - 1; i > -1; i--)
        //        {
        //            stopWatch.Restart();
        //            algorithms[j].Invoke(sortedArrays[i]);
        //            stopWatch.Stop();
        //            long elapsedTicks = stopWatch.ElapsedTicks;
        //            currentPerformance.AddResult(i, elapsedTicks);
        //            Console.WriteLine("Sorting array " + i + " took " + FormatTimeFromTicks(elapsedTicks) + " (" + elapsedTicks + " ticks)");
        //        }
        //        currentTestRun.Add(currentPerformance);
        //        ResetSortedArrays();
        //    }

        //    algorithmPerformances.Add(currentTestRun);
        //    CalculateAlgorithmAveragePerformances();
        //}

        // Format time to a good readable format
        public static string FormatTimeFromTicks(double elapsedTicks)
        {
            double elapsedNanoseconds = (elapsedTicks * nsPerTick);
            return FormatTimeFromNanoseconds(elapsedNanoseconds);
        }
        // Format time to a good readable format
        public static string FormatTimeFromNanoseconds(double elapsedNanoseconds)
        {
            string suffix = "ns";

            if (elapsedNanoseconds > 999)
            {
                elapsedNanoseconds /= 1000;
                suffix = "us";

                if (elapsedNanoseconds > 999)
                {
                    elapsedNanoseconds /= 1000;
                    suffix = "ms";
                }
            }           

            return elapsedNanoseconds.ToString() + " " + suffix;
        }        

        //========== Print functions ==========//
        public static void PrintStopWatchVals()
        {
            long freq = Stopwatch.Frequency;
            Console.WriteLine("Stopwatch frequency: " + freq);
            long ns = (1000 * 1000 * 1000) / freq;
            Console.WriteLine("nanoseconds per tick: " + ns);
        }

        public static void PrintAllArrays()
        {
            Console.WriteLine("Base Arrays:");
            PrintArrays(baseArrays);
            Console.WriteLine("Sorted Arrays:");
            PrintArrays(sortedArrays);
        }

        public static void PrintArrays(List<int[]> array)
        {
            if (array == null)
            {
                array = baseArrays;
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.Count; i++)
            {
                sb.Append(i + ") [");
                for (int j = 0; j < array[i].Length; j++)
                {
                    sb.Append(" ").Append(array[i][j]);
                }
                sb.Append(" ]");
                Console.WriteLine(sb);
                sb.Clear();
            }
        }
    }
}
