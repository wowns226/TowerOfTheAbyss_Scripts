using System;
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

using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using UnityEngine;

namespace ProjectL
{
    public class DynamoDB : IDBClient
    {
        private DynamoDBContext context;
        private AmazonDynamoDBClient DBclient;
        private CognitoAWSCredentials credentials;
        private bool disposedValue;

        public DynamoDB()
        {
            AuthCognitoAWS();
        }

        private void AuthCognitoAWS()
        {
            credentials = new CognitoAWSCredentials("AWS_SERIAL_NUMBER", RegionEndpoint.APNortheast2);
            DBclient = new AmazonDynamoDBClient(credentials, RegionEndpoint.APNortheast2);
            context = new DynamoDBContext(DBclient);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                context.Dispose();
                DBclient.Dispose();
                credentials.Dispose();

                disposedValue = true;
            }
        }

        public async Task<T> LoadData<T>(ulong id, Action<T> complete = null, Action fail = null) where T : IDBData
        {
            var result = await context.LoadAsync<T>(id);

            if (result == null)
            {
                Debug.LogError("Data is Empty");
                fail?.Invoke();
                return result;
            }

            complete?.Invoke(result);

            return result;
        }

        public async void SaveData<T>(T data, Action complete = null) where T : IDBData
        {
            await context.SaveAsync(data);
            complete?.Invoke();
        }
    }
}