using System;

namespace InterfaceActivityBuilder.Base
{
    public class CanonicTreeItem
    {
        public string NodeName { get; private set; }
        public string CanonicName { get; private set; }
        public Type NodeType { get; private set; }

        public CanonicTreeItem(string nodeName, string canonicName, Type nodeType)
        {
            NodeName = nodeName;
            CanonicName = canonicName;
            NodeType = nodeType;
        }

        public override string ToString()
        {
            return string.Format("[CanonicTree: {0} - {1} - {2}]", NodeName, CanonicName, NodeType.Name);
        }
    }
}