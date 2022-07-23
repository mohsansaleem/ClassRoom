using PG.CastleBuilder.Model.Data;
using UnityEngine;

namespace PG.CastleBuilder.Context.Bootstrap
{
    [CreateAssetMenu(menuName = "Game/MetaData")]
    public class DefaultMetaData : ScriptableObject
    {
        [SerializeField]
        public MetaData Meta;
    }
}