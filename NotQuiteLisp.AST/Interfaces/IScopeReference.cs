namespace NotQuiteLisp.AST.Interfaces
{
    public interface IScopeReference
    {
        IScope TargetScope { get; }
    }
}