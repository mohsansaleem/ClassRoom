using PG.ClassRoom.Installer;
using PG.ClassRoom.Model.Data;
using PG.ClassRoom.Service;
using PG.Core.Context;
using PG.Core.Installer;
using UnityEngine.UI;
using Zenject;

namespace PG.ClassRoom.Context.Lobby.Elements.Friend
{
    public class RoomsListItem : ListViewItem<RoomData>
    {
        public Image RoomPicture;
        public Text RoomName;
        public Text MembersCount;
        public Button ItemButton;

        [Inject] private ProjectContextInstaller.Settings _settings;
        [Inject] private readonly Signal.Factory _signalFactory;
        [Inject] private readonly RealtimeHub _realtimeHub;
        
        [Inject]
        public void Construct()
        {
            ItemButton.onClick.AddListener(() =>
            {
                _realtimeHub.JoinRoom(Data.RoomName);
                // TODO: Go to room
                //_signalFactory.Create<AttachRoomToPointerSignal>().AttachRoom(Data.StaticId);
                _signalFactory.Create<UnloadSceneSignal>().Unload(Scenes.Lobby);
            });
        }

        public override void RefreshData(RoomData model)
        {
            base.RefreshData(model);

            if (model != null)
            {
                RoomPicture.sprite = _settings.GetSprite(model.SpriteName);
                RoomName.text = model.RoomName;
                MembersCount.text = model.MembersCount;
            }
        }

        public class Pool : MonoMemoryPool<RoomData, RoomsListItem>
        {
            protected override void Reinitialize(RoomData data, RoomsListItem item)
            {
                base.Reinitialize(data, item);
                item.RefreshData(data);
            }

            protected override void OnSpawned(RoomsListItem item)
            {
                base.OnSpawned(item);
            }
        }
    }
}
