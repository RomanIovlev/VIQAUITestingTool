package ru.viqa.ui_testing.elements.baseClasses.Table;

import ru.viqa.ui_testing.elements.interfaces.IHaveValue;
import ru.viqa.ui_testing.elements.simpleElements.*;

import java.util.ArrayList;
import java.util.List;

import static ru.viqa.ui_testing.common.utils.ReflectionUtils.isClass;

/**
 * Created by 12345 on 25.10.2014.
 */
public abstract class TableLine<T extends IHaveValue> {
    public int startIndex = 1;
    public boolean haveHeader;
    public ElementIndexType elementIndex;

    public Table<T> table;

    protected int count = -1;
    public void setCount(int value) { count = value; }
    public int count() {
        if (count > 0)
            return count;
            return headers != null ? headers.length : 0;
    }

    protected String[] headers;
    public void setHeaders(String[] value) { headers = value; }
    protected abstract String[] getHeadersAction() throws Exception;
    public final String[] headers() throws Exception {
        if (headers != null)
            return headers;
        headers = table.getHeaders(this::getHeadersAction);
        if (headers == null || !(headers.length > 0))
            return null;
        setCount(headers.length);
        if (!haveHeader)
            setHeaders(getNumList(count()));
        return headers;
    }

    protected String[] getNumList(int count) {
        return getNumList(count, 1);
    }
    protected String[] getNumList(int count, int from) {
        List<String> result = new ArrayList<>();
        for (int i = from; i < count + from; i++)
        result.add(i + "");
        return result.toArray(new String[result.size()]);
    }

    public final void update(TableLine<T> tableLine) throws Exception {
        if (tableLine.count > 0)
            setCount(tableLine.count());
        if (tableLine.startIndex != 1)
            startIndex = tableLine.startIndex;
        if (tableLine.headers != null && tableLine.headers.length > 0)
            setHeaders(tableLine.headers());
        if ((isClass(tableLine.getClass(), Columns.class) && !tableLine.haveHeader)
            || (isClass(tableLine.getClass(), Rows.class) && tableLine.haveHeader))
            haveHeader = tableLine.haveHeader;
        if (tableLine.elementIndex != ElementIndexType.Nums)
        elementIndex = tableLine.elementIndex;
    }
}
