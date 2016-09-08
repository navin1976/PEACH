/*
 * This is a converter used in databinding, in order to translate JSON value kept in MainIndex.DataContext
 * Sources used to learn:
 *    http://stackoverflow.com/questions/34713126/uwp-stringformat-on-binding
 *    http://stackoverflow.com/questions/36313998/how-to-parse-and-generate-datetime-objects-in-iso-8601-format
 *    http://stackoverflow.com/questions/21256132/deserializing-dates-with-dd-mm-yyyy-format-using-json-net
 *    https://blog.srife.net/2016/01/04/uwp-how-to-bind-the-calendardatepicker/
 */

using System;
using Windows.UI.Xaml.Data;
using System.Globalization;


namespace DataVisualization.Converters
{
    public class JsonDateConverterString : IValueConverter
    {
        // used when converting from DataContext -> View
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // checking if string is null due to the fact that loading may be done before UI is rendered
            if ((value as string) == "")
            {
                return "";
            }

            string dt = value.ToString();

            DateTime d2 = DateTime.Parse(dt, null, System.Globalization.DateTimeStyles.RoundtripKind);

            // Different implementation using NewtonSoft package:
            // Newtonsoft.Json.Converters.IsoDateTimeConverter conv = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            // DateTime obj = JsonConvert.DeserializeObject<DateTime>(dt, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            // date = obj.Date;

            DateTimeOffset resultTime = d2;

            return resultTime.ToString("dd/MM/yyyy");
            
            
        }

        // converting back from ViewModel -> DataContext
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            DateTimeOffset dto = (DateTimeOffset) value;

            string format = dto.Offset == TimeSpan.Zero
               ? "yyyy-MM-ddTHH:mm:ss.fffZ"
               : "yyyy-MM-ddTHH:mm:ss.fffzzz";

            return dto.ToString(format, CultureInfo.InvariantCulture);

        }

    }
}
