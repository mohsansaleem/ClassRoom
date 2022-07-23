using PG.CastleBuilder.Installer;
using PG.CastleBuilder.Model.Data;
using PG.Core.Context;
using PG.Core.Installer;
using UnityEngine.UI;
using Zenject;

namespace PG.CastleBuilder.Context.Shop.Elements.Friend
{
    public class ModulesListItem : ListViewItem<ModuleData>
    {
        public Image ModulePicture;
        public Text ModuleName;
        public Text ModuleCost;
        public Button ItemButton;

        [Inject] private ProjectContextInstaller.Settings _settings;
        [Inject] private readonly Signal.Factory _signalFactory;
        
        [Inject]
        public void Construct()
        {
            ItemButton.onClick.AddListener(() =>
            {
                _signalFactory.Create<AttachModuleToPointerSignal>().AttachModule(Data.StaticId);
                _signalFactory.Create<UnloadSceneSignal>().Unload(Scenes.Shop);
            });
        }

        public override void RefreshData(ModuleData model)
        {
            base.RefreshData(model);

            if (model != null)
            {
                ModulePicture.sprite = _settings.GetSprite(model.SpriteName);
                ModuleName.text = model.ModuleName;
                ModuleCost.text = $"{model.CostGold}";
            }
        }

        public class Pool : MonoMemoryPool<ModuleData, ModulesListItem>
        {
            protected override void Reinitialize(ModuleData data, ModulesListItem item)
            {
                base.Reinitialize(data, item);
                item.RefreshData(data);
            }

            protected override void OnSpawned(ModulesListItem item)
            {
                base.OnSpawned(item);
            }
        }
    }
}
