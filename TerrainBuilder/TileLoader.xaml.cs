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
using System.Windows.Shapes;

namespace TerrainBuilder
{
    /// <summary>
    /// Logique d'interaction pour TileLoader.xaml
    /// </summary>
    public partial class TileLoader : Window
    {
        public string Path = "No FIle Selected !";
        public int TileX = -1;
        public int TileY = -1;
        public string Name = "";

        Microsoft.Win32.OpenFileDialog FileDialog = new Microsoft.Win32.OpenFileDialog();

        public TileLoader()
        {
            InitializeComponent();
            FileDialog.Multiselect = false;
            FileDialog.Filter = "Tile File|*.png;*.jpg:*.jpeg";
            FileDialog.FileOk += FileDialog_FileOk;
        }

        private void FileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Path = FileDialog.FileName;
            Name = FileDialog.SafeFileName;
            FilePath.Content = Path;
            ((Image)TilePreview).Source = new BitmapImage(new Uri(Path, UriKind.RelativeOrAbsolute));
        }

        private void Button_loadtile_Click(object sender, RoutedEventArgs e)
        {
            FileDialog.ShowDialog();
        }

        private void YValue_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                TileY = int.Parse(((TextBox)sender).Text);
            }
            catch
            {
                TileY = -1;
            }
        }

        private void XValue_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                TileX = int.Parse(((TextBox)sender).Text);
            }
            catch
            {
                TileX = -1;
            }
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (TileX > 0 && TileY > 0 && System.IO.File.Exists(Path) && Name != "")
                Close();
        }
    }
}
