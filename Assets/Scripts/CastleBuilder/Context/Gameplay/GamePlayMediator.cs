using System;
using PG.CastleBuilder.DOTS;
using PG.CastleBuilder.Installer;
using PG.CastleBuilder.Model;
using PG.CastleBuilder.Model.Context;
using PG.CastleBuilder.Model.Data;
using PG.CastleBuilder.Model.Remote;
using PG.CastleBuilder.Views.Gameplay;
using PG.Core.FSM;
using PG.Core.Installer;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.CastleBuilder.Context.Gameplay
{
    public partial class GamePlayMediator : StateMachineMediator
    {
        [Inject] public ProjectContextInstaller.Settings _projectSettings;

        [Inject] private readonly GamePlayView _view;
        [Inject] private readonly GridController _gridController;
        [Inject] private readonly CameraController _cameraController;
        [Inject] private Camera _camera;

        [Inject] private readonly BootstrapModel _bootstrapModel;
        [Inject] private readonly GamePlayModel _gamePlayModel;
        [Inject] private readonly StaticDataModel _staticDataModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;

        [Inject] private ModuleRemoteDataModel.Factory _moduleFactory;
        
        [Inject] private readonly DOTS_Hub _dotsHub;
        
        // Events
        public event Action<ModuleRemoteDataModel> OnClickAtModule;
        public event Action<Vector3> OnClickInsideGrid;
        public event Action OnClickOutsideGrid;
        
        // Variables
        private int _currentStep;
        
        // Raycast stuff to handle the Grid Position & Movement.
        private int layer_mask = LayerMask.GetMask("Grid");

        public override void Initialize()
        {
            base.Initialize();

            StateBehaviours.Add((int)EGamePlayState.Load, new GamePlayStateLoad(this));
            StateBehaviours.Add((int)EGamePlayState.Regular, new GamePlayStateRegular(this));
            StateBehaviours.Add((int)EGamePlayState.PlaceModule, new GamePlayStatePlaceModule(this));
            StateBehaviours.Add((int)EGamePlayState.ModuleMenu, new GamePlayStateModuleMenu(this));
            StateBehaviours.Add((int)EGamePlayState.MoveModule, new GamePlayStateMoveModule(this));

            // Observing Remote Data.
            _remoteDataModel.PlayerName.Subscribe(name => _view.PlayerName.text = name).AddTo(Disposables);
            _remoteDataModel.PlayerLevel.Subscribe(level => _view.PlayerLevel.text = $"Level: {level}").AddTo(Disposables);
            _remoteDataModel.PlayerXPProgress.Subscribe(xp => _view.PlayerXP.value = xp).AddTo(Disposables);
            _remoteDataModel.HappinessProgress.Subscribe(happiness => _view.PlayerHappiness.value = happiness).AddTo(Disposables);
            _remoteDataModel.HappinessSprite.Subscribe(happiness => _view.HappinessIcon.sprite = happiness).AddTo(Disposables);
            _remoteDataModel.Gold.Subscribe(gold => _view.GoldText.text = gold.ToString()).AddTo(Disposables);

            InitEnvironment();
            HandleUIInput();

            OnGamePlayStateChanged(EGamePlayState.Load);
            
            _gamePlayModel.GamePlayState.Subscribe(OnGamePlayStateChanged).AddTo(Disposables);
            
            _dotsHub.Init();
            
            SignalBus.Subscribe<AttachModuleToPointerSignal>(signal =>
            {
                _gamePlayModel.ModuleToAttach.Value =
                    _staticDataModel.MetaData.Modules.Find(m => m.StaticId.Equals(signal.ModuleStaticId));
                _gamePlayModel.ChangeState(EGamePlayState.PlaceModule);
            });
        }

        private void InitEnvironment()
        {
            ChangeZoom(0);
            _cameraController.Position = new Vector3((_staticDataModel.MetaData.GridWidth * Constants.GridTileSize) / 2f, 0, (_staticDataModel.MetaData.GridHeight * Constants.GridTileSize) / 2f);
        }
        
        private void HandleUIInput()
        {
            _view.FriendsToggleButton.onClick.AsObservable().Subscribe(_ =>
            {
                _view.FriendsPanel.SetActive(!_view.FriendsPanel.activeInHierarchy);
                _view.FriendsToggleButtonText.text =
                    _view.FriendsPanel.activeInHierarchy ? "Hide Friends" : "Show Friends";
            }).AddTo(Disposables);

            _view.ZoomInButton.OnClickAsObservable().Subscribe(_ =>
            {
                ChangeZoom(1);
            }).AddTo(Disposables);
            
            _view.ZoomOutButton.OnClickAsObservable().Subscribe(_ =>
            {
                ChangeZoom(-1);
            }).AddTo(Disposables);
            
            _view.ShopButton.onClick.AsObservable().Subscribe(_ =>
            {
                SignalFactory.Create<LoadUnloadScenesSignal>().Load(new[] { Scenes.Shop }).Done
                (
                    () =>
                    {
                        Debug.Log("Loading Shop Finished.");
                    }
                );
                //SignalFactory.Create<AddNewModuleSignal>().AddModule(1);
            }).AddTo(Disposables);

            _view.ErrorPanelOkButton.OnClickAsObservable().Subscribe(_ =>
            {
                HideErrorPanel();
            }).AddTo(Disposables);
            
            _gridController.OnMouseDownAtGrid += OnMouseDownAtGrid;
            _gridController.OnMouseUpAtGrid += OnMouseUpAtGrid;
        }

        private void ChangeZoom(int step)
        {
            _currentStep = Mathf.Clamp(_currentStep + step, 0, _staticDataModel.MetaData.ZoomLevels);
            float stepZoom = (_staticDataModel.MetaData.MinZoomLevel - _staticDataModel.MetaData.MaxZoomLevel) /
                             _staticDataModel.MetaData.ZoomLevels;
            
            _cameraController.CurrentZoom = _staticDataModel.MetaData.MinZoomLevel - _currentStep * stepZoom;

            _view.ZoomOutButton.interactable = _currentStep != 0;
            _view.ZoomInButton.interactable = _currentStep != _staticDataModel.MetaData.ZoomLevels;
        }

        private void OnMouseDownAtGrid()
        {
            if (Utils.GetGridPosition(out var pos))
            {
                if (pos.x < 0 || pos.z < 0 || pos.x >= _staticDataModel.MetaData.GridWidth * Constants.GridTileSize ||
                    pos.z >= _staticDataModel.MetaData.GridHeight * Constants.GridTileSize)
                {
                    _gamePlayModel.SelectedModule.Value = null;
                    OnClickOutsideGrid?.Invoke();
                    return;
                }

                if (_remoteDataModel.UserData.Grid[(int) (pos.x / Constants.GridTileSize),
                    (int) (pos.z / Constants.GridTileSize)])
                {
                    ModuleRemoteDataModel model = null;

                    foreach (var moduleModel in _remoteDataModel.ModuleRemoteDatas.Values)
                    {
                        var modulePos = moduleModel.RemoteData.CurrentPosition;
                        if (pos.x >= modulePos.x && pos.x < modulePos.x + moduleModel.Data.Width * Constants.GridTileSize &&
                            pos.z >= modulePos.z && pos.z < modulePos.z + moduleModel.Data.Length * Constants.GridTileSize)
                        {
                            model = moduleModel;
                            break;
                        }
                    }
                    
                    _gamePlayModel.SelectedModule.Value = model;
                    
                    if (model != null)
                    {
                        OnClickAtModule?.Invoke(model);
                    }
                    else
                    {
                        Debug.LogError($"Unable to find any module at position: [{pos}]");
                    }
                }
                else
                {
                    _gamePlayModel.SelectedModule.Value = null;
                    OnClickInsideGrid?.Invoke(pos);
                }
            }
            else
            {
                Debug.LogError("Unable to get the Grid Position.");
            }
        }
            
        private void OnMouseUpAtGrid()
        {
            
        }
        
        private bool IsValidPosition(ModuleData data, Vector3 pos)
        {
            var grid = _remoteDataModel.UserData.Grid;
                
            int posi = (int)pos.x / Constants.GridTileSize;
            int posy = (int) pos.z / Constants.GridTileSize;

            if (posi < 0 || posi + data.Width > _staticDataModel.MetaData.GridWidth ||
                posy < 0 || posy + data.Length > _staticDataModel.MetaData.GridHeight)
            {
                return false;
            }
                
            for (int i = posi; i < posi + data.Width; i++)
            {
                for (int r = posy; r < posy + data.Length; r++)
                {
                    if (grid[i, r])
                        return false;
                }
            }
                
            return true;
        }
        
        private bool IsValidPosition(ModuleRemoteDataModel model, Vector3 pos)
        {
            var grid = _remoteDataModel.UserData.Grid;
                
            int oldi = (int)model.RemoteData.CurrentPosition.x / Constants.GridTileSize;
            int oldy = (int)model.RemoteData.CurrentPosition.z / Constants.GridTileSize;
                
            int posi = (int)pos.x / Constants.GridTileSize;
            int posy = (int) pos.z / Constants.GridTileSize;

            if (posi < 0 || posi + model.Data.Width > _staticDataModel.MetaData.GridWidth ||
                posy < 0 || posy + model.Data.Length > _staticDataModel.MetaData.GridHeight)
            {
                return false;
            }
                
            for (int i = posi; i < posi + model.Data.Width; i++)
            {
                for (int r = posy; r < posy + model.Data.Length; r++)
                {
                    if (grid[i, r] && 
                        !((i >= oldi && r >= oldy) && 
                          (i < oldi + model.Data.Width && r < oldy + model.Data.Length))
                    )
                        return false;
                }
            }
                
            return true;
        }
        
        private void UpdatePosition(Transform view, ModuleRemoteDataModel model, Vector3 pos)
        {
            var grid = _remoteDataModel.UserData.Grid;
            var old = view.transform.position;
            int posi = (int)old.x / Constants.GridTileSize;
            int posy = (int)old.z / Constants.GridTileSize;

            for (int i = posi; i < posi + model.Data.Width; i++)
            {
                for (int r = posy; r < posy + model.Data.Length; r++)
                {
                    grid[i, r] = false;
                }
            }
                
            posi = (int)pos.x / Constants.GridTileSize;
            posy = (int)pos.z / Constants.GridTileSize;
                
            for (int i = posi; i < posi + model.Data.Width; i++)
            {
                for (int r = posy; r < posy + model.Data.Length; r++)
                {
                    grid[i, r] = true;
                }
            }
                
            // TODO: MS: Use Reactive Property and Set from Command.
            model.RemoteData.CurrentPosition = pos;
            model.CurrentPosition.Value = pos;
                
            // Set on Reactive Change.
            //view.transform.position = pos;
        }

        private void ShowErrorPanel(string message)
        {
            _view.ErrorMessageText.text = message;
            _view.ErrorPanel.SetActive(true);
        }
        
        private void HideErrorPanel()
        {
            _view.ErrorPanel.SetActive(false);
        }
        
        private void OnGamePlayStateChanged(EGamePlayState gamePlayState)
        {
            GoToState((int)gamePlayState);
        }

        public override void Dispose()
        {
            base.Dispose();
            _dotsHub.Cleanup();
            
            _gridController.OnMouseDownAtGrid -= OnMouseDownAtGrid;
            _gridController.OnMouseUpAtGrid -= OnMouseUpAtGrid;
        }
    }
}