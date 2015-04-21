using NotQuiteLisp.AST;
using NotQuiteLisp.Visitors;

namespace NotQuiteLisp.AstTests
{
    public class SampleSilentVisitor : AstVisitor<int>
    {
        private int _numberOfTimesCalled;

        public int NumberOfTimesCalled
        {
            get { return _numberOfTimesCalled; }
        }

        public override int Visit(AstNode subject)
        {
            _numberOfTimesCalled = NumberOfTimesCalled + 1;
            return Visit(subject, false);
        }
    }
}