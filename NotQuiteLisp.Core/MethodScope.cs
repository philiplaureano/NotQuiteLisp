using NotQuiteLisp.AST.Interfaces;

namespace NotQuiteLisp.Core
{
    public class MethodScope<TItem> : Scope<TItem>, INamedScope<TItem>
        where TItem : ISymbol
    {
        private readonly string _methodName;

        public MethodScope(string methodName, IScope<TItem> outerScope) : base(outerScope)
        {
            _methodName = methodName;
        }

        public override string Name
        {
            get { return _methodName; }
        }

        public string MethodName
        {
            get { return _methodName; }
        }
    }
}