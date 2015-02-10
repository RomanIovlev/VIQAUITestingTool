package ru.viqa.ui_testing.elements.interfaces;

import java.util.List;

/**
 * Created by roman.i on 29.09.2014.
 */
public interface ICheckList<T extends Enum>  extends ISelector<T>  {
    void checkGroup(String... values) throws Exception;
    void checkOnly(String... values) throws Exception;
    void checkGroup(T... values) throws Exception;
    void checkOnly(T... values) throws Exception;
    void checkAll() throws Exception;
    void checkAll(Class<T> enumType) throws Exception;
    void uncheckGroup(String... values) throws Exception;
    void uncheckOnly(String... values) throws Exception;
    void uncheckGroup(T... values) throws Exception;
    void uncheckOnly(T... values) throws Exception;
    void uncheckAll() throws Exception;
    void uncheckAll(Class<T> enumType) throws Exception;
    void select(String... valueNames) throws Exception;
    List<String> areSelected() throws Exception;
    List<String> areNotSelected() throws Exception;
    @Deprecated
    String isSelected() throws Exception;
}
