package ru.viqa.ui_testing.common.funcInterfaces;

/**
 * Created by 12345 on 30.09.2014.
 */
public interface FuncTT<T1, T> {
    public T invoke(T1 val1) throws Exception;
}
