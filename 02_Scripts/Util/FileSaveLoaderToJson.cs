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
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace ProjectL
{
    public static class FileSaveLoaderToJson
    {
        private static string dataPath = Application.dataPath;

        public static bool Save<T>(object data)
        {
            Debug.Log("FileSaveLoaderToJson.Save");
            var jsonString = JsonConvert.SerializeObject(data);
            
            var cipherText = Convert.ToBase64String(AesEncryptor.Encrypt(jsonString));

            var path = Path.Combine(dataPath, typeof(T).Name, $"{typeof(T).Name}.txt");
            var directoryInfo = new FileInfo(path).Directory;

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            File.WriteAllText(path, cipherText);

            return true;
        }

        public static T Load<T>()
        {
            Debug.Log("FileSaveLoaderToJson.Load");

            var path = Path.Combine(dataPath, typeof(T).Name, $"{typeof(T).Name}.txt");

            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                Debug.Log("FileSaveLoaderToJson.Load false, fileInfo.Exists is not exist");
                return default(T);
            }
            
            var cipherText = Convert.FromBase64String(File.ReadAllText(path));
            var jsonString = AesEncryptor.Decrypt(cipherText);

            if (jsonString == null)
            {
                Debug.Log("FileSaveLoaderToJson.Load false, json == null");
                return default(T);
            }
            
            T result = JsonConvert.DeserializeObject<T>(jsonString);
            
            return result;
        }

    }
}
