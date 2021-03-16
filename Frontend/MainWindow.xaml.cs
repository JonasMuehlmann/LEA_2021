using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Threading;
using System.ComponentModel;

namespace LEA_2021
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private objectEditor objectEditorWindow;

        public MainWindow()
        {
            InitializeComponent();
            updateObjects();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // close object editor window if main window gets closed
            if (objectEditorWindow != null)
            {
                objectEditorWindow.Close();
            }
        }

        public void OnEditorWindowClosing(object sender, CancelEventArgs e)
        {
            objectEditorWindow = null;
        }

        private void updateObjects(){
            List<ObjectItem> items = new List<ObjectItem>();
            items.Add(new ObjectItem() { Name = "Baum01", Shape = "Rectangle", Material = "wood" });
            items.Add(new ObjectItem() { Name= "Kreis", Shape = "Square", Material = "grass" });
            items.Add(new ObjectItem() { Name = "Stein", Shape = "Plane", Material = "stone" });

            objectList.ItemsSource = items;
        }

        private void TextBoxNumberValidation(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void RenderButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            btn.IsEnabled = false;
            progressBar.Height = 10;
        }

        private objectEditor getobjectEditorWindow()
        {
            if(objectEditorWindow == null)
            {
                objectEditorWindow = new objectEditor();
                objectEditorWindow.Show();
                objectEditorWindow.Closing += OnEditorWindowClosing;
            }

            return objectEditorWindow;
        }

        private void objectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObjectItem currItem = objectList.SelectedItem as ObjectItem;
            
            getobjectEditorWindow().Title = $"{currItem.Name} bearbeiten";
            getobjectEditorWindow().Focus();
        }
    }

    public class ObjectItem
    {
        public string Shape { get; set; }
        public string Material { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{0} (Shape: {1}, Material: {2})", Name, Shape, Material);
        }
    }
}
