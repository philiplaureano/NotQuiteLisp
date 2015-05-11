namespace NotQuiteLisp.AST.Interfaces
{
    public interface INamedScope<TItem> : IScope<TItem>, INameable
        where TItem : ISymbol
    {        
    }
}