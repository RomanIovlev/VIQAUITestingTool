package ru.viqa.ui_testing.elements.complexElements;

import org.openqa.selenium.WebElement;
import org.testng.Assert;
import ru.viqa.test_data_generator.utils.PrintUtils;
import ru.viqa.ui_testing.elements.baseClasses.VIElement;
import ru.viqa.ui_testing.elements.interfaces.ITextList;
import java.util.List;

import static ru.viqa.ui_testing.common.utils.LinqUtils.select;
import static ru.viqa.ui_testing.page_objects.VISite.Alerting;

/**
 * Created by 12345 on 29.01.2015.
 */
public class TextList extends VIElement implements ITextList {
    public TextList() throws Exception { }

    public List<String> waitText(String str) throws Exception {
        if (getTimer().wait(() -> select(waitListOfWebElements(), WebElement::getText).contains(str)))
            return getText();
        else Assert.assertTrue(false, "Wait Text Failed");
        throw Alerting.throwError("Wait Text Failed");
    }

    public List<String> getText() throws Exception {
        return doVIActionResult("Get list of texts", () -> (List<String>) select(waitListOfWebElements(), WebElement::getText),
                PrintUtils::printCollection);
    }

    public String getLastText() throws Exception {
        List<String> results = doVIActionResult("Get list of texts", () -> (List<String>) select(waitListOfWebElements(), WebElement::getText),
                PrintUtils::printCollection);
        return results.get(results.size() - 1);
    }
}
