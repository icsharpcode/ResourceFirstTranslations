using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace ResourcesFirstTranslations.Tests.Sync
{
    public static class EfMoqHelpers
    {
        public static void SetupIQueryable<T>(Mock<DbSet<T>> mockSet, List<T> data) where T : class
        {
            SetupIQueryable<T>(mockSet, data.AsQueryable());
        }

        public static void SetupIQueryable<T>(Mock<DbSet<T>> mockSet, IQueryable<T> data) where T : class
        {
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }

    }
}
