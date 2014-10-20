using VIQA.HAttributes;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;

namespace CommonTestsWithComplexElements.Site.Sections
{
    public class SearchSection : VIElement
    {
        [Name("Поле Поиска")]
        [Locator(ById = "market_search")]
        public ITextField SearchTextField;

        [Name("Кнопка 'Найти'")]
        [Locator(ByXPath = ".//*[contains(text(),'Найти')]//..//..//button")]
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
