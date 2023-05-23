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

using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace ProjectL
{
    public class LoginInfoUI : LobbyUIBase
    {
        [SerializeField]
        private TMP_InputField nickNameInputField;

        private readonly int MaxLength = 24;

        protected override void Start()
        {
            base.Start();

            nickNameInputField.onValidateInput += ValidateInput;
            nickNameInputField.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(nickNameInputField); });
        }

        private char ValidateInput(string text, int charIndex, char addedChar)
        {
            if (!Regex.IsMatch(addedChar.ToString(), "^[a-zA-Z가-힣0-9\\s]*$"))
            {
                return '\0';
            }
            return addedChar;
        }

        private void OnInputFieldValueChanged(TMP_InputField inputField)
        {
            string text = inputField.text;

            if (text.Length > MaxLength)
            {
                text = text.Substring(0, MaxLength);
            }

            int length = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c >= '0' && c <= '9')
                {
                    length++;
                    if (length > MaxLength)
                    {
                        text = text.Substring(0, i);
                        break;
                    }
                }
                else if (c >= '가' && c <= '힣')
                {
                    length += 2;
                    if (length > MaxLength)
                    {
                        text = text.Substring(0, i);
                        break;
                    }
                }
                else // 특수문자, 공백
                {
                    text = text.Remove(i, 1);
                    i--;
                }
            }

            inputField.text = text;
        }

        public void OnClickSetNickname()
        {
            var nickName = nickNameInputField.text;

            Debug.Log($"LoginInfoUI.OnClickSetNickname() NickName : {nickName}");

            if (string.IsNullOrEmpty(nickName))
            {
                Debug.Log("LoginInfoUI.OnClickSetNickname(), NickName is null");
                return;
            }

            D.SelfUser.NickName = nickName;
        }
    }
}
