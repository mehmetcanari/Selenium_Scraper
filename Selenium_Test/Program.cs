#region Using directives

using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

#endregion

namespace Selenium_Test
{
    internal class NetflixLoginTest
    {
        public static void Main(string[] args)
        {
            IWebDriver driver = ChromeBrowser();
            TestPrint testPrint = new TestPrint();
            
            NavigateWebsite(driver, "https://www.netflix.com/tr-en/login");
            testPrint.Text("Website opened : Passed");
            
            Wait(2000);
            
            var personalInfo = PersonalInfo();
            
            Login(driver, personalInfo.Email, personalInfo.Password);
            testPrint.Text("Logged in : Passed");
            
            Wait(2000);

            GetUsers(driver, 4);
            testPrint.Text("Users gotten : Passed");
            
            driver.Close();
        }

        //Opening chrome driver with options
        static IWebDriver ChromeBrowser()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            options.AddArgument("disable-infobars");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--incognito");
            IWebDriver driver = new ChromeDriver(options);
            return driver;
        }
        
        //Website navigation
        static void NavigateWebsite(IWebDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);
        }
        
        //Thread waiting for given time
        static void Wait(int time)
        {
            Thread.Sleep(time);
        }
        
        //Getting personal info from UserInfo class
        static UserInfo PersonalInfo()
        {
            UserInfo userInfo = new UserInfo();
            return userInfo;
        }
        
        //Login system with given username and password
        static void Login(IWebDriver driver, string username, string password)
        {
            IWebElement usernameElement = driver.FindElement(By.Name("userLoginId"));
            IWebElement passwordElement = driver.FindElement(By.Name("password"));
            
            usernameElement.SendKeys(username);
            passwordElement.SendKeys(password);
            
            driver.FindElement(By.XPath("/html/body/div[1]/div/div[3]/div/div/div[1]/form/button")).Click();
        }
        
        //Getting profile count and user names
        static void GetUsers(IWebDriver driver, int profileCount)
        {
            List<IWebElement> elements = new List<IWebElement>();
            int count = 0;
            
            try
            {
                for (int i = 0; i < profileCount; i++)
                {
                    IWebElement element = driver.FindElement(By.XPath("/html/body/div[1]/div/div/div[1]/div[1]/div[2]/div/div/ul/li[" + (i + 1) + "]/div/a/span"));
                    elements.Add(element);
                    count++;
                    Console.WriteLine(count +" "+ element.Text);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There is no " + (count + 1) + ". element: Test Failed");
                driver.Close();
                throw;
            }
        }
    }

    //User can give their own username and password
    class UserInfo
    {
        public readonly string Email = "your valid email";
        public readonly string Password = "your valid password";
    }
    
    class TestPrint
    {
        public void Text(string text)
        {
            Console.WriteLine(text);
        }
    }
}