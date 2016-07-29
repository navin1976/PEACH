using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using SimpleInk;

namespace App2
{
    public partial class MainIndex : Page
    {
        public const string FEATURE_NAME = "SimpleInk ASDASDASD";

        List<Scenario> scenarios = new List<Scenario>
        {
            //new Scenario() { Title="Scenario 1", ClassType=typeof(Scenario1)},
            new Scenario() { Title="Scenario 2", ClassType=typeof(Scenario2)},
            //new Scenario() { Title="Scenario 3", ClassType=typeof(Scenario3)}
        };
    }

    public class Scenario
    {
        public string Title { get; set; }
        public Type ClassType { get; set; }
    }
}
