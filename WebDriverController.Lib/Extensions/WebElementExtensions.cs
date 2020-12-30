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

        public static IWebElement SetInnerHTML(this IWebElement element, Controller controller, string Html)
        {
            string script = $"arguments[0].innerHTML=arguments[1]";
            controller.ExecuteScript(script, element, Html);
            return element;
        }
        public static IWebElement SetOuterHTML(this IWebElement element, Controller controller, string Html)
        {
            string script = $"arguments[0].outerHTML=arguments[1]";
            controller.ExecuteScript(script, element, Html);
            return element;
        }

        public static HtmlNode GetHtmlDocument(this IWebElement element)
        {
            return HtmlNode.CreateNode(element.GetOuterHTML());
        }       

    }
}
