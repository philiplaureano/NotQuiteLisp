namespace NotQuiteLisp.AST
{
    public class KeyValuePairNode : PairNode
    {
        private readonly AtomNode _key;
        private readonly ElementNode _valueNode;

        public KeyValuePairNode(AtomNode key, ElementNode valueNode)
            : base(key, valueNode)
        {
            _key = key;
            _valueNode = valueNode;
        }

        public AtomNode Key
        {
            get
            {
                return _key;
            }
        }

        public ElementNode Value
        {
            get
            {
                return _valueNode;
            }
        }
    }
}