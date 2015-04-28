using NotQuiteLisp.AST.Interfaces;

namespace NotQuiteLisp.Core
{
    public class MethodScope : Scope, INamedScope
    {
        private readonly string _methodName;

        public MethodScope(string methodName, IScope outerScope) : base(outerScope)
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