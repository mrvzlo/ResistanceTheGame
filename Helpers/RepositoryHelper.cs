using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Resistance.Helpers
{
    public static class RepositoryHelper
    {
        public static void InsertOrUpdate<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
        {
            // ReSharper disable once PossibleNullReferenceException
            if (!IsAttached(dbContext, entity))
            {
                var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
                var objectSet = objectContext.CreateObjectSet<TEntity>();

                var keyNames = objectSet.EntitySet.ElementType.KeyMembers.Select(x => x.Name);
                var hasDefaultKeys = keyNames.Any(keyName =>
                {
                    var property = typeof(TEntity).GetProperty(keyName);
                    var defaultValue = property.PropertyType.IsValueType
                        ? Activator.CreateInstance(property.PropertyType)
                        : null;
                    return Equals(property.GetValue(entity), defaultValue);
                });

                dbContext.Entry(entity).State =
                    hasDefaultKeys ? EntityState.Added : EntityState.Modified;
            }
            dbContext.SaveChanges();
        }

        private static bool IsAttached<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class =>
            dbContext.Set<TEntity>().Local.Any(e => e == entity);

        public static void Delete<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
        {
            dbContext.Entry(entity).State = EntityState.Deleted;
            dbContext.SaveChanges();
        }
    }
}