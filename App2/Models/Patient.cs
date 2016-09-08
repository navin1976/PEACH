// Patient model object.
// includes methods to store and retrieve from JSON files.
// Patient schema (i.e. properties = { id, nhsnumber, namekey, etc }) provided by PEACH MDT team's Alex Fael

using System;
using System.Collections.ObjectModel;
using Windows.Data.Json;

namespace DataVisualization.Models
{
    class Patient {

        private const string idKey = "id";
        private const string nhsNumberKey = "nhsNumber";
        private const string nameKey = "name";
        private const string emailKey = "email";
        private const string birthdateKey = "birthdate";
        private const string addressKey = "address";
        private const string postcodeKey = "postcode";
        private const string homePhoneKey = "homePhone";
        private const string workPhoneKey = "workPhone";
        private const string prefContactMethodKey = "prefContactMethod";
        private const string genderKey = "gender";
        private const string ethnicityKey = "ethnicity";
        private const string maritalStatusKey = "maritalStatus";
        private const string gpNameKey = "gpName";
        private const string gpEmailKey = "gpEmail";
        private const string gpAddressKey = "gpAddress";
        private const string gpPostcodeKey = "gpPostcode";
        private const string gpPhoneKey = "gpPhone";
        private const string nextOfKinNameKey = "nextOfKinName";
        private const string nextOfKinEmailKey = "nextOfKinEmail";
        private const string nextOfKinAddressKey = "nextOfKinAddress";
        private const string nextOfKinPostcodeKey = "nextOfKinPostcode";
        private const string nextOfKinPhoneKey = "nextOfKinPhone";

        private string id;
        private string nhsNumber;
        private string name;
        private string email;
        private string birthdate;
        private string address;
        private string postcode;
        private string homePhone;
        private string workPhone;
        private string prefContactMethod;
        private string gender;
        private string ethnicity;
        private string maritalStatus;
        private string gpName;
        private string gpEmail;
        private string gpAddress;
        private string gpPostcode;
        private string gpPhone;
        private string nextOfKinName;
        private string nextOfKinEmail;
        private string nextOfKinAddress;
        private string nextOfKinPostcode;
        private string nextOfKinPhone;

        private const string notesKey = "notes";
        private ObservableCollection<MedicalNote> notes;

        public Patient()
        {
            Id = "";
            NhsNumber = "";
            Name = "";
            Email = "";
            Birthdate = "";
            Address = "";
            Postcode = "";
            HomePhone = null;
            WorkPhone = null;
            PrefContactMethod = "";
            Gender = "";
            Ethnicity = "";
            MaritalStatus = "";
            GpName = "";
            GpEmail = "";
            GpAddress = "";
            GpPostcode = "";
            GpPhone = "";
            NextOfKinName = "";
            NextOfKinEmail = "";
            NextOfKinAddress = "";
            NextOfKinPostcode = "";
            NextOfKinPhone = "";
            notes = new ObservableCollection<MedicalNote>();
        }

