using System;
using System.Data.Entity;
using JetBrains.Annotations;

namespace MyLanguagePalService.DAL.Extensions
{
    public static class ApplicationDbContextExtensions
    {
        public static T AddOrUpdate<T>(this IApplicationDbContext db,
            [NotNull] Func<IApplicationDbContext, IDbSet<T>> dbSetGetter,
            [NotNull] Func<IDbSet<T>, T> searcher,
            [NotNull] Action<T> setter)
            where T : class, new()
        {
            if (dbSetGetter == null)
                throw new ArgumentNullException(nameof(dbSetGetter));

            if (searcher == null)
                throw new ArgumentNullException(nameof(searcher));

            if (setter == null)
                throw new ArgumentNullException(nameof(setter));

            var dbSet = dbSetGetter(db);

            if (dbSet == null)
                throw new InvalidOperationException($"dbSetGetter must not return null");

            var isNew = false;
            var entity = searcher(dbSet);
            if (entity == null)
            {
                entity = new T();
                isNew = true;
            }

            setter(entity);

            if (isNew)
            {
                dbSet.Add(entity);
            }
            else
            {
                db.MarkModified(entity);
            }

            return entity;
        }
    }
}