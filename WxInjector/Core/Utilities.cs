using System;
using System.Windows;
using ControlzEx.Theming;

namespace WxInjector.Core
{

    public static class Utilities
    {

        public static void SetAppTheme(string colorScheme, bool darkMode, bool afterInit = true)
        {
            if (afterInit)
            {
                ThemeManager.Current.ChangeThemeColorScheme(Application.Current, colorScheme);
                ThemeManager.Current.ChangeThemeBaseColor(Application.Current, darkMode ? ThemeManager.BaseColorDarkConst : ThemeManager.BaseColorLightConst);
            }
            else
            {
                var dictionary = new ResourceDictionary
                {
                    Source = darkMode
                        ? new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Dark.{colorScheme}.xaml")
                        : new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.{colorScheme}.xaml")
                };
                Application.Current.Resources.MergedDictionaries.Add(dictionary);
            }
        }

    }

}