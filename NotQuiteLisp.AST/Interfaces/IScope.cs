using System.Collections.Generic;

namespace NotQuiteLisp.AST.Interfaces
{
    public interface IScope<TItem>
    {
        IScope<TItem> OuterScope { get; }
        TItem Resolve(string name);
        void Define(string name, TItem item);

        IEnumerable<KeyValuePair<string, TItem>> Symbols { get; }
    }
}