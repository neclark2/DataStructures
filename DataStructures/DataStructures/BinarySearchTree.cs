using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace DataStructures
{
    public class BinarySearchNode<T>
        where T : IComparable
    {
        public BinarySearchNode<T> Left{ get; set; }
        public BinarySearchNode<T> Right { get; set; }
        public T Value { get; set; }
    }

    public class BinarySearchTree<T> 
        where T : IComparable 
    {
        public BinarySearchNode<T> Root { get; set; }


        public void Add(T value)
        {
            var node = new BinarySearchNode<T> {Value = value};

            if (Root == null)
            {
                Root = node;
                return;
            }

            var parentNode = Root;
            var added = false;
            while (!added)
            {
                if (parentNode.Left == null && node.Value.CompareTo(parentNode.Value) < 0)
                {
                    parentNode.Left = node;
                    added = true;
                }
                else if (parentNode.Right == null && node.Value.CompareTo(parentNode.Value) > 0)
                {
                    parentNode.Right = node;
                    added = true;
                }
                else if (node.Value.CompareTo(parentNode.Value) < 0)
                {
                    parentNode = parentNode.Left;
                }
                else if (node.Value.CompareTo(parentNode.Value) > 0)
                {
                    parentNode = parentNode.Right;
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        public BinarySearchNode<T> FindAtIndex(int index)
        {
            /*
             * Google asked me to implement this in an R1 tech interview, on 3/26/15
             * */

            if (Root == null)
            {
                return null;
            }

            if (index < 0)
            {
                throw new ArgumentException("index must be >= 0");
            }

            var stack = new Stack<BinarySearchNode<T>>();
            var workingNode = Root;
            var count = -1;

            stack.Push(workingNode);
            while (workingNode.Left != null)
            {
                stack.Push(workingNode.Left);
                workingNode = workingNode.Left;
            }

            while (!stack.IsEmpty())
            {
                workingNode = stack.Pop();
                count++;
                if (count == index)
                {
                    return workingNode;
                }

                if (workingNode.Right != null)
                {
                    workingNode = workingNode.Right;
                    stack.Push(workingNode);
                    while (workingNode.Left != null)
                    {
                        workingNode = workingNode.Left;
                        stack.Push(workingNode);
                    }
                }
            }
            return null;
        }

        private class FindResult<T>
        {
            public bool FoundValue { get; set; }
            public T Value { get; set; }
            public int CurrCount { get; set; }
        }

        private FindResult<T> FindAtIndexHelper(BinarySearchNode<T> node, int index, int currCount)
        {
            if (node.Left != null)
            {
                var leftResult = FindAtIndexHelper(node.Left, index, currCount);
                currCount = leftResult.CurrCount;
                if (leftResult.FoundValue)
                {
                    return leftResult;
                }
            }

            currCount++;

            if (currCount == index)
            {
                return new FindResult<T>(){FoundValue = true, CurrCount = currCount, Value = node.Value};
            }

            if (node.Right != null)
            {
                var rightResult = FindAtIndexHelper(node.Right, index, currCount);
                currCount = rightResult.CurrCount;
                if (rightResult.FoundValue)
                {
                    return rightResult;;
                }
            }

            return new FindResult<T> {CurrCount = currCount, FoundValue = false};
        }


        public T FindAtIndexRecursively(int index)
        {
            return FindAtIndexHelper(Root, index, -1).Value;
        }


        public List<T> PostOrder ()
        {
            var list = new List<T>();
            PostOrderHelper(Root, list);
            return list;
        }

        private void PostOrderHelper(BinarySearchNode<T> node, List<T> list )
        {
            if (node.Left != null)
            {
                PostOrderHelper(node.Left, list);
            }

            if (node.Right != null)
            {
                PostOrderHelper(node.Right, list);
            }

            list.Add(node.Value);
        } 


        public List<T> GetAllElements()
        {
            if (Root == null)
            {
                return null;
            }

            var stack = new Stack<BinarySearchNode<T>>();
            var list = new List<T>();
            var workingNode = Root;
            var count = -1;

            stack.Push(workingNode);
            while (workingNode.Left != null)
            {
                stack.Push(workingNode.Left);
                workingNode = workingNode.Left;
            }

            while (!stack.IsEmpty())
            {
                workingNode = stack.Pop();
                list.Add(workingNode.Value);

                if (workingNode.Right != null)
                {
                    workingNode = workingNode.Right;
                    stack.Push(workingNode);
                    while (workingNode.Left != null)
                    {
                        workingNode = workingNode.Left;
                        stack.Push(workingNode);
                    }
                }
            }
            return list;
        }

        public void RebalanceTree()
        {
            var stopwatch = Stopwatch.StartNew();
            var elements = GetAllElements();
            Console.WriteLine("get all elems " + stopwatch.Elapsed);
            var tree = new BinarySearchTree<T>();
            var subLists = new System.Collections.Generic.Queue<List<T>>();
            subLists.Enqueue(elements);

            while(subLists.Any())
            {

                var subList = subLists.Dequeue();
                if (subList.Count == 1)
                {
                    tree.Add(subList.Single());
                }

                else
                {
                    var middleIndex = subList.Count / 2;
                    tree.Add(subList[middleIndex]);

                    var left = subList.GetRange(0, middleIndex);
                    subLists.Enqueue(left);

                    if (middleIndex + 1 < subList.Count)
                    {
                        var right = subList.GetRange(middleIndex + 1, subList.Count - middleIndex - 1);
                        subLists.Enqueue(right);
                    }
                }
            }

            Root = tree.Root;

        }

        public void RebalanceTreeRecursively()
        {
            var elements = GetAllElements();
            Root = RebalanceSubTree(elements);
        }

        private BinarySearchNode<T> RebalanceSubTree(List<T> subTreeElements)
        {
            if (subTreeElements.Count == 0)
            {
                return null;
            }

            var middleIndex = subTreeElements.Count / 2;
            var root = new BinarySearchNode<T>() {Value = subTreeElements[middleIndex]};

            var left = subTreeElements.GetRange(0, middleIndex);
            root.Left = RebalanceSubTree(left);

            if (middleIndex + 1 < subTreeElements.Count)
            {
                var right = subTreeElements.GetRange(middleIndex + 1, subTreeElements.Count - middleIndex - 1);
                root.Right = RebalanceSubTree(right);
            }
            return root;
        }


        public IEnumerable<T> BreadthFirstSearch()
        {
            var list = new List<T>();
            var nextNodes = new Queue<BinarySearchNode<T>>();
            nextNodes.Enqueue(Root);
            

            while (!nextNodes.IsEmpty())
            {

                var node = nextNodes.Dequeue();
                list.Add(node.Value);

                if (node.Left != null)
                {
                    nextNodes.Enqueue(node.Left);
                }

                if (node.Right != null)
                {
                    nextNodes.Enqueue(node.Right);
                }
            }

            return list;
        }

        public bool IsTreeValid()
        {
            return IsTreeValidHelper(Root).IsValid;
        }

        private TreeValidCounter<T> IsTreeValidHelper(BinarySearchNode<T> node)
        {
            TreeValidCounter<T> left = null;
            TreeValidCounter<T> right = null;

            if (node.Left != null)
            {
                left = IsTreeValidHelper(node.Left);
                if (left.IsValid == false)
                {
                    return left;
                }
            }

            if (node.Right != null)
            {
                right = IsTreeValidHelper(node.Right);
                if (right.IsValid == false)
                {
                    return right;
                }
            }

            var returnNode = new TreeValidCounter<T>
            {
                IsValid = true,
                Max = node.Value,
                Min = node.Value
            };
           

            if (left != null)
            {
                if (left.Max.CompareTo(node.Value) > 0)
                {
                    return new TreeValidCounter<T>
                    {
                        IsValid = false
                    };
                }

                if (left.Min.CompareTo(returnNode.Min) < 0)
                {
                    returnNode.Min = left.Min;
                }
            }

            if (right != null)
            {
                if (right.Min.CompareTo(node.Value) < 0)
                {
                    return new TreeValidCounter<T>
                    {
                        IsValid = false
                    };
                }

                if (right.Max.CompareTo(returnNode.Max) > 0)
                {
                    returnNode.Max = right.Max;
                }
            }

            return returnNode;
        }

        public int CountUnivalSubtrees()
        {
            var queue = new Queue<BinarySearchNode<T>>();
            queue.Enqueue(Root);
            var count = 0;

            while (!queue.IsEmpty())
            {
                var node = queue.Dequeue();
                if (IsUnivalSubTree(node.Value, node, queue))
                {
                    count++;
                }
              
            }
            return count;
        }


        private bool IsUnivalSubTree(T value, BinarySearchNode<T> node, Queue<BinarySearchNode<T>> values)
        {
            if (!node.Value.Equals(value))
            {
                values.Enqueue(node);
                return false;
            }

            var isRightUnival = true;
            var isLeftUnival = true;

            if (node.Right != null)
            {
                isRightUnival = IsUnivalSubTree(value, node.Right, values);
            }

            if (node.Left != null)
            {
                isLeftUnival = IsUnivalSubTree(value, node.Left, values);
            }

            return isRightUnival && isLeftUnival;
        } 



        private class TreeValidCounter<T>
            where T: IComparable
        {
            public bool IsValid { get; set; }
            public T Min { get; set; }
            public T Max { get; set; }
        }
    }

    [TestFixture]
    public class BinarySearchTreeTests
    {
        [Test]
        public void Test()
        {
            var bst1 = new BinarySearchTree<int>();
            var bst2 = new BinarySearchTree<int>();
            var inputList = new List<int>();
            var size = 100000;
            var rng = new Random();

            for (var i = 0; i < size; i++)
            {
                inputList.Add(i);
            }

            for (var i = 0; i < size; i++)
            {
                var randomIndex = rng.Next(size);
                var randomVal = inputList[randomIndex];
                inputList[randomIndex] = inputList[i];
                inputList[i] = randomVal;
            }

            foreach (var item in inputList)
            {
                bst1.Add(item);
            }

            var l = bst1.FindAtIndexRecursively(9884);
        }

        [Test]
        public void Test2()
        {
            var outList = new List<int>();
            var inList = new List<int>();
            var stack = new Stack<int>();
            var size = 10000;

            for (var i = 0; i < size; i++)
            {
                inList.Add(i);
                stack.Push(i);
            }



            Parallel.For(0, size - 1, new ParallelOptions() { MaxDegreeOfParallelism = 99 }, val =>
            {
                outList.Add(stack.Pop());
            });

            var diff = inList.Intersect(outList);
        }
    }
}
