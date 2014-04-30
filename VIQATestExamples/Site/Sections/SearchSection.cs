using VIQA.HAttributes;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;

namespace VITestsProject.Site.Sections
{
    public class SearchSection : VIElement
    {
        [Name("Поле Поиска")]
        [Locate(ByXPath = "//*[@class='b-search__input']//*[@class='b-form-input__input']")]
        public ITextArea SearchTextField = new TextField();

        [Name("Кнопка 'Найти")]
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
