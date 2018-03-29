using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParallexScrollForWPF
{
    public class ParallexScrollControl : Border
    {
        public event EventHandler<bool> IsStartParallexScrollingChanged;
        private void OnIsStartParallexScrollingChanged(bool isStartParallexScrolling)
        {
            IsStartParallexScrollingChanged?.Invoke(this, isStartParallexScrolling);
        }

        public static readonly DependencyProperty ParallexScrollSourceProperty =
            DependencyProperty.Register("ParallexScrollSource", typeof(DependencyObject), typeof(ParallexScrollControl), new PropertyMetadata(default(DependencyObject), OnParallexScrollViewerChanged));

        public DependencyObject ParallexScrollSource
        {
            get { return (DependencyObject)GetValue(ParallexScrollSourceProperty); }
            set { SetValue(ParallexScrollSourceProperty, value); }
        }

        private static void OnParallexScrollViewerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var parallexScrollControl = (ParallexScrollControl)d;
            parallexScrollControl.StartParallexScrolling();
        }

        public static readonly DependencyProperty SpeedProperty =
            DependencyProperty.Register("Speed", typeof(double), typeof(ParallexScrollControl), new PropertyMetadata(0.5));

        public double Speed
        {
            get { return (double)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        private bool isStartParallexScrolling = false;
        public bool IsStartParallexScrolling
        {
            get
            {
                return isStartParallexScrolling;
            }
            private set
            {
                if (isStartParallexScrolling == value)
                {
                    return;
                }

                isStartParallexScrolling = value;
                OnIsStartParallexScrollingChanged(value);
            }
        }

        private ScrollViewer ParallexScrollViewer { get; set; }
        private TranslateTransform ParallexScrollTranslateTransform { get; set; }

        public ParallexScrollControl()
        {
            this.Loaded += ParallexScrollControl_Loaded;
            this.Unloaded += ParallexScrollControl_Unloaded;
        }

        private void ParallexScrollControl_Loaded(object sender, RoutedEventArgs e)
        {
            StartParallexScrolling();
        }

        private void ParallexScrollControl_Unloaded(object sender, RoutedEventArgs e)
        {
            StopParallexScrolling();
        }

        private void StartParallexScrolling()
        {
            if (IsStartParallexScrolling || this.IsLoaded == false || ParallexScrollSource == null)
            {
                return;
            }

            if (GetParallexScrollViewer() == false)
            {
                return;
            }

            ParallexScrollTranslateTransform = new TranslateTransform();
            this.RenderTransform = ParallexScrollTranslateTransform;

            IsStartParallexScrolling = true;

            CompositionTarget.Rendering += UpdateParallexScroll;
        }

        private bool GetParallexScrollViewer()
        {
            if (ParallexScrollSource is ScrollViewer)
            {
                ParallexScrollViewer = ParallexScrollSource as ScrollViewer;
            }
            else
            {
                ParallexScrollViewer = ParallexScrollSource.GetVisualChildCollection<ScrollViewer>().FirstOrDefault();
                if (ParallexScrollViewer == null)
                {
                    return false;
                }
            }

            return true;
        }

        private void StopParallexScrolling()
        {
            CompositionTarget.Rendering -= UpdateParallexScroll;
            ParallexScrollTranslateTransform = null;
            ParallexScrollViewer = null;
            this.RenderTransform = null;
            IsStartParallexScrolling = false;
        }

        private void UpdateParallexScroll(object sender, EventArgs e)
        {
            ParallexScrollTranslateTransform.Y = ParallexScrollViewer.VerticalOffset * Speed;
        }
    }
}
