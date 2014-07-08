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
        private Func<string> _fileName;
        public Func<string> FileName
        {
            set { _fileName = value; }
            get { return () => TestContext.CurrentContext.Test.Name + "_fail_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"); }
        }

        public ImageFormat ImgFormat = ImageFormat.Jpeg;

        public ScreenshotAlert(VISite site)
        {
            _site = site;
        }

        public Exception ThrowError(string errorMsg)
        {
            VISite.Logger.Error(errorMsg);
            TakeScreenshot();
            Assert.Fail(errorMsg);
            return new Exception(errorMsg);
        }

        public void TakeScreenshot(string path = null, string outputFileName = null, ImageFormat imgFormat = null)
        {
            if (path == null)
            {
                var imgRoot = DefaultLogger.GetValidUrl(ConfigurationSettings.AppSettings["VIScreenshotsPath"]);
                path = (!string.IsNullOrEmpty(imgRoot))
                    ? imgRoot
                    : LogDirectory ?? "/../.Logs/.Screenshots";
            }
            if (string.IsNullOrEmpty(outputFileName))
                if (_fileName != null)
                    outputFileName = _fileName();
                else
                {
                    outputFileName = DefaultLogger.GetValidUrl(ConfigurationSettings.AppSettings["VIScreenshotsFileName"]);
                    if (string.IsNullOrEmpty(outputFileName))
                        outputFileName = FileName();
                }
            path = DefaultLogger.GetValidUrl(path);
            DefaultLogger.CreateDirectory(path);
            if (imgFormat == null)
                imgFormat = ImgFormat;
            var screenshotPath = path + outputFileName + "." + imgFormat;
            var screenshot = ((ITakesScreenshot)_site.WebDriver).GetScreenshot();
            screenshot.SaveAsFile(screenshotPath, ImgFormat);
            VISite.Logger.Error("Add Screenshot: " + screenshotPath);
        }

    }
}