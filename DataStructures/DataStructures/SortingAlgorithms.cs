using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DataStructures
{
    public class SortingAlgorithms
    {
        
        public static IList<T> MergeSort<T>(IList<T> input) 
            where T: IComparable
        {
            if (input.Count() <= 1)
            {
                return input;
            }

            var middleIndex = input.Count()/2;
            var left = new List<T>();
            var right = new List<T>();

            for (var i = 0; i < middleIndex; i++)
            {
                left.Add(input[i]);
            }

            for (var j = middleIndex; j < input.Count; j++)
            {
                right.Add(input[j]);
            }

            var sortedLeft = MergeSort(left);
            var sortedRight = MergeSort(right);

            var leftIdx = 0;
            var rightIdx = 0;
            var output = new List<T>();
            for (var i = 0; i < input.Count(); i++)
            {
                if (rightIdx > right.Count - 1 || (leftIdx < left.Count() && sortedLeft[leftIdx].CompareTo(sortedRight[rightIdx]) <= 0))
                {
                    output.Add(sortedLeft[leftIdx]);
                    leftIdx++;
                }
                else
                {
                    output.Add(sortedRight[rightIdx]);
                    rightIdx++;
                }
            }
            return output;
        }

        [TestFixture]
        public class MergeSortTests
        {
            [Test]
            public void Tests()
            {
                var randomList = new List<int>();
                var rng = new Random();
                var inputSize = 1000000;
                for (var i = 0; i < inputSize; i++)
                {
                    randomList.Add(rng.Next(inputSize));
                }

                var sortedList = MergeSort(randomList);


            }
        }

    }
}
