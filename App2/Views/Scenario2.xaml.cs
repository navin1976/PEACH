using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Text.Core;
using Windows.Globalization;
using App2.Views;

namespace App2.Views
{

    /// This page shows the code to do ink recognition
    public sealed partial class Scenario2 : Page
    {
        const string InstallRecoText = "You can install handwriting recognition engines for other languages by this: go to Settings -> Time & language -> Region & language, choose a language, and click Options, then click Download under Handwriting";

        private MainIndex rootPage;
        InkRecognizerContainer inkRecognizerContainer = null;
        private IReadOnlyList<InkRecognizer> recoView = null;
        private Language previousInputLanguage = null;
        private CoreTextServicesManager textServiceManager = null;
        private ToolTip recoTooltip;

        public Scenario2()
        {
            this.InitializeComponent();

            // Initialize drawing attributes. These are used in inking mode.
            InkDrawingAttributes drawingAttributes = new InkDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Colors.Red;
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
                    RecoName.Items.Add(recognizer.Name);
                }
            }
            else
            {
                RecoName.IsEnabled = false;
                RecoName.Items.Add("No Recognizer Available");
            }
            RecoName.SelectedIndex = 0;

            // Set the text services so we can query when language changes
            textServiceManager = CoreTextServicesManager.GetForCurrentView();
            textServiceManager.InputLanguageChanged += TextServiceManager_InputLanguageChanged;

            SetDefaultRecognizerByCurrentInputMethodLanguageTag();

            // Initialize reco tooltip
            recoTooltip = new ToolTip();
            recoTooltip.Content = InstallRecoText;
            ToolTipService.SetToolTip(InstallReco, recoTooltip);

            // Initialize the InkCanvas
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            inkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen | Windows.UI.Core.CoreInputDeviceTypes.Touch;

            this.Unloaded += Scenario2_Unloaded;
            this.SizeChanged += Scenario2_SizeChanged;




        }

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
                    //StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                    break;
                case NotifyType.ErrorMessage:
                    //StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                    break;
            }
            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            //StatusBorder.Visibility = (StatusBlock.Text != String.Empty) ? Visibility.Visible : Visibility.Collapsed;
            if (StatusBlock.Text != String.Empty)
            {
                //StatusBorder.Visibility = Visibility.Visible;
                //StatusPanel.Visibility = Visibility.Visible;
            }
            else
            {
                //StatusBorder.Visibility = Visibility.Collapsed;
               // StatusPanel.Visibility = Visibility.Collapsed;
            }
        }


        private void Scenario2_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetCanvasSize();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            rootPage = MainIndex.Current;
            SetCanvasSize();
        }

        private void Scenario2_Unloaded(object sender, RoutedEventArgs e)
        {
            recoTooltip.IsOpen = false;
        }

        private void SetCanvasSize()
        {
            Output.Width = Window.Current.Bounds.Width;
            Output.Height = Window.Current.Bounds.Height / 2;
            inkCanvas.Width = Window.Current.Bounds.Width;
            inkCanvas.Height = Window.Current.Bounds.Height / 2;
        }

        void OnRecognizerChanged(object sender, RoutedEventArgs e)
        {
            string selectedValue = (string)RecoName.SelectedValue;
            SetRecognizerByName(selectedValue);
        }

        async void OnRecognizeAsync(object sender, RoutedEventArgs e)
        {
            IReadOnlyList<InkStroke> currentStrokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
            if (currentStrokes.Count > 0)
            {
                RecognizeBtn.IsEnabled = false;
                ClearBtn.IsEnabled = false;
                RecoName.IsEnabled = false;

                var recognitionResults = await inkRecognizerContainer.RecognizeAsync(inkCanvas.InkPresenter.StrokeContainer, InkRecognitionTarget.All);

                if (recognitionResults.Count > 0)
                {
                    // Display recognition result
                    string str = "Recognition result:";
                    foreach (var r in recognitionResults)
                    {
                        str += " " + r.GetTextCandidates()[0];
                    }
                    this.NotifyUser(str, NotifyType.StatusMessage);
                }
                else
                {
                    this.NotifyUser("No text recognized.", NotifyType.StatusMessage);
                }

                RecognizeBtn.IsEnabled = true;
                ClearBtn.IsEnabled = true;
                RecoName.IsEnabled = true;
            }
            else
            {
                this.NotifyUser("Must first write something.", NotifyType.ErrorMessage);
            }
        }

        void OnClear(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.StrokeContainer.Clear();
            this.NotifyUser("Cleared Canvas.", NotifyType.StatusMessage);
        }

        bool SetRecognizerByName(string recognizerName)
        {
            bool recognizerFound = false;

            foreach (InkRecognizer reco in recoView)
            {
                if (recognizerName == reco.Name)
                {
                    inkRecognizerContainer.SetDefaultRecognizer(reco);
                    recognizerFound = true;
                    break;
                }
            }

            if (!recognizerFound && rootPage != null)
            {
                this.NotifyUser("Could not find target recognizer.", NotifyType.ErrorMessage);
            }

            return recognizerFound;
        }

        private void TextServiceManager_InputLanguageChanged(CoreTextServicesManager sender, object args)
        {
            SetDefaultRecognizerByCurrentInputMethodLanguageTag();
        }

        private void SetDefaultRecognizerByCurrentInputMethodLanguageTag()
        {
            // Query recognizer name based on current input method language tag (bcp47 tag)
            Language currentInputLanguage = textServiceManager.InputLanguage;

            if (currentInputLanguage != previousInputLanguage)
            {
                // try query with the full BCP47 name
                string recognizerName = RecognizerHelper.LanguageTagToRecognizerName(currentInputLanguage.LanguageTag);

                if (recognizerName != string.Empty)
                {
                    for (int index = 0; index < recoView.Count; index++)
                    {
                        if (recoView[index].Name == recognizerName)
                        {
                            inkRecognizerContainer.SetDefaultRecognizer(recoView[index]);
                            RecoName.SelectedIndex = index;
                            previousInputLanguage = currentInputLanguage;
                            break;
                        }
                    }
                }
            }
        }

        private void RecoButton_Click(object sender, RoutedEventArgs e)
        {
            recoTooltip.IsOpen = !recoTooltip.IsOpen;
        }
    }
}