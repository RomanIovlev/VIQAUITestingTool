package ru.viqa.ui_testing.elements.interfaces;

import java.util.List;

/**
 * Created by roman.i on 29.09.2014.
 */
public interface ISelector<T extends Enum> extends IHaveValue {
    void select(String valueName) throws Exception;
    void select(T valueName) throws Exception;
    String getText(String valueName) throws Exception;
    List<String> getAllLabels() throws Exception;
    String isSelected() throws Exception;
}
