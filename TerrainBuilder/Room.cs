using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace TerrainBuilder
{
    class Room
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public int SizeX;
        public int SizeY;

        Dictionary<string, System.Drawing.Image> _TileMaps = new Dictionary<string, System.Drawing.Image>();

        List<s_roomtile> _Room = new List<s_roomtile>();

        System.Drawing.Bitmap bm;
        System.Drawing.Graphics graphic;

        struct s_roomtile
        {
            public s_roomtile(int x, int y, string tileMapName, int tileX, int tileY)
            {
                X = x;
                Y = y;
                _TileMapName = tileMapName;
                TileX = tileX;
                TileY = tileY;
            }

            public int X;
            public int Y;
            public string _TileMapName;
            public int TileX;
            public int TileY;
        }

        private UIElement _Owner;
        private Image _Target;

        public Room(UIElement owner, Image target, int sizeX = 15, int sizeY = 15)
        {
            if (owner == null)
                throw new ArgumentNullException("Owner cannot be null");
            if (target == null)
                throw new ArgumentNullException("Target cannot be null");

            _Owner = owner;
            _Target = target;

            SizeX = sizeX;
            SizeY = sizeY;

            bm = new System.Drawing.Bitmap(64 * SizeX, 64 * SizeY);
            graphic = System.Drawing.Graphics.FromImage(bm);

            drawRoom();
        }

        public void AddTileMap(string path, string name)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(path);

            _TileMaps.Add(name, img);
        }

        public void AddRoomCell(int X, int Y, string _TileMapName, int TileX, int TileY)
        {
            _Room.RemoveAll(i => i.X == X && i.Y == Y);
            _Room.Add(new s_roomtile(X, Y, _TileMapName, TileX, TileY));
            drawRoom();
        }

        IntPtr oldhbpm = IntPtr.Zero;
        public void drawRoom()
        {
            graphic.Clear(System.Drawing.Color.Black);

            int i = 1;
            while (i < SizeX)
            {
                graphic.DrawLine(System.Drawing.Pens.White, new System.Drawing.Point(i * 64 - 1, 0), new System.Drawing.Point(i * 64 - 1, 64 * SizeY));
                graphic.DrawLine(System.Drawing.Pens.White, new System.Drawing.Point(i * 64, 0), new System.Drawing.Point(i * 64, 64 * SizeY));
                graphic.DrawLine(System.Drawing.Pens.White, new System.Drawing.Point(i * 64 + 1, 0), new System.Drawing.Point(i * 64 + 1, 64 * SizeY));
                i++;
            }
            i = 1;
            while (i < SizeY)
            {
                graphic.DrawLine(System.Drawing.Pens.White, new System.Drawing.Point(0, i * 64 - 1), new System.Drawing.Point(64 * SizeX, i * 64 - 1));
                graphic.DrawLine(System.Drawing.Pens.White, new System.Drawing.Point(0, i * 64), new System.Drawing.Point(64 * SizeX, i * 64));
                graphic.DrawLine(System.Drawing.Pens.White, new System.Drawing.Point(0, i * 64 + 1), new System.Drawing.Point(64 * SizeX, i * 64 + 1));
                i++;
            }

            _Room.ForEach((s_roomtile tile) =>
            {
                System.Drawing.Rectangle sourceRect = new System.Drawing.Rectangle(tile.TileX * 64, tile.TileY * 64, 64, 64);
                graphic.DrawImage(_TileMaps[tile._TileMapName], tile.X * 64, tile.Y * 64, sourceRect, System.Drawing.GraphicsUnit.Pixel);
            });

            var hbmp = bm.GetHbitmap();
            var options = BitmapSizeOptions.FromEmptyOptions();

            _Target.Source = Imaging.CreateBitmapSourceFromHBitmap(hbmp, IntPtr.Zero, Int32Rect.Empty, options);

            if (oldhbpm != IntPtr.Zero)
                DeleteObject(oldhbpm);

            oldhbpm = hbmp;

            _Owner.InvalidateMeasure();
            _Owner.InvalidateVisual();
        }
    }
}
