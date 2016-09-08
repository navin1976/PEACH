// Patient model object.
// includes methods to store and retrieve from JSON files.
// Patient schema (i.e. properties = { id, nhsnumber, namekey, etc }) provided by PEACH MDT team's Alex Fael

using System;
using System.Collections.ObjectModel;
using Windows.Data.Json;

namespace DataVisualization.Models
{
    class UserProfile
    {

        private const string idKey = "id";
        private const string nhsNumberKey = "nhsNumber";
        private const string nameKey = "name";
        private const string emailKey = "email";


        private string id;
        private string nhsNumber;
        private string name;
        private string email;



        public UserProfile()
        {
            Id = "";
            NhsNumber = "";
            Name = "";
            Email = "";
         }

        public UserProfile(string jsonString) : this()
        {
            // initialize json controller
            JsonObject jsonObject = JsonObject.Parse(jsonString);

            Id = jsonObject.GetNamedString(idKey, "");
            NhsNumber = jsonObject.GetNamedString(nhsNumberKey, "");
            Name = jsonObject.GetNamedString(nameKey, "");
            Email = jsonObject.GetNamedString(emailKey, "");
        }

        public UserProfile(JsonObject jsonObject)
        {
            Id = jsonObject.GetNamedString(idKey, "");
            NhsNumber = jsonObject.GetNamedString(nhsNumberKey, "");
            Name = jsonObject.GetNamedString(nameKey, "");
            Email = jsonObject.GetNamedString(emailKey, "");
        }

        public string Stringify()
        {
            JsonObject jsonObject = new JsonObject();

            jsonObject[idKey] = JsonValue.CreateStringValue(Id);
            jsonObject[nhsNumberKey] = JsonValue.CreateStringValue(NhsNumber);
            jsonObject[nameKey] = JsonValue.CreateStringValue(Name);
            jsonObject[emailKey] = JsonValue.CreateStringValue(Email);
            return jsonObject.Stringify();
        }

        public JsonObject ToJsonObject()
        {
            JsonObject jsonObject = new JsonObject();

            jsonObject[idKey] = JsonValue.CreateStringValue(Id);
            jsonObject[nhsNumberKey] = JsonValue.CreateStringValue(NhsNumber);
            jsonObject[nameKey] = JsonValue.CreateStringValue(Name);
            jsonObject[emailKey] = JsonValue.CreateStringValue(Email);
            
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
    }
}