namespace ANTLR4.ParserHelpers
{
    using Antlr4.Runtime.Tree;

    public static class StringExtensions
    {
        public static IParseTree ParseWith<TLanguage>(this string inputText)
            where TLanguage : ILanguageStrategy, new()
        {
            var languageStrategy = new TLanguage();
            var treeBuilder = new TreeBuilder(languageStrategy);
            return treeBuilder.CreateParseTree(inputText);
        }
    }
}