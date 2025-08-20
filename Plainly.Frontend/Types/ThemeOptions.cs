using MudBlazor;

namespace Plainly.Frontend.Types;

public class ThemeOptions
{
    public bool IsDarkMode { get; set; } = true;
    public MudTheme Theme { get; set; } = new();
}