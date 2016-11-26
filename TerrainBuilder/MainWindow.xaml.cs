﻿using System;
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
            _RoomImage.MouseMove += _RoomImage_MouseMove;

            CanvasGrid.Children.Add(_RoomImage);

            _Room = new Room(CanvasGrid, _RoomImage);
        }

        private void _RoomImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            painting = false;
        }

        private bool painting = false;
        private void _RoomImage_MouseMove(object sender, MouseEventArgs e)
        {
            int tabId = TilesTab.SelectedIndex;

            if (tabId != -1 && painting)
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

                    _Room.AddRoomCell(selectedX, selectedY, _curentTile.Name, _curentTile.SelectionX, _curentTile.SelectionY);
                }
            }
        }

        private void _RoomImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

                    _Room.AddRoomCell(selectedX, selectedY, _curentTile.Name, _curentTile.SelectionX, _curentTile.SelectionY);

                    painting = true;
                }
            }
        }

        private void _TileLoader_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TabItem _newItem = new TabItem();
            Image _tile = new Image();
            _tile.Source =  new BitmapImage(new Uri(_TileLoader.Path, UriKind.RelativeOrAbsolute), new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.CacheOnly));
            _newItem.Content = _tile;
            _newItem.Header = _TileLoader.Name;
            TilesTab.Items.Add(_newItem);

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
            _Room.Export(_fd.FileName, _Tiles);
        }
    }
}