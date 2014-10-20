using OpenQA.Selenium.Support.PageObjects;
using VIQA.HtmlElements.BaseClasses;
using VIQA.HtmlElements.Interfaces;

namespace W3CSchools_Tests.Site.Sections
{
    public class ExampleFrame : Frame
    {
        [FindsBy(How = How.LinkText, Using = "Start learning HTML now!")]
        public ILink StartLearningLink;
    }
}
