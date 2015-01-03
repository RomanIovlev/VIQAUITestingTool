package ru.viqa.ui_testing.elements.interfaces;

import java.util.List;

/**
 * Created by roman.i on 29.09.2014.
 */
public interface ISelector<T> extends IHaveValue{
    T getElement(String value) throws Exception;
    List<T> getListOfElements() throws Exception;
    void select(String valueName) throws Exception;
    void getLabel(String valueName) throws Exception;
    String isSelected() throws Exception;
}
