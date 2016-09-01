using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace DataVisualization
{
    using Views;
    using Windows.UI;
    using Windows.UI.ViewManagement;

    sealed partial class App : Application
    {

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            #if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                //this.DebugSettings.EnableFrameRateCounter = true;
            }
            #endif
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(640, 400));

            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

            ApplicationView.GetForCurrentView().Title = "PEACH";
            


            if (titleBar != null)
            {
                Color titleBarColor = (Color)App.Current.Resources["SystemChromeMediumColor"];
                titleBar.BackgroundColor = titleBarColor;
                titleBar.ButtonBackgroundColor = titleBarColor;
            }

            /*
            MainIndex shell = Window.Current.Content as MainIndex;

            if (shell == null)
            {
                shell = new MainIndex();
                shell.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
                shell.AppFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
            }
            Window.Current.Content = shell;

            if (shell.AppFrame.Content == null)
            {
                //shell.AppFrame.Navigate(typeof(LandingPage), e.Arguments, new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());
            }

            Window.Current.Activate();
            */

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(Auth), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
            
        }


        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}
