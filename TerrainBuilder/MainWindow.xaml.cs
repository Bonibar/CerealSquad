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
using System.Windows.Interop;

namespace TerrainBuilder
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TileLoader _TileLoader;
        List<Tile> _Tiles = new List<Tile>();

        System.Drawing.Graphics _Gre = System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(64, 64));

        enum e_CellType
        {
            Normal = 0,
            Wall = 1,
            Void = 2,
            Door = 3,
            Spawn = 4
        }

        enum Mode
        {
            Tile,
            Type,
            Monster
        }

        private Mode _CurrentMode = Mode.Tile;
        private int _CurrentLayer = 0;

        private bool painting = false;
        private bool erasing = false;

        public class Tile
        {
            public Tile(string name, string path, int tileX, int tileY, Image image, int originalX, int originalY)
            {
                Name = name;
                Path = path;

                OriginalX = originalX;
                OriginalY = originalY;

                TileX = tileX;
                TileY = tileY;
                SelectionX = -1;
                SelectionY = -1;

                _Image = image;
            }

            public int SelectionX;
            public int SelectionY;
            public int OriginalX;
            public int OriginalY;
            public int TileX;
            public int TileY;
            public string Name;
            public string Path;

            public Image _Image;
        }

        private Image _RoomImage;
        private Room _Room;

        public MainWindow()
        {
            InitializeComponent();

            _RoomImage = new Image();

            _RoomImage.MouseLeftButtonDown += _RoomImage_MouseLeftButtonDown;
            _RoomImage.MouseLeftButtonUp += _RoomImage_MouseLeftButtonUp;
            _RoomImage.MouseRightButtonDown += _RoomImage_MouseRightButtonDown;
            _RoomImage.MouseRightButtonUp += _RoomImage_MouseRightButtonUp;
            _RoomImage.MouseMove += _RoomImage_MouseMove;

            this.KeyDown += MainWindow_KeyDown;

            CanvasGrid.Children.Add(_RoomImage);

            _Room = new Room(CanvasGrid, _RoomImage);
            labelSizeX.Content = "X: " + _Room.SizeX;
            labelSizeY.Content = "Y: " + _Room.SizeY;

            var values = Enum.GetValues(typeof(e_CellType));
            foreach (var type in values)
            {
                ListBoxItem itemType = new ListBoxItem();
                itemType.Content = type.ToString();
                TypeList.Items.Add(itemType);
            }
            TypeList.SelectedIndex = TypeList.Items.Count - 1;
        }

        private void _RoomImage_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            erasing = false;
        }

        private void _RoomImage_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _ERASE(sender, e);
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (_CurrentMode == Mode.Tile)
            {
                if (e.Key == Key.Left)
                {
                    _Room.MoveTiles(-1, 0);
                    _Room.drawRoom();
                    e.Handled = true;
                }
                else if (e.Key == Key.Right)
                {
                    _Room.MoveTiles(1, 0);
                    _Room.drawRoom();
                    e.Handled = true;
                }
                else if (e.Key == Key.Up)
                {
                    _Room.MoveTiles(0, -1);
                    _Room.drawRoom();
                    e.Handled = true;
                }
                else if (e.Key == Key.Down)
                {
                    _Room.MoveTiles(0, 1);
                    _Room.drawRoom();
                    e.Handled = true;
                }
            }
        }

        private void _RoomImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            painting = false;
        }

        private void _RoomImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (painting)
            {
                _PAINT(sender, e);
            }
            else if (erasing)
            {
                _ERASE(sender, e);
            }
        }

        private void _RoomImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_CurrentMode == Mode.Tile)
            {
                _PAINT(sender, e);
            }
        }

        private void _ERASE(object sender, MouseEventArgs e)
        {
            Image currentImage = (Image)sender;
            Point mousepos = e.GetPosition(currentImage);
            int selectedX = (int)((mousepos.X * currentImage.Source.Width) / (64 * currentImage.RenderSize.Width));
            int selectedY = (int)((mousepos.Y * currentImage.Source.Height) / (64 * currentImage.RenderSize.Height));

            _Room.RemoveRoomCell(selectedX, selectedY, _CurrentLayer);

            erasing = true;
        }

        private void _PAINT(object sender, MouseEventArgs e)
        {
            int tabId = TilesTab.SelectedIndex;

            if (tabId != -1)
            {
                TabItem _selectedTab = (TabItem)TilesTab.Items[tabId];
                string tileName = _selectedTab.Header.ToString();

                List<Tile> _validTiles = _Tiles.FindAll(i => i.Name == tileName);
                if (_validTiles.Count > 1)
                    throw new NotSupportedException("Cannot have multiple Tiles with same name");
                else if (_validTiles.Count < 1)
                    throw new IndexOutOfRangeException("No tile found");

                Tile _curentTile = _validTiles.First();

                if (_curentTile.SelectionX != -1 && _curentTile.SelectionY != -1)
                {
                    Image currentImage = (Image)sender;
                    Point mousepos = e.GetPosition(currentImage);
                    int selectedX = (int)((mousepos.X * currentImage.Source.Width) / (_curentTile.TileX * currentImage.RenderSize.Width));
                    int selectedY = (int)((mousepos.Y * currentImage.Source.Height) / (_curentTile.TileY * currentImage.RenderSize.Height));

                    _Room.AddRoomCell(selectedX, selectedY, _curentTile.Name, _curentTile.SelectionX, _curentTile.SelectionY, _CurrentLayer);

                    painting = true;
                }
            }
        }

        private void _TileLoader_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_TileLoader.Name == "")
                return;
            TabItem _newItem = new TabItem();
            Image _tile = new Image();
            _tile.Source =  new BitmapImage(new Uri(_TileLoader.Path, UriKind.RelativeOrAbsolute), new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.CacheOnly));
            _newItem.Content = _tile;
            _newItem.Header = _TileLoader.Name;
            TilesTab.Items.Add(_newItem);

            if (TilesTab.SelectedIndex == -1 && TilesTab.Items.Count > 0)
                TilesTab.SelectedIndex = TilesTab.Items.Count - 1;

            System.Drawing.Image img = System.Drawing.Image.FromFile(_TileLoader.Path);

            Tile _newTile = new Tile(_TileLoader.Name, _TileLoader.Path, _TileLoader.TileX, _TileLoader.TileY, _tile, img.Width, img.Height);
            _Tiles.Add(_newTile);
            _tile.MouseLeftButtonDown += _tile_MouseLeftButtonDown;
            _Room.AddTileMap(_TileLoader.Path, _TileLoader.Name);

            img.Dispose();
        }

        private void _tile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int tabId = TilesTab.SelectedIndex;

            if (tabId != -1)
            {
                TabItem _selectedTab = (TabItem)TilesTab.Items[tabId];
                string tileName = _selectedTab.Header.ToString();

                List<Tile> _validTiles = _Tiles.FindAll(i => i.Name == tileName);
                if (_validTiles.Count > 1)
                    throw new NotSupportedException("Cannot have multiple Tiles with same name");
                else if (_validTiles.Count < 1)
                    throw new IndexOutOfRangeException("No tile found");

                Tile _curentTile = _validTiles.First();

                Image currentImage = (Image)sender;
                Point mousepos = e.GetPosition((IInputElement)sender);
                int selectedX = (int)((mousepos.X * _curentTile.OriginalX) / (_curentTile.TileX * currentImage.RenderSize.Width));
                int selectedY = (int)((mousepos.Y * _curentTile.OriginalY) / (_curentTile.TileY * currentImage.RenderSize.Height));

                _curentTile.SelectionX = selectedX;
                _curentTile.SelectionY = selectedY;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _TileLoader = new TileLoader();
            _TileLoader.Closing += _TileLoader_Closing;
            _TileLoader.ShowDialog();
        }

        Microsoft.Win32.OpenFileDialog _fd = new Microsoft.Win32.OpenFileDialog();
        private void SaveRoom_Click(object sender, RoutedEventArgs e)
        {
            _fd.CheckFileExists = false;
            _fd.ShowDialog();
            if (_fd.FileName != "" && _fd.SafeFileName != "")
                _Room.Export(_fd.FileName, _Tiles);

        }

        private void ToggleGrid_Click(object sender, RoutedEventArgs e)
        {
            if (!_Room.DisplayGrid)
                ((Button)sender).Background = Brushes.LightGray;
            else
                ((Button)sender).Background = Brushes.Transparent;
            _Room.DisplayGrid = !_Room.DisplayGrid;
            _Room.drawRoom();
        }

        private void addSizeX(object sender, RoutedEventArgs e)
        {
            if (_Room.SizeX < 50)
                _Room.SizeX++;
            labelSizeX.Content = "X: " + _Room.SizeX;
            _Room.drawRoom();
        }

        private void removeSizeX(object sender, RoutedEventArgs e)
        {
            if (_Room.SizeX > 4)
                _Room.SizeX--;
            labelSizeX.Content = "X: " + _Room.SizeX;
            _Room.drawRoom();
        }

        private void addSizeY(object sender, RoutedEventArgs e)
        {
            if (_Room.SizeY < 50)
                _Room.SizeY++;
            labelSizeY.Content = "Y: " + _Room.SizeY;
            _Room.drawRoom();
        }

        private void removeSizeY(object sender, RoutedEventArgs e)
        {
            if (_Room.SizeY > 4)
                _Room.SizeY--;
            labelSizeY.Content = "Y: " + _Room.SizeY;
            _Room.drawRoom();
        }

        private void switchMode(object sender, RoutedEventArgs e)
        {
            List<UIElement> _modes = ModePanel.Children.Cast<UIElement>().ToList(); ;

            _modes.ForEach((UIElement i) => {
                if (i == sender)
                {
                    ((Button)i).Background = Brushes.LightGray;
                    ((Button)i).BorderBrush = Brushes.LightGray;

                    switch (((Button)i).Content.ToString())
                    {
                        case "Tile":
                            _CurrentMode = Mode.Tile;
                            TilesTab.Visibility = Visibility.Visible;
                            TypeTab.Visibility = Visibility.Hidden;
                            MonsterTab.Visibility = Visibility.Hidden;
                            break;
                        case "Type":
                            _CurrentMode = Mode.Type;
                            TilesTab.Visibility = Visibility.Hidden;
                            TypeTab.Visibility = Visibility.Visible;
                            MonsterTab.Visibility = Visibility.Hidden;
                            break;
                        case "Monster":
                            _CurrentMode = Mode.Monster;
                            TilesTab.Visibility = Visibility.Hidden;
                            TypeTab.Visibility = Visibility.Hidden;
                            MonsterTab.Visibility = Visibility.Visible;
                            break;
                    }
                }
                else
                {
                    ((Button)i).Background = Brushes.Transparent;
                    ((Button)i).BorderBrush = Brushes.Transparent;
                }
            });
        }

        private void switchLayer(object sender, RoutedEventArgs e)
        {
            List<UIElement> _layers = LayerPanel.Children.Cast<UIElement>().ToList(); ;

            _layers.ForEach((UIElement i) => {
                if (i == sender)
                {
                    ((Button)i).Background = Brushes.LightGray;
                    ((Button)i).BorderBrush = Brushes.LightGray;

                    switch (((Button)i).Content.ToString())
                    {
                        case "L0":
                            _CurrentLayer = 0;
                            break;
                        case "L1":
                            _CurrentLayer = 1;
                            break;
                        case "L2":
                            _CurrentLayer = 2;
                            break;
                    }
                }
                else
                {
                    ((Button)i).Background = Brushes.Transparent;
                    ((Button)i).BorderBrush = Brushes.Transparent;
                }
            });
        }
    }
}
