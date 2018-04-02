namespace Useful.SeleniumCMD.Driver
{
    using System;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium;
    using ISeleniumDrive;

    public class SeleniumChromeConfig : ISeleniumBrowserDrive, IDisposable
    {
        #region [ INSTANCES ]
        private ChromeOptions _chOpt;
        private ChromeDriverService _chDriverService;
        private ChromeDriver _chDriver;
        #endregion

        public SeleniumChromeConfig(
            bool disableExtensions = true,
            string PathDownloadDestination = null, 
            string Path = null)
        {
            if (Path == null)
                _chDriverService = ChromeDriverService.CreateDefaultService();
            else
                _chDriverService = ChromeDriverService.CreateDefaultService(Path);

            _chOpt = new ChromeOptions();

            if (disableExtensions)
                _chOpt.AddArgument("--disable-extensions");

            if (PathDownloadDestination != null)
            {
                _chOpt.AddUserProfilePreference("download.prompt_for_download", false);
                _chOpt.AddUserProfilePreference("download.default_directory", PathDownloadDestination);
            }
        }

        public IWebDriver Instantiate()
        {
            _chDriver = new ChromeDriver(_chDriverService, _chOpt);
            return _chDriver;
        }

        public void Dispose()
        {
            _chDriver.Dispose();
        }
    }
}
