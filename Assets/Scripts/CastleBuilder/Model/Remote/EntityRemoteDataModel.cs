using UniRx;
using UnityEngine;

namespace PG.CastleBuilder.Model.Remote
{
    public class EntityRemoteDataModel
    {
        public ReactiveProperty<Vector3> CurrentPosition;

        public EntityRemoteDataModel()
        {
            CurrentPosition = new ReactiveProperty<Vector3>();
        }
    }
}