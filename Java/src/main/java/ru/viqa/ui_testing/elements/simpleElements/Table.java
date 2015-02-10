package ru.viqa.ui_testing.elements.simpleElements;

import ru.viqa.ui_testing.common.funcInterfaces.*;
import ru.viqa.ui_testing.common.pairs.Pairs;
import ru.viqa.ui_testing.page_objects.PageObjectsInit;
import ru.viqa.ui_testing.elements.baseClasses.Table.Cell;
import ru.viqa.ui_testing.elements.baseClasses.Table.Columns;
import ru.viqa.ui_testing.elements.baseClasses.Table.Rows;
import ru.viqa.ui_testing.page_objects.VISite;
import ru.viqa.ui_testing.elements.baseClasses.*;
import ru.viqa.ui_testing.elements.interfaces.IHaveValue;
import ru.viqa.ui_testing.elements.interfaces.ITable;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import java.util.ArrayList;
import java.util.List;

import static java.util.Arrays.asList;
import static ru.viqa.ui_testing.common.utils.WebDriverByUtils.fillByTemplate;
import static ru.viqa.ui_testing.common.utils.PrintUtils.print;
import static ru.viqa.ui_testing.common.utils.LinqUtils.*;
import static ru.viqa.ui_testing.common.utils.StringUtils.*;

/**
 * Created by 12345 on 26.10.2014.
 */

public class Table<T extends IHaveValue> extends HaveValue implements ITable<T> {

    public Table() throws Exception{
        getColumns().table = this;
        getRows().table = this;
        //GetFooterFunc = t => t.FindElements(By.xpath("//tfoot/tr/td")).Select(el => el.Text).ToArray();
    }
    /*
    public Table(By tableLocator) {
        this();
        Locator = tableLocator;
    }

    public Table(String name = null, By tableLocator = null, By cellLocatorTemplate = null) {
        this()
        Name = name;
        Locator = tableLocator;
        _cellLocatorTemplate = cellLocatorTemplate;
    }*/
    private List<Cell<T>> _allCells = new ArrayList<>();
    public List<Cell<T>> getCells() throws Exception {
        for(String columnName : getColumns().headers())
            for(String rowName : getRows().headers())
                _allCells.add(cell(columnName, rowName));
        return _allCells;
    }

    private Columns<T> _columns = new Columns<T>();
    public Columns<T> getColumns() { return _columns; }
    public List<Cell<T>> getColumn(int colNum) throws Exception { return getRows().getColumn(colNum); }
    public List<Cell<T>> getColumn(String colName) throws Exception { return getRows().getColumn(colName); }
    public void setColumns(Columns<T> value) throws Exception { _columns.update(value); }
    public String[] getHeaders(FuncT<String[]> getHeadersAction) throws Exception {
        return doVIActionResult("Get Header", () -> getHeadersAction.invoke()); }

    private Rows<T> _rows = new Rows<T>();
    public Rows<T> getRows() { return _rows; }
    public List<Cell<T>> getRow(int rowNum) throws Exception { return getColumns().getRow(rowNum); }
    public List<Cell<T>> getRow(String rowName) throws Exception { return getColumns().getRow(rowName); }
    public void setRows(Rows<T> value) throws Exception { _rows.update(value); }

    public void setColumnHeaders(String[] value) { getColumns().setHeaders(value); }
    public void setRowHeaders(String[] value) { getRows().setHeaders(value); }
    public void setColCount(int value) { getColumns().setCount(value); }
    public void setRowCount(int value) { getRows().setCount(value); }

    protected String[] getFooterAction() throws Exception {
        return select(getWebElement().findElements(By.xpath("//tfoot/tr/td[1]")), WebElement::getText)
                .toArray(new String[1]);
    }
    protected String[] _footer;
    public void setFooter(String[] value) { _footer = value; }
    public String[] getFooter() throws Exception {
        if (_footer != null)
            return _footer;
        _footer = doVIActionResult("Get Footer", this::getFooterAction);
        if (_footer == null || _footer.length == 0)
            return null;
        getColumns().setCount(_footer.length);
        return _footer;
    }

    private Class<Text> clazz;
    public T getCellTemplate() throws Exception {
        return (T)(clazz.newInstance());
    }

    private By _cellLocatorTemplate;

    private T createCell() throws Exception {
        T cell = getCellTemplate();
        ((VIElement) cell).Context = new Pairs<>(ContextType.Locator, getLocator(), Context);
        return cell;
        }

