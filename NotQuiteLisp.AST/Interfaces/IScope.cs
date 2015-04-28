using System.Collections.Generic;

namespace NotQuiteLisp.AST.Interfaces
{
    public interface IScope
    {
        IScope OuterScope { get; }
        SymbolNode Resolve(string name);
        void Define(SymbolNode symbol);

        IEnumerable<KeyValuePair<string, SymbolNode>> Symbols { get; }
    }
}