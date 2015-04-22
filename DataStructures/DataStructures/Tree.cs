using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml;
using NUnit.Framework;

namespace DataStructures
{
    public class TreeNode<T>
    {
        public T Value;
        public IEnumerable<TreeNode<T>> Children { get; set; }

        public IEnumerable<T> InOrderSearch()
        {
            return InOrderSearchHelper(this, new List<T>());
        }

        private IEnumerable<T> InOrderSearchHelper(TreeNode<T> node, List<T> output)
        {
            if (node.Children == null)
            {
                output.Add(node.Value);
                return output;
            }

            foreach (var child in node.Children)
            {
                InOrderSearchHelper(child, output);
            }

            output.Add(node.Value);

            return output;
        } 

        
    }


    [TestFixture]
    public class TreeNodeTests
    {
        [Test]
        public void TreeTests()
        {
            var root = new TreeNode<int>();
            root.Value = 1;

            var n2 = new TreeNode<int> {Value = 2};
            var n3 = new TreeNode<int> {Value = 3};
            var n4 = new TreeNode<int> {Value = 4};
            var n5 = new TreeNode<int> {Value = 5};
            var n6 = new TreeNode<int> {Value = 6};
            var n7 = new TreeNode<int> {Value = 7};
            var n8 = new TreeNode<int> {Value = 8};
            var n9 = new TreeNode<int> {Value = 9};

            root.Children = new List<TreeNode<int>> {n2, n3};
            n3.Children = new List<TreeNode<int>> {n4, n5};
            n2.Children = new List<TreeNode<int>> {n6, n7};
            n7.Children = new List<TreeNode<int>> {n8};
            n8.Children = new List<TreeNode<int>> {n9};

            var l = root.InOrderSearch();

        }
    }

   
}
