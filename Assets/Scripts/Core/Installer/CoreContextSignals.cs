using Zenject;
using System;
using RSG;

namespace PG.Core.Installer
{
    public class Signal
    {
        [Inject] public readonly SignalBus SignalBus;

        public readonly Promise OnComplete;

        protected Signal()
        {
            OnComplete = new Promise();
        }

        public class Factory : PlaceholderFactory<Signal>
        {
            private readonly DiContainer _container;

            public Factory(DiContainer container)
            {
                _container = container;
            }

            public T Create<T>() where T : Signal
            {
                return _container.Instantiate<T>();
            }

            public override Signal Create()
            {
                throw new Exception("Use Generic Create method.");
            }
        }
    }
    
    public class RequestStateChangeSignal : Signal
    {
        public int stateType;
    }


    public class LoadSceneSignal : Signal
    {
        public string Scene;

        public IPromise Load(string scene)
        {
            Scene = scene;
            this.Fire();

            return OnComplete;
        }
    }

    public class LoadUnloadScenesSignal : Signal
    {
        public string[] LoadScenes;
        public string[] UnloadScenes;

        public IPromise Load(string[] loadScenes)
        {
            LoadScenes = loadScenes;
            UnloadScenes = null;
            this.Fire();

            return OnComplete;
        }

        public IPromise Load(string loadScene)
        {
            LoadScenes = new[] {loadScene};
            UnloadScenes = null;
            this.Fire();

            return OnComplete;
        }

        public IPromise Unload(string[] unloadScenes)
        {
            LoadScenes = null;
            UnloadScenes = unloadScenes;
            this.Fire();

            return OnComplete;
        }

        public IPromise Unload(string unloadScene)
        {
            LoadScenes = null;
            UnloadScenes = new[] {unloadScene};
            this.Fire();

            return OnComplete;
        }

        public IPromise LoadUnload(string[] loadScenes, string[] unloadScenes)
        {
            LoadScenes = loadScenes;
            UnloadScenes = unloadScenes;
            this.Fire();

            return OnComplete;
        }

        public IPromise LoadUnload(string loadScene, string unloadScene)
        {
            LoadScenes = new[] {loadScene};
            UnloadScenes = new[] {unloadScene};
            this.Fire();

            return OnComplete;
        }
    }

    public class UnloadSceneSignal : Signal
    {
        public string Scene;

        public IPromise Unload(string scene)
        {
            Scene = scene;
            this.Fire();

            return OnComplete;
        }
    }

    public class UnloadAllScenesExceptSignal : Signal
    {
        public string Scene;

        public IPromise UnloadAllExcept(string scene)
        {
            Scene = scene;
            this.Fire();

            return OnComplete;
        }
    }
}