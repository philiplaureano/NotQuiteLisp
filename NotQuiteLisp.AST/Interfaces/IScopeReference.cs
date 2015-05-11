namespace NotQuiteLisp.AST.Interfaces
{
    public interface IScopeReference<TItem>
        where TItem : ISymbol
    {
        IScope<TItem> TargetScope { get; }
    }
}