﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Udemy_Calculator
{
    public partial class ColorPicker_Control : UserControl
    {
        public static readonly DependencyProperty ColorPickerTextProperty = DependencyProperty.Register("ColorPickerText", typeof(string), typeof(ColorPicker_Control), new PropertyMetadata(""));
        public string ColorPickerText
        {
            get { return (string)GetValue(ColorPickerTextProperty); }
            set { SetValue(ColorPickerTextProperty, value); }
        }

        public static readonly DependencyProperty ColorPickerUIDProperty = DependencyProperty.Register("ColorPickerUID", typeof(string), typeof(ColorPicker_Control), new PropertyMetadata(""));
        public string ColorPickerUID
        {
            get { return (string)GetValue(ColorPickerUIDProperty); }
            set { SetValue(ColorPickerUIDProperty, value); }
        }

        public ColorPicker_Control()
        {
            InitializeComponent();
        }

        private void UIColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            if(e.NewValue == null)
            {
                return;
            }

            SolidColorBrush lColorSelected = new SolidColorBrush((Color)e.NewValue);

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() != typeof(OptionWindow))
                {
                    continue;
                }
                var loPtionWindox = window as OptionWindow;

                var lItemSelected = loPtionWindox.ItemsListBox.SelectedItem.ToString();
                var lColorPickerUID = TitleLabel.Uid.ToString();

                foreach (var lTheme in CommonTheme.ListSelectedTheme.Where(p => p.SubThemeAttribute == lItemSelected))
                {
                    if (lColorPickerUID.Contains("Color1"))
                    {
                        lTheme.Color1 = lColorSelected; 
                    }
                    else if (lColorPickerUID.Contains("Color2"))
                    {
                        lTheme.Color2 = lColorSelected;
                    }
                    else if (lColorPickerUID.Contains("Color3"))
                    {
                        lTheme.Color3 = lColorSelected;
                    }
                    else if (lColorPickerUID.Contains("Color4"))
                    {
                        lTheme.Color4 = lColorSelected;
                    }
                }
            }

            // Applying the new theme
            CommonTheme.SetSelectedThemeListObject(CommonTheme.ThemeSelectedName);
        }
    }
}
