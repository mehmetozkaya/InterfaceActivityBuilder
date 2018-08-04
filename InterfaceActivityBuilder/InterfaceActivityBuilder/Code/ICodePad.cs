using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceActivityBuilder.Code
{
    interface ICodePad
    {
        void Write();
        string Code { get; }
    }
}
