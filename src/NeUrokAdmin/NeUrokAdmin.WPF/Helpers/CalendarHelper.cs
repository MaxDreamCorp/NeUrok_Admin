using System.Windows;
using System.Windows.Controls;

namespace NeUrokAdmin.WPF.Helpers
{
    public static class CalendarHelper
    {
        public static readonly DependencyProperty BindableSelectedDatesProperty =
            DependencyProperty.RegisterAttached(
                "BindableSelectedDates",
                typeof(IList<DateTime>),
                typeof(CalendarHelper),
                new PropertyMetadata(null, OnBindableSelectedDatesChanged));

        public static IList<DateTime> GetBindableSelectedDates(DependencyObject obj) =>
            (IList<DateTime>)obj.GetValue(BindableSelectedDatesProperty);

        public static List<DateTime> Dates = new();

        public static void SetBindableSelectedDates(DependencyObject obj, IList<DateTime> value) =>
            obj.SetValue(BindableSelectedDatesProperty, value);

        private static void OnBindableSelectedDatesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Calendar calendar) return;

            calendar.SelectedDatesChanged -= Calendar_SelectedDatesChanged;
            if (e.NewValue is IList<DateTime> dates)
            {
                Dates = dates.ToList();

                calendar.SelectedDates.Clear();
                foreach (var date in dates)
                {
                    calendar.SelectedDates.Add(date);
                }

            }
            calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
        }

        private static void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is Calendar calendar)
            {
                calendar.SelectedDatesChanged -= Calendar_SelectedDatesChanged;

                calendar.SelectedDates.Clear();
                foreach (var date in Dates)
                {
                    calendar.SelectedDates.Add(date);
                }
                calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
            }
        }
    }
}