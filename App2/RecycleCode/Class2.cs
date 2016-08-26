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
using App2.Views;
using Windows.UI.Xaml.Input;
using System.Diagnostics;
using Windows.UI.Input.Inking.Core;
using Windows.UI.Core;

using System.Collections.Generic;
using System.Reflection;
using Windows.UI.Xaml.Shapes;
using System.Numerics;

// sources used:
//https://social.msdn.microsoft.com/Forums/en-US/0b302b80-93ab-41ac-a1d8-8ef7ddbb3e71/uwp-inkcanvas-how-to-consume-pointerpressed-pointerreleased-and-pointermoved-events?forum=wpdevelop
//https://social.msdn.microsoft.com/Forums/office/en-US/ed11a15e-b38e-4a26-88ac-5cc53d8ccec1/uwpxaml-how-to-convert-ink-renderRects-to-points-inkingmanager?forum=wpdevelop
//http://stackoverflow.com/questions/24772775/transformtovisual-returns-different-values-for-uielements-with-different-scales

namespace App2.Views
{
    public sealed partial class CanvasTestPage : Page
    {
        //private MainPage rootPage;
        const int minPenSize = 2;
        const int penSizeIncrement = 2;
        int penSize;

        public CanvasTestPage()
        {
            this.InitializeComponent();
            penSize = minPenSize + penSizeIncrement * PenThickness.SelectedIndex;
            InkDrawingAttributes drawingAttributes = new InkDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Colors.Red;
            drawingAttributes.Size = new Size(penSize, penSize);
            drawingAttributes.IgnorePressure = false;
            drawingAttributes.FitToCurve = true;
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            inkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen; //
            inkCanvas.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
            inkCanvas.InkPresenter.StrokesErased += InkPresenter_StrokesErased;
            inkCanvas.InkPresenter.UnprocessedInput.PointerPressed += UnprocessedInput_PointerPressed;
        }

        private void InkPresenter_StrokesErased(InkPresenter sender, InkStrokesErasedEventArgs args)
        {
        }

        private void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            int i = 0;
            foreach (var hitTestStroke in args.Strokes)
            {
                IReadOnlyList<InkStrokeRenderingSegment> segments;
                PathGeometry pathGeometry = new PathGeometry();
                PathFigureCollection pathFigureCollection = new PathFigureCollection();
                PathFigure pathFigure = new PathFigure();
                PathSegmentCollection pathSegments = new PathSegmentCollection();
                Path path = new Path();
                Point TransFormedPoint;
                Rectangle renderRect = new Rectangle();
                segments = hitTestStroke.GetRenderingSegments();

                // Process each renderRect segment.
                bool first = true;
                foreach (InkStrokeRenderingSegment segment in segments)
                {
                    // The first segment is the starting point for the path.
                    if (first)
                    {
                        pathFigure.StartPoint = segment.BezierControlPoint1;
                        first = false;
                    }

                    // Copy each ink segment into a bezier segment.
                    BezierSegment bezSegment = new BezierSegment();
                    bezSegment.Point1 = segment.BezierControlPoint1;
                    bezSegment.Point2 = segment.BezierControlPoint2;
                    bezSegment.Point3 = segment.Position;
                    pathSegments.Add(bezSegment);
                }

                // Build the path geometerty object.
                pathFigure.Segments = pathSegments;
                pathFigureCollection.Add(pathFigure);
                pathGeometry.Figures = pathFigureCollection;


                // Get boundingrectangle              
                Rect boundingRect = pathGeometry.Bounds;
                renderRect.Width = boundingRect.Width;
                renderRect.Height = boundingRect.Height;
                renderRect.Stroke = new SolidColorBrush(Colors.Blue);
                renderRect.StrokeThickness = 4;
                Canvas.SetLeft(renderRect, boundingRect.X);
                Canvas.SetTop(renderRect, boundingRect.Y);
                Canvas.SetZIndex(renderRect, 99);
                base_black2_1.Children.Add(renderRect);

                Debug.WriteLine("XY_before: " + boundingRect.X + ":" + boundingRect.Y);

                //transform to main page values
                TransFormedPoint = GetPosition(new Point(boundingRect.X, boundingRect.Y), renderRect);
                boundingRect.X = TransFormedPoint.X;
                boundingRect.Y = TransFormedPoint.Y;

                Debug.WriteLine("XY_after: " + boundingRect.X + ":" + boundingRect.Y);

                IEnumerable<UIElement> elementStack = VisualTreeHelper.FindElementsInHostCoordinates(boundingRect, base_black2_1, true);
                int k = 0;
                foreach (UIElement element in elementStack)
                {
                    k++;
                    Path feItem = element as Path;
                    //cast to FrameworkElement, need the Name property
                    if (feItem != null)
                        Debug.WriteLine(k + ". UIElement: " + feItem.Name);
                }
            }
        }