    public Cell<T> cell(int colNum, int rowNum) throws Exception {
        int colIndex = colNum + getColumns().startIndex - 1;
        int rowIndex = rowNum + getRows().startIndex - 1;
        return addCell(colIndex, rowIndex, colNum, rowNum, "", "");
    }

    public Cell<T> cell(String colName, int rowNum) throws Exception {
        int colIndex = getColumnIndex(colName);
        int rowIndex = rowNum + getRows().startIndex - 1;
        return addCell(colIndex, rowIndex, asList(getColumns().headers()).indexOf(colName) + 1, rowNum, colName, "");
    }

    public Cell<T> cell(int colNum, String rowName) throws Exception {
        int colIndex = colNum + getColumns().startIndex - 1;
        int rowIndex = getRowIndex(rowName);
        return addCell(colIndex, rowIndex, colNum, asList(getRows().headers()).indexOf(rowName) + 1, "", rowName);
    }

    public Cell<T> cell(String colName, String rowName) throws Exception {
        int colIndex = getColumnIndex(colName);
        int rowIndex = getRowIndex(rowName);
        return addCell(colIndex, rowIndex, asList(getColumns().headers()).indexOf(colName) + 1,
                asList(getRows().headers()).indexOf(rowName) + 1, colName, rowName);
    }

    private Cell<T> addCell(int colIndex, int rowIndex, int colNum, int rowNum, String colName, String rowName) throws Exception {
        if (first(_allCells, cell -> cell.columnNum == colNum && cell.rowNum == rowNum) == null) {
            Cell<T> cell = createCell(colIndex, rowIndex, colNum, rowNum, colName, rowName);
            _allCells.add(cell);
            return cell;
        }
        return first(_allCells, cell -> cell.columnNum == colNum && cell.rowNum == rowNum).updateData(colName, rowName);
    }

    private List<Cell<T>> matches(List<Cell<T>> list, String pattern) throws Exception {
        return new ArrayList<>(where(list, cell -> ((IHaveValue) cell).getValue().matches(pattern)));
    }

    public List<Cell<T>> findCellsValues(String value) throws Exception {
        return new ArrayList<>(where(getCells(), cell -> ((IHaveValue) cell).getValue().equals(value)));
    }

    public List<Cell<T>> matchCellsValues(String pattern) throws Exception {
        return matches(getCells(), pattern);
    }

    public Cell<T> findFirstCellWithValue(String value) throws Exception {
        for (int colIndex = 1; colIndex <= getColumns().count(); colIndex++)
            for (int rowIndex = 1; rowIndex <= getRows().count(); rowIndex++) {
                Cell<T> cell = getCellFromValue(colIndex, rowIndex, value);
                if (cell != null)
                    return cell;
            }
        return null;
    }

    public Cell<T> findCellInColumn(int colIndex, String value) throws Exception {
        for (int rowIndex = 1; rowIndex <= getRows().count(); rowIndex++) {
            Cell<T> cell = getCellFromValue(colIndex, rowIndex, value);
            if (cell != null)
                return cell;
        }
        return null;
    }

    public Cell<T> findCellInColumn(String colName, String value) throws Exception {
        int colIndex = asList(getColumns().headers()).indexOf(colName) + 1;
        for (int rowIndex = 1; rowIndex <= getRows().count(); rowIndex++) {
            Cell<T> cell = getCellFromValue(colIndex, rowIndex, value);
            if (cell != null)
                return cell;
        }
        return null;
    }

    public List<Cell<T>> matchCellsInColumn(int colIndex, String pattern) throws Exception {
        return matches(getRow(colIndex), pattern);
    }

    public List<Cell<T>> matchCellsInColumn(String colname, String pattern) throws Exception{
        return matches(getRow(colname), pattern);
    }

    //Row filters
    public List<Cell<T>> matchCellsInRow(int rowIndex, String pattern) throws Exception {
        return matches(getColumn(rowIndex), pattern);
    }

    public List<Cell<T>> matchCellsInRow(String rowName, String pattern) throws Exception {
        return matches(getColumn(rowName), pattern);
    }

    private Cell<T> getCellFromValue(int colIndex, int rowIndex, String value) throws Exception {
        Cell<T> cell = cell(colIndex, rowIndex);
        return ((IHaveValue) cell).getValue().equals(value) ? cell : null;
    }

