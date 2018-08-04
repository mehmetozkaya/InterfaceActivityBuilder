using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using InterfaceActivityBuilder.Base;

namespace InterfaceActivityBuilder.Excel
{
    internal class InterfaceExcelReader : InterfaceExcelReaderBase
    {
        public IList<CanonicItem> CanonicList { get; private set; }

        public InterfaceExcelReader()
        {
            CanonicList = new List<CanonicItem>();
        }

        public override IList<CanonicItem> Read(string path)
        {
            try
            {
                var excelTable = ReadExcelToTable(path);
                ExcelToDictionary(excelTable);
                return CanonicList;
            }
            catch (Exception exception)
            {
                throw new Exception("Error while reading excel file : " + exception.Message);
            }
        }

        private void ExcelToDictionary(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                var canonicKey = row[Column_0]?.ToString();
                var canonicValue = row[Column_1]?.ToString();

                if (!canonicValue.Contains(EscapeWord))
                    continue;

                if (canonicValue.Contains(AttributeSpliter)) // Assumption : Attribute rows should be seperate by -- keywords.
                {
                    var attributeList = canonicValue.Split(new string[] { AttributeSpliter }, StringSplitOptions.RemoveEmptyEntries);
                    canonicValue = attributeList.FirstOrDefault().Trim();

                    var attributeValue = attributeList[1].Trim();

                    CanonicList.Add(new AttributeCanonicItem(canonicKey, attributeValue));
                }

                CanonicList.Add(new CanonicItem(canonicKey, canonicValue));
            }
        }

        private DataTable ReadExcelToTable(string path)
        {
            DataSet result = null;
            //using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            //{
            //    using (var reader = ExcelReaderFactory.CreateReader(stream))
            //    {
            //        do
            //        {
            //            while (reader.Read())
            //            {
            //            }
            //        } while (reader.NextResult());

            //        result = reader.AsDataSet();
            //    }
            //}
            return result.Tables[0];
        }

        private const string EscapeWord = "RequestMessage";
        private const string Column_0 = "Column0";
        private const string Column_1 = "Column1";
        private const string AttributeSpliter = "--";
        private const string ArraySpliter = "[";
    }
}
