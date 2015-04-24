using NotQuiteLisp.AST.Interfaces;

namespace NotQuiteLisp.Core
{
    public class GlobalScope : Scope
    {
        public GlobalScope() : base(null)
        {
        }

        public override string Name
        {
            get { return "global"; }
        }
    }
}