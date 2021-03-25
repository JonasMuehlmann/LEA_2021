using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace LEA_2021
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private cameraEditor cameraEditorWindow;

        private Scene currentScene;
        public List<Material> materialItems;

        private bool materialViewerActive;
        private objectEditor objectEditorWindow;

        private Task renderTask;

        public List<Scene> sceneItems;

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();

            sceneItems = new List<Scene>();
            materialItems = new List<Material>();
            getScenes();
        }

        #endregion

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
            var regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void RenderButton_Click(object sender, RoutedEventArgs e)
        {
            currentScene.Save();

            var btn = sender as Button;

            btn.IsEnabled = false;
            ProgressBar.Height = 10;

            renderTask = Task.Run(() => currentScene.Render(true));

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
            var currItem = ObjectList.SelectedItem as Object;

            getobjectEditorWindow().Title = $"{currItem.Name} bearbeiten";
            getobjectEditorWindow().Focus();

            getobjectEditorWindow().DataContext = currItem;
            Console.WriteLine(currItem.Position);
            Console.WriteLine(currItem.Position.X);
        }

        private void cameraEditButton_Click(object sender, RoutedEventArgs e)
        {
            getCameraEditorWindow();
        }


        public void getScenes()
        {
            foreach (var file in Directory.GetFiles("../../../../Backend/scenes"))
                if (Path.GetExtension(file) == ".json" && !Path.GetFileNameWithoutExtension(file).StartsWith("system_"))
                {
                    var scene = new Scene(Path.GetFileNameWithoutExtension(file));
                    sceneItems.Add(scene);
                }

            SceneBox.ItemsSource = sceneItems;
        }

        public void getMaterials()
        {
            foreach (var directory in Directory.GetDirectories("../../../../Backend/scenes/materials"))
                materialItems.Add(new Material(Path.GetFileName(directory)));

            MaterialBox.ItemsSource = materialItems;
        }

        private void sceneBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentScene = SceneBox.SelectedItem as Scene;
            DataContext = currentScene;
            RenderButton.IsEnabled = true;
            closeSubWindows();
        }

        private void MaterialViewerButton_OnClick(object sender, RoutedEventArgs e)
        {
            materialViewerActive = !materialViewerActive;

            if (materialViewerActive)
            {
                sceneContainer.Visibility = Visibility.Collapsed;
                materialViewerContainer.Visibility = Visibility.Visible;
                MaterialViewerButton.Content = "Material-Viewer beenden";

                currentScene = new Scene("system_materialviewer");
                DataContext = currentScene;

                getMaterials();

                RenderButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                currentScene = null;
                DataContext = null;
                MaterialViewerButton.Content = "Material-Viewer starten";
                SceneBox.SelectedIndex = -1;
                sceneContainer.Visibility = Visibility.Visible;
                materialViewerContainer.Visibility = Visibility.Collapsed;
                RenderButton.Visibility = Visibility.Visible;
            }
        }

        private void MaterialBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var material = MaterialBox.SelectedItem as Material;
            currentScene.Objects.First().Material = material;
            RenderButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

            RefractiveIndexInput.Text = material.RefractiveIndex.ToString();
            TransparencySlider.Value = (float) material.Transparency;
        }

        private void MaterialSaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!materialViewerActive)
            {
                throw new ArgumentException("Material-Viewer not active");
            }

            if (MaterialBox.SelectedItem == null)
            {
                MessageBox.Show("Bitte ein Material auswählen");
                return;
            }

            float refractiveIndex = float.Parse(RefractiveIndexInput.Text);
            float transparency = (float) TransparencySlider.Value;

            Dictionary<dynamic, dynamic> jsonData = new();

            jsonData.Add("refractiveIndex", refractiveIndex);
            jsonData.Add("transparency", transparency);

            File.WriteAllText($"{Constants.MaterialsDir}/{currentScene.Objects.First().Material.Name}/config.json",
                JsonConvert.SerializeObject(jsonData, Formatting.Indented)
            );

            currentScene.Objects.First().Material.RefractiveIndex = refractiveIndex;
            currentScene.Objects.First().Material.Transparency = transparency;
            RenderButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void MaterialFolderButton_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer", $@"C:\Users\info\RiderProjects\LEA_2021\Backend\scenes\materials\{currentScene.Objects.First().Material.Name}");
        }
    }

    public class ImageConverter : IValueConverter
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH")]
        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            do
            {
                try
                {
                    var image = new BitmapImage();
                    var ms = new MemoryStream();

                    var buffer = value as Bitmap;
                    lock (buffer)
                    {
                        buffer.Save(ms, ImageFormat.Png);
                    }

                    image.BeginInit();
                    ms.Seek(0, SeekOrigin.Begin);
                    image.StreamSource = ms;
                    image.EndInit();

                    return image;
                }
                catch (InvalidOperationException e)
                {
                }
            } while (true);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}