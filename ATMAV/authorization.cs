using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ATMAV
{
    [TestFixture]
    public class UnitTest1
    {
        static ChromeDriver web = null;
        static Process Server = null;

        [OneTimeSetUp]
        public static void Setup()
        {
            // для запуска IISexpress сервиса приложения
            Server = new Process();
            Server.StartInfo.FileName = @"C:\Program Files\IIS Express\iisexpress.";
            string dir = Directory.GetCurrentDirectory();
            dir = @"E:\SimpleGame\SimpleGame";

            Server.StartInfo.Arguments = @"/path:" + dir + " /port:54591";
            Server.Start();

            web = new ChromeDriver();

            web.Navigate().GoToUrl("http://localhost:54591");
        }

        [SetUp]
        public void RefreshWeb()
        {
            web.Navigate().Refresh();
        }

        [OneTimeTearDown]
        public static void QuitWeb()
        {
            Server.Kill();

            web.Quit();
        }
        [Test]
        [TestCase("registerLink")]
        [TestCase("loginLink")]
        [TestCase("loginSubmit")]
        [TestCase("Password")]
        [TestCase("Email")]
        public void CheckVisableElementLoginPage(string id)
        {
            web.FindElement(By.Id("loginLink")).Click();
            Assert.AreEqual(true, web.FindElement(By.Id(id)).Displayed);
        }
        [Test]
        [TestCase("registerLink")]
        [TestCase("loginLink")]
        [TestCase("registSubmit")]
        [TestCase("Password")]
        [TestCase("ConfirmPassword")]
        [TestCase("Email")]
        public void CheckVisableElementRegistrPage(string id)
        {
            web.FindElement(By.Id("registerLink")).Click();
            Assert.AreEqual(true, web.FindElement(By.Id(id)).Displayed);
        }
        [Test]
        [TestCase("loginLink", "http://localhost:54591/Account/Login")]
        [TestCase("registerLink", "http://localhost:54591/Account/Register")]
        public void ClickBtnLoginPage(string id, string expUrl)
        {
            web.FindElement(By.Id(id)).Click();
            Assert.AreEqual(expUrl, web.Url);
        }
    }
}
