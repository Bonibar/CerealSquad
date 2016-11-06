using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.GameWorld
{
    static class WorldParser
    {
        public class t_roompos
        {
            private t_roompos() { }

            public t_roompos(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; }
            public int Y { get; }
        }

        public class t_roomcontent
        {
            private t_roomcontent() { }
            public t_roomcontent(string roomPath, e_RoomType type)
            {
                RoomPath = roomPath;
                Type = type;
            }

            public string RoomPath { get; }
            public e_RoomType Type { get; }
        }

        public enum e_RoomType
        {
            Fighting = 0,
            Transition = 1
        }

        private static string FILE_HASHEDKEY = "6bdec4937a3e3762c6ad9c59038aac089742cacbf332b94bcdc779d39ddf8e53";

        private static bool checkHash(string line)
        {
            return line.Equals(FILE_HASHEDKEY);
        }

        private static Dictionary<t_roompos, t_roomcontent> loadRooms(string[] lines)
        {
            Dictionary<t_roompos, t_roomcontent> rooms = new Dictionary<t_roompos, t_roomcontent>();
            uint startline = 0;
            uint endline;

            lines.First(x => x.Equals("#define Rooms"));
            while (startline < lines.Length && !lines[startline].Equals("#define Rooms"))
                startline++;
            startline++;

            endline = startline;
            while (endline < lines.Length && !lines[endline].Contains("#define"))
                endline++;

            if (endline == startline || startline >= lines.Length)
                throw new FormatException("No world defined");

            uint currentRow = 0;
            while (startline < endline)
            {
                string[] values = lines[startline].Split('|');
                if (values.Length != 3)
                    throw new FormatException("Wrong world declaration format");
                try
                {
                    string[] positions = values[0].Split(':');
                    int posX = int.Parse(positions[0]);
                    int posY = int.Parse(positions[1]);
                    string roomPath = values[1];
                    e_RoomType roomType = (e_RoomType)int.Parse(values[2]);
                    rooms.Add(new t_roompos(posX, posY), new t_roomcontent(roomPath, roomType));
                }
                catch (Exception e)
                {
                    throw new FormatException("Wrong format in room declaration", e);
                }

                startline++;
                currentRow++;
            }

            if (rooms.Count <= 0)
                throw new FormatException("No world defined");

            return rooms;
        }

        public static Dictionary<t_roompos, t_roomcontent> ParseWorld(string path)
        {
            string[] lines = null;
            if (!System.IO.File.Exists(path))
                throw new System.IO.FileNotFoundException("File " + path + " not found!");
            lines = System.IO.File.ReadAllLines(path);

            lines = lines.Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
            lines.ToList().ForEach(x => x.Trim());

            if (!checkHash(lines[0]))
                throw new FormatException("Wrong file hash (" + path + ")");

            Dictionary<t_roompos, t_roomcontent> world = loadRooms(lines);

            return world;
        }
    }
}
