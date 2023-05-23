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
using UnityEngine.UI;

namespace ProjectL
{
    public class LoadInfoUI : LobbyUIBase
    {
        [SerializeField]
        private Image fillImage;

        [DataObservable]
        public string Percent
        {
            get
            {
                if (AssetDownloader.Instance == null)
                {
                    return string.Empty;
                }

                fillImage.fillAmount = AssetDownloader.Instance.Percentage;
                return $"{AssetDownloader.Instance.Percentage: 0.0} %";
            }
        }

        private void Update()
        {
            this.NotifyObserver();
        }
    }
}
