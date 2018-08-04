using System;
using System.IO;
using System.Linq;
using System.Text;
using InterfaceActivityBuilder.Base;
using InterfaceActivityBuilder.Tree;

namespace InterfaceActivityBuilder.Code
{
    internal class CodePad : ICodePad
    {
        private readonly TreeNode<CanonicTreeItem> _canonicTree;
        private readonly StringBuilder _code;

        internal CodePad(TreeNode<CanonicTreeItem> canonicTree)
        {
            _canonicTree = canonicTree;
            _code = new StringBuilder();

            //TODO : Build class first part
            BuildActivityClass();
        }

        private void BuildActivityClass()
        {
            var rootName = _canonicTree.Data.NodeName;

            string mainName = rootName.Replace("RequestMessage", "");
            string typeName = rootName.Replace("Message", "Type");
            string proxyNamespace = _canonicTree.Data.NodeType.Namespace;
            var nativeName = mainName.Substring(0, 1).ToLower() + mainName.Substring(1);

            var className = $"AmxPeru{mainName}Activity";
            var requestClassName = $"AmxPeru{mainName}RequestDTO";
            var responseClassName = $"AmxPeru{mainName}ResponseDTO";

            _code.AppendLine("using System;");
            _code.AppendLine("using System.Activities;");
            _code.AppendLine("using System.Collections.Generic;");
            _code.AppendLine("using System.Linq;");
            _code.AppendLine($"using {proxyNamespace};");
            _code.AppendLine("");
            _code.AppendLine("namespace AmxPeruPSBActivities.Activities.External");
            _code.AppendLine("{");
            _code.Append("\t");
            _code.AppendLine($"public class {className} : AmxPeruExternalInterfaceActivityBase<{mainName}Port, {nativeName}Request, {nativeName}Response>");
            _code.Append("\t");
            _code.AppendLine("{");
            _code.Append("\t");
            _code.Append("\t");
            _code.AppendLine($"public InArgument<{requestClassName}> Request " + "{ get; set; }");
            _code.Append("\t");
            _code.Append("\t");
            _code.AppendLine($"public OutArgument<{responseClassName}> Response " + "{ get; set; }");
            _code.AppendLine("");

            _code.Append("\t");
            _code.Append("\t");
            _code.AppendLine($"public override {requestClassName} CreateRequest()");
            _code.Append("\t");
            _code.Append("\t");
            _code.AppendLine("{");
            _code.Append("\t");
            _code.Append("\t");
            _code.Append("\t");
            _code.AppendLine($"var requestDTO = Request.Get(Context);");
            _code.Append("\t");
            _code.Append("\t");
            _code.Append("\t");
            _code.AppendLine($"var requestMessage = new {typeName}();");
            _code.AppendLine("");
        }

