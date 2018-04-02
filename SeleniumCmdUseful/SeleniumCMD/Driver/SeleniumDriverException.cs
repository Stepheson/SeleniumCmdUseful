namespace Useful.SeleniumCMD.Driver.Exception
{
    using ISeleniumDrive;
    using System;

    public class SeleniumBrowserException : Exception
    {
        public SeleniumBrowserException(ISeleniumBrowserDrive drive)
        : base($"O navegador {drive.GetType()} deve ser aberto antes.") { }
    }
}
