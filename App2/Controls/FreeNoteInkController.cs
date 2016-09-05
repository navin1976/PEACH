// source: Universal Windows Samples provided by Microsoft @ https://github.com/Microsoft/Windows-universal-samples
// modified to merge with prostate inking form 
// by Duy Tuan Dao, UCL MSc CS 2015-2016 
// contact: ucabdao@ucl.ac.uk
// extension methods and controls for FreeNotesPage

using System;
using System.Collections.Generic;
using Windows.Globalization;
using Windows.UI.Input.Inking;
using Windows.UI.Text.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DataVisualization.Views
{
    public sealed partial class FreeNotesPage : Page
    {

        // Resize Canvas in the case of using the NavPanel and rotating the tablet device
        private void NotesPenPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetCanvasSize();
        }

        private void NotesPenPage_Unloaded(object sender, RoutedEventArgs e)
        {
            recoTooltip.IsOpen = false;
        }

        // Set Canvas size on load to match that of the textbox that is underneath
        private void SetCanvasSize()
        {
            freeNoteTextBox.Width = TextBoxGrid.ActualWidth;
            freeNoteTextBox.Height = TextBoxGrid.Height;
            hwCanvas.Width= TextBoxGrid.ActualWidth;
            hwCanvas.Height = TextBoxGrid.Height;
        }

        // Set recognizer upon change of combbobox
        void OnRecognizerChanged(object sender, RoutedEventArgs e)
        {
            string selectedValue = (string)cbHandWritingRecos.SelectedValue;
            SetRecognizerByName(selectedValue);
        }

        // asynchronous recognition method for each stroke
        async void OnRecognizeAsync(object sender, RoutedEventArgs e)
        {
            IReadOnlyList<InkStroke> currentStrokes = hwCanvas.InkPresenter.StrokeContainer.GetStrokes();
            if (currentStrokes.Count > 0)
            {
                btnInkRecognizer.IsEnabled = false;
                btnClearInk.IsEnabled = false;
                cbHandWritingRecos.IsEnabled = false;

                var recognitionResults = await inkRecognizerContainer.RecognizeAsync(hwCanvas.InkPresenter.StrokeContainer, InkRecognitionTarget.All);

                if (recognitionResults.Count > 0)
                {
                    string str;
                    // Display recognition result
                    if (this.freeNoteTextBox.Text == "")
                    {
                        str = " ";
                    }
                    else
                    {
                        str = "";
                    }

                    foreach (var r in recognitionResults)
                    {
                        str += " " + r.GetTextCandidates()[0];
                    }
                    this.NotifyUser("Recognition result:" + str, NotifyType.StatusMessage);
                    this.AppendHandWritingToBox(str);
                }
                else
                {
                    this.NotifyUser("No text recognized.", NotifyType.StatusMessage);
                }

                // re-enable the buttons
                btnInkRecognizer.IsEnabled = true;
                btnClearInk.IsEnabled = true;
                cbHandWritingRecos.IsEnabled = true;
            }
            else // something went wrong: notify user
            {
                this.NotifyUser("Must first write something.", NotifyType.ErrorMessage);
            }
        }

        // clearing ink strokes from the input textbox
        void OnClearHw(object sender, RoutedEventArgs e)
        {
            hwCanvas.InkPresenter.StrokeContainer.Clear();
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

            if (!recognizerFound) // && rootPage != null)
            {
                this.NotifyUser("Could not find target recognizer.", NotifyType.ErrorMessage);
            }

            return recognizerFound;
        }

        // event handler for input language change
        private void TextServiceManager_InputLanguageChanged(CoreTextServicesManager sender, object args)
        {
            SetDefaultRecognizerByCurrentInputMethodLanguageTag();
        }

        // method to set default recognizer
        private void SetDefaultRecognizerByCurrentInputMethodLanguageTag()
        {
            // Query recognizer name based on current input method language tag (bcp47 tag)
            Language currentInputLanguage = textServiceManager.InputLanguage;

            if (currentInputLanguage != previousInputLanguage)
            {
                // try query with the full BCP47 name
                string recognizerName = HandWritingRecognizerHelper.LanguageTagToRecognizerName(currentInputLanguage.LanguageTag);

                if (recognizerName != string.Empty)
                {
                    for (int index = 0; index < recoView.Count; index++)
                    {
                        if (recoView[index].Name == recognizerName)
                        {
                            inkRecognizerContainer.SetDefaultRecognizer(recoView[index]);
                            cbHandWritingRecos.SelectedIndex = index;
                            previousInputLanguage = currentInputLanguage;
                            break;
                        }
                    }
                }
            }
        }

        // event handler for recognition butter click
        private void RecoButton_Click(object sender, RoutedEventArgs e)
        {
            recoTooltip.IsOpen = !recoTooltip.IsOpen;
        }

        // append string to the textbox in the form page
        private void AppendHandWritingToBox(string str)
        {
            this.freeNoteTextBox.Text += str;
        }
    }
}

