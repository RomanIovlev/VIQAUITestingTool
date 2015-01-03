package ru.viqa.ui_testing.elements.interfaces;

/**
 * Created by roman.i on 29.09.2014.
 */
public interface ICheckList extends ISelector  {
    void checkGroup(String... values) throws Exception;
    void uncheckGroup(String... values) throws Exception;
    void checkOnly(String... values) throws Exception;
    void uncheckOnly(String... values) throws Exception;
    void clear(String... values) throws Exception;
}
