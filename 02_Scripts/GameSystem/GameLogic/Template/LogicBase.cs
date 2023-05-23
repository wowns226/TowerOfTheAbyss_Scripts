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
using System.Reflection;
using UnityEngine;

namespace ProjectL
{
    public class LogicBase<T> : MonoBehaviour where T : Enum
    {
        //싱글톤을 상속받으면 없을 경우 만들어 버려서 게임 로직에는 문제가 있을 수 있는 부분이라 별도로 생성
        private static LogicBase<T> instance;
        public static LogicBase<T> Instance => instance;

        private Dictionary<T, Action> changeStatusAction = new Dictionary<T, Action>();
        private Dictionary<T, Action<bool>> changeStatusFlagAction = new Dictionary<T, Action<bool>>();
        private Dictionary<T, List<MethodInfo>> statusMethods = new Dictionary<T, List<MethodInfo>>();
        private Coroutine currentProcessCoroutine;

        private bool isInterruptionStatus;

        private T status;
        public T Status
        {
            get => status;
            private set
            {
                status = value;
                ChangeStatusEvent(value);
            }
        }

        protected virtual void Awake()
        {
            instance = this;

            RegisterChangeStatusEvent();

            RegisterStatusMethod();

            EnterStatus();
        }

        private void RegisterChangeStatusEvent()
        {
            var enumTypes = Enum.GetValues(typeof(T));

            foreach (var enumType in enumTypes)
            {
                if (changeStatusFlagAction.ContainsKey((T)enumType) == false)
                    changeStatusFlagAction.Add((T)enumType, null);
            }
        }

        private void RegisterStatusMethod()
        {
            var enumTypesString = Enum.GetNames(typeof(T));

            foreach (var enumTypeString in enumTypesString)
            {
                T enumType = (T)Enum.Parse(typeof(T), enumTypeString);

                statusMethods.Add(enumType, new List<MethodInfo>());

                var enterMethod = GetType().GetMethod("Enter" + enumTypeString, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var processMethod = GetType().GetMethod("Process" + enumTypeString, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var leaveMethod = GetType().GetMethod("Leave" + enumTypeString, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                statusMethods[enumType].Add(enterMethod);
                statusMethods[enumType].Add(processMethod);
                statusMethods[enumType].Add(leaveMethod);
            }
        }

        private void EnterStatus()
        {
            var firstStatus = (T)Enum.GetValues(typeof(T)).GetValue(0);
            EnterStatus(firstStatus);
        }

        public void EnterStatus(T targetStatus)
        {
            if (currentProcessCoroutine != null)
            {
                StopCoroutine(currentProcessCoroutine);
                currentProcessCoroutine = null;
            }

            if (isInterruptionStatus)
                statusMethods[Status][2]?.Invoke(this, null); //Prev Leave

            Status = targetStatus;
            currentProcessCoroutine = StartCoroutine(ProcessStatusMethod());
        }

        public void NextStatus()
        {
            var statusArray = Enum.GetValues(typeof(T));

            for (int i = 0; i < statusArray.Length; i++)
            {
                if (statusArray.GetValue(i).Equals(Status))
                {
                    if (statusArray.Length > i + 1)
                    {
                        T nextStatus = (T)statusArray.GetValue(i + 1);
                        EnterStatus(nextStatus);
                        break;
                    }
                }
            }
        }

        private IEnumerator ProcessStatusMethod()
        {
            isInterruptionStatus = true;

            statusMethods[Status][0]?.Invoke(this, null); //Enter

            IEnumerator enumerator = (IEnumerator)statusMethods[Status][1]?.Invoke(this, null); //Process
            while (enumerator.MoveNext())
                yield return enumerator.Current;

            statusMethods[Status][2]?.Invoke(this, null); //Leave

            isInterruptionStatus = false;

            NextStatus();
        }


        public void AddStatusEvent(T targetStatus, Action<bool> action)
        {
            changeStatusFlagAction.TryAdd(targetStatus, null);
            changeStatusFlagAction[targetStatus] += action;
        }

        public void RemoveStatusEvent(T targetStatus, Action<bool> action)
        {
            if (changeStatusFlagAction.ContainsKey(targetStatus) == false)
                return;

            changeStatusFlagAction[targetStatus] -= action;
        }

        public void AddStatusEvent(T targetStatus, Action action)
        {
            changeStatusAction.TryAdd(targetStatus, null);

            changeStatusAction[targetStatus] += action;
        }

        public void RemoveStatusEvent(T targetStatus, Action action)
        {
            if (changeStatusAction.ContainsKey(targetStatus) == false)
                return;

            changeStatusAction[targetStatus] -= action;
        }

        private void ChangeStatusEvent(T targetStatus)
        {
            foreach (var actionDic in changeStatusFlagAction)
            {
                if (actionDic.Key.HasFlag(targetStatus))
                    actionDic.Value?.Invoke(true);
                else
                    actionDic.Value?.Invoke(false);
            }

            foreach (var actionDic in changeStatusAction)
            {
                if (actionDic.Key.HasFlag(targetStatus))
                    actionDic.Value?.Invoke();
            }
        }

    }
}
