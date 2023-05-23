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
using System.Collections.Generic;
using UnityEngine;

namespace ProjectL
{
    public class User
    {
        private Dictionary<IngameMapScene, int> chapterRecord = new Dictionary<IngameMapScene, int>();

        private string achievement;
        public string Achievement => achievement;

        private ulong id;
        public ulong ID => id;

        private string nickName;
        public string NickName
        { 
            get => nickName;
            set
            {
                nickName = value;
                onChangedNickName?.Invoke(this);
                SaveUserData();
            }
        }
        public static Action<User> onChangedNickName;

        private bool IsGuest { get; }

        private bool isLoadDone;
        public bool IsLoadDone { get => IsGuest || isLoadDone; private set => isLoadDone = value; }

        public User()
        {
            Debug.Log("User.Create(), is Guest Login");

            IsGuest = true;
        }

        public User(ulong id)
        {
            Debug.Log($"User.Create(), _id : {id.ToString()}");

            this.id = id;

            LoadUserData();
        }

        public void SetRecord(IngameMapScene chapter, int round)
        {
            Debug.Log($"User.SetRecord(), _id : {id.ToString()}, chapter : {chapter.ToString()}, round : {round}");

            chapterRecord.TryAdd(chapter, round);

            chapterRecord[chapter] = round;
            SaveUserData();
        }

        public int GetRecord(IngameMapScene chapter)
        {
            chapterRecord.TryAdd(chapter, 0);

            Debug.Log($"User.GetRecord(), _id : {id.ToString()}, chapter : {chapter.ToString()}, round : {chapterRecord[chapter]}");

            return chapterRecord[chapter];
        }

        private void SaveUserData()
        {
            if (IsGuest)
            {
                Debug.Log("User.SaveUserData(), Guest is return");
                return;
            }

            Debug.Log($"User.SaveUserData(), ID : {ID.ToString()}, NickName : {NickName}");
            var data = new UserInfoDBData();

            data.id = ID;
            data.nickname = NickName;
            data.achievement = achievement;

            var chapters = new List<ChapterDBData>();
            foreach (var item in chapterRecord)
            {
                chapters.Add(new ChapterDBData((int)item.Key, item.Value));
            }
            data.chapters = chapters;

            DBManager.Instance.SaveData(data);
        }

        private async void LoadUserData()
        {
            IsLoadDone = false;

            if (IsGuest)
            {
                Debug.Log("User.LoadUserData(), Guest is return");
                return;
            }

            Debug.Log($"User.LoadUserData(), ID : {ID.ToString()}");
            var data = await DBManager.Instance.LoadData<UserInfoDBData>(ID);

            nickName = data.nickname;
            achievement = data.achievement;

            data.chapters.ForEach(chapter =>
            {
                chapterRecord[(IngameMapScene)chapter.chapter] = chapter.floor;
            });

            IsLoadDone = true;
        }
    }
}
