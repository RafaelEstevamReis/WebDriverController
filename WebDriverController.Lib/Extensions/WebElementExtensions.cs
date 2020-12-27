using HtmlAgilityPack;
using OpenQA.Selenium;

namespace RafaelEstevam.WebDriverController.Extensions
{
    public static class WebElementExtensions
    {
        public static string GetInnerHtml(this IWebElement element)
        {
            // https://quirksmode.org/dom/html/
            return element.GetAttribute("innerHTML");
        }
        public static string GetOuterHTML(this IWebElement element)
        {
            // https://quirksmode.org/dom/html/
            return element.GetAttribute("outerHTML");
        }
        public static HtmlNode GetHtmlDocument(this IWebElement element)
        {
            return HtmlNode.CreateNode(element.GetOuterHTML());
        }       

    }
}
