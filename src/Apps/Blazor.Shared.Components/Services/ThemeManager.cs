using Microsoft.JSInterop;

#pragma warning disable IDE0290 // Use primary constructor

namespace Niu.Nutri.Shared.Blazor.Components.Services
{
    public class ThemeManager : IThemeManager
    {
        public string CurrentTheme { get; set; } = "light-theme";

        public async Task SetThemeAsync(string themeName, IJSRuntime jSRuntime)
        {
            CurrentTheme = themeName;
            await jSRuntime.InvokeVoidAsync("localStorage.setItem", "currentTheme", themeName);
        }

        public async Task<string> GetThemeAsync(IJSRuntime jSRuntime)
        {
            var themeName = await jSRuntime.InvokeAsync<string>("localStorage.getItem", "currentTheme");
            CurrentTheme = themeName ?? "light-theme";
            return CurrentTheme;
        }
    }
}

#pragma warning restore IDE0290 // Use primary constructor