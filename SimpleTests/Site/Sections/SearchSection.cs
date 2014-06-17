using VIQA.HAttributes;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;

namespace SimpleTests.Site.Sections
{
    public class SearchSection : VIElement
    {
        [Name("Поле Поиска")]
        [Locate(ByXPath = ".//*[@class='b-form-input__input']")]
        public ITextField SearchTextField;

        [Name("Кнопка 'Найти'")]
        [Locate(ByXPath = ".//*[contains(text(),'Найти')]//..//..//input")]
        [ClickLoadsPage("Productpage")]

        public IButton SearchButton;

        public void SearchProduct(string productName)
        {
            VISite.Logger.Event("SearchProduct: " + productName);
            SearchTextField.NewInput(productName);
            SearchButton.Click();
        }
    }
}
