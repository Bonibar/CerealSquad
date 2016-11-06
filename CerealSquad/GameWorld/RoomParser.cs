using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.GameWorld
{
    static class RoomParser
    {
        public class t_cellpos
        {
            private t_cellpos() { }

            public t_cellpos(uint row, uint column)
            {
                Row = row;
                Column = column;
            }

            public uint Row { get; }
            public uint Column { get; }
        }

        public class t_cellcontent
        {
            private t_cellcontent() { }
            public t_cellcontent(int texture, string texturePath, e_CellType type)
            {
                Texture = texture;
                TexturePath = texturePath;
                Type = type;
            }

            public int Texture { get; }
            public string TexturePath { get; }
            public e_CellType Type { get; }
        }

        public enum e_CellType
        {
            Normal = 0,
            Wall = 1
        }

        private static string FILE_HASHEDKEY = "58672f161bdbe31526fd8384909d4aa22b8fd91da8fce113ea083fbd6022e73e";

        private static bool checkHash(string line)
        {
            return line.Equals(FILE_HASHEDKEY);
        }

        private static Dictionary<int, string> loadTiles(string[] lines)
        {
            Dictionary<int, string> tiles = new Dictionary<int, string>();
            uint startline = 0;
            uint endline;

            lines.First(x => x.Equals("#define Tiles"));
            while (startline < lines.Length && !lines[startline].Equals("#define Tiles"))
                startline++;
            startline++;

            endline = startline;
            while (endline < lines.Length && !lines[endline].Contains("#define"))
                endline++;

            if (endline == startline || startline >= lines.Length)
                throw new FormatException("No tile defined");

            while (startline < endline)
            {
                string[] values = lines[startline].Split('|');
                if (values.Length != 2)
                    throw new FormatException("Wrong tile declaration format");
                try
                {
                    int id = int.Parse(values[0]);
                    string path = values[1];

                    tiles.Add(id, path);
                } catch (Exception e)
                {
                    throw new FormatException("Wrong id in tile declaration", e);
                }

                startline++;
            }

            if (tiles.Count <= 0)
                throw new FormatException("No tile defined");

            return tiles;
        }

        private static Dictionary<t_cellpos, t_cellcontent> loadRoom(string[] lines, Dictionary<int, string> tiles)
        {
            Dictionary<t_cellpos, t_cellcontent> cells = new Dictionary<t_cellpos, t_cellcontent>();
            uint startline = 0;
            uint endline;

            lines.First(x => x.Equals("#define Room"));
            while (startline < lines.Length && !lines[startline].Equals("#define Room"))
                startline++;
            startline++;

            endline = startline;
            while (endline < lines.Length && !lines[endline].Contains("#define"))
                endline++;

            if (endline == startline || startline >= lines.Length)
                throw new FormatException("No room defined");

            uint currentRow = 0;
            while (startline < endline)
            {
                string[] columns = lines[startline].Split(' ');
                uint currentColumn = 0;
                while (currentColumn < columns.Length)
                {
                    string[] values = columns[currentColumn].Split(':');
                    if (values.Length != 3)
                        throw new FormatException("Wrong room declaration format");
                    try
                    {
                        int textureId = int.Parse(values[0]);
                        int tileId = int.Parse(values[1]);
                        e_CellType cellType = (e_CellType)int.Parse(values[2]);
                        cells.Add(new t_cellpos(currentRow, currentColumn), new t_cellcontent(textureId, tiles[tileId], cellType));
                    }
                    catch (Exception e)
                    {
                        throw new FormatException("Wrong format in cell declaration", e);
                    }
                    currentColumn++;
                }

                startline++;
                currentRow++;
            }

            if (cells.Count <= 0)
                throw new FormatException("No room defined");

            return cells;
        }

        public static Dictionary<t_cellpos, t_cellcontent> ParseRoom(string path)
        {
            string[] lines = null;
            if (!System.IO.File.Exists(path))
                throw new System.IO.FileNotFoundException("File " + path + "not found!");
            lines = System.IO.File.ReadAllLines(path);

            lines = lines.Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
            lines.ToList().ForEach(x=>x.Trim());
            
            if (!checkHash(lines[0]))
                throw new FormatException("Wrong file hash (" + path + ")");


            Dictionary<int, string> tiles = loadTiles(lines);

            Dictionary<t_cellpos, t_cellcontent> room = loadRoom(lines, tiles);

            return room;
        }
    }
}
