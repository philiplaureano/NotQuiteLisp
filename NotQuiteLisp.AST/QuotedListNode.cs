namespace NotQuiteLisp.AST
{
    using NotQuiteLisp.AST.Interfaces;

    public class QuotedListNode : ElementNode
    {
        private readonly ListNode _listNode;

        public QuotedListNode(ListNode listNode)
        {
            this._listNode = listNode;
        }

        public ListNode ListNode
        {
            get
            {
                return this._listNode;
            }
        }

        public override System.Collections.Generic.IEnumerable<INode<AstNode>> Children
        {
            get
            {
                yield return _listNode;
            }
        }

        public override INode<AstNode> Clone()
        {
            return new QuotedListNode((ListNode)_listNode.Clone());
        }
    }
}