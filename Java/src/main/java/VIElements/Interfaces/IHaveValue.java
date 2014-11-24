package VIElements.Interfaces;

import Common.FuncInterfaces.*;

import java.util.Objects;

/**
 * Created by 12345 on 11.05.2014.
 */
public interface IHaveValue extends  IVIElement {
    void setValue(String value) throws Exception;
    String getValue() throws Exception;
    //FuncTT<Object, Object> getFillRule();
}
