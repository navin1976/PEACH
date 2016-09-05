// Custom pen for inkToolBar
// Written and added to the inkToolBar as the default ball point pen has no transparency when inking

using System.Numerics;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace DataVisualization.Controls
{
    // Creating my own custom pen in order to provide for lower opacity during inking
    // in this way radiologists can view the underlying prostate gland and its specific sectors
    // this model is bound as a static resource to the ProstateSegment.xaml.cs group
    public class MyCustomPen : InkToolbarCustomPen
    {
        // override drawing attributes to get to the pen controls
        protected override InkDrawingAttributes CreateInkDrawingAttributesCore(Brush brush, double strokeWidth)
        {
            InkDrawingAttributes inkDrawingAttributes = new InkDrawingAttributes();
            inkDrawingAttributes.PenTip = PenTipShape.Circle;
            inkDrawingAttributes.Size = new Windows.Foundation.Size(strokeWidth, strokeWidth);
            SolidColorBrush solidColorBrush = brush as SolidColorBrush;
            if (solidColorBrush != null)
            {
                inkDrawingAttributes.Color = solidColorBrush.Color;
            }
            else
            {
                // default ink
                inkDrawingAttributes.Color = Colors.Red;
            }

            // ignore pressure & provide lower opacity
            inkDrawingAttributes.IgnorePressure = true;
            inkDrawingAttributes.DrawAsHighlighter = true;

            Matrix3x2 matrix = Matrix3x2.CreateRotation(45);
            inkDrawingAttributes.PenTipTransform = matrix;

            // return selection of attributes
            return inkDrawingAttributes;
        }
    }
}
