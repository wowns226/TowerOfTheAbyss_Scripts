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

using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectL
{
    public enum DBType
    {
        DynamoDB = 0,
        SteamCloud = 1,
    }

    public class DBManager : MonoSingleton<DBManager>
    {
        [SerializeField]
        private DBType dbType;

        private IDBClient dbClient;

        private void Awake()
        {
            SetDBClient();
        }

        private void SetDBClient()
        {
            switch (dbType)
            {
                case DBType.DynamoDB:
                    dbClient = new DynamoDB();
                    break;
                case DBType.SteamCloud:
                    dbClient = new SteamCloud();
                    break;
            }
        }

        public void SaveData<T>(T data, Action complete = null) where T : IDBData => dbClient.SaveData(data, complete);
        public Task<T> LoadData<T>(ulong id, Action<T> complete = null, Action fail = null) where T : IDBData => dbClient.LoadData(id, complete, fail);
    }
}
