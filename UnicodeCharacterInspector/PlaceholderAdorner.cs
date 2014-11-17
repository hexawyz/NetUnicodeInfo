using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace UnicodeCharacterInspector
{
	internal static partial class Placeholder
	{
		private sealed class PlaceholderAdorner : Adorner
		{
			public string Text { get; set; }

			public PlaceholderAdorner(UIElement adornedElement)
				: base(adornedElement)
			{
				IsHitTestVisible = false;
			}

			protected override Visual GetVisualChild(int index) { throw new InvalidOperationException(); }

			protected override int VisualChildrenCount { get { return 0; } }

			protected override void OnRender(DrawingContext drawingContext)
			{
				var control = ((Control)AdornedElement);

				Rect adornedElementRect = new Rect(AdornedElement.RenderSize);
				drawingContext.DrawText
				(
					new FormattedText
					(
						Text,
						CultureInfo.CurrentCulture,
						FlowDirection.LeftToRight,
						new Typeface
						(
							control.FontFamily,
							control.FontStyle,
							control.FontWeight,
							control.FontStretch,
							SystemFonts.MessageFontFamily
						),
						control.FontSize,
						SystemColors.GrayTextBrush
					),
					// The offsets where experimentally determined on a Windows 8.1 machine. I'm willing to accept anything better. 😉
					new Point(control.BorderThickness.Left + control.Padding.Left + 5, control.BorderThickness.Top + control.Padding.Top + 3)
				);
			}
		}
	}
}
