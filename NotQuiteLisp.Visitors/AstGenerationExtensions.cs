using ANTLR4.ParserHelpers;
using NotQuiteLisp.AST;
using NotQuiteLisp.AST.Interfaces;
using NotQuiteLisp.Parser;

namespace NotQuiteLisp.Visitors
{
    public static class AstGenerationExtensions
    {
        public static INode<AstNode> CreateAstNodes(this string inputText)
        {
            var tree = inputText.ParseWith<NqlLanguage>();
            var converter = new ParseTreeConverter();

            return converter.Visit(tree);
        }
    }
}