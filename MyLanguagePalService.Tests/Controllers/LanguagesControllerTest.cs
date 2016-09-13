using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyLanguagePalService.Controllers;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.Tests.Controllers
{
    [TestClass]
    public class LanguagesControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            var mockContext = new Mock<IApplicationDbContext>();

            var languages = new List<LanguageDal>()
            {
                new LanguageDal()
                {
                    Id = 1,
                    Name = "English"
                },
                new LanguageDal()
                {
                    Id = 1,
                    Name = "Russian"
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<LanguageDal>>();
            mockSet.As<IQueryable<LanguageDal>>().Setup(m => m.Provider).Returns(languages.Provider);
            mockSet.As<IQueryable<LanguageDal>>().Setup(m => m.Expression).Returns(languages.Expression);
            mockSet.As<IQueryable<LanguageDal>>().Setup(m => m.ElementType).Returns(languages.ElementType);
            mockSet.As<IQueryable<LanguageDal>>().Setup(m => m.GetEnumerator()).Returns(languages.GetEnumerator());

            mockContext.Setup(x => x.Languages)
                .Returns(mockSet.Object);

            var db = mockContext.Object;

            // Act
            var controller = new LanguagesController(db);
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}