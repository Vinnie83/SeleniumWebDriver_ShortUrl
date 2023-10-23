using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests
{
    public class SeleniumTests
    {
       
        private WebDriver driver;
        private const string baseUrl = "https://shorturl1.velinski.repl.co/urls";
        


        [SetUp]

        public void OpenWebApp()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);  
            driver.Manage().Window.Maximize();
            driver.Url = baseUrl;
        }

        [TearDown]
        
        public void CloseWebApp()
        {
            driver.Quit();  
        }

        [Test]  

        public void Test_TableTopLeftCell()
        {
           var linkShortUrl = driver.FindElement(By.LinkText("Short URLs")); 
           linkShortUrl.Click();

            var tableHeaderLeftCell = driver.FindElement(By.CssSelector("tr > th:nth-child(1)"));

            Assert.That(tableHeaderLeftCell.Text, Is.EqualTo("Original URL")); 
        }

        [Test]

        public void Test_AddValidUrl()
        {
            var linkAddUrl = driver.FindElement(By.LinkText("Add URL"));
            linkAddUrl.Click();

            var inputAddUrl = driver.FindElement(By.Id("url"));
            inputAddUrl.SendKeys("https://www.derwesten.de/ ");

            var buttonCreate = driver.FindElement(By.XPath("//button[@type='submit']"));
            buttonCreate.Click();   

            Assert.That(driver.PageSource.Contains("https://www.derwesten.de/"));

            var tableLastRow = driver.FindElements(By.CssSelector("table > tbody > tr")).Last();
            var tableLastRowFirstCell = tableLastRow.FindElements(By.CssSelector("td")).First();

            Assert.That(tableLastRowFirstCell.Text, Is.EqualTo("https://www.derwesten.de/"));

        }

        [Test]
        public void Test_AddInvalidUrl()
        {
            var linkAddUrl = driver.FindElement(By.LinkText("Add URL"));
            linkAddUrl.Click();

            var inputAddUrl = driver.FindElement(By.Id("url"));
            inputAddUrl.SendKeys("bbbbbb");

            var buttonCreate = driver.FindElement(By.XPath("//button[@type='submit']"));
            buttonCreate.Click();

            var labelErrorMessage = driver.FindElement(By.XPath("//div[@class='err']"));  
            Assert.That(labelErrorMessage.Text, Is.EqualTo("Invalid URL!"));

        }

        [Test]
        public void Test_VisitNonExistingUrl()
        {
            driver.Url = "http://shorturl.nakov.repl.co/go/invalid536524)";

            var labelErrorMessage = driver.FindElement(By.XPath("//div[@class='err']"));
            Assert.That(labelErrorMessage.Text, Is.EqualTo("Cannot navigate to given short URL"));

            Assert.True(labelErrorMessage.Displayed, "Error message is displayed");

        }

        [Test]
        public void Test_CounterIncrease()
        {
            var linkShortUrl = driver.FindElement(By.LinkText("Short URLs"));
            linkShortUrl.Click();

            var tableFirstRow = driver.FindElements(By.CssSelector("table > tbody > tr")).First();
            var oldCounter = int.Parse(tableFirstRow.FindElements(By.CssSelector("td")).Last().Text);

            var linkToClickCell = tableFirstRow.FindElements(By.CssSelector("td"))[1];

            var linktoClick = linkToClickCell.FindElement(By.TagName("a"));
            linktoClick.Click();

            driver.SwitchTo().Window(driver.WindowHandles[0]);

            driver.Navigate().Refresh();    

            tableFirstRow = driver.FindElements(By.CssSelector("table > tbody > tr")).First();
            var newCounter = int.Parse(tableFirstRow.FindElements(By.CssSelector("td")).Last().Text);

            Assert.That(newCounter, Is.EqualTo(oldCounter + 1));    
        }

    }
}