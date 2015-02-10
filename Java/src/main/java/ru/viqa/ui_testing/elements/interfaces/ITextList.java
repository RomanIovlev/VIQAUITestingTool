package ru.viqa.ui_testing.elements.interfaces;

import java.util.List;

/**
 * Created by 12345 on 29.01.2015.
 */
public interface ITextList extends IVIElement {
    List<String> waitText(String str) throws Exception;
    List<String> getText() throws Exception;
}
