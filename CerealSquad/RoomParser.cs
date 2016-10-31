using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    public static class RoomParser
    {
        public struct t_cellpos
        {
            uint Row;
            uint Column;
        }

        public struct t_cellcontent
        {
            int Texture;
            string TexturePath;
            e_CellType Type;
        }

        public enum e_CellType
        {
            Normal,
            Wall
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
                throw new System.ArgumentException("Wrong file hash (" + path + ")");

            int rowCount = lines.Length - 1;
            int columnCount = lines[1].Split(' ').Length;

            Dictionary<t_cellpos, t_cellcontent> result = new Dictionary<t_cellpos, t_cellcontent>();

            uint currentRow = 0;
            while (currentRow < rowCount)
            {
                string[] columns = lines[currentRow].Split(' ');
                uint currentColumn = 0;
                //while (currentColumn < )
                currentRow++;
            }

            System.Diagnostics.Debug.WriteLine("Rows: " + rowCount + " - Columns: " + columnCount);

            return null;
        }
    }
}
