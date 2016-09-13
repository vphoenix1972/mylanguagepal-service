using System.Data.Entity;
using System.Linq;
using Moq;

namespace MyLanguagePalService.Tests
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
    }
}