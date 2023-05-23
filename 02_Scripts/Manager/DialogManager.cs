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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectL
{
    public class DialogManager : MonoSingleton<DialogManager>
    {
        [Serializable]
        private class DialogItem
        {
            public DialogBase dialog;
        }

        [SerializeField]
        private List<DialogItem> dialogs;
        public List<string> DialogsName => dialogs.ConvertAll(dialog => dialog.dialog.name);

        private List<DialogBase> openDialogs = new List<DialogBase>();

        public List<DialogBase> OpenDialogs => openDialogs;
        public DialogBase TopDialog => openDialogs.Count == 0 ? null : openDialogs.LastOrDefault(dialog => dialog.IsModal == false);
        public int DialogCount => openDialogs.Count;

        public void OpenDialog(string name, Action<DialogBase> onComplete = null) { OpenDialog<DialogBase>(name, onComplete); }
        public void OpenDialog<T>(string name, Action<T> onComplete = null) where T : DialogBase
        {
            Debug.Log($"DialogManager.OpenDialog(), DialogName : {name}");

            StartCoroutine(CreateDialog(name, onComplete));
        }

        private IEnumerator CreateDialog<T>(string name, Action<T> onComplete) where T : DialogBase
        {
            var dialogItem = dialogs.Find(dialogItem => dialogItem.dialog.transform.name.Equals(name));

            if (dialogItem == null)
                yield break;

            var process = ObjectPoolManager.Instance.NewAsync(name, onComplete: obj =>
           {
               onComplete?.Invoke(obj.GetComponent<T>());
               obj.GetComponent<T>().OpenDialog();
           });

            while (process.MoveNext())
            {
                yield return process.Current;
            }
        }

        public IEnumerator CacheAllDialog()
        {
            var dialogItems = dialogs.ConvertAll(item => item.dialog.transform.name);

            if (dialogItems.Count == 0)
                yield break;

            foreach (var dialog in dialogItems)
            {
                var process = ObjectPoolManager.Instance.NewAsync(dialog, activeImmediately: false, onComplete: obj =>
                    {
                        ObjectPoolManager.Instance.Return(obj);
                    });

                while (process.MoveNext())
                {
                    yield return process.Current;
                }
            }
        }

        public void CloseAllDialogs() => OpenDialogs.ToList().ForEach(dialog => dialog.CloseDialog());

    }
}
