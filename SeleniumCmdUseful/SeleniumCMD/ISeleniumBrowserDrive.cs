namespace Useful.SeleniumCMD.ISeleniumDrive
{
    using OpenQA.Selenium;

    public interface ISeleniumBrowserDrive
    {
        IWebDriver Instantiate();
        void Dispose();
    }
}
