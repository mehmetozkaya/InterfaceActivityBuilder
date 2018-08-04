using InterfaceActivityBuilder.Base;
using System.Collections.Generic;

namespace InterfaceActivityBuilder.Excel
{
    interface IInterfaceExcelReader
    {
        IList<CanonicItem> Read(string path);
    }
}
