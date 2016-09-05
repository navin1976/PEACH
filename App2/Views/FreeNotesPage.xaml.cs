//https://blog.srife.net/2016/01/04/uwp-how-to-bind-the-calendardatepicker/
//https://social.msdn.microsoft.com/Forums/expression/en-US/0e1fc1dd-3ff0-4a27-ad99-a82a0f6577e9/uwpxamlcalendardatepicker-not-binding-to-date-property?forum=wpdevelop
//https://msdn.microsoft.com/windows/uwp/controls-and-patterns/tabs-pivot

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using Windows.Media.SpeechRecognition;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Media;
using Windows.Globalization;
using Windows.UI.Xaml.Documents;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;
using Windows.UI.Text.Core;
using System.Diagnostics;

namespace DataVisualization.Views
{
    public sealed partial class FreeNotesPage : Page
    {
        Symbol inputSymbol = (Symbol)0xEC87;
        Symbol SaveFile = (Symbol)0xE105;
        Symbol OpenFile = (Symbol)0xE8E5;
        Symbol TouchWriting = (Symbol)0xED5F;
        Symbol HelpLegacy = (Symbol)0xE11B;
        Symbol RecogSymbol = (Symbol)0xEF16;

        DateTime bindingToday = DateTime.Today;


        InkRecognizerContainer inkRecognizerContainer = null;
        const string InstallRecoText = 
            "You can install handwriting recognition engines for other languages by this: go to Settings -> Time & language -> Region & language, choose a language, and click Options, then click Download under Handwriting";
        private IReadOnlyList<InkRecognizer> recoView = null;
        private Language previousInputLanguage = null;
        private CoreTextServicesManager textServiceManager = null;
        private ToolTip recoTooltip;

        // Voice Recognition
        private CoreDispatcher dispatcher;
        private SpeechRecognizer speechRecognizer;
        private bool isListening;
        private StringBuilder dictatedTextBuilder;

        private static uint HResultPrivacyStatementDeclined = 0x80045509;

        public FreeNotesPage()
        {
            this.InitializeComponent(); // initialize XAML
            this.InitializeForm();
            this.initializeProstateInking();
            //inkToolBarPanel.Visibility = Visibility.Collapsed;
            examCalendarDatePicker.Date = bindingToday;
            dobCalendarDatePicker.MinDate = new DateTime(1900, 1, 1);
            dobCalendarDatePicker.MaxDate = DateTime.Today;

        }

        // Upon entering the scenario, ensure that we have permissions to use the Microphone
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Keep track of the UI thread dispatcher, as speech events will come in on a separate thread.
            dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

            bool permissionGained = await AudioCapturePermissions.RequestMicrophonePermission();
            if (permissionGained)
            {
                btnContinuousRecognize.IsEnabled = true;
                PopulateLanguageDropdown();
                await InitializeRecognizer(SpeechRecognizer.SystemSpeechLanguage);
            }
            else
            {
                this.freeNoteTextBox.Text = "Permission to access microphone denied: Settings->Privacy->Microphone.";
                btnContinuousRecognize.IsEnabled = false;
                cbLanguageSelection.IsEnabled = false;
            }

            PivotMain.Visibility = Visibility.Visible;
            inkCanvas.Width = Window.Current.Bounds.Width;
            inkCanvas.Height = Window.Current.Bounds.Height;
            hwCanvas.Width = Window.Current.Bounds.Width;
        }



        public DateTime SomeDateTime
        {
            get { return (DateTime)GetValue(SomeDateTimeProperty); }
            set { SetValue(SomeDateTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SomeDateTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SomeDateTimeProperty =
            DependencyProperty.Register("bindingToday", typeof(DateTime), typeof(MainIndex), new PropertyMetadata(0));


        private void btnSaveNote_Click(object sender, RoutedEventArgs e)
        {
            // saving to database implementation
        }
        private void btnLoadNote_Click(object sender, RoutedEventArgs e)
        {
            // saving to database implementation
        }



        // Adding Notification Box
        public enum NotifyType
        {
            StatusMessage,
            ErrorMessage
        };

        public void NotifyUser(string strMessage, NotifyType type)
        {
            switch (type)
            {
                case NotifyType.StatusMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.LightGreen);
                    break;
                case NotifyType.ErrorMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                    break;
            }
            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            StatusBorder.Visibility = (StatusBlock.Text != String.Empty) ? Visibility.Visible : Visibility.Collapsed;
            if (StatusBlock.Text != String.Empty)
            {
                StatusBorder.Visibility = Visibility.Visible;
            }
            else
            {
                StatusBorder.Visibility = Visibility.Collapsed;
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    // MessageBox.Show("LoginAppBar launched");
                    inkToolBarPanel.Visibility = Visibility.Collapsed;
                    break;

                case 1:
                    // MessageBox.Show("DefaultAppBar launched");
                    inkToolBarPanel.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void InkToolBarInfo_Tapped(object sender, RoutedEventArgs e)
        {
            inkToolBarInfo.Flyout.ShowAt(sender as FrameworkElement);
        }
    }
}
