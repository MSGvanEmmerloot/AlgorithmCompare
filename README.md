# algorithmCompare

## Synopsis

The goal of this project is to demonstrate the performance differences between certain algorithms.
Currently, it features algorithms that can be used to sort the contents of an array.

## Usage

To initialize the ArrayCompare class, execute the ArrayCompare.Init() function. 
Inside this function, the algorithms are selected that have to be tested. Algorithms can easily be added or removed from the List.
Currently, the comparison functionality is executed by executing the ArrayCompare.RunTestBench() function.
This function will execute all algorithms for all provided arrays and measure the elapsed time for every combination.

## Code Example

The following example shows how the functions are implemented in the current project. It executes 5 measurements, shows the average results and then adds the results to the graph.

In MainWindow.xaml.cs:
```
// Run 5 measurements for every algorithm for every array
ArrayCompare.RunTestBench(5);

// Refresh the datatable called arrayData
arrayData.Items.Refresh();

// Get all measurements from every algorithm for array 1
List<double[]> algorithmPerformanceList = new List<double[]>();            
for(int i=0; i< ArrayCompare.algorithmNames.Count; i++)
{
	algorithmPerformanceList.Add(ArrayCompare.GetResultArrayDouble(i, 1));
}

// Add the retrieved results to the graph
graphData.AddAlgorithmsDataToGraph(algorithmPerformanceList);
```

## Motivation

In order to visualize the efficiency of multiple sorting algorithms, I decided to make this program.
First and foremost, this project exists because it seemed interesting to me to create it.

## Credits

This project is done solely by myself

## License

MIT License

Copyright (c) 2019 Mika van Emmerloot

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
