
using System.IO;
using UnityEngine;

namespace PG.ClassRoom
{
    public static class Constants
    {
        public static string DataFolder => Path.Combine(Application.persistentDataPath, "Data"); 
        public static string MetaDataFile => Path.Combine(DataFolder,"MetaData.json");
        public static string GameStateFile => Path.Combine(DataFolder,"GameState.json");
        
        public static int RoomsListSize = 4;

        public const int GridTileSize = 5;
        public const int FriendsListSize = 7;
        public const int ModulesListSize = 4;
    }
}
