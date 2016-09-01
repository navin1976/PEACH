
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
        Symbol inputSymbol = (Symbol)0xE104;
        Symbol SaveFile = (Symbol)0xE105;
        Symbol OpenFile = (Symbol)0xE118;
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
            isListening = false; // controller for active listening
            dictatedTextBuilder = new StringBuilder(); // string builder is used to append voice strings

            // initialize inking attributes
            InkDrawingAttributes drawingAttributes = new InkDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Colors.Blue;
            double penSize = 4;
            drawingAttributes.Size = new Windows.Foundation.Size(penSize, penSize);
            drawingAttributes.IgnorePressure = false;
            drawingAttributes.FitToCurve = true;

            // Show the available recognizers
            inkRecognizerContainer = new InkRecognizerContainer();
            recoView = inkRecognizerContainer.GetRecognizers();
            if (recoView.Count > 0)
            {
                foreach (InkRecognizer recognizer in recoView)
                {
                    cbHandWritingRecos.Items.Add(recognizer.Name);
                }
            }
            else
            {
                cbHandWritingRecos.IsEnabled = false;
                cbHandWritingRecos.Items.Add("No Recognizer Available");
            }
            cbHandWritingRecos.SelectedIndex = 2;

            // Set the text services so we can query when language changes
            textServiceManager = CoreTextServicesManager.GetForCurrentView();
            textServiceManager.InputLanguageChanged += TextServiceManager_InputLanguageChanged;

            SetDefaultRecognizerByCurrentInputMethodLanguageTag();

            // Initialize recognizer's tooltip
            recoTooltip = new ToolTip();
            recoTooltip.Content = InstallRecoText;
            ToolTipService.SetToolTip(InstallReco, recoTooltip);

            // Initialize the InkCanvas
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            inkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen | Windows.UI.Core.CoreInputDeviceTypes.Touch;

            this.Unloaded += NotesPenPage_Unloaded;
            this.SizeChanged += NotesPenPage_SizeChanged;
        }
        
        // Keyboard / Surface Pen toggle switch. It hides the inkCanvas so that direct
        // Typing to the textbox is possible
        private void Toggle_inputSwitch(object sender, RoutedEventArgs e)
        {
            if (inkToggle.IsChecked == true)
            {
                inkToggle.Content = new SymbolIcon(Symbol.Keyboard);
                this.inkCanvas.Visibility = Visibility.Collapsed;
                freeNoteTextBox.IsReadOnly = false;
                ToggleButtonIndicator.Text = "Using Touch Keyboard";
            }
            else
            {
                //inputSymbol = (Symbol)0xE104;
                inkToggle.Content = new SymbolIcon((Symbol)0xE104);
                this.inkCanvas.Visibility = Visibility.Visible;
                freeNoteTextBox.IsReadOnly = true;
                ToggleButtonIndicator.Text = "Using Surface Pen";
            }
        }

        private void btnSaveNote_Click(object sender, RoutedEventArgs e)
        {
            // saving to database implementation
        }
        private void btnLoadNote_Click(object sender, RoutedEventArgs e)
        {
            // saving to database implementation
        }

        private void txtAP_LostFocus(object sender, RoutedEventArgs e)
        {
            assignVolumeScore();
        }

        private void txtTR_LostFocus(object sender, RoutedEventArgs e)
        {
            assignVolumeScore();
        }

        private void txtCC_LostFocus(object sender, RoutedEventArgs e)
        {
            assignVolumeScore();
        }

        private void assignVolumeScore()
        {
            if (canMeasureVolume())
            {
                try
                {
                    double APscore = Convert.ToDouble(txtAP.Text);
                    double TRscore = Convert.ToDouble(txtTR.Text);
                    double CCscore = Convert.ToDouble(txtCC.Text);
                    double newVolume = APscore * TRscore * CCscore * 0.52;
                    txtVolume.Text = newVolume.ToString();
                } catch (Exception e)
                {
                    txtVolume.Text = "Error";
                    Debug.WriteLine(e.ToString());
                }
            } else
            {
                txtVolume.Text = "";
            }
        }

        private bool canMeasureVolume()
        {
            return ((txtAP.Text != "") && 
                (txtTR.Text != "") && 
                (txtCC.Text != ""));
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

    }
}
