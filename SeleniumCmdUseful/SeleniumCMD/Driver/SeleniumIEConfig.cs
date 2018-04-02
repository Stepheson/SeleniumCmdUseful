namespace Useful.SeleniumCMD.Driver
{
    using ISeleniumDrive;
    using OpenQA.Selenium;
    using OpenQA.Selenium.IE;
    using System;

    public class SeleniumIEConfig : ISeleniumBrowserDrive, IDisposable
    {
        #region [ INSTANCES ]
        private InternetExplorerDriverService _ieDriverService;
        private InternetExplorerOptions _ieOpt;
        private InternetExplorerDriver _ieDriver;
        #endregion

        public SeleniumIEConfig(
            string PathDownloadDestination = null,
            string Path = null)
        {
            _ieDriverService = InternetExplorerDriverService.CreateDefaultService();
            _ieDriverService.HideCommandPromptWindow = true;

            _ieOpt = new InternetExplorerOptions()
            { IntroduceInstabilityByIgnoringProtectedModeSettings = true, };
        }

        public IWebDriver Instantiate()
        {
            _ieDriver = new InternetExplorerDriver(_ieDriverService, _ieOpt);
            return _ieDriver;
        }

        public void Dispose()
        {
            _ieDriver.Dispose();
        }
    }
}
