package ru.viqa.ui_testing.elements.interfaces;

/**
 * Created by 12345 on 11.05.2014.
 */
public interface IText extends IHaveValue {
    String getText() throws Exception;
    @Deprecated
    void setValue(String value) throws Exception;

}
