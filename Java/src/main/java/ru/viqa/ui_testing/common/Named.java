package ru.viqa.ui_testing.common;

import ru.viqa.ui_testing.common.funcInterfaces.FuncT;

/**
 * Created by roman.i on 24.09.2014.
 */
public class Named {
    private String _name;
    protected FuncT<String> DefaultNameFunc = () -> "";
    public String getName() throws Exception {
        return (_name != null) ? _name : DefaultNameFunc.invoke();
    }
    public void setName(String name) throws Exception { _name = name; }

}
