package ru.viqa.ui_testing.common.loggers;

import ru.viqa.ui_testing.common.interfaces.ILogger;
import ru.viqa.ui_testing.page_objects.VISite;

import java.io.*;

import static ru.viqa.ui_testing.common.utils.StringUtils.LineBreak;
import static ru.viqa.ui_testing.common.utils.TimeUtils.nowTime;
import static java.lang.String.format;
import static ru.viqa.ui_testing.page_objects.VISite.getRunId;

/**
 * Created by roman.i on 25.09.2014.
 */
public class DefaultLogger implements ILogger {
    private String logFileName = "%s.log";
    private static String logRecordTemplate = LineBreak + "[%s] %s: %s" + LineBreak;
    private String logRecord(String typeName, String msg) {
        return format(logRecordTemplate, typeName, nowTime("yyyy-MM-dd HH:mm:ss.S"), msg);
    }
    private String logFolder = ".logs/";
    public boolean CreateFoldersForLogTypes = true;
    private String runId;

    public void setLogRecord(String template) { logRecordTemplate = template; }
    public void setLogFolder(String folderName) { logFolder = folderName; }
    public void setLogFileName(String logFileName) { this.logFileName = logFileName; }

    public DefaultLogger() {
        runId = getRunId();
    }

    public static String getValidUrl(String logPath)
    {
        if (logPath == null || logPath.equals(""))
            return "";
        String result = logPath.replace("/", "\\");
        if (result.charAt(1) != ':')
            if (result.substring(0, 3).equals("..\\"))
                result = result.substring(2);
            if (result.charAt(0) != '\\')
                result = "\\" + result;
        return (result.charAt(result.length() - 1) == '\\')
                ? result
                : result + "\\";
    }

    private File getFile(String fileName) throws Exception {
        return new File(getLogDirrectory(runId) + format(logFileName, fileName));
    }

    public String getLogDirrectory(String fileName) throws Exception {
        return new File(".").getCanonicalPath() + getValidUrl(logFolder) + (CreateFoldersForLogTypes ? fileName + "\\" : "");
    }

    public void createDirFile(File file) throws IOException {
        file.getParentFile().mkdirs();
        if (!file.exists())
            file.createNewFile();
    }

    protected void InLog(String fileName, String typeName, String msg) throws Exception {
        File file = getFile(fileName);
        createDirFile(file);
        writeInFile(file, logRecord(typeName, msg));
    }

    private void writeInFile(File file, String msg) throws Exception {
        FileWriter fw = new FileWriter(file, true);
        fw.write(msg);
        fw.close();
    }

    public void event(String msg) throws Exception {
        InLog("Event", "Event", msg);
    }

    public void error(String msg) throws Exception {
        InLog("Error", "Error", msg);
        InLog("Event", "Error", msg);
    }

}
