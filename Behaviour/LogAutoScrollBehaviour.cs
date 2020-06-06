using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Comparator.Behaviour
{
    public class LogAutoScrollBehaviour : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.TextChanged += OnTextChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            bool scrollToEnd = e.Changes.Any() && AssociatedObject.CaretIndex == e.Changes.First().Offset;
            if (scrollToEnd)
            {
                AssociatedObject.CaretIndex = AssociatedObject.Text.Length;
                AssociatedObject.ScrollToEnd();
            }
        }
    }
}
