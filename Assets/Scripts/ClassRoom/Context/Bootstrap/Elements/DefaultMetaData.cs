using PG.ClassRoom.Model.Data;
using UnityEngine;

namespace PG.ClassRoom.Context.Bootstrap
{
    [CreateAssetMenu(menuName = "Game/MetaData")]
    public class DefaultMetaData : ScriptableObject
    {
        [SerializeField]
        public MetaData Meta;
    }
}