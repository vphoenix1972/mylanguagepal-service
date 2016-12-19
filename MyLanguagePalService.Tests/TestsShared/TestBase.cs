using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;
using Expression = Castle.DynamicProxy.Generators.Emitters.SimpleAST.Expression;

namespace MyLanguagePalService.Tests.TestsShared
{
    public class TestBase
    {
        private Mock<IApplicationDbContext> _db;

        public Mock<IApplicationDbContext> DbMock
        {
            get
            {
                if (_db == null)
                {
                    _db = new Mock<IApplicationDbContext>();
                }

                return _db;
            }
        }

        public IApplicationDbContext Db
        {
            get
            {
                if (_db == null)
                {
                    _db = new Mock<IApplicationDbContext>();
                }

                return _db.Object;
            }
        }

        public Mock<IDbSet<PhraseDal>> CreatePhrasesMockDbSet(IList<PhraseDal> data)
        {
            return CreateMockDbSet(data, db => db.Phrases);
        }

        public Mock<ILanguagesService> GetLanguageServiceStub()
        {
            var languageServiceMock = new Mock<ILanguagesService>();
            languageServiceMock.SetupAllProperties();
            languageServiceMock.Setup(m => m.CheckIfLanguageExists(It.IsAny<int>())).Returns(true);
            return languageServiceMock;
        }

        public Mock<IDbSet<T>> CreateMockDbSet<T>(IList<T> data, Expression<Func<IApplicationDbContext, IDbSet<T>>> expression) where T : class
        {
            var mockSet = CreateMockDbSet(data.AsQueryable());
            DbMock.Setup(expression).Returns(mockSet.Object);
            return mockSet;
        }

        public Mock<IDbSet<T>> CreateMockDbSet<T>(IQueryable<T> querable) where T : class
        {
            var mockSet = new Mock<IDbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(querable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(querable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(querable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(querable.GetEnumerator());
            return mockSet;
        }

        public T GetStub<T>() where T : class
        {
            var mock = new Mock<T>();
            mock.SetupAllProperties();
            return mock.Object;
        }

        public void AssertValidationFailedException(ValidationFailedException vfe, string fieldNameToCheck)
        {
            Assert.IsNotNull(vfe);
            Assert.IsNotNull(vfe.Errors);
            Assert.IsTrue(vfe.Errors.Any(error => error.FieldName == fieldNameToCheck));
        }
    }
}
