using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.Data.Json;
using DataVisualization.Models;
using Windows.UI.Popups;

namespace DataVisualization.Views
{
    public sealed partial class PatientPage : Page
    {
        // A pointer back to the main page.  This is needed if you want to call methods in MainPage such
        // as NotifyUser()
        MainIndex rootPage = MainIndex.Current;
        string inputJson;//= "test";
        PatientList patientList;
        Symbol SaveFile = (Symbol)0xE74E;
        Symbol OpenFile = (Symbol)0xE896;

        public PatientPage()
        {
            this.InitializeComponent();
            loadJson();

            Patient patient = rootPage.DataContext as Patient;
            if (patient != null)
            {
                this.Stringify.IsEnabled = true;
            } else
            {
                this.Stringify.IsEnabled = false;
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }



        private void Parse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedPatient = (Patient)cbPatientList.SelectedItem;
                rootPage.DataContext = new Patient();
                rootPage.DataContext = selectedPatient;
                Stringify.IsEnabled = true;
                rootPage.DisplayPatientInformationPanels();
                //rootPage.NotifyUser("JSON string parsed successfully.", NotifyType.StatusMessage);
            }
            catch (Exception ex)
            {
                if (!IsExceptionHandled(ex))
                {
                    throw ex;
                }
            }
        }

        private async void loadJson()
        {
            // Create sample file; replace if exists.
            var localizationDirectory = Windows.ApplicationModel.Package.Current.InstalledLocation;
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                Windows.Storage.StorageFile sampleFile = await localizationDirectory.GetFileAsync("patient.json");
                inputJson = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
                //JsonInput.Text = inputJson;
            }
            catch (Exception ex)
            {
                //JsonInput.Text = storageFolder.Path ;
            }

            try
            {
                patientList = new PatientList(inputJson);
                cbPatientList.ItemsSource = patientList.PatientsArchive;
                //rootPage.NotifyUser("JSON string parsed successfully.", NotifyType.StatusMessage);
            }
            catch (Exception ex)
            {
                if (!IsExceptionHandled(ex))
                {
                    throw ex;
                }
            }

        }



        private async void Stringify_Click(object sender, RoutedEventArgs e)
        {
            var yesCommand = new UICommand("Yes", cmd => { });
            var okCommand = new UICommand("OK", cmd => { });
            var cancelCommand = new UICommand("Cancel", cmd => { });
            string title;
            string content;

            /*
             * Can add JSON using content dialog box as well: 
             */
            //var panel = new StackPanel();
            //panel.Orientation = Orientation.Vertical;
            //panel.Children.Add(new TextBlock
            //{
            //    Text = "This middleware layer creates JSON patient object to be pushed to SQL.\r\nDo you want to proceed?\r\n\r\n " +
            //            inputJson,
            //    TextWrapping = TextWrapping.WrapWholeWords
            //});

            try
            {
                inputJson = "";
                Patient patient = rootPage.DataContext as Patient;
                Debug.Assert(patient != null);
                inputJson = patient.Stringify();
                title = "Save patient data";
                content = "This middleware layer creates JSON patient object to be pushed to SQL.\r\nDo you want to proceed?\r\n\r\n";
                content += inputJson;
                var saveDialog = new MessageDialog(content, title);
                
                saveDialog.Options = MessageDialogOptions.None;
                saveDialog.Commands.Add(yesCommand);

                saveDialog.DefaultCommandIndex = 0;
                saveDialog.CancelCommandIndex = 0;
                if (cancelCommand != null)
                {
                    saveDialog.Commands.Add(cancelCommand);
                    saveDialog.CancelCommandIndex = (uint)saveDialog.Commands.Count - 1;
                }

                var command = await saveDialog.ShowAsync();

                if (command == yesCommand)
                {
                    // push toSQL
                }
                else
                {
                    // handle cancel command
                }

            }
            catch (Exception ex)
            {
                // handle if cannot push
            }

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = rootPage.DataContext as Patient;
            Debug.Assert(patient != null);
            patient.NotesArchive.Add(new MedicalNote());
            //rootPage.NotifyUser("New row added.", NotifyType.StatusMessage);
        }

        private bool IsExceptionHandled(Exception ex)
        {
            JsonErrorStatus error = JsonError.GetJsonStatus(ex.HResult);
            if (error == JsonErrorStatus.Unknown)
            {
                return false;
            }

            //rootPage.NotifyUser(error + ": " + ex.Message, NotifyType.ErrorMessage);
            return true;
        }

        private void cbPatientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Parse.IsEnabled = true;
        }
    }
}
