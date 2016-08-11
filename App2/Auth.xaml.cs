using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace App2
{

    using Views;
    using Windows.UI;
    using Windows.UI.ViewManagement;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class Auth : Page
    {
        public Auth()
        {
            this.InitializeComponent();
        }

        //private void button_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Frame.Navigate(typeof(MainIndex));
        //}

        //private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        //{
        //}

        //private void username_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //}

        //private void textBlock_Copy_SelectionChanged(object sender, RoutedEventArgs e)
        //{

        //}




        public void Login_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainIndex));
        }

        public void SignUp_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Views/SignUpPage.xaml", UriKind.Relative
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
