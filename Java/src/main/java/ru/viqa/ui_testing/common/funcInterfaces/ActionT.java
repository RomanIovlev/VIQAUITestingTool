package ru.viqa.ui_testing.common.funcInterfaces;

/**
 * Created by 12345 on 30.09.2014.
 */
public interface ActionT<T> {
    public void invoke(T val) throws Exception;
}
