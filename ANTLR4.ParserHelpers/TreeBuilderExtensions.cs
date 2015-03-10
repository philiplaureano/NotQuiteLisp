namespace ANTLR4.ParserHelpers
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;

    public static class TreeBuilderExtensions
    {
        public static IParseTree CreateParseTree(this ITreeBuilder builder, string inputText)
        {
            var input = new AntlrInputStream(inputText);
            return builder.CreateTree(input);
        }
    }
}