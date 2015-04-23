namespace NotQuiteLisp.AST.Interfaces
{
    public interface IScope
    {
        string Name { get; }
        IScope OuterScope { get; }
        void Define(SymbolNode symbol);
        SymbolNode Resolve(string name);
    }    
}