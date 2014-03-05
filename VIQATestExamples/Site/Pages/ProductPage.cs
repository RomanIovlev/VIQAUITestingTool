using VIQA.HAttributes;
using VIQA.SiteClasses;
using VITestsProject.Site.Sections;

namespace VITestsProject.Site.Pages
{
    [Page(Title = "Выбор по параметрам - Яндекс.Маркет", Url = "http://market.yandex.ru/guru.xml")]
    public class ProductPage : VIPage
    {
        [Name(Name = "Filter section")]
        [Locate(ByClassName = "b-gurufilters")]
        public static FilterSection FilterSection;
    }
}
