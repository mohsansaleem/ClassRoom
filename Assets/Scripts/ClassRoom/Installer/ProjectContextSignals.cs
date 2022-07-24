﻿using PG.Core.Installer;
using RSG;

namespace PG.ClassRoom.Installer
{
    public class AttachModuleToPointerSignal : Signal
    {
        public uint ModuleStaticId;

        public Promise AttachModule(uint moduleStaticId)
        {
            ModuleStaticId = moduleStaticId;
            this.Fire();

            return OnComplete;
        }
    }
}