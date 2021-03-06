﻿using System;
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

        protected double Tolerance { get; set; } = 0.000001;

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

        protected Mock<IDbSet<PhraseDal>> CreatePhrasesMockDbSet(IList<PhraseDal> data = null)
        {
            return CreateMockDbSet(data, db => db.Phrases);
        }

        protected Mock<IDbSet<KnowledgeLevelDal>> CreateKnowledgeLevelsMockDbSet(IList<KnowledgeLevelDal> data = null)
        {
            return CreateMockDbSet(data, db => db.KnowledgeLevels);
        }

        protected ILanguagesService GetLanguageServiceStubObject()
        {
            return GetLanguageServiceStub().Object;
        }

        protected IPhrasesService GetPhrasesServiceStubObject()
        {
            return CreatePhrasesServiceStub().Object;
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

        protected Mock<IPhrasesService> CreatePhrasesServiceStub()
        {
            var mock = new Mock<IPhrasesService>();
            mock.SetupAllProperties();
            mock.Setup(m => m.CheckIfPhraseExists(It.IsAny<int>())).Returns(true);
            return mock;
        }

        protected Mock<IFramework> CreateFrameworkStub()
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
            if (data == null)
                data = new List<T>();

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

        protected IList<PhraseDal> GeneratePhrases(int count, Func<int, PhraseDal, PhraseDal> onAfterGenerationCallback = null)
        {
            var result = new List<PhraseDal>();

            for (var i = 0; i < count; i++)
            {
                var phrase = new PhraseDal()
                {
                    Id = i + 1,
                    LanguageId = 1,
                    Text = $"Phrase {i + 1}"
                };

                if (onAfterGenerationCallback != null)
                    phrase = onAfterGenerationCallback(i, phrase);

                result.Add(phrase);
            }

            return result;
        }

        protected IList<KnowledgeLevelDal> GenerateKnowledgeLevels(int count, Func<int, KnowledgeLevelDal, KnowledgeLevelDal> onAfterGenerationCallback = null)
        {
            var result = new List<KnowledgeLevelDal>();

            for (var i = 0; i < count; i++)
            {
                var e = new KnowledgeLevelDal()
                {
                    Id = i + 1,
                    TaskId = 1,
                    CurrentLevel = 0,
                    LastRepetitonTime = DateTime.UtcNow,
                    PhraseId = 1,
                    PreviousLevel = null
                };

                if (onAfterGenerationCallback != null)
                    e = onAfterGenerationCallback(i, e);

                result.Add(e);
            }

            return result;
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

        protected void AssertIsArgumentExceptionThrown(Action fn, string expectedParamName = null, string expectedMessage = null)
        {
            AssertIsExceptionThrown<ArgumentException>(fn,
                ex =>
                {
                    if (expectedParamName != null)
                        Assert.AreEqual(expectedParamName, ex.ParamName);
                    if (expectedMessage != null)
                        Assert.AreEqual(expectedMessage, ex.Message);
                });
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
            Exception exception = null;

            try
            {
                fn();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            var targetException = exception as T;

            if (targetException == null)
            {
                if (exception != null)
                    Assert.Fail($"Expected excception '{typeof(T)}' was not thrown, actual exception '{exception.GetType()}'");
                Assert.Fail($"Expected excception '{typeof(T)}' was not thrown, no exceptions were thrown");
            }

            if (additionalAssertion != null)
                additionalAssertion(targetException);
        }

        protected void AssertItemsNotNull<T>(IEnumerable<T> collection, Action<T, int> additionalCheck = null)
        {
            var index = -1;
            foreach (var item in collection)
            {
                index++;

                if (item == null)
                    Assert.Fail($"Item at index '{index}' is null");

                if (additionalCheck != null)
                    additionalCheck(item, index);
            }
        }

        protected void AssertPhrasesIds<T>(IList<int> expectedIds, IList<T> phrases) where T : Phrase
        {
            AssertIds(expectedIds, phrases, e => e.Id);
        }

        protected void AssertAllowedPhrasesIds<T>(IList<int> allowedIds, IList<T> phrases, int? expectedCount = null) where T : Phrase
        {
            AssertAllowedIds(allowedIds, phrases, e => e.Id, expectedCount);
        }

        protected void AssertIds<T>(IList<int> expectedIds, IList<T> entities, Func<T, int> getId) where T : class
        {
            Assert.IsNotNull(entities);
            Assert.AreEqual(expectedIds.Count, entities.Count);

            foreach (var id in expectedIds)
            {
                var count = entities.Count(e => getId(e) == id);

                if (count < 1)
                {
                    Assert.Fail($"Expected id '{id}' was not found");
                }

                if (count > 1)
                {
                    Assert.Fail($"Id '{id}' is not unique in actual list");
                }
            }
        }

        protected void AssertAllowedIds<T>(IList<int> allowedIds, IList<T> entities, Func<T, int> getId, int? expectedCount = null) where T : class
        {
            Assert.IsNotNull(entities);

            if (expectedCount.HasValue)
                Assert.AreEqual(expectedCount.Value, entities.Count);

            foreach (var entity in entities)
            {
                var id = getId(entity);
                var count = allowedIds.Count(e => e == id);

                if (count < 1)
                {
                    Assert.Fail($"Id '{id}' is not allowed");
                }
            }
        }

        protected void SetIds(IList<PhraseDal> list)
        {
            SetIds(list, (id, e) => e.Id = id);
        }

        protected void SetIds(IList<KnowledgeLevelDal> list)
        {
            SetIds(list, (id, e) => e.Id = id);
        }

        protected void SetIds<T>(IList<T> list, Action<int, T> setId)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                setId(i + 1, item);
            }
        }
    }
}
