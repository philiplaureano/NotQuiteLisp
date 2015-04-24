using System;
using System.Collections.Generic;
using NotQuiteLisp.AST.Interfaces;

namespace NotQuiteLisp.AST
{
    public class ScopedNode : AstNode, IScopeReference
    {
        private readonly AstNode _targetNode;
        private readonly IScope _targetScope;
        private readonly Func<AstNode, IScope, IScope> _cloneScope;

        public ScopedNode(AstNode targetNode, IScope targetScope, Func<AstNode, IScope, IScope> cloneScope)
        {
            _targetNode = targetNode;
            _targetScope = targetScope;
            _cloneScope = cloneScope;
        }

        public override IEnumerable<AstNode> Children
        {
            get { return _targetNode.Children; }
        }

        public override AstNode Clone()
        {
            var oldScope = _targetScope;
            var newScope = _cloneScope(_targetNode, oldScope);
            return new ScopedNode(_targetNode.Clone(), newScope, _cloneScope);
        }

        public IScope TargetScope
        {
            get { return _targetScope; }
        }
    }
}