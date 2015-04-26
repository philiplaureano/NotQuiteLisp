using NotQuiteLisp.AST;
using NotQuiteLisp.Visitors;

namespace NotQuiteLisp.Core
{
    using System.Linq.Expressions;

    using NotQuiteLisp.AST.Interfaces;

    public class ScopeBuilder : IVisitor<AstNode, IScope>
    {
        private readonly IScope _rootScope;

        public ScopeBuilder(IScope rootScope)
        {
            this._rootScope = rootScope;
        }

        public IScope Visit(AstNode subject)
        {
            return (IScope)this.Invoke("GetScope", subject);
        }

        public void SetScope(SymbolNode node, IScope parentScope)
        {
            parentScope.Define(node);
        }

        public void SetScope(ListNode node, IScope parentScope)
        {
            foreach (var child in node.Children)
            {
                this.Invoke("SetScope", child, _rootScope);
            }
        }

        public IScope GetScope(RootNode node)
        {
            foreach (var child in node.Children)
            {
                this.Invoke("SetScope", child, _rootScope);
            }

            return new BoundScope(_rootScope, node);
        }
    }
}