using System;
using Newtonsoft.Json;
using UnityEngine;

namespace PG.ClassRoom.Model.Data
{
    [Serializable]
    public class GridEntityRemoteData
    {
        public System.Numerics.Vector3 Position;

        [JsonIgnore]
        public int Width => GridEntityData.Width;
        [JsonIgnore]
        public int Length => GridEntityData.Length;
        
        [JsonIgnore] public GridEntityData GridEntityData { get; set; }
        
        [JsonIgnore]
        public Vector3 CurrentPosition
        {
            get => new Vector3(Position.X, Position.Y, Position.Z);
            set => Position = new System.Numerics.Vector3(value.x, value.y, value.z);
        }
    }
}