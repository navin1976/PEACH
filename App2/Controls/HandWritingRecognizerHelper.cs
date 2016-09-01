using System;
using System.Collections.Generic;

namespace DataVisualization
{
    class HandWritingRecognizerHelper
    {
        // create a host to hold the recognizers, which will be then loaded into a combobox
        // with use of data binding

        private static Dictionary<string, string> Bcp47ToRecognizerNameDictionary = null;

        private static void EnsureDictionary()
        {
            if (Bcp47ToRecognizerNameDictionary == null)
            {
                Bcp47ToRecognizerNameDictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
                Bcp47ToRecognizerNameDictionary.Add("en-US", "Microsoft English (US)");
                Bcp47ToRecognizerNameDictionary.Add("en-GB", "Microsoft English (UK)");
                Bcp47ToRecognizerNameDictionary.Add("en-CA", "Microsoft English (Canada)");
                Bcp47ToRecognizerNameDictionary.Add("en-AU", "Microsoft English (Australia)");
            }
        }

        public static string LanguageTagToRecognizerName(string bcp47tag)
        {
            EnsureDictionary();
            string recognizerName = string.Empty;
            try
            {
                recognizerName = Bcp47ToRecognizerNameDictionary[bcp47tag];
            }
            catch (KeyNotFoundException)
            {
                recognizerName = string.Empty;
            }

            return recognizerName;
        }
    }
}
