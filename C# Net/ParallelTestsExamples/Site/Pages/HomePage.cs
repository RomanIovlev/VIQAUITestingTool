using ParallelTestsExamples.Site.Sections;
using VIQA.HAttributes;
using VIQA.SiteClasses;

namespace ParallelTestsExamples.Site.Pages
{
    [Page(Title = "Яндекс.Маркет", Url = "http://market.yandex.ru/")]
    public class HomePage : VIPage
    {
        [Name("Search section")]
        [Locator(ByClassName = "search__table")]
        public SearchSection SearchSection;

    }
}
