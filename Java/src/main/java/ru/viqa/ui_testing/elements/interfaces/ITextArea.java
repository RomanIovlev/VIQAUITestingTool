package ru.viqa.ui_testing.elements.interfaces;

/**
 * Created by 12345 on 28.09.2014.
 */
public interface ITextArea extends IText {
    void input(String text) throws Exception;
    void newInput(String text) throws Exception;
    void clear() throws Exception;
    void focus() throws Exception;
    String[] getLines() throws Exception;
}
