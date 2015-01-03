package ru.viqa.ui_testing.common.interfaces;

/**
 * Created by roman.i on 24.09.2014.
 */
public interface ILogger {
    void Event(String msg) throws Exception;
    void Error(String errorMsg) throws Exception;
}
