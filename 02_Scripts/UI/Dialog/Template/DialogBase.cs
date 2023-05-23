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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectL
{
    public abstract class DialogBase : DataContainer
    {
        [SerializeField]
        private bool isModal;
        public bool IsModal => isModal;
        
        [SerializeField]
        private bool isBlockEscape;
        public bool IsBlockEscape => isBlockEscape;
        
        private Canvas canvas;

        private List<DialogBase> subModals = new List<DialogBase>();

        private bool isShow;
        public bool IsShow => isShow;

        private Coroutine closeCoroutine;
        private float autoClose;
        public float AutoClose
        {
            get => autoClose;
            set
            {
                autoClose = value;

                if(closeCoroutine != null)
                {
                    StopCoroutine(closeCoroutine);
                    closeCoroutine = null;
                }

                closeCoroutine = StartCoroutine(AutoCloseAsync());
            }
        }

        protected override void Awake()
        {
            base.Awake();
            canvas = GetComponentInChildren<Canvas>();
        }

        protected virtual void Start() { }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        protected virtual void OnDestroy() { }

        public virtual void OpenDialog() => Show();
        public virtual void CloseDialog() => Hide();

        protected void Show()
        {
            if (DialogManager.Instance.TopDialog && DialogManager.Instance.TopDialog.name.Equals(name) && isModal == false)
            {
                Debug.Log($"DialogBase.Show() try to open same dialog, DialogName : {name}");
                ObjectPoolManager.Instance.Return(gameObject);
                return;
            }

            if (isModal && DialogManager.Instance.TopDialog != null)
            {
                DialogManager.Instance.TopDialog.subModals.Add(this);
            }

            canvas.sortingOrder = DialogManager.Instance.DialogCount;
            isShow = true;
            DialogManager.Instance.OpenDialogs.Add(this);
        }

        protected void Hide()
        {
            isShow = false;

            subModals.ForEach(dialog =>
            {
                if (dialog.IsShow)
                {
                    dialog.CloseDialog();
                }
            });
            subModals.Clear();

            DialogManager.Instance.OpenDialogs.Remove(this);
            ObjectPoolManager.Instance.Return(gameObject);
        }

        private IEnumerator AutoCloseAsync()
        {
            Debug.Log($"DialogBase.AutoCloseAsync() Start, Dlg : {name}, Time : {AutoClose}");
            yield return new WaitForSeconds(AutoClose);

            Debug.Log($"DialogBase.AutoCloseAsync() End -> Close, Dlg : {name}, Time : {AutoClose}");
            CloseDialog();
        }

    }
}
