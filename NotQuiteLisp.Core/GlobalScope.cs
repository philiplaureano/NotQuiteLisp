using NotQuiteLisp.AST.Interfaces;

namespace NotQuiteLisp.Core
{
    public class GlobalScope<TItem> : Scope<TItem>, INamedScope<TItem>
        where TItem : ISymbol
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