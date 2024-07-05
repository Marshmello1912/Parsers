using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Threading.Tasks;

namespace parserJSE
{
    internal class Parser
    {
        IConfiguration config;
        IBrowsingContext context;

        public Parser()
        {
            config = Configuration.Default;
            context = BrowsingContext.New(config);
        }

        public async Task<IHtmlCollection<IElement>> GetElems(String htmlDoc, String selectorTemplate)
        {
            IDocument document = await context.OpenAsync(req => req.Content(htmlDoc));
            IHtmlCollection<IElement> elements = document.QuerySelectorAll(selectorTemplate);

            return elements;
        }
    }
}
