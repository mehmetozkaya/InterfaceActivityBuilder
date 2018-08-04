using System.Linq;

namespace InterfaceActivityBuilder.Base
{
    internal class AttributeCanonicItem : CanonicItem
    {
        public AttributeCanonicItem(string name, string value) : base(name, value)
        {
            PopulateAttributes();
        }

        private void PopulateAttributes()
        {
            // attributeName (agg="antiguedadCliente")
            // attributeName (agg='antiguedadCliente')
            // attributeName (agg=antiguedadCliente)

            var attributeName = Attributes.Last();

            var modifiedName = attributeName.Split('(')?.FirstOrDefault()?.Trim();
            var modifiedValue = string.Empty;

            var attrArray = attributeName.Split('"');
            if (attrArray.Length > 1)
            {
                modifiedValue = attrArray?[1]?.Trim();
            }
            else
            {
                attrArray = attributeName.Split('\'');
                if (attrArray.Length > 1)
                {
                    modifiedValue = attrArray?[1]?.Trim();
                }
                else
                {
                    attrArray = attributeName.Split('=');
                    if (attrArray.Length > 1)
                    {
                        modifiedValue = attrArray?[1]?.Replace(")", "").Trim();
                    }
                }
            }

            Attributes[Attributes.Count - 1] = modifiedName;
            Name = modifiedValue + AttributeKeyword;
        }

        private const string AttributeKeyword = "_attributeName";
    }
}
