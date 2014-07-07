using System;
using System.Configuration;
using System.Drawing.Imaging;
using NUnit.Framework;
using OpenQA.Selenium;
using VIQA.Common.Interfaces;
using VIQA.SiteClasses;

namespace VIQA.Common
{
    public class ScreenshotAlert : IAlerting
    {
        private readonly VISite _site;
        private readonly string _path = "/../.Screenshots";

        public ScreenshotAlert(VISite site)
        {
            _site = site;
            var imgRoot = DefaultLogger.GetValidUrl(ConfigurationSettings.AppSettings["VIScreenshotsPath"]);
            if (!string.IsNullOrEmpty(imgRoot))
                _path = imgRoot;
            var logDirectory = DefaultLogger.GetValidUrl(_path) + "\\";
            DefaultLogger.CreateDirectory(logDirectory);
            _path = logDirectory + "Fail_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".jpg";
        }

        public Exception ThrowError(string errorMsg)
        {
            VISite.Logger.Error(errorMsg);
            TakeScreenshot();
            Assert.Fail(errorMsg);
            return new Exception(errorMsg);
        }

        public void TakeScreenshot()
        {
            var screenshot = ((ITakesScreenshot)_site.WebDriver).GetScreenshot();
            screenshot.SaveAsFile(_path, ImageFormat.Jpeg);
        }

    }
}