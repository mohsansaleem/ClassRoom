using System;
using System.Collections.Generic;

namespace PG.ClassRoom.Model.Data
{
    [Serializable]
    public class UserData
    {
        public string PlayerName;
        public long PlayerLevel;
        public long PlayerXP;

        public List<ModuleRemoteData> ModulesInGrid;
        
        public bool[,] Grid;
        
        public long Gold;
        public long Happiness;

        public List<FriendData> Friends;
    }
}