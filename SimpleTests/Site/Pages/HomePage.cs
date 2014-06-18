using SimpleTests.Site.Sections;
using VIQA.HAttributes;
using VIQA.SiteClasses;

namespace SimpleTests.Site.Pages
{
    [Name("HomePage"), Page(Title = "Яндекс.Маркет", Url = "http://market.yandex.ru/", UrlCheckType = PageCheckType.Equal, TitleCheckType = PageCheckType.Contains, IsHomePage = true)]
    public class HomePage : VIPage
    {
        [Name("Search section")]
        [Locate(ByClassName = "search__table")]
        public SearchSection SearchSection;

    }
}

