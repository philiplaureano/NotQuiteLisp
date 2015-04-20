namespace NotQuiteLisp.Core
{
    public class AnonymousScope : Scope
    {
        public AnonymousScope(IScope outerScope) : base(outerScope)
        {
        }

        public override string Name
        {
            get { return null; }
        }
    }
}