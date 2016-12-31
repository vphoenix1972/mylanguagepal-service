using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyLanguagePal.Core.Framework;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.Tests.TestsShared
{
    public abstract class TestBase
    {
        private Mock<IApplicationDbContext> _db;

        protected Mock<IApplicationDbContext> DbMock
        {
            get
            {
                if (_db == null)
                {
                    // Create db stub
                    _db = GetStub<IApplicationDbContext>();
                    CreateMockDbSet(new List<TaskSettingsDal>(), db => db.TaskSettings);
                }

                return _db;
            }
        }

        protected IApplicationDbContext Db => DbMock.Object;

        protected Mock<IDbSet<PhraseDal>> CreatePhrasesMockDbSet(IList<PhraseDal> data)
        {
            return CreateMockDbSet(data, db => db.Phrases);
        }

        protected ILanguagesService GetLanguageServiceStubObject()
        {
            return GetLanguageServiceStub().Object;
        }

        protected IPhrasesService GetPhrasesServiceStubObject()
        {
            return GetPhrasesServiceStub().Object;
        }

        protected Mock<ILanguagesService> GetLanguageServiceStub()
        {
            var languageServiceMock = new Mock<ILanguagesService>();
            languageServiceMock.SetupAllProperties();
            languageServiceMock.Setup(m => m.CheckIfLanguageExists(It.IsAny<int>())).Returns(true);
            languageServiceMock.Setup(m => m.GetDefaultLanguage())
                .Returns(new Language()
                {
                    Id = 1,
                    Name = "English"
                });
            return languageServiceMock;
        }

        protected Mock<IPhrasesService> GetPhrasesServiceStub()
        {
            var mock = new Mock<IPhrasesService>();
            mock.SetupAllProperties();
            mock.Setup(m => m.CheckIfPhraseExists(It.IsAny<int>())).Returns(true);
            return mock;
        }

        protected Mock<IFramework> GetFrameworkStub()
        {
            var mock = new Mock<IFramework>();
            mock.SetupAllProperties();
            return mock;
        }

        protected Mock<IDbSet<TaskSettingsDal>> CreateTaskSettingsMockDbSet(IList<TaskSettingsDal> data = null)
        {
            if (data == null)
                data = new List<TaskSettingsDal>();

            return CreateMockDbSet(data, db => db.TaskSettings);
        }

        protected Mock<IDbSet<T>> CreateMockDbSet<T>(IList<T> data, Expression<Func<IApplicationDbContext, IDbSet<T>>> expression) where T : class
        {
            var mockSet = CreateMockDbSet(data.AsQueryable());
            DbMock.Setup(expression).Returns(mockSet.Object);
            return mockSet;
        }

        protected Mock<IDbSet<T>> CreateMockDbSet<T>(IQueryable<T> querable) where T : class
        {
            var mockSet = new Mock<IDbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(querable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(querable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(querable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(querable.GetEnumerator());
            return mockSet;
        }

        protected T GetStubObject<T>() where T : class
        {
            return GetStub<T>().Object;
        }

        protected Mock<T> GetStub<T>() where T : class
        {
            var mock = new Mock<T>();
            mock.SetupAllProperties();
            return mock;
        }

        protected void AssertThatNoRecordsWasAdded<T>(Mock<IDbSet<T>> mockDbSet) where T : class
        {
            mockDbSet.Verify(m => m.Add(It.IsAny<T>()), Times.Never);
        }

        protected void AssertThatOnlyOneRecordWasMarkedAsModified<T>(Expression<Func<T, bool>> predicate)
        {
            DbMock.Verify(m => m.MarkModified(It.IsAny<T>()), Times.Once);
            DbMock.Verify(m => m.MarkModified(It.Is(predicate)), Times.Once);
        }

        protected void AssertIsValidationFailedExceptionTrown(Action fn, string fieldNameToCheck)
        {
            AssertIsExceptionThrown<ValidationFailedException>(fn,
                vfe =>
                {
                    Assert.IsNotNull(vfe.Errors);
                    Assert.IsTrue(vfe.Errors.Any(error => error.FieldName == fieldNameToCheck));
                });
        }

        protected void AssertValidationFailedException(ValidationFailedException vfe, string fieldNameToCheck)
        {
            Assert.IsNotNull(vfe);
            Assert.IsNotNull(vfe.Errors);
            Assert.IsTrue(vfe.Errors.Any(error => error.FieldName == fieldNameToCheck));
        }

        protected void AssertIsArgumentNullExceptionThrown(Action fn, string expectedArgumentName = null)
        {
            AssertIsExceptionThrown<ArgumentNullException>(fn,
                ex =>
                {
                    if (expectedArgumentName != null)
                        Assert.AreEqual(expectedArgumentName, ex.ParamName);
                });
        }

        protected void AssertIsExceptionThrown<T>(Action fn, Action<T> additionalAssertion = null) where T : Exception
        {
            T targetException = null;

            try
            {
                fn();
            }
            catch (T ex)
            {
                targetException = ex;
            }

            Assert.IsNotNull(targetException);

            if (additionalAssertion != null)
                additionalAssertion(targetException);
        }
    }
}
