using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Niu.Nutri.Shared.Blazor.Components.Models
{
    public class AggregatePageModel
    {
        public AggregatePageModel(Type type)
        {
            this.FullName = type.FullName;
            this.GroupName = this.FullName?.Split(".").FirstOrDefault(x => x.EndsWith("Agg"))!;
            this.Path = type.GetCustomAttributes<RouteAttribute>(inherit: true).FirstOrDefault()?.Template!;

            if (this.Path != null)
                this.PageName = string.Join(" ", this.Path?.Split(@"/")!);
            else
            {
                this.GroupName = "ActionButtons";
                this.Path = this.FullName!;
                this.PageName = this.FullName?.Split(".").LastOrDefault()!;
            }
        }

        public AggregatePageModel(Type type, bool isAssembly)
        {
            this.FullName = type.FullName;
            var splitted = this.FullName?.Split(".");
            this.PageName = this.FullName?.Split(".").FirstOrDefault(x => x.StartsWith("Page") && char.IsDigit(x.LastOrDefault()))!;

            this.GroupName = splitted?.ElementAtOrDefault(5)!;

            this.Path = type.FullName?.Split(".").LastOrDefault()!;
        }

        public string ExternalId { get; set; }
        public string? Path { get; set; }
        public string? FullName { get; }
        public string GroupName { get; set; }
        public string PageName { get; set; }
    }
}
