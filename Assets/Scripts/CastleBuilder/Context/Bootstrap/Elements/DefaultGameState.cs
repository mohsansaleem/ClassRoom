using PG.CastleBuilder.Model.Data;
using UnityEngine;

namespace PG.CastleBuilder.Context.Bootstrap
{
    [CreateAssetMenu(menuName = "Game/Default GameState")]
    public class DefaultGameState : ScriptableObject
    {
        [SerializeField]
        public UserData User;
    }
}