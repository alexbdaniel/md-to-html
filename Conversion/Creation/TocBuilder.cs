using System.Diagnostics.CodeAnalysis;
using System.Text;
using HtmlAgilityPack;

namespace Conversion.Creation;

public class TocItem
{
    public string Id { get; private init; }
    public string InnerText { get; private init; }
    public UInt16 Level { get; private init; }
    public List<TocItem> Children { get; set; }
    
    public TocItem(string id, string innerText, UInt16 level, List<TocItem>? children = null)
    {
        Id = id;
        InnerText = innerText;
        Level = level;
        Children = children ?? [];
    }
}

[SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
public static class TocBuilder
{
    public static string BuildToc(HtmlDocument document)
    {
        List<TocItem> tocItems = GetTocItems(document);
        
        var builder = new StringBuilder();
        
        UInt16 recCount = 0;
        builder.Append("<ul>");
        BuildTocString(tocItems, ref recCount, ref builder);
        builder.Append("</ul>");
        
        return builder.ToString();
    }

    private static void BuildTocString(List<TocItem> tocItems, ref UInt16 recCount, ref StringBuilder builder)
    {
        if (++recCount > 1000)
            throw new Exception("Recursion limit exceeded.");
        
        foreach (TocItem header in tocItems)
        {
            builder.Append("<li>");
            builder.Append($"<a href=\"#{header.Id}\">{header.InnerText}</a>");
            
            if (header.Children.Count > 0)
            {
                builder.Append("<ul>");
                BuildTocString(header.Children, ref recCount, ref builder);
                builder.Append("</ul>");
            }

            builder.Append("</li>");
        }
    }
    
    private static List<TocItem> GetTocItems(HtmlDocument document)
    {
        const string xpath = "//*[self::h2 or self::h3]";
        var nodes = document.DocumentNode.SelectNodes(xpath);
        
        var headers = new List<TocItem>();
        
        UInt16 previousLevel = 2;
        
        foreach (HtmlNode node in nodes)
        {
            UInt16 currentLevel = GetNodeLevel(node.Name);

            if (currentLevel == 2)
            { 
                var tocItem = new TocItem(node.Id, node.InnerText, currentLevel);
                headers.Add(tocItem);
                previousLevel = currentLevel;
                continue;
            }
            
            if (currentLevel >= previousLevel)
            {
                TocItem currentItem = new TocItem(node.Id, node.InnerText, currentLevel);
                headers.Last().Children.Add(currentItem);
            }


            previousLevel = currentLevel;
        }

        return headers;
    }
    
    private static UInt16 GetNodeLevel(string nodeName)
    {
        nodeName = nodeName.ToLower().Trim();
        bool parsed = UInt16.TryParse(nodeName.Replace("h", ""), out UInt16 level);
        if (parsed)
            return level;

        return 0;
    }
    
    private static string GetId(string innerText) =>
        innerText
            .Trim()
            .ToLower()
            .Replace(" ", "-")
            .Replace("_", "-")
            .Replace(".", "-");
}