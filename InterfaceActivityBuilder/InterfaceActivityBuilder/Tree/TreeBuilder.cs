using System;
using System.Linq;
using System.Collections.Generic;
using InterfaceActivityBuilder.Base;

namespace InterfaceActivityBuilder.Tree
{
    internal class TreeBuilder : ITreeBuilder
    {
        private readonly IList<CanonicItem> _canonicList;
        public TreeNode<CanonicTreeItem> CanonicTree { get; private set; }

        public TreeBuilder(IList<CanonicItem> canonicList)
        {
            _canonicList = canonicList;
            Initialize();
        }

        private void Initialize()
        {
            var rootTypeName = _canonicList.First().Root;
            var rootType = ReflectionHelper.GetRequestType(rootTypeName);

            CanonicTreeItem canonicTreeItem = new CanonicTreeItem(rootTypeName, rootTypeName, rootType);
            CanonicTree = new TreeNode<CanonicTreeItem>(canonicTreeItem);
        }

        public void Build()
        {
            try
            {
                foreach (var canonic in _canonicList)
                {
                    AddChildToTree(CanonicTree, canonic);
                }

                // Remove this
                var str = CanonicTree.ToString();
            }
            catch (Exception exception)
            {
                throw new Exception("Error while building tree : " + exception.Message);
            }
        }

        private void AddChildToTree(TreeNode<CanonicTreeItem> canonicTree, CanonicItem canonic)
        {
            if (canonic == null || canonic.Attributes == null || canonic.Attributes.Count == 0)
                return;

            var firstItem = canonic.Attributes.First();
            TreeNode<CanonicTreeItem> foundNode = canonicTree.FindTreeNode(node => node.Data.NodeName == firstItem);
            if (foundNode == null)
            {
                var propertyType = ReflectionHelper.GetCurrentPropertyType(canonicTree.Data, firstItem);
                CanonicTreeItem treeItem = new CanonicTreeItem(firstItem, canonic.Name, propertyType);
                var currentNode = canonicTree.AddChild(treeItem);

                canonic.Attributes.RemoveAt(0);
                AddChildToTree(currentNode, canonic);
            }
            else
            {
                canonic.Attributes.RemoveAt(0);
                AddChildToTree(foundNode, canonic);
            }
        }
    }
}
