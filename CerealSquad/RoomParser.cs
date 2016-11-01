using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    public static class RoomParser
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
            Wall = 0,
            Normal = 1
        }

        private static string FILE_HASHEDKEY = "58672f161bdbe31526fd8384909d4aa22b8fd91da8fce113ea083fbd6022e73e";

        private static bool checkHash(string line)
        {
            return line.Equals(FILE_HASHEDKEY);
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
                throw new ArgumentException("Wrong file hash (" + path + ")");

            int rowCount = lines.Length - 1;
            int columnCount = lines[1].Split(' ').Length;

            System.Diagnostics.Debug.WriteLine("Rows: " + rowCount + " - Columns: " + columnCount);

            //lines.ToList().ForEach(x => System.Diagnostics.Debug.WriteLine(x));

            Dictionary<t_cellpos, t_cellcontent> result = new Dictionary<t_cellpos, t_cellcontent>();

            uint currentRow = 0;
            while (currentRow < rowCount)
            {
                string[] columns = lines[currentRow + 1].Split(' ');
                uint currentColumn = 0;
                if (columns.Length != columnCount)
                    throw new ArgumentException("Wrong map format (" + path + ")");
                while (currentColumn < columnCount)
                {
                    string[] values = columns[currentColumn].Split(':');
                    result.Add(new t_cellpos(currentRow, currentColumn), new t_cellcontent(int.Parse(values[0]), "Assets/Tiles/TestTile.png", (e_CellType)int.Parse(values[2])));
                    currentColumn++;
                }
                currentRow++;
            }

            System.Diagnostics.Debug.WriteLine("Result: " + result.Count);

            return result;
        }
    }
}
