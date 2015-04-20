using NotQuiteLisp.AST;
using NotQuiteLisp.Visitors;

namespace NotQuiteLisp.AstTests
{
    public class SampleVisitor : AstVisitor<int>
    {
        private int _numberOfTimesCalled;

        public int NumberOfTimesCalled
        {
            get { return _numberOfTimesCalled; }
        }

        public int Visit(SymbolNode node)
        {
            _numberOfTimesCalled = NumberOfTimesCalled + 1;
            return 0;
        }
    }
}