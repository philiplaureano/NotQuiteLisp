using NotQuiteLisp.AST.Interfaces;

namespace NotQuiteLisp.Core
{
    public class AnonymousScope<TItem> : Scope<TItem>
        where TItem : ISymbol
    {
        public AnonymousScope(IScope<TItem> outerScope) : base(outerScope)
        {
        }

        public override string Name
        {
            get { return null; }
        }
    }
}