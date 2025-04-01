using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using System;

namespace InterShareWindows.Views;

public sealed partial class CustomProgressRing : UserControl
{
    public static readonly DependencyProperty ValueProperty =
           DependencyProperty.Register(
               nameof(Value),
               typeof(double),
               typeof(CustomProgressRing),
               new PropertyMetadata(0.0, OnValueChanged));

    public static readonly DependencyProperty IsIndeterminateProperty =
        DependencyProperty.Register(
            nameof(IsIndeterminate),
            typeof(bool),
            typeof(CustomProgressRing),
            new PropertyMetadata(false, OnIsIndeterminateChanged));

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public bool IsIndeterminate
    {
        get => (bool)GetValue(IsIndeterminateProperty);
        set => SetValue(IsIndeterminateProperty, value);
    }

    public CustomProgressRing()
    {
        this.InitializeComponent();
    }

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is CustomProgressRing ring)
        {
            ring.UpdateDeterminateState((double)e.NewValue);
        }
    }

    private static void OnIsIndeterminateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is CustomProgressRing ring)
        {
            ring.UpdateIndeterminateState((bool)e.NewValue);
        }
    }

    private void UpdateDeterminateState(double value)
    {
        // Ensure the value is between 0 and 100
        value = Math.Clamp(value, 0.0, 100.0);

        double angle = (value / 100.0) * 360.0;
        double radians = (angle - 90) * Math.PI / 180.0;

        double radius = RootGrid.ActualWidth / 2;
        Point centerPoint = new Point(radius, radius);
        Point endPoint = new Point(
            radius + radius * Math.Cos(radians),
            radius + radius * Math.Sin(radians)
        );

        ProgressArcSegment.Point = endPoint;
        ProgressArcSegment.IsLargeArc = value > 50;
    }

    private void UpdateIndeterminateState(bool isIndeterminate)
    {
        if (isIndeterminate)
        {
            IndeterminateArc.Visibility = Visibility.Visible;
            DeterminateArc.Visibility = Visibility.Collapsed;
            IndeterminateStoryboard?.Begin();
        }
        else
        {
            IndeterminateStoryboard?.Stop();
            IndeterminateArc.Visibility = Visibility.Collapsed;
            DeterminateArc.Visibility = Visibility.Visible;
        }
    }
}