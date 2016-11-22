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

namespace TerrainBuilder
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TileLoader _TileLoader;
        List<Tile> _Tiles = new List<Tile>();

        public struct Tile
        {
            public Tile(string name, string path, int tileX, int tileY)
            {
                Name = name;
                Path = path;
                TileX = tileX;
                TileY = tileY;
            }

            public int TileX;
            public int TileY;
            public string Name;
            public string Path;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void _TileLoader_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Tile _newTile = new Tile(_TileLoader.Name, _TileLoader.Path, _TileLoader.TileX, _TileLoader.TileY);
            _Tiles.Add(_newTile);
            TabItem _newItem = new TabItem();
            Image _tile = new Image();
            _tile.Source =  new BitmapImage(new Uri(_newTile.Path, UriKind.RelativeOrAbsolute));
            _newItem.Content = _tile;
            _newItem.Header = _newTile.Name;
            TilesTab.Items.Add(_newItem);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _TileLoader = new TileLoader();
            _TileLoader.Closing += _TileLoader_Closing;
            _TileLoader.ShowDialog();
        }

        private void loadTile(string path)
        {
            
        }
    }
}
