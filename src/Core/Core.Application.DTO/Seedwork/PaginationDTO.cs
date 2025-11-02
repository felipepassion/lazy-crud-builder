using System.Collections;
using System.Runtime.Serialization;

namespace Niu.Nutri.Core.Application.DTO.Seedwork
{
    [DataContract]
    public class PaginationDTO<T> : IEnumerable, IEnumerable<T>
    {

        public PaginationDTO()
        {
            Data = new List<T>();
        }

        public PaginationDTO(IEnumerable<T> data, int currentPage, int pageSize, int items)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            Items = items;
            Data = data.ToList();
        }

        public PaginationDTO(IPagination<T> source)
        {
            CurrentPage = source.CurrentPage;
            PageSize = source.PageSize;
            Items = source.Items;
            Data = source.ToList();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [DataMember]
        public List<T> Data { get; private set; }

        [DataMember]
        public int PageSize { get; private set; }

        [DataMember]
        public int CurrentPage { get; private set; }

        [DataMember]
        public int Items { get; private set; }

        [IgnoreDataMember]
        public int Pages
        {
            get
            {
                int result = (int)Math.Ceiling((double)Items / PageSize);
                return result == 0 ? 1 : result;
            }
        }

        [IgnoreDataMember]
        public int First
        {
            get
            {
                return (CurrentPage - 1) * PageSize + 1;
            }
        }

        [IgnoreDataMember]
        public int Last
        {
            get
            {
                return First + Data.Count() - 1;
            }
        }

        [IgnoreDataMember]
        public bool HasPreviousPage
        {
            get { return CurrentPage > 1; }
        }

        [IgnoreDataMember]
        public bool HasNextPage
        {
            get { return CurrentPage < Pages; }
        }

        public static PaginationDTO<T> Empty(int currentPage, int pageSize)
        {
            return new PaginationDTO<T>(new List<T>(0), currentPage, pageSize, 0);
        }
    }
}
