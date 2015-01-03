package ru.viqa.ui_testing.common;

import ru.viqa.ui_testing.common.funcInterfaces.FuncT;
import ru.viqa.ui_testing.elements.baseClasses.VIElement;

/**
 * Created by 12345 on 20.11.2014.
 */
public interface Scenario {
    public Object invoke(VIElement viElement, String actionName, FuncT<Object> viAction) throws Exception;/* {
        return viAction.invoke();
    }*/
}
