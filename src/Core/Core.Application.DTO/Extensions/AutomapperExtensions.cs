using Niu.Nutri.Core.Application.DTO.Seedwork;
using System.Diagnostics;

namespace Niu.Nutri.Core.Application.DTO.Extensions
{
    public static class AutomapperExtensions
    {
        public static T ProjectedAs<T>(this object obj) where T : class
        {
            try
            {
                if (obj is null) return null;
                var test = MapperFactory.Mapper.Map<T>(obj);
                return MapperFactory.Mapper.Map<T>(obj);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Console.WriteLine(ex);
                throw;
            }
        }

        public static T[] ProjectedAsCollection<T>(this object obj) where T : class
        {
            if (obj is null) return new T[0];
            return MapperFactory.Mapper.Map<IEnumerable<T>>(obj)?.ToArray() ?? new T[0];
        }
    }
}
