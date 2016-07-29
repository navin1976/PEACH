using System;
using System.Collections.Generic;

namespace App2
{
    class RecognizerHelper
    {
        private static Dictionary<string, string> Bcp47ToRecognizerNameDictionary = null;

        private static void EnsureDictionary()
        {
            if (Bcp47ToRecognizerNameDictionary == null)
            {
                Bcp47ToRecognizerNameDictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
                Bcp47ToRecognizerNameDictionary.Add("en-US", "Microsoft English (US) Handwriting Recognizer");
                Bcp47ToRecognizerNameDictionary.Add("en-GB", "Microsoft English (UK) Handwriting Recognizer");
                Bcp47ToRecognizerNameDictionary.Add("en-CA", "Microsoft English (Canada) Handwriting Recognizer");
                Bcp47ToRecognizerNameDictionary.Add("en-AU", "Microsoft English (Australia) Handwriting Recognizer");
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
