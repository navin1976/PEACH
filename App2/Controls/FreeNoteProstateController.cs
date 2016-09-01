using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Input.Inking;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Diagnostics;
using Windows.UI.Core;
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;
using System.Linq;
using Windows.UI.Xaml.Controls.Primitives;

namespace DataVisualization.Views
{
    public sealed partial class FreeNotesPage : Page
    {

        //public Skeleton.ProstateSegmentInk ViewModel { get; set; }



        private void InkPresenter_StrokesErased(InkPresenter sender, InkStrokesErasedEventArgs args)
        {
            this.ReScoreAllStrokes();

        }

        private void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            foreach (var hitTestStroke in args.Strokes)
            {
                this.processSingleStroke(hitTestStroke);
            }
        }


        private void processSingleStroke(InkStroke hitTestStroke)
        {
            //this.txtCount.Text = string.Empty;
            Point TransFormedPoint;
            List<Path> allElementStack = new List<Path>();
            foreach (var myPoint in hitTestStroke.GetInkPoints().Reverse())
            {
                TransFormedPoint = GetPosition(myPoint.Position, inkCanvas);
                IEnumerable<UIElement> elementStack =
                    VisualTreeHelper.FindElementsInHostCoordinates(TransFormedPoint, canvasDraw, true);

                foreach (UIElement element in elementStack)
                {
                    Path feItem = new Path();
                    feItem = element as Path;
                    if (feItem != null)
                    {
                        allElementStack.Add(feItem);
                    }
                }
            }
            triggerScore(allElementStack.Distinct() as IEnumerable<Path>, hitTestStroke.DrawingAttributes.Color);
        }


        private void triggerScore(IEnumerable<Path> IEnumerableElementStack, Windows.UI.Color Color)
        {
            // since dark gray carries no color, run scoring only if color is red, amber or green.
            if (Color.ToString() != "#FFA9A9A9")
            {


                // first, determine score
                int score = 0;
                switch (Color.ToString())
                {
                    case "#FFFF0000": // red
                        score = 5;
                        break;
                    case "#FFFFA500": //amber
                        score = 4;
                        break;
                    case "#FF008000": //green
                        score = 3;
                        break;
                }

                foreach (Path element in IEnumerableElementStack)
                {
                    //Debug.WriteLine("UIElement: " + element.Name);
                    //Debug.WriteLine("Color: " + Color.ToString());
                    //txtCount.Text = $"UIElement: " + element.Name;//$"Hit {this.hitElements.Count} shapes";
                    TextBlock destTextBlock = lookUpFeedbackBox(element.Name);
                    int txtScore = Convert.ToInt32(destTextBlock.Text);

                    if (txtScore < score)
                    {
                        destTextBlock.Text = score.ToString();
                    }
                }

            }
        }



        private void UnprocessedInput_PointerPressed(InkUnprocessedInput sender, Windows.UI.Core.PointerEventArgs args)
        {
            //basePathOne.Fill = new SolidColorBrush(Windows.UI.Colors.Blue);
        }

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    //rootPage = MainPage.Current;
        //    inkCanvas.Width = Window.Current.Bounds.Width;
        //    inkCanvas.Height = Window.Current.Bounds.Height;
        //}

        private void CurrentToolChanged(InkToolbar sender, object args)
        {
            //bool enabled = sender.ActiveTool.Equals(toolButtonLasso);
        }

        private void Toggle_Custom(object sender, RoutedEventArgs e)
        {
            if (toggleButton.IsChecked == true)
            {
                inkCanvas.InkPresenter.InputDeviceTypes |= CoreInputDeviceTypes.Touch;
            }
            else
            {
                inkCanvas.InkPresenter.InputDeviceTypes &= ~CoreInputDeviceTypes.Touch;
            }
        }

        private void inkToolBarLoad_FocusEngaged(Control sender, FocusEngagedEventArgs args)
        {

        }

        private void EraseAllInk(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.StrokeContainer.Clear();
            this.resetTextBlocks();
        }

        private Point GetPosition(Point ptrPt, UIElement p)
        {
            GeneralTransform gt = p.TransformToVisual(null);
            Point screenPoint;
            screenPoint = gt.TransformPoint(ptrPt);
            return screenPoint;
        }


        private bool DoesPointContainElement(Point testPoint, string elementName, UIElement referenceFrame)
        {
            IEnumerable<UIElement> elementStack =
              VisualTreeHelper.FindElementsInHostCoordinates(testPoint, referenceFrame);
            foreach (UIElement item in elementStack)
            {
                FrameworkElement feItem = item as FrameworkElement;
                //cast to FrameworkElement, need the Name property
                if (feItem != null)
                {
                    if (feItem.Name.Equals(elementName))
                    {
                        return true;
                    }
                }
            }
            // elementName was not in this stack 
            return false;
        }

        void OnClear(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.StrokeContainer.Clear();
        }


