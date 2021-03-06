﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyLanguagePalService.Areas.Site.Controllers;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;
using MyLanguagePalService.Tests.TestsShared;

namespace MyLanguagePalService.Tests.Controllers
{
    [TestClass]
    public class LanguagesControllerTest : TestBase
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

            var mockSet = CreateMockDbSet(languages);

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