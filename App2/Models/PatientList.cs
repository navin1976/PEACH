// Patient model object.
// includes methods to store and retrieve from JSON files.
// Patient schema (i.e. properties = { id, nhsnumber, namekey, etc }) provided by PEACH MDT team's Alex Fael

using System;
using System.Collections.ObjectModel;
using Windows.Data.Json;

namespace DataVisualization.Models
{
    class PatientList
    {
        private const string patientsKey = "patients";
        private ObservableCollection<Patient> patients;

        public PatientList()
        {
            patients = new ObservableCollection<Patient>();
        }

        public PatientList(string jsonString) : this()
        {
            // initialize json controller
            JsonObject jsonObject = JsonObject.Parse(jsonString);

            // NotesArchive
            foreach (IJsonValue jsonValue in jsonObject.GetNamedArray(patientsKey, new JsonArray()))
            {
                if (jsonValue.ValueType == JsonValueType.Object)
                {
                    PatientsArchive.Add(new Patient(jsonValue.GetObject()));
                }
            }
        }

        public string Stringify()
        {
            JsonObject jsonObject = new JsonObject();

            // Notes
            JsonArray jsonArray = new JsonArray();
            foreach (Patient patient in PatientsArchive)
            {
                jsonArray.Add(patient.ToJsonObject());
            }

            jsonObject[patientsKey] = jsonArray;

            return jsonObject.Stringify();
        }

        public ObservableCollection<Patient> PatientsArchive
        {
            get
            {
                return patients;
            }
        }

        public double Timezone { get; set; }
        public bool Verified { get; set; }
    }
}