using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using ClassIsland.Models;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;

namespace ClassIsland.Services;

public class ThemeService : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }

    public ITheme? CurrentTheme { get; private set; }

    public event EventHandler<ThemeUpdatedEventArgs>? ThemeUpdated;

    public int CurrentRealThemeMode { get; private set; } = 0;

    public void SetTheme(int themeMode, Color primary, Color secondary)
    {
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        var lastPrimary = theme.PrimaryMid.Color;
        var lastSecondary = theme.SecondaryMid.Color;
        var lastBaseTheme = theme.GetBaseTheme();
        switch (themeMode)
        {
            case 0:
                try
                {
                    var key = Registry.CurrentUser.OpenSubKey(
                        "Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize");
                    if (key != null)
                    {
                        if ((int?)key.GetValue("AppsUseLightTheme") == 0)
                        {
                            theme.SetBaseTheme(new MaterialDesignDarkTheme());
                        }
                        else
                        {
                            theme.SetBaseTheme(new MaterialDesignLightTheme());
                        }
                    }
                }
                catch
                {
                    theme.SetBaseTheme(new MaterialDesignLightTheme());
                }
                break;

            case 1:
                theme.SetBaseTheme(new MaterialDesignLightTheme());
                break;
            case 2:
                theme.SetBaseTheme(new MaterialDesignDarkTheme());
                break;
        }
        
        ((Theme)theme).ColorAdjustment = new ColorAdjustment()
        {
            DesiredContrastRatio = 4.5F,
            Contrast = Contrast.Medium,
            Colors = ColorSelection.All
        };
        
        theme.SetPrimaryColor(primary);
        theme.SetSecondaryColor(secondary);
        var lastTheme = paletteHelper.GetTheme();

        if (lastPrimary == theme.PrimaryMid.Color &&
            lastSecondary == theme.SecondaryMid.Color &&
            lastBaseTheme == theme.GetBaseTheme())
        {
            return;
        }


        paletteHelper.SetTheme(theme);
        CurrentTheme = theme;
        ThemeUpdated?.Invoke(this, new ThemeUpdatedEventArgs
        {
            ThemeMode = themeMode,
            Primary = primary,
            Secondary = secondary,
            RealThemeMode = theme.GetBaseTheme() == BaseTheme.Light ? 0 : 1
        });
        CurrentRealThemeMode = theme.GetBaseTheme() == BaseTheme.Light ? 0 : 1;
    }
}