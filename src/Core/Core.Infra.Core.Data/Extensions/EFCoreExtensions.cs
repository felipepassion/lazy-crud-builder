using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Niu.Nutri.Core.Application.DTO.Attributes;
using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Entities;
using Niu.Nutri.CrossCuting.Infra.Utils.Extensions;
using System.Reflection;

namespace Niu.Nutri.Core.Infra.Data.Extensions
{
    public static class EfExtensions
    {
        public static bool IsManyToMany(this INavigation navigation)
        {
            var joinEntityType = navigation.ForeignKey.DeclaringEntityType;
            return joinEntityType.IsOwned() && joinEntityType.GetForeignKeys().Count() == 2;
        }

        /// <summary>
        /// Ensures that all navigation properties (up to a certain depth) are eagerly loaded when entities are resolved from this
        /// DbSet.
        /// </summary>
        /// <returns>The queryable representation of this DbSet</returns>
        public static IQueryable<TEntity> IncludeAll<TEntity>(
            this DbSet<TEntity> dbSet,
            int maxDepth = int.MaxValue) where TEntity : class
        {
            IQueryable<TEntity> result = dbSet;
            var context = dbSet.GetService<ICurrentDbContext>().Context;
            var includePaths = GetIncludePaths<TEntity>(context, maxDepth);

            foreach (var includePath in includePaths)
            {
                result = result.Include(includePath);
            }

            return result;
        }

        public static IQueryable<T> Include<T>(this IQueryable<T> source, IEnumerable<string> navigationPropertyPaths)
            where T : class
        {
            if (navigationPropertyPaths == null)
                return source;

            return navigationPropertyPaths.Aggregate(source, (query, path) => query.Include(path));
        }

        public static IEnumerable<string> GetIncludePaths(this DbContext context, Type clrEntityType, int maxDepth = int.MaxValue)
        {
            var entityType = context.Model.FindEntityType(clrEntityType);
            var includedNavigations = new HashSet<INavigation>();
            var stack = new Stack<IEnumerator<INavigation>>();
            var hash = new HashSet<string>();
            while (true)
            {
                var entityNavigations = new List<INavigation>();
                if (stack.Count <= maxDepth)
                {
                    if (entityType != null)
                    {
                        foreach (var navigation in entityType.GetNavigations())
                        {
                            if (includedNavigations.Add(navigation))
                                entityNavigations.Add(navigation);
                        }

                        var listIncludes = entityType.ClrType.GetProperties()
                            .Where(x => x.GetCustomAttributes<ListingPicker>().Any())
                            .ToDictionary(x => clrEntityType.Name + x.PropertyType.FullName?.Substring(19).Split(',')[0].Split(".").Last(), x => x.Name);
                        if (listIncludes.Any())
                        {
                            foreach (var navigation2 in entityType.GetDeclaredReferencingForeignKeys().Where(x => x.PrincipalToDependent == null))
                            {
                                if (listIncludes.ContainsKey(navigation2.DeclaringEntityType.Name))
                                    yield return listIncludes[navigation2.DeclaringEntityType.Name];
                            }
                        }
                    }
                }
                if (entityNavigations.Count == 0)
                {
                    if (stack.Count > 0)
                        yield return string.Join(".", stack.Reverse().Select(e => e.Current.Name));
                }
                else
                {
                    foreach (var navigation in entityNavigations)
                    {
                        var inverseNavigation = navigation.Inverse;
                        if (inverseNavigation != null)
                            includedNavigations.Add(inverseNavigation);
                    }
                    stack.Push(entityNavigations.GetEnumerator());
                }
                while (stack.Count > 0 && !stack.Peek().MoveNext())
                    stack.Pop();
                if (stack.Count == 0) break;
                entityType = stack.Peek().Current.TargetEntityType;
            }
        }

