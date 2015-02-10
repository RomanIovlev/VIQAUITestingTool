package ru.viqa.ui_testing.common.alertings;

import ru.viqa.ui_testing.common.interfaces.IAlerting;
import ru.viqa.ui_testing.page_objects.VISite;
import org.openqa.selenium.TakesScreenshot;

import java.io.File;
import java.io.IOException;

import static org.apache.commons.io.FileUtils.copyFile;
import static org.openqa.selenium.OutputType.FILE;
import static ru.viqa.ui_testing.common.loggers.DefaultLogger.getValidUrl;
import static org.testng.Assert.fail;
import static ru.viqa.ui_testing.page_objects.VISite.*;

/**
 * Created by 12345 on 03.10.2014.
 */
public class ScreenshotAlert implements IAlerting {

    private VISite site;
    public String LogDirectory;
    private String fileName;
    public String getDefaultFileName() {
        return "_fail_" + RunId;
    }

    public ScreenshotAlert(VISite site) {
        this.site = site;
        this.fileName = "fail_" + RunId;
    }

    public Exception throwError(String errorMsg) throws Exception
    {
        Logger.error(errorMsg);
        takeScreenshot();
        throw new Exception(errorMsg);
    }

    public void takeScreenshot() throws Exception {
        takeScreenshot(null, null);
    }
    public void takeScreenshot(String path, String outputFileName) throws Exception {
        if (outputFileName == null || outputFileName.equals(""))
            outputFileName = fillFileName();
        path = new File(".").getCanonicalPath() + getValidUrl(path != null ? path : fillPath());
        String screensFilePath = getFileName(path + outputFileName);
        new File(screensFilePath).getParentFile().mkdirs();
        File screensFile = ((TakesScreenshot)site.getWebDriver()).getScreenshotAs(FILE);
        copyFile(screensFile, new File(screensFilePath));
        Logger.event("Add Screenshot: " + screensFilePath);
    }
    private String getFileName(String fileName) {
        int num = 1;
        String newName = fileName;
        while (new File(newName + ".jpg").exists())
            newName = fileName + "_" + num ++;
        return newName + ".jpg";
    }


    private String fillPath() throws IOException {
        String imgRoot = getValidUrl(getProperty("vi.screenshot.path"));
        return (imgRoot != null && !imgRoot.equals(""))
                ? imgRoot
                : LogDirectory != null ? LogDirectory : "/../.logs/" + RunId;
    }

    private String fillFileName() throws IOException {
        if (fileName != null)
            return fileName;
        String outputFileName = getValidUrl(getProperty("vi.screenshot.fileName"));
        if (outputFileName == null || outputFileName.equals(""))
            return getDefaultFileName();
        return outputFileName;
    }


}
