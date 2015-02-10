package ru.viqa.ui_testing.elements.interfaces;

/**
 * Created by roman.i on 29.09.2014.
 */
public interface ICheckbox extends IClickable, IHaveValue {
    void check() throws Exception;
    void uncheck() throws Exception;
    Boolean isChecked() throws Exception;
    @Deprecated
    void clickOpenPage(String openPageName) throws Exception;
}
