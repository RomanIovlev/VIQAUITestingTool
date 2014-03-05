using VIQA.HAttributes;
using VIQA.SiteClasses;
using VITestsProject.Site.Sections;

namespace VITestsProject.Site.Pages
{
    [Page(Title = "Яндекс.Маркет2", Url = "http://market.yandex.ru/2")]
    public class HomePage : VIPage
    {
        [Name(Name = "Search section")]
        [Locate(ByClassName = "b-head-search")]
        public static SearchSection SearchSection;

        public static void SearchProduct(string productName)
        {
            SearchSection.SearchTextField.Input(productName);
            SearchSection.SearchButton.Click();
        }
    }
}
