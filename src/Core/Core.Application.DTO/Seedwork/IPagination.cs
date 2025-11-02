using System.Collections;

namespace Niu.Nutri.Core.Application.DTO.Seedwork
{

    /// <summary>
    /// The interface supports a collection of objects of pagination.
    /// </summary>
    /// <remarks>Used mostly in search results.</remarks>
    public interface IPagination : IEnumerable
    {

        /// <summary>
        /// Gets the current page number.
        /// </summary>
        /// <remarks></remarks>
        int CurrentPage { get; }

        /// <summary>
        /// Gets the number of items per page.
        /// </summary>
        /// <remarks></remarks>
        int PageSize { get; }

        /// <summary>
        /// Gets the total number of elements.
        /// </summary>
        /// <remarks></remarks>
        int Items { get; }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        /// <remarks></remarks>
        int Pages { get; }

        /// <summary>
        ///Gets the index of the first element on the page.
        /// </summary>
        /// <remarks></remarks>
        int First { get; }

        /// <summary>
        /// Gets the index of the last element on the page.
        /// </summary>
        /// <remarks></remarks>
        int Last { get; }

        /// <summary>
        /// Gets a value indicating whether an instance of the previous page.
        /// </summary>
        /// <remarks></remarks>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Gets a value indicating whether the instance has the next page.
        /// </summary>
        /// <remarks></remarks>
        bool HasNextPage { get; }
    }

    /// <summary>
    /// Generic for <see cref="IPagination"/>.
    /// </summary>
    /// <typeparam name="T">The type of object that is paging.</typeparam>
    /// <remarks></remarks>
    public interface IPagination<T> : IPagination, IEnumerable<T> { }
}
