namespace NotQuiteLisp.AST
{
    public class KeyValuePairNode : PairNode
    {
        private readonly AtomNode _key;
        private readonly ElementNode _valueNode;

        public KeyValuePairNode(AtomNode key, ElementNode valueNode)
            : base(key, valueNode)
        {
            this._key = key;
            this._valueNode = valueNode;
        }

        public AtomNode Key
        {
            get
            {
                return this._key;
            }
        }

        public ElementNode Value
        {
            get
            {
                return this._valueNode;
            }
        }
    }
}