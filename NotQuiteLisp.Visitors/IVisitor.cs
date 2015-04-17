namespace NotQuiteLisp.Visitors
{
    public interface IVisitor<in TSubject, out TResult>
    {
        TResult Visit(TSubject subject);
    }
}