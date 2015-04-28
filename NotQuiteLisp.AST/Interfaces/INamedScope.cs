namespace NotQuiteLisp.AST.Interfaces
{
    public interface INamedScope : IScope
    {
        string Name { get; }
    }
}