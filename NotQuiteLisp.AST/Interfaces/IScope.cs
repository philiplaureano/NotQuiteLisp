using System.Collections.Generic;

namespace NotQuiteLisp.AST.Interfaces
{
    public interface IScope<TItem>
        where TItem : ISymbol
    {
        IScope<TItem> OuterScope { get; }
        TItem Resolve(string name);
        void Define(TItem symbol);

        IEnumerable<KeyValuePair<string, TItem>> Symbols { get; }
    }
}