using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.Core;

namespace MyLanguagePalService.Tests.TestsShared
{
    public static class TestUtils
    {
        public static Mock<IDbSet<T>> CreateMockDbSet<T>(IQueryable<T> querable) where T : class
        {
            var mockSet = new Mock<IDbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(querable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(querable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(querable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(querable.GetEnumerator());
            return mockSet;
        }

        public static T GetStub<T>() where T : class
        {
            var mock = new Mock<T>();
            mock.SetupAllProperties();
            return mock.Object;
        }

        public static void AssertValidationFailedException(ValidationFailedException vfe, string fieldNameToCheck)
        {
            Assert.IsNotNull(vfe);
            Assert.IsNotNull(vfe.Errors);
            Assert.IsTrue(vfe.Errors.Any(error => error.FieldName == fieldNameToCheck));
        }
    }
}