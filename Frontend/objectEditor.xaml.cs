using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace LEA_2021
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class objectEditor : Window
    {
        #region Constructors

        public objectEditor()
        {
            InitializeComponent();

            // MessageBox.Show(DataContext.ToString());
        }

        #endregion

        private void TextBoxNumberValidation(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }

    public class VectorConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MessageBox.Show(value.ToString());
            Console.WriteLine(value);
            throw new NotImplementedException();
        }
    }
}