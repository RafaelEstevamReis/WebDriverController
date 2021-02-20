# WebDriver Controller

This is a chainable wrapper around OpenQA.Selenium WebDriver to ease the use for small projects

Does this at least compile? 
\
![.NET](https://github.com/RafaelEstevamReis/WebDriverController/workflows/.NET/badge.svg)

[![NuGet](https://buildstats.info/nuget/RafaelEstevam.WebDriverController)](https://www.nuget.org/packages/RafaelEstevam.WebDriverController/)
[![The MIT License](https://img.shields.io/github/license/RafaelEstevamReis/WebDriverController)](https://github.com/RafaelEstevamReis/WebDriverController/blob/master/LICENSE)

------
Jump to:
- [WebDriver Controller](#webdriver-controller)
  - [Using](#using)
  - [Example](#example)

## Using

Import package, create a new WDController with your IWebDriver and done

Install the [NuGet package](https://www.nuget.org/packages/RafaelEstevam.WebDriverController): `Install-Package RafaelEstevam.WebDriverController`

I suggest using this package for crawled data storage: [SqliteWrapper](https://github.com/RafaelEstevamReis/SqliteWrapper) [![NuGet](https://buildstats.info/nuget/Simple.Sqlite)](https://www.nuget.org/packages/Simple.Sqlite)

## Example

Full working example:
~~~C#
using RafaelEstevam.WebDriverController;
using RafaelEstevam.WebDriverController.Actions;
using RafaelEstevam.WebDriverController.Extensions;

// use your driver of choice
using IWebDriver driver = new ChromeDriver();
var ctr = new Controller(driver);

ctr.GoTo("https://quotes.toscrape.com/")
   // wait until first quotes are visible (author clickable)
   .WaitUntil_IsClickable(By.LinkText("(about)")) //.Do(new WaitUntil(By.LinkText("(about)"), WaitUntil.Is.Clickable))
   .Interact(By.LinkText("Login"), (w, e) => //.Do(new Interact(By.LinkText("Login"), (w, e) =>
   {
       e.Click();
   })
   .WaitUntil_IsClickable(By.XPath("//form/input[2]"))
   //.Do(new WaitUntil(By.XPath("//form/input[2]"), WaitUntil.Is.Clickable))
   .Inspect((Controller c) =>
   {
       // You can change element's content
       c.FindElement(By.XPath("//form//label"))
        .SetInnerHTML(c, "Put the name down there...");

       c.FindElement(By.Id("username"))
        .SendKeys("me@myself.com");
       c.FindElement(By.Id("password"))
       .SendKeys("secret");

       // take proof
       var ss = c.GetScreenshot();
       //ss.SaveAsFile(...)

       c.FindElement(By.XPath("//form/input[2]"))
        .Submit();
   })
   .Inspect((controller, document) =>
   {
       Console.WriteLine("Logged in !");
       // all html is at:
       var html = controller.PageSource;
       // Extract all the things with HtmlAgilityPack html parsing !
       var node = document.DocumentNode;
   })
   // end the party
   .Quit();
~~~
[see full source](https://github.com/RafaelEstevamReis/WebDriverController/blob/main/WebDriverController.Tests/Program.cs)
