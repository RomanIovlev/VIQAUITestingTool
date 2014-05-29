using VIQA.HAttributes;
using VIQA.SiteClasses;
using Settings.VITestsProject.Site.Sections;

namespace Settings.VITestsProject.Site.Pages
{
    [Page(Title = "Выбор по параметрам - Яндекс.Маркет", Url = "http://market.yandex.ru/guru.xml")]
    public class ProductPage : VIPage
    {
        [Name("Filter section")]
        [Locate(ByClassName = "b-gurufilters")]
        public FilterSection FilterSection;
    }
}
