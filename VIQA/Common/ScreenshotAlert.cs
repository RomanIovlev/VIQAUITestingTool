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
        public readonly string LogDirectory;
        public string FileName;
        public ImageFormat ImgFormat = ImageFormat.Png;

        public ScreenshotAlert(VISite site)
        {
            _site = site;
            var imgRoot = DefaultLogger.GetValidUrl(ConfigurationSettings.AppSettings["VIScreenshotsPath"]);
            LogDirectory = (!string.IsNullOrEmpty(imgRoot))
                ? imgRoot
                : "/../.Log/.Screenshots";
            var fileName = DefaultLogger.GetValidUrl(ConfigurationSettings.AppSettings["VIScreenshotsFileName"]);
            FileName = (!string.IsNullOrEmpty(fileName))
                ? fileName
                : TestContext.CurrentContext.Test.Name + "_fail_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            LogDirectory = DefaultLogger.GetValidUrl(LogDirectory) + "\\";
            DefaultLogger.CreateDirectory(LogDirectory);
        }

        public Exception ThrowError(string errorMsg)
        {
            VISite.Logger.Error(errorMsg);
            TakeScreenshot(LogDirectory, FileName, ImgFormat);
            VISite.Logger.Error("Add Screenshot: " + LogDirectory + FileName);
            Assert.Fail(errorMsg);
            return new Exception(errorMsg);
        }

        public void TakeScreenshot(string path, string outputFileName, ImageFormat imgFormat)
        {
            var screenshot = ((ITakesScreenshot)_site.WebDriver).GetScreenshot();
            screenshot.SaveAsFile(path + outputFileName + "." + imgFormat, ImgFormat);
        }

    }
}