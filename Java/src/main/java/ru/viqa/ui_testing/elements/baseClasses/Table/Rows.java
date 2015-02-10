package ru.viqa.ui_testing.elements.baseClasses.Table;

import ru.viqa.ui_testing.elements.interfaces.IHaveValue;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import java.util.*;

import static ru.viqa.ui_testing.common.utils.LinqUtils.*;
import static java.lang.String.format;
import static ru.viqa.ui_testing.page_objects.VISite.Alerting;

/**
 * Created by 12345 on 26.10.2014.
 */
public class Rows<T extends IHaveValue> extends TableLine<T> {
    public Rows() {
        haveHeader = false;
        elementIndex = ElementIndexType.Nums;
    }

    protected String[] getHeadersAction() throws Exception {
        return select(table.getWebElement().findElements(By.xpath("//tr/td[1]")), WebElement::getText)
                .toArray(new String[1]);
    }

    private Exception getRowsException(String rowName, Exception ex) throws Exception {
        return Alerting.throwError(format("Can't Get Rows '%s'. Exception: %s", rowName, ex));
    }

    public final List<Cell<T>> getColumn(String name) throws Exception {
        try { return new ArrayList<>(select(table.getColumns().headers(), colName -> table.cell(colName, name))); }
        catch (Exception ex) { throw getRowsException(name, ex); }
    }

    public final List<Cell<T>> getColumn(int num) throws Exception {
        int rowsCount = -1;
        if (count > 0)
            rowsCount = count;
        else if (headers != null && (headers.length > 0))
            rowsCount = headers.length;
        if (rowsCount > 0 && rowsCount < num)
        throw Alerting.throwError(format("Can't Get Row '%s'. [num] > RowsCount(%s).", num, rowsCount));
        try {
            List<Cell<T>> result = new ArrayList<>();
            for (int colNum = 1; colNum <= table.getColumns().count(); colNum++)
                result.add(table.cell(colNum, num));
            return result;
        }
        catch (Exception ex) { throw getRowsException(num + "", ex); }
    }
}
