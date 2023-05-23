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
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ProjectL
{
    public class AssetDownloader : Singleton<AssetDownloader>
    {
        private AsyncOperationHandle handle;

        private bool isDownloadComplete;
        public bool IsDownloadComplete => isDownloadComplete;
        
        private bool isLoadComplete;
        public bool IsLoadComplete => isLoadComplete;

        public float Percentage
        {
            get
            {
                if(handle.IsDone)
                {
                    return 100;
                }

                return handle.PercentComplete;
            }
        }

        public void Download()
        {
            Debug.Log("DownloadAssets. Start");

            isDownloadComplete = false;

            handle = Addressables.DownloadDependenciesAsync("LoadScene");

            handle.Completed += handler =>
            {
                Debug.Log("DownloadAssets. Complete");
                isDownloadComplete = true;
                Addressables.Release(handler);
            };
        }
        
        public void Load()
        {
            Debug.Log("LoadAssets. Start");

            isLoadComplete = false;

            handle = Addressables.LoadAssetAsync<GameObject>("LoadScene");

            handle.Completed += handler =>
            {
                Debug.Log("LoadAssets. Complete");
                isLoadComplete = true;
                Addressables.Release(handler);
            };
        }
    }
}
