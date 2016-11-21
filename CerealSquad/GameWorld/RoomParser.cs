using System;
using CerealSquad.Global;
using System.Collections.Generic;
using System.Linq;

namespace CerealSquad.GameWorld
{
    static class RoomParser
    {
        public class s_room
        {
            public s_room(Dictionary<s_Pos<uint>, t_cellcontent> cells, List<s_crate> crates, List<s_ennemy> ennemies)
            {
                Cells = cells;
                Crates = crates;
                Ennemies = ennemies;
            }

            public Dictionary<s_Pos<uint>, t_cellcontent> Cells;
            public List<s_crate> Crates;
            public List<s_ennemy> Ennemies;
        }

        public class s_crate
        {
            public List<e_TrapType> Types = new List<e_TrapType>();
            public List<s_Pos<int>> Pos = new List<s_Pos<int>>();
        }

        public class s_ennemy
        {
            public List<e_EnnemyType> Types = new List<e_EnnemyType>();
            public List<s_Pos<int>> Pos = new List<s_Pos<int>>();
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
            Wall = 1,
            Void = 2
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

            lines.First(x => x.Equals("#define Tiles")); // Mandatory Define
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

        private static List<s_ennemy> loadEnnemies(string[] lines)
        {
            List<s_ennemy> ennemies = new List<s_ennemy>();
            uint startline = 0;
            uint endline;

            while (startline < lines.Length && !lines[startline].Equals("#define Ennemies"))
                startline++;
            startline++;

            endline = startline;
            while (endline < lines.Length && !lines[endline].Contains("#define"))
                endline++;

            if (endline == startline || startline >= lines.Length)
                return ennemies;

            while (startline < endline)
            {
                s_ennemy ennemy = new s_ennemy();
                string[] columns = lines[startline].Split('|');
                if (columns.Length != 2)
                    throw new FormatException("Wrong ennemy declaration");
                string[] types = columns[0].Split(';');
                foreach (string type in types)
                {
                    ennemy.Types.Add((e_EnnemyType)(int.Parse(type)));
                }
                string[] positions = columns[1].Split(';');
                foreach (string position in positions)
                {
                    string[] pos = position.Split(':');
                    if (pos.Length != 2)
                        throw new FormatException("Wrong position definition for ennemy declaration");
                    ennemy.Pos.Add(new s_Pos<int>(int.Parse(pos[0]), int.Parse(pos[1])));
                }
                ennemies.Add(ennemy);
                startline++;
            }

            if (ennemies.Count <= 0)
                throw new FormatException("No ennemy defined");

            return ennemies;
        }

        private static List<s_crate> loadCrates(string[] lines)
        {
            List<s_crate> crates = new List<s_crate>();
            uint startline = 0;
            uint endline;

            while (startline < lines.Length && !lines[startline].Equals("#define Crates"))
                startline++;
            startline++;

            endline = startline;
            while (endline < lines.Length && !lines[endline].Contains("#define"))
                endline++;

            if (endline == startline || startline >= lines.Length)
                return crates;

            while (startline < endline)
            {
                s_crate crate = new s_crate();
                string[] columns = lines[startline].Split('|');
                if (columns.Length != 2)
                    throw new FormatException("Wrong crate declaration");
                string[] types = columns[0].Split(';');
                foreach (string type in types)
                {
                    crate.Types.Add((e_TrapType)(int.Parse(type)));
                }
                string[] positions = columns[1].Split(';');
                foreach (string position in positions)
                {
                    string[] pos = position.Split(':');
                    if (pos.Length != 2)
                        throw new FormatException("Wrong position definition for crate declaration");
                    crate.Pos.Add(new s_Pos<int>(int.Parse(pos[0]), int.Parse(pos[1])));
                }
                crates.Add(crate);
                startline++;
            }

            if (crates.Count <= 0)
                throw new FormatException("No crate defined");

            return crates;
        }

        private static Dictionary<s_Pos<uint>, t_cellcontent> loadRoom(string[] lines, Dictionary<int, string> tiles)
        {
            Dictionary<s_Pos<uint>, t_cellcontent> cells = new Dictionary<s_Pos<uint>, t_cellcontent>();
            uint startline = 0;
            uint endline;

            lines.First(x => x.Equals("#define Room")); // Mandatory define
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
                        cells.Add(new s_Pos<uint>(currentColumn, currentRow), new t_cellcontent(textureId, tiles[tileId], cellType));
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

        public static s_room ParseRoom(string path)
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

            Dictionary<s_Pos<uint>, t_cellcontent> cells = loadRoom(lines, tiles);
            List<s_crate> traps = loadCrates(lines);
            List<s_ennemy> ennemies = loadEnnemies(lines);

            return new s_room(cells, traps, ennemies);
        }
    }
}
