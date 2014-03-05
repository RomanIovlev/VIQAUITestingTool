using VIQA.HAttributes;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;

namespace VITestsProject.Site.Sections
{
    public class SearchSection : VISection
    {
        [Name(Name = "Поле Поиска")]
        [Locate(ById = "search-input")]
        public ITextArea SearchTextField = new TextField();

        [Name(Name = "Кнопка 'Найти")]
        [Locate(ByXPath = "//*[contains(text(),'Найти')]//..//..//input")]
        public IButton SearchButton = new Button();
    }
}
