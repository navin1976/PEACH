// Prostate Segment Ink model
// Unused in implementation due to complexity of changes needed
// Deliverable for PEACH as a recommendation for further development and improvements

/* INFORMATION
 * 
 * Development reason and use:
 *      Ideal architecture for this appplication software assumes the ability for the app to host and visualize
 *      several and various human organs MRI scans. Each organ/MRI scan would be broken down to respective
 *      PATH objects that are rendered on a canvas, and this class serves as a model for the development
 *      showing how a pair of a Path and TextBlock could be used in MVVM reference for the most optimal
 *      architecture and databinding.
 *      
 * Benefits of using this model:
 *      ability to create collections of ProstateSegment inks that gives reference taking Path -> return TextBox
 *      shortens the ViewModel code by eliminating the lookup table methods
 */

using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace DataVisualization
{
    public class ProstateSegmentInk
    {
        public Path SegmentPath { get; set; }
        public TextBlock SegmentTextBlock { get; set; }
        public string Name { get; set; }
        public int locationCanvasX;
        public int locationCanvasY;

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

    //public class ProstateSegmentInkViewModel
    //{
    //    //private ProstateSegmentInk defaultRecording = new ProstateSegmentInk();
    //    //public ProstateSegmentInk DefaultRecording { get { return this.defaultRecording; } }

    //    public ProstateSegmentInkViewModel()
    //    {
    //        this.Add(new ProstateSegmentInk()
    //        {
    //            SegmentPath = new Path() {
    //                Name = "",
    //                Data = ProstateSegmentInk.StringToPath("")
    //            },
    //            SegmentTextBlock = new TextBlock() {
    //                Name = ""
    //            }
    //        });

    //        this.recordings.Add(new ProstateSegmentInk()
    //        {
    //            SegmentPath = new Path()
    //            {
    //                Name = "",
    //                Data = ProstateSegmentInk.StringToPath("")
    //            },
    //            SegmentTextBlock = new TextBlock()
    //            {
    //                Name = ""
    //            }
    //        });
    //    }

    //}

}