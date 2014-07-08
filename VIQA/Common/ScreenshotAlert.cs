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
        public string LogDirectory;
        private string _fileName;
        public Func<string> FileName = () => TestContext.CurrentContext.Test.Name + "_fail_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        public ImageFormat ImgFormat = ImageFormat.Jpeg;

        public ScreenshotAlert(VISite site)
        {
            _site = site;
            var imgRoot = DefaultLogger.GetValidUrl(ConfigurationSettings.AppSettings["VIScreenshotsPath"]);
            LogDirectory = (!string.IsNullOrEmpty(imgRoot))
                ? imgRoot
                : "/../.Logs/.Screenshots";
            _fileName = DefaultLogger.GetValidUrl(ConfigurationSettings.AppSettings["VIScreenshotsFileName"]);
            if (!string.IsNullOrEmpty(_fileName))
                _fileName = FileName();
            LogDirectory = DefaultLogger.GetValidUrl(LogDirectory);
            DefaultLogger.CreateDirectory(LogDirectory);
        }

        public Exception ThrowError(string errorMsg)
        {
            VISite.Logger.Error(errorMsg);
            TakeScreenshot(LogDirectory, _fileName, ImgFormat);
            Assert.Fail(errorMsg);
            return new Exception(errorMsg);
        }

        public void TakeScreenshot(string path = null, string outputFileName = null, ImageFormat imgFormat = null)
        {
            if (path == null)
                path = LogDirectory;
            path = DefaultLogger.GetValidUrl(path);
            if (outputFileName == null)
                outputFileName = _fileName;
            if (imgFormat == null)
                imgFormat = ImgFormat;
            var screenshot = ((ITakesScreenshot)_site.WebDriver).GetScreenshot();
            var screenshotPath = path + outputFileName + "." + imgFormat;
            screenshot.SaveAsFile(screenshotPath, ImgFormat);
            VISite.Logger.Error("Add Screenshot: " + screenshotPath);
        }

    }
}