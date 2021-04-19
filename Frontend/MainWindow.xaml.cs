// Copyright 2021 Jonas Muehlmann, Tim Dreier
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
        #region Properties

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
            // close both windows if opened
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

            // create async task to render image
            renderTask = Task.Run(() => currentScene.Render(true));

            renderTask.GetAwaiter().OnCompleted(() =>
            {
                btn.IsEnabled = true;
                ProgressBar.Height = 0;
            });
        }

        private objectEditor getobjectEditorWindow()
        {
            // open object editor window if not instantiated and return
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
            // open camera editor window if not instantiated and return
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
            // open object editor window and set title and dataContext
            var currItem = ObjectList.SelectedItem as Object;

            getobjectEditorWindow().Title = $"{currItem.Name} bearbeiten";
            getobjectEditorWindow().Focus();

            getobjectEditorWindow().DataContext = currItem;
        }

        private void cameraEditButton_Click(object sender, RoutedEventArgs e)
        {
            getCameraEditorWindow();
        }


        public void getScenes()
        {
            // get all scenes
            foreach (var file in Directory.GetFiles(Constants.SceneDir))
            {
                // skip file if extension is not json or its a system config
                if (Path.GetExtension(file) == ".json" && !Path.GetFileNameWithoutExtension(file).StartsWith("system_"))
                {
                    var scene = new Scene(Path.GetFileNameWithoutExtension(file));
                    sceneItems.Add(scene);
                }
            }

            SceneBox.ItemsSource = sceneItems;
        }

        public void getMaterials()
        {
            // iterate materials in folder
            foreach (var directory in Directory.GetDirectories(Constants.MaterialsDir))
            {
                materialItems.Add(new Material(Path.GetFileName(directory)));
            }

            MaterialBox.ItemsSource = materialItems;
        }

        private void sceneBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // set selected scene as datacontext and close all subwindows to prevent displaydata bugs
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
                // collapse default scene editor, show material viewer container
                sceneContainer.Visibility = Visibility.Collapsed;
                materialViewerContainer.Visibility = Visibility.Visible;
                MaterialViewerButton.Content = "Material-Viewer beenden";

                // set current scene to materialviewer
                currentScene = new Scene("system_materialviewer");
                DataContext = currentScene;

                // get all meterials from filesystem
                getMaterials();

                // hide render button
                RenderButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                // aaaaand reverse...
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
            if (!materialViewerActive)
            {
                throw new ArgumentException("Material-Viewer not active");
            }
            
            var material = MaterialBox.SelectedItem as Material;
            // set material of first object in current scene as currently selected material
            currentScene.Objects.First().Material = material;
            
            // trigger render click
            RenderButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

            // set settings to input fields
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

            // write material settings to json config
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
            // hard-coded material viewer, only for presentation
            Process.Start("explorer",
                $@"C:\Users\info\RiderProjects\LEA_2021\Backend\scenes\materials\{currentScene.Objects.First().Material.Name}");
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
            // convert bitmap to bitmapimage
            // Catch windows bu g "object is currenty in use elsewhere" and try again 
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