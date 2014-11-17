using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace UnicodeCharacterInspector
{
	// Inspired by http://stackoverflow.com/questions/833943/watermark-hint-text-placeholder-textbox-in-wpf
	internal static partial class Placeholder
	{
		public static string GetText(DependencyObject obj)
		{
			return (string)obj.GetValue(TextProperty);
		}

		public static void SetText(DependencyObject obj, string value)
		{
			obj.SetValue(TextProperty, value);
		}

		public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text", typeof(string), typeof(Placeholder), new PropertyMetadata(OnTextChanged));

		private static readonly HashSet<Control> handledControls = new HashSet<Control>();

		private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as TextBoxBase;

			if (d == null) return;

			if (e.NewValue == null || "".Equals(e.NewValue)) UnregisterControl(control);
			else UpdateControl(control, (string)e.NewValue);
		}

		private static void OnControlLoaded(object sender, RoutedEventArgs e)
		{
			var control = (Control)sender;
			UpdateControl(control, GetText(control));
		}

		private static void OnTextBoxGotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
		{
			var control = (Control)sender;
			RemoveAdorner(control);
		}

		private static void UpdateControl(Control control, string placeholderText)
		{
			if (handledControls.Add(control))
			{
				RegisterControl(control);
			}

			if (IsEmpty(control))
			{
				var adorner = GetAdorner(control) ?? AddAdorner(control);
				if (adorner != null) adorner.Text = placeholderText;
			}
			else
			{
				RemoveAdorner(control);
			}
		}

		private static void RegisterControl(Control control)
		{
			control.Loaded += OnControlLoaded;

			var textBox = control as TextBoxBase;
			if (textBox != null) RegisterTextBox(textBox);
		}

		private static void RegisterTextBox(TextBoxBase textBox)
		{
			textBox.GotKeyboardFocus += OnTextBoxGotKeyboardFocus;
			textBox.LostKeyboardFocus += OnControlLoaded;
		}

		private static void UnregisterControl(Control control)
		{
			if (handledControls.Remove(control))
			{
				control.Loaded -= OnControlLoaded;

				var textBox = control as TextBoxBase;
				if (textBox != null) UnregisterTextBox(textBox);

				RemoveAdorner(control);
			}
		}

		private static void UnregisterTextBox(TextBoxBase textBox)
		{
			textBox.GotKeyboardFocus -= OnTextBoxGotKeyboardFocus;
			textBox.LostKeyboardFocus -= OnControlLoaded;
		}

		private static bool IsEmpty(Control control)
		{
			var textBox = control as TextBox;

			if (textBox != null) return string.IsNullOrEmpty(textBox.Text);

			return false;
		}

		private static PlaceholderAdorner GetAdorner(Control control)
		{
			var layer = AdornerLayer.GetAdornerLayer(control);

			if (layer != null)
			{
				var adorners = layer.GetAdorners(control);

				if (adorners != null)
				{
					foreach (var adorner in adorners)
					{
						var placeholder = adorner as PlaceholderAdorner;

						if (placeholder != null) return placeholder;
					}
				}
			}

			return null;
		}

		private static PlaceholderAdorner AddAdorner(Control control)
		{
			var layer = AdornerLayer.GetAdornerLayer(control);

			if (layer == null) return null;

			var placeholder = new PlaceholderAdorner(control);

			layer.Add(placeholder);

			return placeholder;
		}

		private static void RemoveAdorner(Control control)
		{
			var layer = AdornerLayer.GetAdornerLayer(control);

			if (layer == null) return;

			var adorners = layer.GetAdorners(control);

			if (adorners == null) return;

			foreach (var adorner in adorners)
			{
				var placeholder = adorner as PlaceholderAdorner;

				if (placeholder != null)
				{
					layer.Remove(placeholder);
					return;
				}
			}
		}
	}
}