        public Point GetPosition(Point ptrPt, UIElement p)
        {
            GeneralTransform gt = p.TransformToVisual(null);
            Point screenPoint;
            screenPoint = gt.TransformPoint(new Point(0, 0));
            return screenPoint;
        }

        internal static void FindChildren<T>(List<T> results, DependencyObject startNode) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(startNode);
            for (int i = 0; i < count; i++)
            {
                DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
                if ((current.GetType()).Equals(typeof(T)) || (current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
                {
                    T asType = (T)current;
                    results.Add(asType);
                }
                FindChildren<T>(results, current);
            }
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

        private void UnprocessedInput_PointerPressed(InkUnprocessedInput sender, Windows.UI.Core.PointerEventArgs args)
        {
            basePathOne.Fill = new SolidColorBrush(Windows.UI.Colors.Blue);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //rootPage = MainPage.Current;
            inkCanvas.Width = Window.Current.Bounds.Width;
            inkCanvas.Height = Window.Current.Bounds.Height;
        }

        void OnPenColorChanged(object sender, RoutedEventArgs e)
        {
            if (inkCanvas != null)
            {
                InkDrawingAttributes drawingAttributes = inkCanvas.InkPresenter.CopyDefaultDrawingAttributes();

                // Use button's background to set new pen's color
                var btnSender = sender as Button;
                var brush = btnSender.Background as Windows.UI.Xaml.Media.SolidColorBrush;

                drawingAttributes.Color = brush.Color;
                inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            }
        }

        void OnPenThicknessChanged(object sender, RoutedEventArgs e)
        {
            if (inkCanvas != null)
            {
                InkDrawingAttributes drawingAttributes = inkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
                penSize = minPenSize + penSizeIncrement * PenThickness.SelectedIndex;
                string value = ((ComboBoxItem)PenType.SelectedItem).Content.ToString();
                if (value == "Highlighter" || value == "Calligraphy")
                {
                    // Make the pen tip rectangular for highlighter and calligraphy pen
                    drawingAttributes.Size = new Size(penSize, penSize * 2);
                }
                else
                {
                    drawingAttributes.Size = new Size(penSize, penSize);
                }
                inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            }
        }

        void OnPenTypeChanged(object sender, RoutedEventArgs e)
        {
            if (inkCanvas != null)
            {
                InkDrawingAttributes drawingAttributes = inkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
                string value = ((ComboBoxItem)PenType.SelectedItem).Content.ToString();

                if (value == "Ballpoint")
                {
                    drawingAttributes.Size = new Size(penSize, penSize);
                    drawingAttributes.PenTip = PenTipShape.Circle;
                    drawingAttributes.DrawAsHighlighter = false;
                    drawingAttributes.PenTipTransform = System.Numerics.Matrix3x2.Identity;
                }
                else if (value == "Highlighter")
                {
                    // Make the pen rectangular for highlighter
                    drawingAttributes.Size = new Size(penSize, penSize * 2);
                    drawingAttributes.PenTip = PenTipShape.Rectangle;
                    drawingAttributes.DrawAsHighlighter = true;
                    drawingAttributes.PenTipTransform = System.Numerics.Matrix3x2.Identity;
                }
                if (value == "Calligraphy")
                {
                    drawingAttributes.Size = new Size(penSize, penSize * 2);
                    drawingAttributes.PenTip = PenTipShape.Rectangle;
                    drawingAttributes.DrawAsHighlighter = false;

                    // Set a 45 degree rotation on the pen tip
                    double radians = 45.0 * Math.PI / 180;
                    drawingAttributes.PenTipTransform = System.Numerics.Matrix3x2.CreateRotation((float)radians);
                }
                inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            }
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
        }

        private void TouchInkingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen | Windows.UI.Core.CoreInputDeviceTypes.Touch;
        }

        private void TouchInkingCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen;
        }

        private void ErasingModeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.Erasing;
        }

        private void ErasingModeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.Inking;
        }
    }
}