        async void OnSaveAsync(object sender, RoutedEventArgs e)
        {
            // We don't want to save an empty file
            if (inkCanvas.InkPresenter.StrokeContainer.GetStrokes().Count > 0)
            {
                var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                savePicker.FileTypeChoices.Add("Gif with embedded ISF", new System.Collections.Generic.List<string> { ".gif" });

                Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
                if (null != file)
                {
                    try
                    {
                        using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            await inkCanvas.InkPresenter.StrokeContainer.SaveAsync(stream);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
            }
            else
            {
            }
        }

        async void OnLoadAsync(object sender, RoutedEventArgs e)
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".gif");
            openPicker.FileTypeFilter.Add(".isf");
            Windows.Storage.StorageFile file = await openPicker.PickSingleFileAsync();
            if (null != file)
            {
                using (var stream = await file.OpenSequentialReadAsync())
                {
                    try
                    {
                        await inkCanvas.InkPresenter.StrokeContainer.LoadAsync(stream);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            this.ReScoreAllStrokes();
        }

        private void ReScoreAllStrokes()
        {
            this.resetTextBlocks();
            if (inkCanvas.InkPresenter.StrokeContainer.GetStrokes().Count > 0)
            {
                foreach (var varStroke in inkCanvas.InkPresenter.StrokeContainer.GetStrokes())
                {
                    processSingleStroke(varStroke);
                }
            }
        }

        private void initializeProstateInking()
        {
            InkDrawingAttributes drawingAttributes = new InkDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Colors.Red;
            drawingAttributes.FitToCurve = true;
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            inkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen; //
            inkCanvas.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
            inkCanvas.InkPresenter.StrokesErased += InkPresenter_StrokesErased;

            var eraser = inkToolBar.GetToolButton(InkToolbarTool.Eraser) as InkToolbarEraserButton;

            var flyout = FlyoutBase.GetAttachedFlyout(eraser) as Flyout;

            if (flyout != null)
            {
                var button = flyout.Content as Button;
                if (button != null)
                {
                    var newButton = new Button();
                    newButton.Style = button.Style;
                    newButton.Content = button.Content;
                    newButton.Click += EraseAllInk;
                    flyout.Content = newButton;
                }
            }
        }


        private TextBlock lookUpFeedbackBox(String uiName)
        {
            TextBlock returnBox = null;
            switch (uiName)
            {
                case "inkProstate_1A":
                    returnBox = this.txtProstate_1A;
                    break;
                case "inkProstate_1B":
                    returnBox = this.txtProstate_1B;
                    break;
                case "inkProstate_2A":
                    returnBox = this.txtProstate_2A;
                    break;
                case "inkProstate_2B":
                    returnBox = this.txtProstate_2B;
                    break;
                case "inkProstate_2C":
                    returnBox = this.txtProstate_2C;
                    break;
                case "inkProstate_2D":
                    returnBox = this.txtProstate_2D;
                    break;
                case "inkProstate_2E":
                    returnBox = this.txtProstate_2E;
                    break;
                case "inkProstate_2F":
                    returnBox = this.txtProstate_2F;
                    break;
                case "inkProstate_2G":
                    returnBox = this.txtProstate_2G;
                    break;
                case "inkProstate_2H":
                    returnBox = this.txtProstate_2H;
                    break;
                case "inkProstate_3A":
                    returnBox = this.txtProstate_3A;
                    break;
                case "inkProstate_3B":
                    returnBox = this.txtProstate_3B;
                    break;
                case "inkProstate_3C":
                    returnBox = this.txtProstate_3C;
                    break;
                case "inkProstate_3D":
                    returnBox = this.txtProstate_3D;
                    break;
                case "inkProstate_3E":
                    returnBox = this.txtProstate_3E;
                    break;
                case "inkProstate_3F":
                    returnBox = this.txtProstate_3F;
                    break;
                case "inkProstate_3G":
                    returnBox = this.txtProstate_3G;
                    break;
                case "inkProstate_3H":
                    returnBox = this.txtProstate_3H;
                    break;
                case "inkProstate_4A":
                    returnBox = this.txtProstate_4A;
                    break;
                case "inkProstate_4B":
                    returnBox = this.txtProstate_4B;
                    break;
                case "inkProstate_4C":
                    returnBox = this.txtProstate_4C;
                    break;
                case "inkProstate_4D":
                    returnBox = this.txtProstate_4D;
                    break;
                case "inkProstate_4E":
                    returnBox = this.txtProstate_4E;
                    break;
                case "inkProstate_4F":
                    returnBox = this.txtProstate_4F;
                    break;
                case "inkProstate_4G":
                    returnBox = this.txtProstate_4G;
                    break;
                case "inkProstate_4H":
                    returnBox = this.txtProstate_4H;
                    break;
                case "inkProstate_BullsEye":
                    returnBox = this.txtProstate_BullsEye;
                    break;
            }

            return returnBox;
        }


        private void resetTextBlocks()
        {
            this.txtProstate_1A.Text = "0";
            this.txtProstate_1B.Text = "0";
            this.txtProstate_2A.Text = "0";
            this.txtProstate_2B.Text = "0";
            this.txtProstate_2C.Text = "0";
            this.txtProstate_2D.Text = "0";
            this.txtProstate_2E.Text = "0";
            this.txtProstate_2F.Text = "0";
            this.txtProstate_2G.Text = "0";
            this.txtProstate_2H.Text = "0";
            this.txtProstate_3A.Text = "0";
            this.txtProstate_3B.Text = "0";
            this.txtProstate_3C.Text = "0";
            this.txtProstate_3D.Text = "0";
            this.txtProstate_3E.Text = "0";
            this.txtProstate_3F.Text = "0";
            this.txtProstate_3G.Text = "0";
            this.txtProstate_3H.Text = "0";
            this.txtProstate_4A.Text = "0";
            this.txtProstate_4B.Text = "0";
            this.txtProstate_4C.Text = "0";
            this.txtProstate_4D.Text = "0";
            this.txtProstate_4E.Text = "0";
            this.txtProstate_4F.Text = "0";
            this.txtProstate_4G.Text = "0";
            this.txtProstate_4H.Text = "0";
            this.txtProstate_BullsEye.Text = "0";
        }

    }
}
