using System;
using System.Collections.Generic;
using System.Linq;
using PG.ClassRoom.Model.Context;
using PG.ClassRoom.Model.Data;
using PG.ClassRoom.Model.Remote;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using UnityEngine;

namespace PG.ClassRoom.Context.Lobby
{
    public partial class LobbyMediator
    {
        private class LobbyStateRoomsJoinLobby : LobbyState
        {
            public LobbyStateRoomsJoinLobby(Lobby.LobbyMediator mediator) : base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();
                View.ShowConnect();
            }

            public override void OnStateExit()
            {
                View.HideConnect();
                base.OnStateExit();
            }
        }
    }
}