using InterfaceActivityBuilder.Code;
using InterfaceActivityBuilder.Excel;
using InterfaceActivityBuilder.Tree;

namespace InterfaceActivityBuilder
{
    public class ClaroCanonicResolver : CanonicResolverBase
    {
        private readonly string _excelPath;
        private readonly IInterfaceExcelReader _excelReader;
        private ITreeBuilder _treeBuilder;
        private ICodePad _codePad;

        public ClaroCanonicResolver(string path)
        {
            _excelPath = path;
            _excelReader = new InterfaceExcelReader();
        }

        public override void Resolve()
        {
            //var canonicList = _excelReader.Read(_excelPath);

            //_treeBuilder = new TreeBuilder(canonicList);
            //_treeBuilder.Build();

            //_codePad = new CodePad(_treeBuilder.CanonicTree);
            //_codePad.Write();

            //var activityCode = _codePad.Code;
        }        
    }
}
