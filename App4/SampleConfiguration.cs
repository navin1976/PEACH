
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Speech;

namespace App4
{
    public partial class MainPage : Page
    {
        public const string FEATURE_NAME = "Speech Recognition\nand TTS";

        List<Scenario> scenarios = new List<Scenario>
        {
            // new Scenario() { Title="Predefined Dictation Grammar", ClassType=typeof(PredefinedDictationGrammarScenario)},
            new Scenario() { Title="Continuous Dictation", ClassType=typeof(ContinuousDictationScenario)},
        };
    }

    public class Scenario
    {
        public string Title { get; set; }
        public Type ClassType { get; set; }
    }
}
