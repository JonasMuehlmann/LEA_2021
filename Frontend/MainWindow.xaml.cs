using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LEA_2021
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private cameraEditor cameraEditorWindow;

        private Scene currentScene;
        private objectEditor objectEditorWindow;

        private Task renderTask;

        public List<Scene> sceneItems;

        public MainWindow()
        {
            InitializeComponent();

            sceneItems = new List<Scene>();
            getScenes();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            closeSubWindows();
        }

        public void closeSubWindows()
        {
            objectEditorWindow?.Close();

            cameraEditorWindow?.Close();
        }

        public void OnEditorWindowClosing(object sender, CancelEventArgs e)
        {
            objectEditorWindow = null;
        }

        public void OnCameraWindowClosing(object sender, CancelEventArgs e)
        {
            cameraEditorWindow = null;
        }

        private void TextBoxNumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void RenderButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            btn.IsEnabled = false;
            ProgressBar.Height = 10;

            renderTask = Task.Run(currentScene.Render);

            renderTask.GetAwaiter().OnCompleted(() =>
            {
                btn.IsEnabled = true;
                ProgressBar.Height = 0;
                // OutputImage.Source = currentScene.Image;
            });
        }

        private objectEditor getobjectEditorWindow()
        {
            if (objectEditorWindow == null)
            {
                objectEditorWindow = new objectEditor();
                objectEditorWindow.Show();
                objectEditorWindow.Closing += OnEditorWindowClosing;
            }

            return objectEditorWindow;
        }

        private cameraEditor getCameraEditorWindow()
        {
            if (cameraEditorWindow == null)
            {
                cameraEditorWindow = new cameraEditor();
                cameraEditorWindow.Show();
                cameraEditorWindow.Closing += OnCameraWindowClosing;
            }

            return cameraEditorWindow;
        }

        private void objectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Object currItem = ObjectList.SelectedItem as Object;

            getobjectEditorWindow().Title = $"{currItem.Name} bearbeiten";
            getobjectEditorWindow().Focus();
        }

        private void cameraEditButton_Click(object sender, RoutedEventArgs e)
        {
            getCameraEditorWindow();
        }


        public void getScenes()
        {
            foreach (string file in Directory.GetFiles("../../../../Backend/scenes"))
            {
                if (Path.GetExtension(file) == ".json")
                {
                    sceneItems.Add(new Scene(Path.GetFileNameWithoutExtension(file)));
                }
            }

            SceneBox.ItemsSource = sceneItems;
        }

        private void sceneBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentScene = SceneBox.SelectedItem as Scene;
            DataContext = currentScene;
            RenderButton.IsEnabled = true;
            closeSubWindows();
        }
    }

    public class ImageConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            MemoryStream ms = new MemoryStream();
            ((Bitmap) value)?.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}