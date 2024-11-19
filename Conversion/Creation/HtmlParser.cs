using HtmlAgilityPack;
using MarkdownSharp;

namespace Conversion.Creation;

public class HtmlParser
{
    public string? Header1;
    
    public async Task<HtmlDocument> ParseAsync(string markdownFilePath)
    {
        var document = new HtmlDocument();
        string html = await GetMarkdownAsHtmlString(markdownFilePath);
        document.LoadHtml(html);
        
        const string xpath = "//*[self::h1 or self::h2 or self::h3]";
        var nodes = document.DocumentNode.SelectNodes(xpath);

        foreach (HtmlNode node in nodes)
        {
            SetHeader1(node.Name);
            
            string text = node.InnerText;

            string id = text
                .Trim()
                .ToLower()
                .Replace(" ", "-")
                .Replace("_", "-")
                .Replace(".", "-");

            node.Id = id;
        }

        return document;
    }

    private void SetHeader1(string nodeName)
    {
        if (Header1 != null)
           return;

        nodeName = nodeName.ToLower().Trim();
        if (nodeName != "h1")
            return;

        Header1 = nodeName;
    }
    
    private async Task<string> GetMarkdownAsHtmlString(string markdownFilePath)
    {
        FileInfo file = new FileInfo(markdownFilePath);
        
        using var reader = new StreamReader(file.FullName);
        string md = await reader.ReadToEndAsync();

        var markdownOptions = new MarkdownOptions
        {
            AutoHyperlink = false
        };
        
        var transformer = new Markdown(markdownOptions);
        string html = transformer.Transform(md);

        return html;
    }
}