package ru.viqa.ui_testing.elements.interfaces;

import ru.viqa.ui_testing.elements.baseClasses.Table.Cell;
import ru.viqa.ui_testing.elements.baseClasses.Table.Columns;
import ru.viqa.ui_testing.elements.baseClasses.Table.Rows;

import java.util.List;

/**
 * Created by roman.i on 20.10.2014.
 */

public interface ITable<T extends IHaveValue> extends IHaveValue {
    Cell<T> cell(int colNum, int rowNum) throws Exception;
    Cell<T> cell(String colName, int rowNum) throws Exception;
    Cell<T> cell(int colNum, String rowName) throws Exception;
    Cell<T> cell(String colName, String rowName) throws Exception;
    List<Cell<T>> findCellsValues(String value) throws Exception;
    List<Cell<T>> matchCellsValues(String regex) throws Exception;
    Cell<T> findFirstCellWithValue(String value) throws Exception;

    Cell<T> findCellInColumn(int colIndex, String value) throws Exception;
    List<Cell<T>> matchCellsInColumn(int colIndex, String regex) throws Exception;
    Cell<T> findCellInColumn(String colName, String value) throws Exception;
    List<Cell<T>> matchCellsInColumn(String colname, String regex) throws Exception;

    Cell<T> findCellInRow(int rowIndex, String value) throws Exception;
    List<Cell<T>> matchCellsInRow(int rowIndex, String regex) throws Exception;
    Cell<T> findCellInRow(String rowName, String value) throws Exception;
    List<Cell<T>> matchCellsInRow(String rowName, String pattern) throws Exception;

    List<Cell<T>> findColumnByRowValue(int rowIndex, String value) throws Exception;
    List<Cell<T>> findColumnByRowValue(String rowName, String value) throws Exception;
    List<Cell<T>> findRowByColumnValue(int colIndex, String value) throws Exception;
    List<Cell<T>> findRowByColumnValue(String colName, String value) throws Exception;

    Columns<T> getColumns();
    List<Cell<T>> getColumn(int colNum) throws Exception;
    List<Cell<T>> getColumn(String colName) throws Exception;
    Rows<T> getRows();
    List<Cell<T>> getRow(int rowNum) throws Exception;
    List<Cell<T>> getRow(String rowName) throws Exception;
    String[] getFooter() throws Exception;
}
