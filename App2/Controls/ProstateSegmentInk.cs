using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace DataVisualization
{
    public class ProstateSegmentInk
    {
        public Path SegmentPath { get; set; }
        public TextBlock SegmentTextBlock { get; set; }

        public ProstateSegmentInk()
        {
        }

        public int Score
        {
            get
            {
                return Convert.ToInt32(this.SegmentTextBlock.Text);
            }
        }

        public static Geometry StringToPath(string pathData)
        {
            string xamlPath =
                "<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>"
                + pathData + "</Geometry>";

            return Windows.UI.Xaml.Markup.XamlReader.Load(xamlPath) as Geometry;
        }
    }

    public class ProstateSegmentInkViewModel
    {
        //private ProstateSegmentInk defaultRecording = new ProstateSegmentInk();
        //public ProstateSegmentInk DefaultRecording { get { return this.defaultRecording; } }

        private ObservableCollection<ProstateSegmentInk> recordings = new ObservableCollection<ProstateSegmentInk>();
        public ObservableCollection<ProstateSegmentInk> Recordings { get { return this.recordings; } }

        public ProstateSegmentInkViewModel()
        {
            this.recordings.Add(new ProstateSegmentInk()
            {
                SegmentPath = new Path() {
                    Name = "1",
                    Data = ProstateSegmentInk.StringToPath("123asda")
                },
                SegmentTextBlock = new TextBlock() {
                    Name = " cos"
                }
            });

            this.recordings.Add(new ProstateSegmentInk()
            {
                SegmentPath = new Path()
                {
                    Name = "1",
                    Data = ProstateSegmentInk.StringToPath("123asda")
                },
                SegmentTextBlock = new TextBlock()
                {
                    Name = " cos"
                }
            });

        }

    }

}