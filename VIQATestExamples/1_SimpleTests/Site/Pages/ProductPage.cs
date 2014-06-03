﻿using VIQA.HAttributes;
using VIQA.SiteClasses;
using VITestsProject.Simple.Site.Sections;

namespace VITestsProject.Simple.Site.Pages
{
    [Page(Title = "Выбор по параметрам - Яндекс.Маркет", Url = "http://market.yandex.ru/guru.xml")]
    public class ProductPage : VIPage
    {
        [Name("Filter section")]
        [Locate(ByClassName = "b-gurufilters")]
        public FilterSection FilterSection;
    }
}
