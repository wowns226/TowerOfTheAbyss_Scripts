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
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace ProjectL
{
    public enum LanguageType
    {
        English = 0,
        Korean = 1,
    }
    
    public static class Localization
    {
        public static CustomAction onLocalizeChanged = new CustomAction();

        public static string GetLocalizedString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }
            
            var locale = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("New Table", key);
            string localizedString = string.Empty;

            if(locale.IsDone)
            {
                localizedString = locale.Result;
            }
            else
            {
               locale.Completed += locale => localizedString = locale.Result;
            }

            if(string.IsNullOrEmpty(localizedString))
            {
                Debug.LogError($"{key} do not exist in Localization StringTable.\n Please Update Localized String in Localization StringTable");
            }

            return localizedString;
        }

        public static void SetLanguage(int index)
        {
            ChangeLanguage(index);
        }

        private static async void ChangeLanguage(int index)
        {
            await LocalizationSettings.InitializationOperation.Task;

            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

            onLocalizeChanged?.Invoke();

            Debug.Log($"Localization.Instance.ChangeLanguage : {LocalizationSettings.AvailableLocales.Locales[index]}");
        }
    }
}
