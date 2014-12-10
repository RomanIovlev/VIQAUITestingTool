package Common.Alertings;

import Common.Loggers.DefaultLogger;
import Common.Interfaces.IAlerting;
import SiteClasses.VISite;
import org.apache.commons.io.FileUtils;
import org.openqa.selenium.OutputType;
import org.openqa.selenium.TakesScreenshot;
import org.testng.*;

import java.io.File;

import static Common.Loggers.DefaultLogger.getValidUrl;
import static org.testng.Assert.fail;

/**
 * Created by 12345 on 03.10.2014.
 */
public class ScreenshotAlert implements IAlerting {

    private VISite _site;
    private ITestResult _result;
    public String LogDirectory;
    private String _fileName;
    public String getFileName() {
        return  _result.getTestName() + "_fail_" + VISite.RunId;
    }
    public void setFileName(String fileName) {
        _fileName = fileName;
    }

    public ScreenshotAlert(VISite site)
    {
        _site = site;
    }

    public Exception throwError(String errorMsg) throws Exception
    {
        VISite.Logger.Error(errorMsg);
        takeScreenshot();
        fail(errorMsg);
        throw new Exception();
    }

    public void takeScreenshot() throws Exception {
        takeScreenshot(null, null);
    }
    public void takeScreenshot(String path, String outputFileName) throws Exception {
        DefaultLogger logger = new DefaultLogger();
        if (path == null)
        {
            String imgRoot = getValidUrl(VISite.getProperty("vi.screenshot.path"));
            path = (imgRoot != null && !imgRoot.equals(""))
                    ? imgRoot
                    : LogDirectory != null ? LogDirectory : "/../.Logs/.Screenshots";
        }
        if (outputFileName == null || outputFileName.equals(""))
            if (_fileName != null)
                outputFileName = _fileName;
            else
            {
                outputFileName = getValidUrl(VISite.getProperty("vi.screenshot.fileName"));
                if (outputFileName == null || outputFileName.equals(""))
                    outputFileName = getFileName();
            }
        path = getValidUrl(path);
        final String finalPath = path;
        logger.LogDirectoryRoot = () -> finalPath;
        File file = logger.getFile(outputFileName);
        logger.createDirFile(file);
        String screenshotPath = path + outputFileName;
        File screenshotFile = ((TakesScreenshot)_site.getWebDriver()).getScreenshotAs(OutputType.FILE);
        FileUtils.copyFile(screenshotFile, new File(screenshotPath));
        VISite.Logger.Event("Add Screenshot: " + screenshotPath);
    }

}
