using Msi.Led.Controller;
using Msi.Led.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Msi.Led.Gui
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var value = default(byte);
            if (!Byte.TryParse((sender as TextBox).Text + e.Text, out value))
            {
                e.Handled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Program.Main(this.GetColor());
        }

        private Color GetColor()
        {
            var red = default(byte);
            var green = default(byte);
            var blue = default(byte);

            Byte.TryParse(this.Red.Text, out red);
            Byte.TryParse(this.Green.Text, out green);
            Byte.TryParse(this.Blue.Text, out blue);

            return new Color()
            {
                Red = red,
                Green = green,
                Blue = blue
            };
        }
    }
}
