using PG.ClassRoom.Model.Data;
using UnityEngine;

namespace PG.ClassRoom.Context.Bootstrap
{
    [CreateAssetMenu(menuName = "Game/Default GameState")]
    public class DefaultGameState : ScriptableObject
    {
        [SerializeField]
        public UserData User;
    }
}