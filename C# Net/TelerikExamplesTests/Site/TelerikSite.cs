using TelerikExamplesTests.Site.Pages;
using VIQA.HAttributes;
using VIQA.SiteClasses;

namespace TelerikExamplesTests.Site
{
    [Site(Domain = "http://demos.telerik.com//")]
    public class TelerikSite : VISite
    {
        [Page(Title = "ComboBox page", Url = "http://demos.telerik.com/kendo-ui/combobox/index")]
        public ComboBoxPage ComboBoxPage;
    }
}