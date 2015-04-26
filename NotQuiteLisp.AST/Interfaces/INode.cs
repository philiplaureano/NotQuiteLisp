namespace NotQuiteLisp.AST.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface INode<T>
    {
        Guid NodeId { get; }

        IEnumerable<INode<T>> Children { get; }

        INode<T> Clone();
    }
}