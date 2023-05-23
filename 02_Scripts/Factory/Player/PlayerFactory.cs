// Copyright (C) <2023>  
//     Authors : Shin YongUk <dyddyddnr@naver.com>
//     Lim jaejun <wowns226@naver.com>
//  
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//  
//     You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>

using UnityEngine;

namespace ProjectL
{
    public enum PlayerType
    {
        Ally = 0,
        Enemy = 1,
    }

    public class PlayerFactory : Singleton<PlayerFactory>
    {
        public void CreatePlayerGroup()
        {
            Debug.Log("PlayerFactory.CreatePlayerGroup()");
            ObjectPoolManager.Instance.New(nameof(PlayerGroup), PlayerManager.Instance.transform, playerGroup => { D.SelfPlayerGroup = playerGroup.GetComponent<PlayerGroup>(); });
        }

        public void Create(PlayerType type, ulong userID)
        {
            Debug.Log($"PlayerFactory.Create(), type : {type.ToString()}, userID : {userID.ToString()}");

            switch (type)
            {
                case PlayerType.Ally:
                    ObjectPoolManager.Instance.New(nameof(Player), D.SelfPlayerGroup.transform, obj => { InitAllyPlayer(obj.GetComponent<Player>(), userID); });
                    break;
                case PlayerType.Enemy:
                    ObjectPoolManager.Instance.New(nameof(Player), D.SelfPlayerGroup.transform, obj => { InitEnemyPlayer(obj.GetComponent<Player>(), userID); });
                    break;
            }
        }

        private void InitAllyPlayer(Player player, ulong userID)
        {
            player.Init(userID, PlayerType.Ally);
            D.SelfPlayer = player;
            D.SelfPlayerGroup.allyPlayers.Add(player);
        }

        private void InitEnemyPlayer(Player player, ulong userID)
        {
            player.Init(userID, PlayerType.Enemy);
            D.SelfEnemyPlayer = player;
            D.SelfPlayerGroup.enemyPlayers.Add(player);
        }
    }
}
