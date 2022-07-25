using System;

namespace PG.ClassRoom.Model.Data
{
    [Serializable]
    public class RoomData
    {
        public string RoomId;
        public string SpriteName;
        public string RoomName { get; set; }
        public string MembersCount { get; set; }
    }
}