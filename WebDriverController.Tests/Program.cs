using System;

using (IWebDriver driver = new ChromeDriver())
{
    var homeURL = "http://quotes.toscrape.com/";

    var ctr = new WDController(driver);

    ctr.Do(new Actions.Redirect(homeURL))
       .Inspect((WDController c, IWebDriver d) => { })
       ;


}

Console.WriteLine("-END-");
Console.ReadKey();