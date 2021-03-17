using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LEA_2021
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private cameraEditor cameraEditorWindow;
        private objectEditor objectEditorWindow;

        public List<Scene> sceneItems;

        public MainWindow()
        {
            InitializeComponent();

            sceneItems = new List<Scene>();
            getScenes();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // close object editor window if main window gets closed
            if (objectEditorWindow != null)
            {
                objectEditorWindow.Close();
            }

            if (cameraEditorWindow != null)
            {
                cameraEditorWindow.Close();
            }
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
                if (System.IO.Path.GetExtension(file) == ".json")
                {
                    sceneItems.Add(new Scene(System.IO.Path.GetFileNameWithoutExtension(file)));
                }
            }

            SceneBox.ItemsSource = sceneItems;
        }

        private void sceneBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DataContext = SceneBox.SelectedItem as Scene;
        }
    }
}