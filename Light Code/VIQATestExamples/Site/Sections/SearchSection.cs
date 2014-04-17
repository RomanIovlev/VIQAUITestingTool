using VIQA.HAttributes;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;

namespace VITestsProject.Site.Sections
{
    public class SearchSection : VIElement
    {
        [Name(Name = "Поле Поиска")]
        [Locate(ById = "search-input")]
        public ITextArea SearchTextField = new TextField();

        [Name(Name = "Кнопка 'Найти")]
        [Locate(ByXPath = "//*[contains(text(),'Найти')]//..//..//input")]
        [ClickReloadsPage]
        public IButton SearchButton = new Button();

        public void SearchProduct(string productName)
        {
            SearchTextField.NewInput(productName);
            SearchButton.Click();
        }
    }
}
