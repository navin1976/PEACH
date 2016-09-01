// source: https://github.com/Microsoft-Build-2016/CodeLabs-UWP/blob/master/Workshop/Module1-AdaptiveUI/Source/End/Microsoft.Labs.SightsToSee/Microsoft.Labs.SightsToSee/Models/NavMenuItem.cs
// source: https://github.com/MvvmCross/MvvmCross-Samples/blob/master/XPlatformMenus/XPlatformMenus.UWP/NavMenuItem.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace DataVisualization
{
    // Data to represent an item in the nav menu.
    public class NavMenuItem
    {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public char SymbolAsChar
        {
            get
            {
                return (char)this.Symbol;
            }
        }

        public Type DestPage { get; set; }
        public object Arguments { get; set; }
    }
}