        public Patient(string jsonString) : this()
        {
            // initialize json controller
            JsonObject jsonObject = JsonObject.Parse(jsonString);

            Id = jsonObject.GetNamedString(idKey, "");
            NhsNumber = jsonObject.GetNamedString(nhsNumberKey, "");
            Name = jsonObject.GetNamedString(nameKey, "");
            Email = jsonObject.GetNamedString(emailKey, "");
            Birthdate = jsonObject.GetNamedString(birthdateKey, "");
            Address = jsonObject.GetNamedString(addressKey, "");
            Postcode = jsonObject.GetNamedString(postcodeKey, "");
            PrefContactMethod = jsonObject.GetNamedString(prefContactMethodKey, "");
            Gender = jsonObject.GetNamedString(genderKey, "");
            Ethnicity = jsonObject.GetNamedString(ethnicityKey, "");
            MaritalStatus = jsonObject.GetNamedString(maritalStatusKey, "");
            GpName = jsonObject.GetNamedString(gpNameKey, "");
            GpEmail = jsonObject.GetNamedString(gpEmailKey, "");
            GpAddress = jsonObject.GetNamedString(gpAddressKey, "");
            GpPostcode = jsonObject.GetNamedString(gpPostcodeKey, "");
            GpPhone = jsonObject.GetNamedString(gpPhoneKey, "");
            NextOfKinName = jsonObject.GetNamedString(nextOfKinNameKey, "");
            NextOfKinEmail = jsonObject.GetNamedString(nextOfKinEmailKey, "");
            NextOfKinAddress = jsonObject.GetNamedString(nextOfKinAddressKey, "");
            NextOfKinPostcode = jsonObject.GetNamedString(nextOfKinPostcodeKey, "");
            NextOfKinPhone = jsonObject.GetNamedString(nextOfKinPhoneKey, "");

            // HomePhone  
            IJsonValue homePhoneJsonValue = jsonObject.GetNamedValue(homePhoneKey);
            if (homePhoneJsonValue.ValueType == JsonValueType.Null)
            {
                HomePhone = null;
            }
            else
            {
                HomePhone = homePhoneJsonValue.GetString();
            }

            // WorkPhone  
            IJsonValue workPhoneJsonValue = jsonObject.GetNamedValue(workPhoneKey);
            if (workPhoneJsonValue.ValueType == JsonValueType.Null)
            {
                WorkPhone = null;
            }
            else
            {
                WorkPhone = workPhoneJsonValue.GetString();
            }

            // NotesArchive
            foreach (IJsonValue jsonValue in jsonObject.GetNamedArray(notesKey, new JsonArray()))
            {
                if (jsonValue.ValueType == JsonValueType.Object)
                {
                    NotesArchive.Add(new MedicalNote(jsonValue.GetObject()));
                }
            }
        }

        public Patient(JsonObject jsonObject)
        {
            Id = jsonObject.GetNamedString(idKey, "");
            NhsNumber = jsonObject.GetNamedString(nhsNumberKey, "");
            Name = jsonObject.GetNamedString(nameKey, "");
            Email = jsonObject.GetNamedString(emailKey, "");
            Birthdate = jsonObject.GetNamedString(birthdateKey, "");
            Address = jsonObject.GetNamedString(addressKey, "");
            Postcode = jsonObject.GetNamedString(postcodeKey, "");
            PrefContactMethod = jsonObject.GetNamedString(prefContactMethodKey, "");
            Gender = jsonObject.GetNamedString(genderKey, "");
            Ethnicity = jsonObject.GetNamedString(ethnicityKey, "");
            MaritalStatus = jsonObject.GetNamedString(maritalStatusKey, "");
            GpName = jsonObject.GetNamedString(gpNameKey, "");
            GpEmail = jsonObject.GetNamedString(gpEmailKey, "");
            GpAddress = jsonObject.GetNamedString(gpAddressKey, "");
            GpPostcode = jsonObject.GetNamedString(gpPostcodeKey, "");
            GpPhone = jsonObject.GetNamedString(gpPhoneKey, "");
            NextOfKinName = jsonObject.GetNamedString(nextOfKinNameKey, "");
            NextOfKinEmail = jsonObject.GetNamedString(nextOfKinEmailKey, "");
            NextOfKinAddress = jsonObject.GetNamedString(nextOfKinAddressKey, "");
            NextOfKinPostcode = jsonObject.GetNamedString(nextOfKinPostcodeKey, "");
            NextOfKinPhone = jsonObject.GetNamedString(nextOfKinPhoneKey, "");

            // HomePhone  
            IJsonValue homePhoneJsonValue = jsonObject.GetNamedValue(homePhoneKey);
            if (homePhoneJsonValue.ValueType == JsonValueType.Null)
            {
                HomePhone = null;
            }
            else
            {
                HomePhone = homePhoneJsonValue.GetString();
            }

            // WorkPhone  
            IJsonValue workPhoneJsonValue = jsonObject.GetNamedValue(workPhoneKey);
            if (workPhoneJsonValue.ValueType == JsonValueType.Null)
            {
                WorkPhone = null;
            }
            else
            {
                WorkPhone = workPhoneJsonValue.GetString();
            }

            // NotesArchive
            foreach (IJsonValue jsonValue in jsonObject.GetNamedArray(notesKey, new JsonArray()))
            {
                if (jsonValue.ValueType == JsonValueType.Object)
                {
                    NotesArchive.Add(new MedicalNote(jsonValue.GetObject()));
                }
            }

        }

