using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RafaelEstevam.WebDriverController;
using RafaelEstevam.WebDriverController.Actions;
using RafaelEstevam.WebDriverController.Extensions;

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
       // Change the html contents of the 'Username' Label
       c.FindElement(By.XPath("//form//label"))
        .SetInnerHTML(c, "Put the name down there...");
       // Fill credentials
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

Console.WriteLine("-END-");
Console.ReadKey();
