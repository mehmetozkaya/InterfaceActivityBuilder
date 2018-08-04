using InterfaceActivityBuilder.Base;

namespace InterfaceActivityBuilder.Tree
{
    interface ITreeBuilder
    {
        void Build();
        TreeNode<CanonicTreeItem> CanonicTree { get; }
    }
}
