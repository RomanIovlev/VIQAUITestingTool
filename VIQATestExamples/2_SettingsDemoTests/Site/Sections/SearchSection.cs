﻿using VIQA.HAttributes;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;

namespace VITestsProject.Standard.Site.Sections
{
    public class SearchSection : VIElement
    {
        [Name("Поле Поиска")]
        [Locate(ByXPath = "//*[@class='b-search__input']//*[@class='b-form-input__input']")]
        public ITextField SearchTextField;

        [Name("Кнопка 'Найти'")]
        [Locate(ByXPath = "//*[contains(text(),'Найти')]//..//..//input")]
        [WaitPageLoad]
        public IButton SearchButton;

        public void SearchProduct(string productName)
        {
            VISite.Logger.Event("SearchProduct: " + productName);
            SearchTextField.NewInput(productName);
            SearchButton.Click();
        }
    }
}
