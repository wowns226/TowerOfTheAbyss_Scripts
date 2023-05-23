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
using Michsky.UI.Dark;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ProjectL
{
    public class DlgMessageBox : DialogBase
    {
        private CustomAction onClosed = new CustomAction();

        [SerializeField]
        private Button oKButton;
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private Animator panelAnimator;
        [SerializeField]
        private UIDissolveEffect uIDissolveEffect;

        private string title;
        [DataObservable]
        public string Title
        {
            get => title;
            set
            {
                title = value;
                this.NotifyObserver("Title");
            }
        }

        private string content;
        [DataObservable]
        public string Content
        {
            get => content;
            set
            {
                content = value;
                this.NotifyObserver("Content");
            }
        }

        private bool isShowOKButton = true;
        [DataObservable]
        public bool IsShowOKButton
        {
            get => isShowOKButton;
            set
            {
                isShowOKButton = value;
                this.NotifyObserver("IsShowOKButton");
            }
        }

        private bool isShowCancelButton = true;
        [DataObservable]
        public bool IsShowCancelButton
        {
            get => isShowCancelButton;
            set
            {
                isShowCancelButton = value;
                this.NotifyObserver("IsShowCancelButton");
            }
        }

        public override void OpenDialog()
        {
            AddOKEvent(() => Clear());
            AddOKEvent(() => base.CloseDialog());
            base.OpenDialog();

            panelAnimator.Play("In");
            uIDissolveEffect.location = 1;
            uIDissolveEffect.DissolveIn();
        }

        public override void CloseDialog()
        {
            onClosed.Invoke();
            Clear();

            base.CloseDialog();
        }

        private void Clear()
        {
            oKButton.onClick.RemoveAllListeners();
            onClosed.Clear();

            Content = string.Empty;
            Title = string.Empty;

            IsShowCancelButton = true;
            IsShowOKButton = true;
        }
        
        public void AddOKEvent(UnityAction action) => oKButton.onClick.AddListener(action);
        public void AddCancelEvent(Action action) => onClosed.Add(action);
    }
}
