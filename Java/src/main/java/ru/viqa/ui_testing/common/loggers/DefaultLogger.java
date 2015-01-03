package ru.viqa.ui_testing.common.loggers;

import ru.viqa.ui_testing.common.funcInterfaces.*;
import ru.viqa.ui_testing.common.interfaces.ILogger;

import java.io.*;

import static ru.viqa.ui_testing.common.utils.StringUtils.LineBreak;
import static ru.viqa.ui_testing.common.utils.TimeUtils.nowTime;
import static java.lang.String.format;

/**
 * Created by roman.i on 25.09.2014.
 */
public class DefaultLogger implements ILogger {
    public FuncT<String> LogFileFormat =  () -> "%s_" + nowTime("yyyy-MM-dd-HH-mm-ss-S") + ".log";
    private static String LogRecordTemplate = LineBreak + "[%s] %s: %s" + LineBreak;
    private FuncTTT<String, String, String> LogRecord = (String s1, String s2) -> format(LogRecordTemplate, s1, nowTime("yyyy-MM-dd HH:mm:ss.S"), s2);
    public FuncT<String> LogDirectoryRoot = () -> ".Logs/";
    public boolean CreateFoldersForLogTypes = true;

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

    public File getFile(String fileName) throws Exception {
        File file = new File(".");
        String current = file.getCanonicalPath();
        String logDirectory = current + getValidUrl(LogDirectoryRoot.invoke()) + (CreateFoldersForLogTypes ? fileName + "s\\" : "");
        return new File(logDirectory + format(LogFileFormat.invoke(), fileName));
    }

    public void createDirFile(File file) throws IOException {
        file.getParentFile().mkdirs();
        if (!file.exists())
            file.createNewFile();
    }

    protected void InLog(String fileName, String typeName, String msg) throws Exception {
        File file = getFile(fileName);
        createDirFile(file);
        writeInFile(file, LogRecord.invoke(typeName, msg));
    }

    private void writeInFile(File file, String msg) throws Exception {
        FileWriter fw = new FileWriter(file, true);
        fw.write(msg);
        fw.close();
    }

    public void Event(String msg) throws Exception {
        InLog("Event", "Event", msg);
    }

    public void Error(String msg) throws Exception {
        InLog("Error", "Error", msg);
        InLog("Event", "Error", msg);
    }

}
