using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterfaceActivityBuilder.Tree
{
    public class TreeNode<T> : IEnumerable<TreeNode<T>>
    {

        public T Data { get; set; }
        public TreeNode<T> Parent { get; set; }
        public ICollection<TreeNode<T>> Children { get; set; }

        public Boolean HasSiblings
        {
            get { return this.Parent?.Children?.Count - 1 > this.Parent?.Children?.ToList()?.IndexOf(this); }
        }

        public Boolean IsRoot
        {
            get { return Parent == null; }
        }

        public Boolean IsLeaf
        {
            get { return Children.Count == 0; }
        }

        public int Level
        {
            get
            {
                if (this.IsRoot)
                    return 0;
                return Parent.Level + 1;
            }
        }


        public TreeNode(T data)
        {
            this.Data = data;
            this.Children = new LinkedList<TreeNode<T>>();

            this.ElementsIndex = new LinkedList<TreeNode<T>>();
            this.ElementsIndex.Add(this);
        }

        public TreeNode<T> AddChild(T child)
        {
            TreeNode<T> childNode = new TreeNode<T>(child) { Parent = this };
            this.Children.Add(childNode);

            this.RegisterChildForSearch(childNode);

            return childNode;
        }

        public override string ToString()
        {
            var tree = string.Empty;
            foreach (var node in this)
            {
                string indent = CreateIndent(node.Level);
                tree += Environment.NewLine;
                tree += indent + (node?.Data?.ToString() ?? "null");
            }

            return tree;
        }

        private String CreateIndent(int depth)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < depth; i++)
            {
                sb.Append("--");
            }
            return sb.ToString();
        }


        #region searching

        private ICollection<TreeNode<T>> ElementsIndex { get; set; }

        private void RegisterChildForSearch(TreeNode<T> node)
        {
            ElementsIndex.Add(node);
            if (Parent != null)
                Parent.RegisterChildForSearch(node);
        }

        public TreeNode<T> FindTreeNode(Func<TreeNode<T>, bool> predicate)
        {
            return this.ElementsIndex.FirstOrDefault(predicate);
        }

        #endregion


        #region iterating

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            yield return this;
            foreach (var directChild in this.Children)
            {
                foreach (var anyChild in directChild)
                    yield return anyChild;
            }
        }

        #endregion
    }

    // Sample Usage of TreeNode
    class SampleData
    {
        public static TreeNode<string> GetSet1()
        {
            TreeNode<string> root = new TreeNode<string>("root");
            {
                TreeNode<string> node0 = root.AddChild("node0");
                TreeNode<string> node1 = root.AddChild("node1");
                TreeNode<string> node2 = root.AddChild("node2");
                {
                    TreeNode<string> node20 = node2.AddChild(null);
                    TreeNode<string> node21 = node2.AddChild("node21");
                    {
                        TreeNode<string> node210 = node21.AddChild("node210");
                        TreeNode<string> node211 = node21.AddChild("node211");
                    }
                }
                TreeNode<string> node3 = root.AddChild("node3");
                {
                    TreeNode<string> node30 = node3.AddChild("node30");
                }
            }

            return root;
        }
    }

    class SampleIterating
    {
        public void TestMain()
        {
            TreeNode<string> treeRoot = SampleData.GetSet1();
            foreach (TreeNode<string> node in treeRoot)
            {
                string indent = CreateIndent(node.Level);
                Console.WriteLine(indent + (node.Data ?? "null"));
            }
        }

        static void MainTest(string[] args)
        {
            TreeNode<string> treeRoot = SampleData.GetSet1();
            foreach (TreeNode<string> node in treeRoot)
            {
                string indent = CreateIndent(node.Level);
                Console.WriteLine(indent + (node.Data ?? "null"));
            }
        }

        private static String CreateIndent(int depth)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < depth; i++)
            {
                sb.Append(' ');
            }
            return sb.ToString();
        }
    }

    class SampleSearching
    {
        private void Main(string[] args)
        {
            TreeNode<string> treeRoot = SampleData.GetSet1();
            TreeNode<string> found = treeRoot.FindTreeNode(node => node.Data != null && node.Data.Contains("210"));

            Console.WriteLine("Found: " + found);
        }
    }

}
