using System;
using System.Collections.Generic;

namespace PG.CastleBuilder.Model.Data
{
    [Serializable]
    public class MetaData
    {
        // Modules in the Game.
        public List<ModuleData> Modules;

        public int ZoomLevels;
        public float MinZoomLevel;
        public float MaxZoomLevel;

        public int GridWidth;
        public int GridHeight;

        // Each entry mention what XP is required to level up.
        // Zero-based indexing. Level - 1 => XP Required for Level
        public int[] PlayerLevels;
    }
}