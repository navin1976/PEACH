using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace DataVisualization.Models
{
    class MedicalNote
    {
        private const string idKey = "id";
        private const string schoolKey = "school";
        private const string noteKey = "note";
        private const string dateKey = "date";

        private string id;
        private string note;
        private string date;

        public MedicalNote()
        {
            Id = "";
            Note = "";
            Date = "";
        }

        public MedicalNote(JsonObject jsonObject)
        {
            JsonObject schoolObject = jsonObject.GetNamedObject(schoolKey, null);
            if (schoolObject != null)
            {
                Id = schoolObject.GetNamedString(idKey, "");
                Note = schoolObject.GetNamedString(noteKey, "");
            }
            Date = jsonObject.GetNamedString(dateKey);
        }

        public JsonObject ToJsonObject()
        {
            JsonObject schoolObject = new JsonObject();
            schoolObject.SetNamedValue(idKey, JsonValue.CreateStringValue(Id));
            schoolObject.SetNamedValue(noteKey, JsonValue.CreateStringValue(Note));

            JsonObject jsonObject = new JsonObject();
            jsonObject.SetNamedValue(schoolKey, schoolObject);
            jsonObject.SetNamedValue(dateKey, JsonValue.CreateStringValue(Date));

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

        public string Note
        {
            get
            {
                return note;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                note = value;
            }
        }

        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                date = value;
            }
        }
    }
}
