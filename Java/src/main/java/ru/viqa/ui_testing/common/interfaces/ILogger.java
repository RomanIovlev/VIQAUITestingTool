package ru.viqa.ui_testing.common.interfaces;

/**
 * Created by roman.i on 24.09.2014.
 */
public interface ILogger {
    void event(String msg) throws Exception;
    void error(String errorMsg) throws Exception;

    void setLogRecord(String template);
    void setLogFolder(String folderName);
    void setLogFileName(String logFileName);
}
