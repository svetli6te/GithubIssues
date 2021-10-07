using Faker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Reflection;

namespace UITests
{
    [TestClass]
    public class CreateIssueTests
    {
        protected IWebDriver Driver { get; set; }

        [TestInitialize]                   
        public void Initialize()
        {
            Driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            Driver.Manage().Window.Maximize();
            Driver.Navigate().GoToUrl("https://github.com/login");
        }

        [TestCleanup]
        public void EndSession()
        {
            Driver.Quit();
        }

        [TestMethod]
        public void CreateIssueAndValidateCreation()
        {
            //Login
            IWebElement userNameField = Driver.FindElementWithWait(By.Id("login_field"));
            IWebElement passwordField = Driver.FindElementWithWait(By.Id("password"));
            IWebElement loginButton = Driver.FindElementWithWait(By.Name("commit"));

            userNameField.SendKeys("svetla4manolova@gmail.com");
            passwordField.SendKeys("svetli6te438981");
            loginButton.Click();

            //Open Repository
            IWebElement searchField = Driver.FindElementWithWait(By.Id("dashboard-repos-filter-left"));
            searchField.SendKeys("GithubIssues");
            IWebElement repositoryLink = Driver.FindElementWithWait(By.XPath("//a[@data-hovercard-url='/svetli6te/GithubIssues/hovercard']"));
            repositoryLink.Click();

            //Navigate to issues tab
            IWebElement issuesTab = Driver.FindElementWithWait(By.XPath("//span[@data-content='Issues']"));
            issuesTab.Click();

            //Create new issue
            IWebElement newIssueButton = Driver.FindElementWithWait(By.XPath("//a[@role='button']"));
            newIssueButton.Click();
            IWebElement issueTitle = Driver.FindElementWithWait(By.Id("issue_title"));
            string title = Lorem.Sentence();
            issueTitle.SendKeys(title);
            IWebElement submitNewIssueButton = Driver.FindElementWithWait(By.XPath("//button[normalize-space(text())='Submit new issue']"));
            submitNewIssueButton.Click();

            //Validate new issue is created
            IWebElement newIssueTitle = Driver.FindElementWithWait(By.XPath("//h1[contains(@class,'header-title')]"));

            Assert.IsTrue(newIssueTitle.Text.Contains(title));
        }       
    }
}
