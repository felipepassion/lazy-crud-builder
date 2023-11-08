using LazyCrud.Core.Application.DTO.Attributes;
using LazyCrud.Core.Application.DTO.Extensions;
using LazyCrud.Core.Domain.CrossCutting;
using MediatR;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Models
{
    public interface IEntityDTO
    {
        public IRequest<DomainResponse> Command { get; set; }
        public string Title { get; }
        public string SubTitle { get; }
        public string SubTitlePropertyName { get; }
        public string TititleWithSubtitle { get; }
        public string DisplayNameTitle { get; }
        public string DisplayNameSubTitle { get; }
        public int? Id { get; set; }
        public string ExternalId { get; set; }
        public string[] FieldsToValidate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string H2AndSubTitle { get; }
        public string CustomTitleOrH2 { get; }
        public string H1 { get; }
        public string H1AndTitle { get; }
        public bool IsSubmiting { get; set; }
        public bool IsManuallySubmiting { get; set; }

        public string GetRoute();
        public string GetPageIndexRoute();
        public string GetRoute(object id);
        public string GetDeleteRoute();
        public string GetSearchRoute();
        public string GetSelectRoute();
        public string GetSummaryRoute();
        public string GetMyTypeName();
    }

    public class EntityDTO : IEntityDTO, IEqualityComparer
    {
        public bool IsCreated => Id.HasValue;

        public IRequest<DomainResponse> Command { get; set; }
        public int? Id { get; set; }
        public string ExternalId { get; set; }
        public string[] FieldsToValidate { get; set; } = new string[0];
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string TititleWithSubtitle => $"{Title}{(string.IsNullOrWhiteSpace(SubTitle) ? "" : $" - {SubTitle}")}";

        public bool IsSubmiting { get; set; }
        public bool IsManuallySubmiting { get; set; }

        public EntityDTO()
        {
            CreatedAt = CreatedAt ?? DateTime.UtcNow;
        }

        public void Created(EntityDTO newObj)
        {
            if (newObj == null) return;
            Id = newObj.Id;
            CreatedAt = newObj.CreatedAt;
            UpdatedAt = newObj.UpdatedAt;
        }

        public virtual string GetRoute()
        {
            return $"api/{GetMyTypeName()}";
        }

        public virtual string GetPageIndexRoute()
        {
            return $"{GetMyTypeName()}";
        }

        public virtual string GetRoute(object id)
        {
            return $"{GetRoute()}/{id}";
        }

        public virtual string GetDeleteRoute()
        {
            return $"{GetRoute()}/delete";
        }
        public virtual string GetSearchRoute()
        {
            return $"{GetRoute()}/search";
        }
        public virtual string GetSelectRoute()
        {
            return $"{GetRoute()}/select";
        }
        public virtual string GetSummaryRoute()
        {
            return $"{GetRoute()}/summary";
        }
        public virtual string CountRoute()
        {
            return $"{GetRoute()}/count";
        }
        public virtual string GetRegisterPageRoute()
        {
            return $"{GetMyTypeName()}/Cadastro";
        }

        public virtual string GetMyTypeName()
        {
            var type = GetType().Name;
            var route = type.Replace("[]", "").Replace("DTO", "").Replace("Listining", "");
            if (route.StartsWith("Base"))
                route = route.Substring("Base".Length);
            return $"{route}";
            //return $"{route}?id={ExternalId}";
        }

        public string GetFieldPlaceholder(string col)
        {
            MemberInfo property = GetType().GetProperty(col);
            if (property == null) return null;

            var dd = property.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;

            return dd?.DisplayName ?? property.Name;
        }

        public string GetFieldPlaceholder(PropertyInfo property)
        {
            if (property == null) return null;

            var dd = property.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;

            return dd?.DisplayName ?? property.Name;
        }

        public string Title
        {
            get
            {
                var val = this.GetValueWithAttribute<Title>()?.ToString();
                if (string.IsNullOrWhiteSpace(val))
                {
                    //val = this.GetType().GetCustomAttribute<H1>()?.Title ?? null;
                }
                return val;
            }
        }

        public string SubTitle
        {
            get
            {
                var val = this.GetValueWithAttribute<Subtitle>()?.ToString();
                if (string.IsNullOrWhiteSpace(val))
                {
                    //val = this.GetType().GetCustomAttribute<H2>()?.Title ?? null;
                }
                return val;
            }
        }

        public string DisplayNameTitle
        {
            get
            {
                var property = this.GetFieldInfoByWithAttribute<Title>();
                return property?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? property?.Name ?? "Título";
            }
        }

        public string DisplayNameSubTitle
        {
            get
            {
                var property = this.GetFieldInfoByWithAttribute<Subtitle>();
                return property?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? property?.Name ?? "SubTítulo";
            }
        }

        public string TitlePropertyName
        {
            get
            {
                var property = this.GetFieldInfoByWithAttribute<Title>();
                return property?.Name;
            }
        }

        public string H1
        {
            get
            {
                return GetType().GetCustomAttribute<H1>()?.Title;
            }
        }

        public string H2
        {
            get
            {
                return GetType().GetCustomAttribute<H2>()?.Title;
            }
        }

        public string H1AndTitle
        {
            get
            {
                return H1 + (string.IsNullOrWhiteSpace(Title) ? "" : $"{(string.IsNullOrWhiteSpace(H1) ? "" : " - ")}{Title}");
            }
        }
        public string H2AndSubTitle
        {
            get
            {
                return H2 + (string.IsNullOrWhiteSpace(SubTitle) ? "" : $" - {SubTitle}");
            }
        }

        public string CustomTitleOrH2
        {
            get
            {
                return string.IsNullOrWhiteSpace(Title) ? H2 : Title;
            }
        }

        public string CustomTitleOrH1
        {
            get
            {
                return string.IsNullOrWhiteSpace(Title) ? H1 : Title;
            }
        }

        public string SubTitlePropertyName
        {
            get
            {
                var property = this.GetFieldInfoByWithAttribute<Subtitle>();
                return property?.Name;
            }
        }

        string buildParam(string name, object val, bool and = true)
        {
            if (string.IsNullOrWhiteSpace(val?.ToString())) return null;
            return $"{name}={val}{(and ? "&" : "")}";
        }

        public string BuildQuerySearchString(string searchText, bool getOnlyActive = true) =>
                string.IsNullOrWhiteSpace(searchText) ? "" :
                $"{buildParam($"{TitlePropertyName}Contains", searchText)}" +
                $"{buildParam($"{SubTitlePropertyName}Contains", searchText)}" +
                $"{buildParam("Active", getOnlyActive)}" +
                $"{buildParam("IsOrSpecification", true)}";

        public new bool Equals(object? x, object? y)
        {
            return (x as IEntityDTO)?.Id == (y as IEntityDTO)?.Id;
        }

        public int GetHashCode(object obj)
        {
            return (obj as IEntityDTO)?.Id ?? base.GetHashCode();
        }
    }
}
