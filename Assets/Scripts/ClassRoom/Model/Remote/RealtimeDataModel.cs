using System.Linq;
using PG.ClassRoom.Installer;
using PG.ClassRoom.Model.Data;
using Photon.Realtime;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.ClassRoom.Model.Remote
{
    public class RealtimeDataModel
    {
        [Inject] private ProjectContextInstaller.Settings _settings;

        public readonly ReactiveDictionary<string, RoomInfo> RoomsRemoteDatas;

        public RealtimeDataModel()
        {
            RoomsRemoteDatas = new ReactiveDictionary<string, RoomInfo>();
        }

        public void ClearRooms()
        {
            RoomsRemoteDatas.Clear();
        }
    }
}

