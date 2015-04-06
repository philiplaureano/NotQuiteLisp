namespace NotQuiteLisp.AST
{
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

        public override System.Collections.Generic.IEnumerable<AstNode> Children
        {
            get
            {
                yield return _listNode;
            }
        }
    }
}