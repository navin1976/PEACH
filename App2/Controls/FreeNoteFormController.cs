using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;
using Windows.UI.Text.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// further broken down to:
//      1. FreeNoteInkController
//      2. FreeNoteVoiceController

namespace DataVisualization.Views
{
    public sealed partial class FreeNotesPage : Page
    {


        private void InitializeForm()
        {
            isListening = false; // controller for active listening
            dictatedTextBuilder = new StringBuilder(); // string builder is used to append voice strings

            // initialize inking attributes
            InkDrawingAttributes drawingAttributes = new InkDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Colors.Blue;
            double penSize = 2;
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
            hwCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            hwCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Pen;

            this.Unloaded += NotesPenPage_Unloaded;
            this.SizeChanged += NotesPenPage_SizeChanged;
        }



        // Keyboard / Surface Pen toggle switch. It hides the hwCanvas so that direct
        // Typing to the textbox is possible
        private void Toggle_inputSwitch(object sender, RoutedEventArgs e)
        {
            if (inkToggle.IsChecked == true)
            {
                inkToggle.Content = new SymbolIcon(Symbol.Keyboard);
                this.hwCanvas.Visibility = Visibility.Collapsed;
                freeNoteTextBox.IsReadOnly = false;
                ToggleButtonIndicator.Text = "Using Touch Keyboard";
            }
            else
            {
                //inputSymbol = (Symbol)0xE104;
                inkToggle.Content = new SymbolIcon((Symbol)0xE104);
                this.hwCanvas.Visibility = Visibility.Visible;
                freeNoteTextBox.IsReadOnly = true;
                ToggleButtonIndicator.Text = "Using Surface Pen";
            }
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
                }
                catch (Exception e)
                {
                    txtVolume.Text = "Error";
                    Debug.WriteLine(e.ToString());
                }
            }
            else
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
    }
}
