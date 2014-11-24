package VIElements.BaseClasses.Table;

/**
 * Created by 12345 on 25.10.2014.
 */

import VIElements.BaseClasses.*;
import VIElements.Interfaces.IHaveValue;

public class Cell<T extends IHaveValue> extends VIElement {
    public T Element;
    public int ColumnNum;
    public int RowNum;
    public String ColumnName;
    public String RowName;

    public Cell(T element, int columnNum, int rowNum, String colName, String rowName) throws Exception {
        Element = element;
        ColumnNum = columnNum;
        RowNum = rowNum;
        ColumnName = colName;
        RowName = rowName;
    }

    public Cell<T> UpdateData(String colName, String rowName) {
        if ((ColumnName == null || ColumnName.equals("")) && !(colName == null || colName.equals("")))
            ColumnName = colName;
        if ((RowName == null || RowName.equals("")) && !(rowName == null || rowName.equals("")))
        RowName = rowName;
        return this;
    }
}
