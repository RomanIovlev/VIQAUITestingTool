package VIElements.Interfaces;

import java.util.List;

/**
 * Created by roman.i on 29.09.2014.
 */
public interface ICheckList {
    void checkGroup(String... values) throws Exception;
    void uncheckGroup(String... values) throws Exception;
    void checkOnly(String... values) throws Exception;
    void uncheckOnly(String... values) throws Exception;
    void clear(String... values) throws Exception;
}
