using System.Text;
using Conversion.Configuration;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;

namespace Conversion.Creation;

public class Converter
{
    private readonly StringBuilder builder = new();
    private readonly ConfigurationOptions options;

    public Converter(IOptions<ConfigurationOptions> options)
    {
        this.options = options.Value;
    }
    
    public async Task<string> ConvertAsync(string markdownFilePath)
    {
        var parser = new HtmlParser();
        HtmlDocument document = await parser.ParseAsync(markdownFilePath);
        string content = document.DocumentNode.OuterHtml;
        
        BuildHead(content);
        
        string toc = TocBuilder.BuildToc(document);
        BuildBody(content, toc);
        
        builder.Append("</html>");
        
        return builder.ToString();
    }
    
    #region Body
    
    private void BuildBody(string content, string toc)
    {
        builder.Append("<body class=\"stackedit\">");
        AddStyles();
        
        //toc
        builder.Append("<div class=\"stackedit__left\">");
        builder.Append("<div class=\"stackedit__toc\">");
        builder.Append(toc);
        builder.Append("</div>");
        builder.Append("</div>");
        
        //content
        builder.Append("<div class=\"stackedit__right\">");
        builder.Append("<div class=\"stackedit__html\">");
        builder.Append(content);
        builder.Append("</div>");
        builder.Append("</div>");
        
        AddJavaScript();
        builder.Append("</body>");
    }

    private void AddJavaScript()
    {
        builder.Append("<script>");
        builder.Append("</script>");
    }
    
    private void AddStyles()
    {
        using var reader = new StreamReader(options.StyleSheetFilePath);
        string styles = reader.ReadToEnd();
        
        builder.Append("<style>");
        builder.Append(styles);
        builder.Append("</style>");
    }

    #endregion


    /// <summary>
    /// Builds head and meta parts.
    /// </summary>
    /// <param name="content"></param>
    private void BuildHead(string content)
    {
        string title = GetHtmlTagContent(content, "<h1>","</h1>");
        const string parts1 = "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>";
        
        builder.Append(parts1);
        builder.Append(title);
        builder.Append("</title></head>");
    }

    private static string GetHtmlTagContent(string html, string startFlag, string endFlag)
    {
        int start = html.IndexOf(startFlag, StringComparison.OrdinalIgnoreCase) + startFlag.Length;
        int end = html.IndexOf(endFlag, StringComparison.OrdinalIgnoreCase) - endFlag.Length + 1;

        string content = html.Substring(start, end);

        return content;
    }
    
    
}