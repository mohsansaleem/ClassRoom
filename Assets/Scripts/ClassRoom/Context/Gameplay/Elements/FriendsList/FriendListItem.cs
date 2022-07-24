using PG.ClassRoom.Installer;
using PG.ClassRoom.Model.Data;
using PG.Core.Context;
using UnityEngine.UI;
using Zenject;

namespace PG.ClassRoom.Context.Gameplay
{
    public class FriendListItem : ListViewItem<FriendData>
    {
        public Image FriendPicture;
        public Text FriendName;
        public Text FriendLevel;

        [Inject] private ProjectContextInstaller.Settings _settings;
        
        [Inject]
        public void Construct()
        {
        }

        public override void RefreshData(FriendData model)
        {
            base.RefreshData(model);

            if (model != null)
            {
                FriendPicture.sprite = _settings.GetSprite(model.SpriteName);
                FriendName.text = model.Name;
                FriendLevel.text = $"Level {model.Level}";
            }
        }

        public class Pool : MonoMemoryPool<FriendData, FriendListItem>
        {
            protected override void Reinitialize(FriendData data, FriendListItem item)
            {
                base.Reinitialize(data, item);
                item.RefreshData(data);
            }

            protected override void OnSpawned(FriendListItem item)
            {
                base.OnSpawned(item);
            }
        }
    }
}
