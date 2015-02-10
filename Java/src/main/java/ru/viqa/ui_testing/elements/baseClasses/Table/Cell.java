package ru.viqa.ui_testing.elements.baseClasses.Table;

/**
 * Created by 12345 on 25.10.2014.
 */

import ru.viqa.ui_testing.elements.baseClasses.*;
import ru.viqa.ui_testing.elements.interfaces.IHaveValue;

public class Cell<T extends IHaveValue> extends VIElement {
    public T element;
    public int columnNum;
    public int rowNum;
    public String columnName;
    public String rowName;

    public Cell(T element, int columnNum, int rowNum, String colName, String rowName) throws Exception {
        this.element = element;
        this.columnNum = columnNum;
        this.rowNum = rowNum;
        columnName = colName;
        this.rowName = rowName;
    }

    public Cell<T> updateData(String colName, String rowName) {
        if ((columnName == null || columnName.equals("")) && !(colName == null || colName.equals("")))
            columnName = colName;
        if ((this.rowName == null || this.rowName.equals("")) && !(rowName == null || rowName.equals("")))
        this.rowName = rowName;
        return this;
    }
}
