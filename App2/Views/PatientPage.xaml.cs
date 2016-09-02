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
using System.Diagnostics;
using Windows.Data.Json;
using DataVisualization.Models;
using System.Threading.Tasks;

namespace DataVisualization.Views
{
    public sealed partial class PatientPage : Page
    {
        // A pointer back to the main page.  This is needed if you want to call methods in MainPage such
        // as NotifyUser()
        MainIndex rootPage = MainIndex.Current;
        string inputJson;//= "test";

        public PatientPage()
        {
            this.InitializeComponent();
            loadJson();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            rootPage.DataContext = new Patient();



        }

        private void Parse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                rootPage.DataContext = new Patient(inputJson);
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

            try
            {
                Windows.Storage.StorageFolder storageFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile =
                    await storageFolder.GetFileAsync("C:/patient.json");

                string inputJson = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
                JsonInput.Text = inputJson;
            }





            catch (Exception ex)
            {
                JsonInput.Text = ex.ToString();
            }


            //await Task.Run(() =>
            //{
            //    Task.Yield();

            //});
        }

        private void Stringify_Click(object sender, RoutedEventArgs e)
        {
            inputJson = "";
            Patient patient = rootPage.DataContext as Patient;
            Debug.Assert(patient != null);
            inputJson = patient.Stringify();

            //rootPage.NotifyUser("JSON object serialized to string successfully.", NotifyType.StatusMessage);
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
    }
}
