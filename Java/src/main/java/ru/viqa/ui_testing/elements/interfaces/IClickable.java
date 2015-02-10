package ru.viqa.ui_testing.elements.interfaces;

/**
 * Created by 12345 on 11.05.2014.
 */
public interface IClickable extends IVIElement
{
    void click() throws Exception;
    void clickOnInvisible() throws Exception;
    void clickOpenPage(String openPageName) throws Exception;
}
