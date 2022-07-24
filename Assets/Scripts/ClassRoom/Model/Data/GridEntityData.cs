﻿using System;
using UnityEngine;

namespace PG.ClassRoom.Model.Data
{
    [Serializable]
    public class GridEntityData
    {
        [Range(1, 10)]
        public int Width;
        [Range(1, 10)]
        public int Length;
    }
}