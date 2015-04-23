using System.Collections.Generic;
using NotQuiteLisp.AST.Interfaces;

namespace NotQuiteLisp.AST
{
    public class MethodDefinitionNode : SymbolNode, IScopeContainer
    {
        private readonly string _methodName;
        private readonly IEnumerable<ParameterDefinitionNode> _parameters;
        private readonly AstNode _methodBody;

        public MethodDefinitionNode(string methodName, IEnumerable<ParameterDefinitionNode> parameters, AstNode methodBody) : base(methodName)
        {
            _methodName = methodName;
            _parameters = parameters;
            _methodBody = methodBody;
        }

        public AstNode MethodBody
        {
            get { return _methodBody; }
        }

        public IEnumerable<ParameterDefinitionNode> Parameters
        {
            get { return _parameters; }
        }

        public string MethodName
        {
            get { return _methodName; }
        }
    }
}