        public string Stringify()
        {
            JsonObject jsonObject = new JsonObject();

            jsonObject[idKey] = JsonValue.CreateStringValue(Id);
            jsonObject[nhsNumberKey] = JsonValue.CreateStringValue(NhsNumber);
            jsonObject[nameKey] = JsonValue.CreateStringValue(Name);
            jsonObject[emailKey] = JsonValue.CreateStringValue(Email);
            jsonObject[birthdateKey] = JsonValue.CreateStringValue(Birthdate);
            jsonObject[addressKey] = JsonValue.CreateStringValue(Address);
            jsonObject[postcodeKey] = JsonValue.CreateStringValue(Postcode);
            jsonObject[prefContactMethodKey] = JsonValue.CreateStringValue(PrefContactMethod);
            jsonObject[genderKey] = JsonValue.CreateStringValue(Gender);
            jsonObject[ethnicityKey] = JsonValue.CreateStringValue(Ethnicity);
            jsonObject[maritalStatusKey] = JsonValue.CreateStringValue(MaritalStatus);
            jsonObject[gpNameKey] = JsonValue.CreateStringValue(GpName);
            jsonObject[gpEmailKey] = JsonValue.CreateStringValue(GpEmail);
            jsonObject[gpAddressKey] = JsonValue.CreateStringValue(GpAddress);
            jsonObject[gpPostcodeKey] = JsonValue.CreateStringValue(GpPostcode);
            jsonObject[gpPhoneKey] = JsonValue.CreateStringValue(GpPhone);
            jsonObject[nextOfKinNameKey] = JsonValue.CreateStringValue(NextOfKinName);
            jsonObject[nextOfKinEmailKey] = JsonValue.CreateStringValue(NextOfKinEmail);
            jsonObject[nextOfKinAddressKey] = JsonValue.CreateStringValue(NextOfKinAddress);
            jsonObject[nextOfKinPostcodeKey] = JsonValue.CreateStringValue(NextOfKinPostcode);
            jsonObject[nextOfKinPhoneKey] = JsonValue.CreateStringValue(NextOfKinPhone);

            // Notes
            JsonArray jsonArray = new JsonArray();
            if (NotesArchive != null) {
                foreach (MedicalNote medicalNote in NotesArchive)
                {
                    jsonArray.Add(medicalNote.ToJsonObject());
                }
            }
            // Treating a blank string as null for HomePhone
            if (String.IsNullOrEmpty(HomePhone))
            {
                jsonObject[homePhoneKey] = JsonValue.CreateNullValue();
            }
            else
            {
                jsonObject[homePhoneKey] = JsonValue.CreateStringValue(HomePhone);
            }

            // Treating a blank string as null for HomePhone
            if (String.IsNullOrEmpty(WorkPhone))
            {
                jsonObject[workPhoneKey] = JsonValue.CreateNullValue();
            }
            else
            {
                jsonObject[workPhoneKey] = JsonValue.CreateStringValue(WorkPhone);
            }
            jsonObject[nameKey] = JsonValue.CreateStringValue(Name);
            jsonObject[notesKey] = jsonArray;


            return jsonObject.Stringify();
        }

