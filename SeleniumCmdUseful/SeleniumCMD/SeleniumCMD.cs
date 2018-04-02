namespace Useful.SeleniumCMD
{
    using System;
    using System.Collections.Generic;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using ISeleniumDrive;
    using Enum;

    public class SeleniumCMD : IDisposable
    {
        #region [ INSTANCES ]
        private ISeleniumBrowserDrive ISBDrive;
        private WebDriverWait _webWait;
        private IWebDriver _wDrive;

        private int _timeoutDefaultSEC;
        private int _pollingDefaultSEC;
        private bool _disposable;
        #endregion

        #region [ CTOR ]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isbDrive"></param>
        /// <param name="timeoutPadraoSEG"></param>
        /// <param name="pollingPadraoQTD"></param>
        public SeleniumCMD(ISeleniumBrowserDrive isbDrive, int timeoutDefaultSEC = 10, 
            int pollingDefaultSEC = 1, bool disposable = true)
        {
            ISBDrive = isbDrive;
            _timeoutDefaultSEC = timeoutDefaultSEC;
            _pollingDefaultSEC = pollingDefaultSEC;
            _disposable = disposable;
        }
        ~SeleniumCMD()
        {
            if(_disposable)
                Dispose();
        }
        #endregion

        #region [ Selenium Necessary ]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idField"></param>
        /// <param name="typeSelectEnum"></param>
        /// <returns></returns>
        private By GetBy(
            string idField, TypeSelectEnum typeSelectEnum)
        {
            switch (typeSelectEnum)
            {
                case TypeSelectEnum.Id:
                    return By.Id(idField);
                case TypeSelectEnum.Name:
                    return By.Name(idField);
                case TypeSelectEnum.Class:
                    return By.ClassName(idField);
                case TypeSelectEnum.XPath:
                    return By.XPath(idField);
                case TypeSelectEnum.Css:
                    return By.CssSelector(idField);
                case TypeSelectEnum.LinkText:
                    return By.LinkText(idField);
                case TypeSelectEnum.PartialLinkText:
                    return By.PartialLinkText(idField);
                default:
                    return By.Id(idField);
            }
        }
        
        private TimeSpan GetInSec(int sec)
        { return TimeSpan.FromSeconds(sec); }

        #endregion

        #region [ METHODS ]

        #region [ BROWSER ]
        public SeleniumCMD OpenBrowser()
        {
            _wDrive = ISBDrive.Instantiate();
            _webWait = new WebDriverWait(_wDrive, TimeSpan.FromSeconds(_timeoutDefaultSEC));
            return this;
        }
        public SeleniumCMD MaximizeBrowser()
        {
            _wDrive.Manage().Window.Maximize();
            return this;
        }
        public SeleniumCMD CloseBrowser()
        {
            try
            {
                _wDrive.Quit();
            }
            finally { Dispose(); }

            return this;
        }
        #endregion

        #region [ NAVIGATION ]
        public SeleniumCMD LoadURL(string url)
        {
            _wDrive.Navigate().GoToUrl(url);
            return this;
        }
        public SeleniumCMD Refresh()
        {
            _wDrive.Navigate().Refresh();
            return this;
        }

        #endregion

        #region [ COMPONENTS ]
        //________ GET
        public IWebElement GetElement(string idField, TypeSelectEnum byType,
            int timeout = 1, int pollingInterval = 1)
        {
            return ExecActionWait(w => w.FindElement(
                GetBy(idField, byType)), timeout, pollingInterval);
        }
        
        public ICollection<IWebElement> GetElementList(string idField, 
            TypeSelectEnum byType, int timeout = 2, int pollingInterval = 1)
        {
            return ExecActionWait(w => w.FindElements(
                GetBy(idField, byType)), timeout, pollingInterval);
        }

        //________ FOCUS
        public SeleniumCMD FocusedElement(string idField, TypeSelectEnum byType, out bool focused)
        {
            var element = GetElement(idField, byType);
            focused = element.Equals(_wDrive.SwitchTo().ActiveElement());
            return this;
        }
        public SeleniumCMD FocusedElement(IWebElement element, out bool focused)
        {
            focused = element.Equals(_wDrive.SwitchTo().ActiveElement());
            return this;
        }

        public SeleniumCMD FocusElement(string idField, TypeSelectEnum byType)
        {
            GetElement(idField, byType).Click();
            return this;
        }
        public SeleniumCMD FocusElement(IWebElement element)
        {
            element.Click();
            return this;
        }

        //________ ACTIVE
        public SeleniumCMD ElementEnabled(string idField, TypeSelectEnum byType, out bool enabled)
        {
            enabled = GetElement(idField, byType, 1).Enabled;
            return this;
        }

        public SeleniumCMD WaitElementEnabled(string idField, TypeSelectEnum byType,
            out bool enabled, int timeout = 10, int pollingInterval = 1)
        {
            enabled = GetElement(idField, byType, timeout, pollingInterval).Enabled;
            return this;
        }

        //________ VISIBLE
        public SeleniumCMD ElementVisible(string idField, TypeSelectEnum byType, out bool visible)
        {
            visible = GetElement(idField, byType).Displayed;
            return this;
        }

        public SeleniumCMD WaitElementVisible(string idField, TypeSelectEnum byType,
            out bool visible, int timeout = 10, int pollingInterval = 1)
        {
            visible = GetElement(idField, byType, timeout, pollingInterval).Displayed;
            return this;
        }

        //________ Input TEXT
        public SeleniumCMD TypeInField(string text, string idField, TypeSelectEnum byType)
        {
            GetElement(idField, byType).SendKeys(text);
            return this;
        }
        public SeleniumCMD TypeInField(string text, IWebElement element)
        {
            element.SendKeys(text);
            return this;
        }

        public SeleniumCMD CleanField(string idField, TypeSelectEnum byType)
        {
            GetElement(idField, byType).Clear();
            return this;
        }
        public SeleniumCMD CleanField(IWebElement element)
        {
            element.Clear();
            return this;
        }
        
        //________ Input BUTTON
        public SeleniumCMD ClickButton(string idField, TypeSelectEnum byType)
        {
            GetElement(idField, byType).Click();
            return this;
        }
        public SeleniumCMD ClickButton(IWebElement element)
        {
            element.Click();
            return this;
        }

        //________ Input OPTION
        public SeleniumCMD SelecionaOptionPorValor(
            string value, string idField, TypeSelectEnum byType)
        {
            var element = GetElement(idField, byType);
            SelectElement se = new SelectElement(element);
            se.SelectByText(value);
            return this;
        }

        public SeleniumCMD SelecionaOptionPorTexto(
            string value, string idField, TypeSelectEnum byType)
        {
            var element = GetElement(idField, byType);
            SelectElement se = new SelectElement(element);
            se.SelectByText(value);
            return this;
        }
        #endregion

        #region [ PAGE ]
        public SeleniumCMD WaitPage()
        {
            WaitPage(_timeoutDefaultSEC);

            return this;
        }
        public SeleniumCMD WaitPage(int timeout)
        {
            ExecActionWait(w => w.Manage().Timeouts().ImplicitWait.Add(GetInSec(timeout)));
            return this;
        }
        public SeleniumCMD ChangeFrame(string frameName)
        {
            ExecActionWait(w => w.SwitchTo().Frame(frameName));
            return this;
        }
        public SeleniumCMD ChangeFrame(string frameName, int timeout)
        {
            ExecActionWait(w => w.SwitchTo().Frame(frameName), timeout);
            return this;
        }
        #endregion

        #endregion

        #region [ USEFUL ]
        private T ExecActionWait<T>(Func<IWebDriver, T> action)
        {
            _webWait.Timeout.Add(GetInSec(_timeoutDefaultSEC));
            _webWait.PollingInterval.Add(GetInSec(_timeoutDefaultSEC));
            return _webWait.Until(action);
        }
        private T ExecActionWait<T>(Func<IWebDriver, T> action, int timeout)
        {
            _webWait.Timeout.Add(GetInSec(timeout));
            _webWait.PollingInterval.Add(GetInSec(_timeoutDefaultSEC));
            return _webWait.Until(action);
        }
        private T ExecActionWait<T>(Func<IWebDriver, T> action, int timeout, int pollingInterval)
        {
            _webWait.Timeout.Add(GetInSec(timeout));
            _webWait.PollingInterval.Add(GetInSec(pollingInterval));
            return _webWait.Until(action);
        }

        /// <summary>
        /// Dispose Instance
        /// </summary>
        public void Dispose()
        {
            ISBDrive.Dispose();
        }
        #endregion
    }
}