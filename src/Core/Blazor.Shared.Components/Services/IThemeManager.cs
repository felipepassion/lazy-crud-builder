
using Microsoft.JSInterop;

namespace Niu.Nutri.Shared.Blazor.Components.Services
{
    public interface IThemeManager
    {
        string CurrentTheme { get; }

        Task<string> GetThemeAsync(IJSRuntime jSRuntime);
        Task SetThemeAsync(string themeName, IJSRuntime jSRuntime);
    }
}