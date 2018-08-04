using InterfaceActivityBuilder.Base;
using System.Collections.Generic;

namespace InterfaceActivityBuilder.Excel
{
    internal abstract class InterfaceExcelReaderBase : IInterfaceExcelReader
    {
        public abstract IList<CanonicItem> Read(string path);
    }
}
