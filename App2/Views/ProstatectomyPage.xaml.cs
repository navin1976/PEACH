using Windows.UI.Xaml.Controls;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls.Primitives;

// sources used:
//https://social.msdn.microsoft.com/Forums/en-US/0b302b80-93ab-41ac-a1d8-8ef7ddbb3e71/uwp-inkcanvas-how-to-consume-pointerpressed-pointerreleased-and-pointermoved-events?forum=wpdevelop
//https://social.msdn.microsoft.com/Forums/office/en-US/ed11a15e-b38e-4a26-88ac-5cc53d8ccec1/uwpxaml-how-to-convert-ink-renderRects-to-points-inkingmanager?forum=wpdevelop
//http://stackoverflow.com/questions/24772775/transformtovisual-returns-different-values-for-uielements-with-different-scales

namespace DataVisualization.Views
{
    public sealed partial class ProstatectomyPage : Page
    {
        public ProstatectomyPage()
        {
            //this.ViewModel = new ProstateSegmentInk();
            this.InitializeComponent();
        }




    }
}