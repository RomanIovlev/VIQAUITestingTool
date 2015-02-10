package ru.viqa.ui_testing.elements.complexElements;

import org.openqa.selenium.WebElement;
import ru.viqa.test_data_generator.utils.PrintUtils;
import ru.viqa.ui_testing.elements.baseClasses.VIElement;
import ru.viqa.ui_testing.elements.interfaces.ITextList;

import java.util.List;

import static java.lang.Thread.sleep;
import static ru.viqa.ui_testing.common.utils.LinqUtils.select;

/**
 * Created by 12345 on 29.01.2015.
 */
public class TextList extends VIElement implements ITextList {
    public TextList() throws Exception { }

    public List<String> waitText(String str) throws Exception {
        getTimer().wait(() -> select(waitListOfWebElements(), WebElement::getText).contains(str));
        return getText();
    }

    public List<String> getText() throws Exception {
        return doVIActionResult("Get list of texts", () -> (List<String>) select(waitListOfWebElements(), WebElement::getText),
                PrintUtils::printCollection); }
}
