using System.Collections.Generic;
using System.Linq;

namespace InterfaceActivityBuilder.Base
{
    internal class CanonicItem
    {
        public string Name { get; protected set; }
        public string Value { get; private set; }
        public string Root { get; private set; }
        public IList<string> Attributes { get; protected set; }

        public CanonicItem(string name, string value)
        {
            Name = name;
            Value = value;
            PopulateAttributes();
        }

        private void PopulateAttributes()
        {
            var canonicValues = Value.Split(Spliter).ToList();
            Root = canonicValues.FirstOrDefault();
            canonicValues.RemoveAt(0);

            if (Value.Contains(ArraySpliter))
                canonicValues = AddArrayItems(canonicValues);

            Attributes = canonicValues;
        }

        private List<string> AddArrayItems(List<string> canonicValues)
        {
            // ConsultarRetencionEnrutamientoRequestMessage / _solicitud / _employees[1] / _partyRoleGroup / name
            // ConsultarRetencionEnrutamientoRequestMessage / _solicitud / _employees[2] / _partyRoleGroup / name

            List<string> newAttributeList = new List<string>();

            foreach (var item in canonicValues)
            {
                if (item.Contains(ArraySpliter))
                {
                    var number = item.Substring(item.IndexOf('[') + 1, 1);
                    var newItem = item.Remove(item.IndexOf('['));

                    newAttributeList.Add(newItem);
                    newAttributeList.Add(newItem + arraySuffix + number);
                }
                else
                {
                    newAttributeList.Add(item);
                }
            }
            return newAttributeList;
        }

        public override string ToString()
        {
            return string.Format("[Canonic: {0} - {1}]", Name, Value);
        }

        private const char Spliter = '/';
        private const string ArraySpliter = "[";
        private const string arraySuffix = "_arrayItem";
    }
}