        public JsonObject ToJsonObject()
        {
            JsonObject jsonObject = new JsonObject();

            jsonObject[idKey] = JsonValue.CreateStringValue(Id);
            jsonObject[nhsNumberKey] = JsonValue.CreateStringValue(NhsNumber);
            jsonObject[nameKey] = JsonValue.CreateStringValue(Name);
            jsonObject[emailKey] = JsonValue.CreateStringValue(Email);
            jsonObject[birthdateKey] = JsonValue.CreateStringValue(Birthdate);
            jsonObject[addressKey] = JsonValue.CreateStringValue(Address);
            jsonObject[postcodeKey] = JsonValue.CreateStringValue(Postcode);
            jsonObject[prefContactMethodKey] = JsonValue.CreateStringValue(PrefContactMethod);
            jsonObject[genderKey] = JsonValue.CreateStringValue(Gender);
            jsonObject[ethnicityKey] = JsonValue.CreateStringValue(Ethnicity);
            jsonObject[maritalStatusKey] = JsonValue.CreateStringValue(MaritalStatus);
            jsonObject[gpNameKey] = JsonValue.CreateStringValue(GpName);
            jsonObject[gpEmailKey] = JsonValue.CreateStringValue(GpEmail);
            jsonObject[gpAddressKey] = JsonValue.CreateStringValue(GpAddress);
            jsonObject[gpPostcodeKey] = JsonValue.CreateStringValue(GpPostcode);
            jsonObject[gpPhoneKey] = JsonValue.CreateStringValue(GpPhone);
            jsonObject[nextOfKinNameKey] = JsonValue.CreateStringValue(NextOfKinName);
            jsonObject[nextOfKinEmailKey] = JsonValue.CreateStringValue(NextOfKinEmail);
            jsonObject[nextOfKinAddressKey] = JsonValue.CreateStringValue(NextOfKinAddress);
            jsonObject[nextOfKinPostcodeKey] = JsonValue.CreateStringValue(NextOfKinPostcode);
            jsonObject[nextOfKinPhoneKey] = JsonValue.CreateStringValue(NextOfKinPhone);

            // Notes
            JsonArray jsonArray = new JsonArray();
            foreach (MedicalNote medicalNote in NotesArchive)
            {
                jsonArray.Add(medicalNote.ToJsonObject());
            }

            // Treating a blank string as null for HomePhone
            if (String.IsNullOrEmpty(HomePhone))
            {
                jsonObject[homePhoneKey] = JsonValue.CreateNullValue();
            }
            else
            {
                jsonObject[homePhoneKey] = JsonValue.CreateStringValue(HomePhone);
            }

            // Treating a blank string as null for HomePhone
            if (String.IsNullOrEmpty(WorkPhone))
            {
                jsonObject[workPhoneKey] = JsonValue.CreateNullValue();
            }
            else
            {
                jsonObject[workPhoneKey] = JsonValue.CreateStringValue(WorkPhone);
            }
            jsonObject[nameKey] = JsonValue.CreateStringValue(Name);
            jsonObject[notesKey] = jsonArray;

            return jsonObject;
        }

        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                id = value;
            }
        }

        public string HomePhone
        {
            get
            {
                return homePhone;
            }
            set
            {
                homePhone = value;
            }
        }

        public string WorkPhone
        {
            get
            {
                return workPhone;
            }
            set
            {
                workPhone = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                name = value;
            }
        }

        public string NhsNumber
        {
            get
            {
                return nhsNumber;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                nhsNumber = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                email = value;
            }
        }

        public string Birthdate
        {
            get
            {
                return birthdate;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                birthdate = value;
            }
        }

        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                address = value;
            }
        }

        public string Postcode
        {
            get
            {
                return postcode;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                postcode = value;
            }
        }

        public string PrefContactMethod
        {
            get
            {
                return prefContactMethod;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                prefContactMethod = value;
            }
        }

        public string Gender
        {
            get
            {
                return gender;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                gender = value;
            }
        }

        public string Ethnicity
        {
            get
            {
                return ethnicity;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                ethnicity = value;
            }
        }

        public string MaritalStatus
        {
            get
            {
                return maritalStatus;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                maritalStatus = value;
            }
        }

        public string GpName
        {
            get
            {
                return gpName;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                gpName = value;
            }
        }

        public string GpEmail
        {
            get
            {
                return gpEmail;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                gpEmail = value;
            }
        }

        public string GpAddress
        {
            get
            {
                return gpAddress;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                gpAddress = value;
            }
        }

        public string GpPostcode
        {
            get
            {
                return gpPostcode;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                gpPostcode = value;
            }
        }

        public string GpPhone
        {
            get
            {
                return gpPhone;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                gpPhone = value;
            }
        }

        public string NextOfKinName
        {
            get
            {
                return nextOfKinName;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                nextOfKinName = value;
            }
        }

        public string NextOfKinEmail
        {
            get
            {
                return nextOfKinEmail;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                nextOfKinEmail = value;
            }
        }

        public string NextOfKinAddress
        {
            get
            {
                return nextOfKinAddress;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                nextOfKinAddress = value;
            }
        }

        public string NextOfKinPostcode
        {
            get
            {
                return nextOfKinPostcode;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                nextOfKinPostcode = value;
            }
        }

        public string NextOfKinPhone
        {
            get
            {
                return nextOfKinPhone;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                nextOfKinPhone = value;
            }
        }

        public ObservableCollection<MedicalNote> NotesArchive
        {
            get
            {
                return notes;
            }
        }
    }
}