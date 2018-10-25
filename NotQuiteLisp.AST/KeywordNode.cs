namespace NotQuiteLisp.AST
{
    using NotQuiteLisp.AST.Interfaces;

    public class KeywordNode : AtomNode
    {
        private readonly string _keyword;

        public KeywordNode(string keyword)
        {
            _keyword = keyword;
        }

        public string Keyword
        {
            get
            {
                return _keyword;
            }
        }

        public override INode<AstNode> Clone()
        {
            return new KeywordNode(_keyword);
        }
    }
}