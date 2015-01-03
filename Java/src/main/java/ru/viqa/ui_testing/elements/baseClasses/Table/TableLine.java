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
    public int StartIndex = 1;
    public boolean HaveHeaders;
    public ElementIndexType ElementIndex;

    public Table<T> Table;

    protected int _count = -1;
    public void setCount(int value) { _count = value; }
    public int getCount() {
        if (_count > 0)
            return _count;
            return _headers != null ? _headers.length : 0;
    }

    protected String[] _headers;
    public void setHeaders(String[] value) { _headers = value; }
    protected abstract String[] getHeadersAction() throws Exception;
    public final String[] getHeaders() throws Exception {
        if (_headers != null)
            return _headers;
        _headers = Table.doVIActionResult("Get Rows Headers", this::getHeadersAction);
        if (_headers == null || !(_headers.length > 0))
            return null;
        setCount(_headers.length);
        if (!HaveHeaders)
            setHeaders(getNumList(getCount()));
        return _headers;
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
        if (tableLine._count > 0)
            setCount(tableLine.getCount());
        if (tableLine.StartIndex != 1)
            StartIndex = tableLine.StartIndex;
        if (tableLine._headers != null && tableLine._headers.length > 0)
            setHeaders(tableLine.getHeaders());
        if ((isClass(tableLine.getClass(), Columns.class) && !tableLine.HaveHeaders)
            || (isClass(tableLine.getClass(), Rows.class) && tableLine.HaveHeaders))
            HaveHeaders = tableLine.HaveHeaders;
        if (tableLine.ElementIndex != ElementIndexType.Nums)
        ElementIndex = tableLine.ElementIndex;
    }
}
