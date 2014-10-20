using SimpleTests.Site.Sections;
using VIQA.HAttributes;
using VIQA.SiteClasses;

namespace SimpleTests.Site.Pages
{
    [Name("ProductPage"), Page(Title = "выбор по параметрам на Яндекс.Маркет", Url = "guru.xml")]
    public class ProductPage : VIPage
    {
        [Name("Filter section")]
        [Locator(ByClassName = "b-gurufilters")]
        public FilterSection FilterSection;
    }
}
