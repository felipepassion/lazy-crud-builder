using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Models;
using Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects;
using System.Reflection;

namespace Niu.Nutri.Core.Blazor.Shared.Components.DefaultDesign
{
    [Authorize]
    public class BaseListiningPage : LayoutComponentBase
    {
        [SupplyParameterFromQuery][Parameter] public int? Page { get; set; }
        [SupplyParameterFromQuery][Parameter] public int? Size { get; set; }
        [SupplyParameterFromQuery][Parameter] public bool? OrderByDesc { get; set; }
        [SupplyParameterFromQuery][Parameter] public string OrderBy { get; set; }
    }

    public class ListiningContext : IListiningContext
    {
        public ListiningContext(
            NavigationManager navigationManager,
            string title,
            int totalOfItems,
            IEnumerable<PropertyDetails> properties,
            Func<Task> paginationLayoutRefresh,
            string queryString = null,
            int? page = 0,
            int? size = 10000,
            string orderBy = null,
            bool? orderByDescending = true,
            bool? OpenNewPageOnRegisterButtonClicked = true)
        {
            Size = size ?? 100;
            Page = page ?? 0;
            Title = title;
            OrderBy = orderBy ?? $"{nameof(EntityDTO.CreatedAt)}";
            Properties = properties;
            PaginationLayoutRefresh = paginationLayoutRefresh;
            QueryString = queryString ?? $"{nameof(ActivableEntityDTO.Active)}=true";
            OrderByDesc = orderByDescending ?? false;
            this.OpenNewPageOnRegisterButtonClicked = OpenNewPageOnRegisterButtonClicked;
            TotalOfItems = totalOfItems;
            NavigationManager = navigationManager;
        }

        public string Title { get; set; }
        public string QueryString { get; set; }
        public IEnumerable<PropertyDetails> Properties { get; set; }
        public Func<Task> PaginationLayoutRefresh { get; set; }
        public bool? OrderByDesc { get; set; } = true;
        public bool? OpenNewPageOnRegisterButtonClicked { get; }
        //public CadastroModal RegisterModal { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public string OrderBy { get; set; }
        public int TotalOfItems { get; set; }
        public int Max { get { return (int)(TotalOfItems / Size); } }
        public int Min { get { return Page - 2 > 0 ? Page - 2 : 0; } }

        public NavigationManager NavigationManager { get; set; }

        public void NavigateToRegisterPage<T>(string id) where T : EntityDTO, new()
        { NavigationManager.NavigateTo(@$"/{new T().GetMyTypeName()}/cadastro/{id}"); }

        public void Navigate<T>(string queryString = null, int? page = null, int? size = null, bool? orderByDescending = true)
           where T : EntityDTO, new()
        {
            this.QueryString = queryString ?? this.QueryString;
            this.Page = page ?? this.Page;
            this.Size = size ?? this.Size;
            this.OrderByDesc = orderByDescending ?? this.OrderByDesc;

            NavigationManager.NavigateTo($"{new T().GetMyTypeName()}?" +
                $"{buildParam(nameof(Page), Page)}" +
                $"{buildParam(nameof(Size), Size)}" +
                $"{buildParam(nameof(OrderBy), OrderBy)}" +
                $"{buildParam(nameof(OrderByDesc), OrderByDesc)}" +
                $"{QueryString}");

            if (PaginationLayoutRefresh != null)
                PaginationLayoutRefresh();
        }

        string buildParam(string name, object val, bool and = true)
        {
            if (string.IsNullOrWhiteSpace(val?.ToString())) return null;
            return $"{name}={val}{(and ? "&" : "")}";
        }

        public void OpenRegisterModal<T>(string id = null)
            where T : EntityDTO, new()
        {
            if (OpenNewPageOnRegisterButtonClicked == true)
            {
                NavigateToRegisterPage<T>(id);
            }
            else
            {
                var model = new T() { ExternalId = id };

                var types = (from type in Assembly.GetExecutingAssembly().GetTypes()
                             where
                             type.FullName?.Contains($".{model.GetMyTypeName()}.Cadastro", StringComparison.InvariantCultureIgnoreCase) == true
                             select type);

                var myType = (from type in model.GetType().Assembly.GetTypes()
                              where
                             type.Name.Equals($"{model.GetMyTypeName()}DTO", StringComparison.InvariantCultureIgnoreCase) == true
                              select type).FirstOrDefault();

                var t = types.LastOrDefault(x => x.FullName.EndsWith(".Cadastro.Cadastro"));

                var rightContainerType = types.Where(x => x.FullName.EndsWith("RightContainer", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                //if (this.RegisterModal.OnCloseModal == null)
                //{
                //    this.RegisterModal.OnCloseModal = () => Task.Run(PaginationLayoutRefresh);
                //}

                //this.RegisterModal.RenderFormContent(t, rightContainerType, model, this.PaginationLayoutRefresh);
            }
        }

    }

    public interface IListiningContext
    {
        public void OpenRegisterModal<T>(string id = null) where T : EntityDTO, new();
        string OrderBy { get; set; }
        bool? OrderByDesc { get; set; }
        int Page { get; set; }
        int Size { get; set; }
        string Title { get; set; }
        string QueryString { get; set; }
        int TotalOfItems { get; set; }
        int Max { get; }
        int Min { get; }
        IEnumerable<PropertyDetails> Properties { get; set; }
        Func<Task> PaginationLayoutRefresh { get; set; }
        NavigationManager NavigationManager { get; }
        //CadastroModal RegisterModal { get; set; }
        public void Navigate<T>(string queryString = null, int? page = null, int? size = null, bool? orderByDescending = null) where T : EntityDTO, new();
        public void NavigateToRegisterPage<T>(string id) where T : EntityDTO, new();
    }
}
