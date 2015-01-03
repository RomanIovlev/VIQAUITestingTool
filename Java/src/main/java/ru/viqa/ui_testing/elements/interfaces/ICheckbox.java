package ru.viqa.ui_testing.elements.interfaces;

/**
 * Created by roman.i on 29.09.2014.
 */
public interface ICheckbox {
    void check() throws Exception;
    void uncheck() throws Exception;
    Boolean isChecked() throws Exception;
}
