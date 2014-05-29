using VIQA.HAttributes;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;

namespace Simple.VITestsProject.Site.Sections
{
    public class FilterSection : VIElement
    {
        [Name("Цена От")] 
        [Locate(ByXPath = "//*[@class='b-gurufilters__filter-inputs']/input[contains(@id,'-0')]")] 
        public readonly ITextField TextFieldFrom;

        [Name("Цена До"), Locate(ByXPath = "//*[@class='b-gurufilters__filter-inputs']/input[contains(@id,'-1')]")]
        public readonly ITextField TextFieldTo;

        [Name("Wi-fi"), Locate(ByXPath = "//*[@class='b-gurufilters']//*[contains(text(),'Wi-Fi')]//..//input")] 
        public readonly ICheckbox WiFiCheckbox;

        public IButton ShowResultsButton { get { return new Button("Показать", "input[value='Показать']"); } }
    }
}