        /// <remarks>
        /// Adapted from https://stackoverflow.com/a/49597502/1636276
        /// </remarks>
        private static IEnumerable<string> GetIncludePaths<T>(DbContext context, int maxDepth = int.MaxValue)
        {
            if (maxDepth < 0)
                throw new ArgumentOutOfRangeException(nameof(maxDepth));

            var entityType = context.Model.FindEntityType(typeof(T));
            var includedNavigations = new HashSet<INavigation>();
            var stack = new Stack<IEnumerator<INavigation>>();

            while (true)
            {
                var entityNavigations = new List<INavigation>();

                if (stack.Count <= maxDepth && entityType != null)
                {
                    foreach (var navigation in entityType.GetNavigations())
                    {
                        if (includedNavigations.Add(navigation))
                            entityNavigations.Add(navigation);
                    }
                }

                if (entityNavigations.Count == 0)
                {
                    if (stack.Count > 0)
                        yield return string.Join(".", stack.Reverse().Select(e => e.Current!.Name));
                }
                else
                {
                    foreach (var navigation in entityNavigations)
                    {
                        var inverseNavigation = navigation.Inverse;
                        if (inverseNavigation != null)
                            includedNavigations.Add(inverseNavigation);
                    }

                    stack.Push(entityNavigations.GetEnumerator());
                }

                while (stack.Count > 0 && !stack.Peek().MoveNext())
                    stack.Pop();

                if (stack.Count == 0)
                    break;

                entityType = stack.Peek().Current!.TargetEntityType;
            }
        }

        public static IQueryable<TEntity> IncludeAllRecursively<TEntity>(this IQueryable<TEntity> queryable,
            int maxDepth = int.MaxValue, bool addSeenTypesToIgnoreList = true, HashSet<Type>? ignoreTypes = null)
            where TEntity : class
        {
            var type = typeof(TEntity);
            var includes = new List<string>();
            ignoreTypes ??= new HashSet<Type>();
            GetIncludeTypes(ref includes, prefix: string.Empty, type, ref ignoreTypes, addSeenTypesToIgnoreList, maxDepth);
            foreach (var include in includes)
            {
                queryable = queryable.Include(include);
            }

            return queryable;
        }

        private static void GetIncludeTypes(ref List<string> includes, string prefix, Type type, ref HashSet<Type> ignoreSubTypes,
            bool addSeenTypesToIgnoreList = true, int maxDepth = int.MaxValue)
        {
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var getter = property.GetGetMethod();
                if (getter != null)
                {
                    var isVirtual = getter.IsVirtual;
                    if (isVirtual && getter.GetType().ContainsBase<Entity>())
                    {
                        var propPath = (prefix + "." + property.Name).TrimStart('.');
                        if (maxDepth <= propPath.Count(c => c == '.')) { break; }

                        includes.Add(propPath);

                        var subType = property.PropertyType;
                        if (ignoreSubTypes.Contains(subType))
                        {
                            continue;
                        }
                        else if (addSeenTypesToIgnoreList)
                        {
                            // add each type that we have processed to ignore list to prevent recursions
                            ignoreSubTypes.Add(type);
                        }

                        var isEnumerableType = subType.GetInterface(nameof(System.Collections.IEnumerable)) != null;
                        var genericArgs = subType.GetGenericArguments();
                        if (isEnumerableType && genericArgs.Length == 1)
                        {
                            // sub property is collection, use collection type and drill down
                            var subTypeCollection = genericArgs[0];
                            if (subTypeCollection != null)
                            {
                                GetIncludeTypes(ref includes, propPath, subTypeCollection, ref ignoreSubTypes, addSeenTypesToIgnoreList, maxDepth);
                            }
                        }
                        else
                        {
                            // sub property is no collection, drill down directly
                            GetIncludeTypes(ref includes, propPath, subType, ref ignoreSubTypes, addSeenTypesToIgnoreList, maxDepth);
                        }
                    }
                }
            }
        }
    }
}