    public Cell<T> findCellInRow(int rowIndex, String value) throws Exception {
        for (int colIndex = 1; colIndex <= getColumns().count(); colIndex++) {
            Cell<T> cell = getCellFromValue(colIndex, rowIndex, value);
            if (cell != null)
                return cell;
        }
        return null;
    }

    public Cell<T> findCellInRow(String rowName, String value) throws Exception {
        int rowIndex = asList(getRows().headers()).indexOf(rowName) + 1;
        for (int colIndex = 1; colIndex <= getColumns().count(); colIndex++) {
            Cell<T> cell = getCellFromValue(colIndex, rowIndex, value);
            if (cell != null)
                return cell;
        }
        return null;
    }

    public List<Cell<T>> findColumnByRowValue(int rowIndex, String value) throws Exception {
        Cell<T> columnCell = findCellInRow(rowIndex, value);
        return columnCell != null ? getColumns().getRow(columnCell.columnNum) : null;
    }

    public List<Cell<T>> findColumnByRowValue(String rowName, String value) throws Exception {
        Cell<T> columnCell = findCellInRow(rowName, value);
        return columnCell != null ? getColumns().getRow(columnCell.columnNum) : null;
    }

    public List<Cell<T>> findRowByColumnValue(int colIndex, String value) throws Exception {
        Cell<T> rowCell = findCellInColumn(colIndex, value);
        return rowCell != null ? getRows().getColumn(rowCell.rowNum) : null;
    }

    public List<Cell<T>> findRowByColumnValue(String colName, String value) throws Exception {
        Cell<T> rowCell = findCellInColumn(colName, value);
        return rowCell != null ? getRows().getColumn(rowCell.rowNum) : null;
    }

    private int getColumnIndex(String name) throws Exception {
        int nameIndex;
        String[] headers = getColumns().headers();
        if (headers != null && asList(headers).contains(name))
            nameIndex = asList(headers).indexOf(name);
        else
            throw VISite.Alerting.throwError("Can't Get Column: '" + name + "'. " + ((headers == null)
                    ? "ColumnHeaders is Null" : ("Available ColumnHeaders: " + print(headers, ", ", "'{0}'") + ")")));
        return nameIndex + getColumns().startIndex;
    }

    private int getRowIndex(String name) throws Exception {
        int nameIndex;
        String[] headers = getRows().headers();
        if (headers != null && asList(headers).contains(name))
        nameIndex = asList(headers).indexOf(name);
        else
        throw VISite.Alerting.throwError("Can't Get Row: '" + name + "'. " +
                ((headers == null) ? "RowHeaders is Null" : ("Available RowHeaders: " + print(headers, ", ", "'{0}'") + ")")));
        return nameIndex + getRows().startIndex;
    }

    @Override
    public String getValueAction() throws Exception {
        return "||X|" + print(getColumns().headers(), "|") + "||" + LineBreak +
            print(new ArrayList(select(getRows().headers(),
                rowName -> "||" + rowName + "||" +
                    print(new ArrayList(select(where(getCells(),
                        cell -> cell.rowName.equals(rowName)),
                            cell -> ((IHaveValue)cell).getValue())), "|") + "||")), LineBreak);
    }

    @Override
    public void setValueAction(String string) {

    }

    private Cell<T> createCell(int colIndex, int rowIndex, String colName, String rowName) throws Exception {
        return createCell(colIndex, rowIndex, asList(getColumns().headers()).indexOf(colName) + 1,
            asList(getRows().headers()).indexOf(rowName) + 1, colName, rowName);
    }

    private Cell<T> createCell(int colIndex, int rowIndex, int colNum, int rowNum) throws Exception {
        return createCell(colIndex, rowIndex, colNum, rowNum, "", "");
    }
    private Cell<T> createCell(int colIndex, int rowIndex, int colNum, int rowNum, String colName, String rowName) throws Exception {
        VIElement cell = (VIElement)createCell();

        if (_cellLocatorTemplate == null)
            _cellLocatorTemplate = (cell.haveLocator())
                ? cell.getLocator()
                : By.xpath("//tr[{1}]/td[{0}]");
        if (!cell.haveLocator())
            cell.setLocator(fillByTemplate(_cellLocatorTemplate != null
                ? _cellLocatorTemplate : By.xpath("//tr[%s]/td[%s]"), rowIndex, colIndex));
        PageObjectsInit.initSubElements(cell);
        return new Cell((T)cell.getVIElement(), colNum, rowNum, colName, rowName);
    }
}
