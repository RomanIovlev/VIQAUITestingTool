package ru.viqa.ui_testing.common.funcInterfaces;

/**
 * Created by 12345 on 30.09.2014.
 */
public interface FuncT<T> {
    public T invoke() throws Exception;
}