        public void Write()
        {
            foreach (var node in _canonicTree)
            {
                if (node.IsRoot)
                    continue;

                if (node.Level == 1)
                {
                    _code.AppendLine($"requestMessage.{node.Data.NodeName} = new {node.Data.NodeType}");
                    _code.AppendLine("{");
                    continue;
                }

                if (node.IsLeaf)
                {
                    // if attributename parameter dont need to use dto
                    if (node.Data.CanonicName.Contains(AttributeKeyword))
                    {
                        var attributeName = node.Data.CanonicName.Replace(AttributeKeyword, string.Empty);

                        AddTabs(node.Level);
                        _code.AppendLine($"{node.Data.NodeName} = \"{attributeName}\"");
                    }
                    else
                    {
                        AddTabs(node.Level);
                        _code.AppendLine($"{node.Data.NodeName} = requestDTO.{node.Data.CanonicName}");
                    }

                    CloseBrackets(node);
                    continue;
                }

                if (node.Data.NodeType.IsArray)
                {
                    AddTabs(node.Level);
                    string value = string.Format("{0} = {1}new {2}", node.Data.NodeName, node.Data.NodeType.IsArray ? "ArrayWith(" : string.Empty, node.Data.NodeType.ToString().Replace("[]", ""));
                    _code.AppendLine(value);

                    AddTabs(node.Level);
                    _code.AppendLine("{");
                    continue;
                }

                if (node.Data.NodeName.Contains(ArrayItemKeyword))
                {
                    if (node.Data.NodeName.Contains(ArrayItem_1_Keyword))
                    {
                        var firstIndex = _code.ToString().LastIndexOf("ArrayWith(") + "ArrayWith(".Length;
                        var secondIndex = _code.ToString().LastIndexOf("{") + 1;
                        _code.Remove(firstIndex, secondIndex - firstIndex);
                    }

                    AddTabs(node.Level);
                    _code.AppendLine($"new {node.Data.NodeType}");

                    AddTabs(node.Level);
                    _code.AppendLine("{");
                    continue;
                }

                // default
                AddTabs(node.Level);
                _code.AppendLine($"{node.Data.NodeName} = new {node.Data.NodeType}");

                AddTabs(node.Level);
                _code.AppendLine("{");
            }

            CloseActivityRequestMethod();
            DownloadFile();
        }
        private void DownloadFile()
        {
            var rootName = _canonicTree.Data.NodeName;
            string mainName = rootName.Replace("RequestMessage", "");
            string typeName = rootName.Replace("Message", "Type");
            string proxyNamespace = _canonicTree.Data.NodeType.Namespace;
            var className = $"AmxPeru{mainName}Activity";

            try
            {
                StreamWriter sw = new StreamWriter($"C:\\{className}.cs");
                sw.WriteLine(_code);
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        private void CloseActivityRequestMethod()
        {
            var rootName = _canonicTree.Data.NodeName;
            string mainName = rootName.Replace("RequestMessage", "");
            string typeName = rootName.Replace("Message", "Type");
            string proxyNamespace = _canonicTree.Data.NodeType.Namespace;
            var nativeName = mainName.Substring(0, 1).ToLower() + mainName.Substring(1);

            _code.AppendLine("");
            _code.Append("\t");
            _code.Append("\t");
            _code.Append("\t");
            _code.AppendLine($"return new {nativeName}Request");
            _code.Append("\t");
            _code.Append("\t");
            _code.Append("\t");
            _code.AppendLine("{");
            _code.Append("\t");
            _code.Append("\t");
            _code.Append("\t");
            _code.Append("\t");
            _code.AppendLine($"{mainName}RequestMessage = requestMessage");
            _code.Append("\t");
            _code.Append("\t");
            _code.Append("\t");
            _code.AppendLine("};");
            _code.Append("\t");
            _code.Append("\t");
            _code.AppendLine("}");

            _code.Append("\t");
            _code.Append("\t");
            _code.AppendLine($"public override void MapResponse({nativeName}Response channelResponse)");
            _code.Append("\t");
            _code.Append("\t");
            _code.AppendLine("{");
            _code.Append("\t");
            _code.Append("\t");
            _code.Append("\t");
            _code.AppendLine($"throw new NotImplementedException();");
            _code.Append("\t");
            _code.Append("\t");
            _code.AppendLine("}");
            _code.Append("\t");
            _code.AppendLine("}");
            _code.AppendLine("}");
        }

        private void AddTabs(int level)
        {
            for (int i = 0; i < level - 1; i++)
            {
                _code.Append("\t");
            }
        }

        private void CloseBrackets(TreeNode<CanonicTreeItem> node)
        {
            if (node == null)
                return;

            if (node.Level < 2)
            {
                PutPunctiation(";");
                return;
            }

            try
            {
                if (!node.HasSiblings)
                {
                    AddTabs(node.Level - 1);
                    if (!node.Parent.Data.NodeType.IsArray)
                    {
                        _code.AppendLine("}");
                    }
                    else
                    {
                        if (node.Data.NodeName.Contains(ArrayItemKeyword))
                            _code.AppendLine(").ToArray()");
                        else
                            _code.AppendLine("}).ToArray()");
                    }

                    if (node.Parent != null)
                        CloseBrackets(node.Parent);
                }
                else
                {
                    PutPunctiation(",");
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void PutPunctiation(string punctiation)
        {
            _code.Insert(_code.ToString().Trim().Length, punctiation);
        }

        public string Code => _code.ToString();

        private const string AttributeKeyword = "_attributeName";
        private const string ArrayItemKeyword = "arrayItem";
        private const string ArrayItem_1_Keyword = "arrayItem1";


        ///////////////////////// ///////////////////////// ///////////////////////// ///////////////////////// /////////////////////////
        // Remove this part
        public void WriteRemove()
        {
            foreach (var node in _canonicTree.Where(p => p.Level == 1))
            {
                _code.AppendLine($"requestMessage.{node.Data.NodeName} = new {node.Data.NodeType}");
                _code.AppendLine("{");
                _code.AppendLine(PrepareTreeNodes(node));
                _code.AppendLine("};");
                continue;
            }
        }
        private string PrepareTreeNodes(TreeNode<CanonicTreeItem> node)
        {
            var itemCode = new StringBuilder();

            foreach (var child in node.Children)
            {
                if (child.IsLeaf)
                {
                    AddTabs2(child.Level, itemCode);
                    itemCode.AppendLine($"{child.Data.NodeName} = requestDTO.{child.Data.CanonicName}");
                    PutPunctiation2(child.Level, itemCode);
                    continue;
                }

                AddTabs2(child.Level, itemCode);
                itemCode.AppendLine($"{node.Data.NodeName} = new {node.Data.NodeType}");
                AddTabs2(child.Level, itemCode);
                itemCode.AppendLine("{");
                AddTabs2(child.Level, itemCode);
                itemCode.AppendLine(PrepareTreeNodes(child));
                AddTabs2(child.Level, itemCode);
                itemCode.AppendLine("}");
            }
            return itemCode.ToString();
        }
        private void AddTabs2(int level, StringBuilder item)
        {
            for (int i = 0; i < level - 1; i++)
            {
                item.Append("\t");
            }
        }
        private void PutPunctiation2(int level, StringBuilder item)
        {
            var IslastBracket = level - 1 == 1;
            var punctiation = string.Empty;
            if (IslastBracket)
                return; // punctiation = ";";
            else
                punctiation = ",";

            item.Insert(item.ToString().Trim().Length, punctiation);
        }

        ///////////////////////// ///////////////////////// ///////////////////////// ///////////////////////// /////////////////////////
    }
}
