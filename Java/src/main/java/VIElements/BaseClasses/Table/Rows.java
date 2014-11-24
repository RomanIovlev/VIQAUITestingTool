package VIElements.BaseClasses.Table;

import SiteClasses.VISite;
import VIElements.BaseClasses.HaveValue;
import VIElements.Interfaces.IHaveValue;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import java.util.*;

import static Common.Utils.LinqUtils.*;
import static java.lang.String.format;

/**
 * Created by 12345 on 26.10.2014.
 */
public class Rows<T extends IHaveValue> extends TableLine<T> {
    public Rows() {
        HaveHeaders = false;
        ElementIndex = ElementIndexType.Nums;
    }

    protected String[] getHeadersAction() throws Exception {
        return select(Table.getWebElement().findElements(By.xpath(".//tr/td[1]")), WebElement::getText)
                .toArray(new String[1]);
    }

    private Exception getRowsException(String rowName, Exception ex) throws Exception {
        return VISite.Alerting.ThrowError(format("Can't Get Rows '%s'. Exception: %s", rowName, ex));
    }

    public final List<Cell<T>> getColumn(String name) throws Exception {
        try { return new ArrayList<>(select(Table.getColumns().getHeaders(), colName -> Table.cell(colName, name))); }
        catch (Exception ex) { throw getRowsException(name, ex); }
    }

    public final List<Cell<T>> getColumn(int num) throws Exception {
        int rowsCount = -1;
        if (_count > 0)
            rowsCount = _count;
        else if (_headers != null && (_headers.length > 0))
            rowsCount = _headers.length;
        if (rowsCount > 0 && rowsCount < num)
        throw VISite.Alerting.ThrowError(format("Can't Get Row '%s'. [num] > RowsCount(%s).", num, rowsCount));
        try {
            List<Cell<T>> result = new ArrayList<>();
            for (int colNum = 1; colNum <= Table.getColumns().getCount(); colNum++)
                result.add(Table.cell(colNum, num));
            return result;
        }
        catch (Exception ex) { throw getRowsException(num + "", ex); }
    }
}
