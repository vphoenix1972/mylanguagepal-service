using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLanguagePalService.Controllers;

namespace MyLanguagePalService.Tests.Controllers
{
    [TestClass]
    public class AboutControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            var controller = new AboutController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
