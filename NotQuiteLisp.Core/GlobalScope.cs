using NotQuiteLisp.AST.Interfaces;

namespace NotQuiteLisp.Core
{
    public class GlobalScope : Scope, INamedScope
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