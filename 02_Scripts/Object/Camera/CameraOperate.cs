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

using System.Linq;
using UnityEngine;

namespace ProjectL
{
    public class CameraOperate : MonoBehaviour
    {
        private float zoomSpeed = 50f;
        private float mouseMoveSpeed = 50f;
        private float keyboardMoveSpeed = 50f;

        public new Camera camera;
        private Transform cameraTransform;
        
        public bool isKeyOperate = true;

        [SerializeField]
        private Vector3 firstTR;
        [SerializeField]
        private float firstFOV = 60;

        private float limitMinX;
        private float limitMaxX;
        private float limitMinZ;
        private float limitMaxZ;
        private float limitMinY;
        private float limitMaxY;

        private Vector3 moveLimitOffSet;

        private bool IsOpenDialog => DialogManager.Instance.TopDialog && DialogManager.Instance.TopDialog.GetType() != typeof(DlgSlowEffect);

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Confined;
            cameraTransform = camera.transform;
            firstTR = cameraTransform.position;
            firstFOV = camera.fieldOfView;
        }

        private void Start()
        {
            moveLimitOffSet = new Vector3(5.0f, 5.0f, 5.0f);

            limitMinX = D.SelfBoard.points.Min(point => point.transform.position.x) - moveLimitOffSet.x;
            limitMaxX = D.SelfBoard.points.Max(point => point.transform.position.x) + moveLimitOffSet.x;
            limitMinZ = D.SelfBoard.points.Min(point => point.transform.position.z) - moveLimitOffSet.z;
            limitMaxZ = D.SelfBoard.points.Max(point => point.transform.position.z) + moveLimitOffSet.z;
            limitMinY = firstTR.y - moveLimitOffSet.y;
            limitMaxY = firstTR.y + moveLimitOffSet.y;

            SetSensitivity(SettingManager.Instance.ControlOption.CurrentData);
            SettingManager.Instance.ControlOption.onValueChanged.Add(SetSensitivity);

            CameraManager.Instance.cameraParentTransform = transform;
            
            Debug.Log($"LimitX min : {limitMinX} max : {limitMaxX}");
            Debug.Log($"LimitZ min : {limitMinZ} max : {limitMaxZ}");
            Debug.Log($"Min Point X : {D.SelfBoard.points.Min(point => point.transform.position.x)} Max Point X : {D.SelfBoard.points.Max(point => point.transform.position.x)}");
            Debug.Log($"Min Point Z : {D.SelfBoard.points.Min(point => point.transform.position.z)} Max Point Z : {D.SelfBoard.points.Max(point => point.transform.position.z)}");
        }

        private void SetSensitivity(ControlData data)
        {
            Debug.Log($"CameraOperator.SetSensitivity(), zoom : {data.zoomSensitivity}, mouse : {data.mouseSensitivity}, keyboard : {data.keyboardSensitivity}");
            zoomSpeed = data.zoomSensitivity;
            mouseMoveSpeed = data.mouseSensitivity;
            keyboardMoveSpeed = data.keyboardSensitivity;
        }

        private void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.Confined;
            SettingManager.Instance?.ControlOption.onValueChanged.Remove(SetSensitivity);
        }

        private void LateUpdate()
        {
            //마우스가 ui 위에 있으면 리턴
            //if (EventSystem.current.IsPointerOverGameObject())
            //{
            //    return;
            //}

            if(CameraManager.Instance.IsFreeze)
            {
                return;
            }    

            if(PlayRoundLogic.Instance.Status != RoundLogic.PlayRound)
            {
                return;
            }

            if(IsOpenDialog)
            {
                return;
            }

            if(Input.GetKey(GetKey(InputType.MoveReset)))
            {
                Reset_Camera_Pos();
                return;
            }

            if(isKeyOperate)
            {
                MoveKeyBoard();
            }

            MoveMouse();

            ZoomInOut();

            BlockLimitPos();
        }

        private void ZoomInOut()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            var changedFov = camera.fieldOfView + scroll * -5f * Time.unscaledDeltaTime * zoomSpeed;

            if(changedFov < 10)
            {
                changedFov = 10;
            }

            if(changedFov > firstFOV)
            {
                changedFov = firstFOV;
            }

            camera.fieldOfView = changedFov;
        }

        private void MoveMouse()
        {
            Vector3 pos = camera.ScreenToViewportPoint(Input.mousePosition);

            var moveOffset = Vector3.zero;

            if(pos.x < 0.01f) moveOffset -= cameraTransform.right;
            if(pos.x > 0.99f) moveOffset += cameraTransform.right;
            if(pos.y < 0.01f) moveOffset -= cameraTransform.up;
            if(pos.y > 0.99f) moveOffset += cameraTransform.up;

            if(moveOffset != Vector3.zero)
            {
                cameraTransform.position += moveOffset * (mouseMoveSpeed * Time.unscaledDeltaTime * 0.2f);
            }
        }

        private void MoveKeyBoard()
        {
            float speed = keyboardMoveSpeed;

            if(Input.GetKey(GetKey(InputType.MoveSpeedUp)))
            {
                speed *= 2f;
            }

            if(Input.GetKey(GetKey(InputType.MoveForward)))
            {
                cameraTransform.position += cameraTransform.up * (Time.unscaledDeltaTime * speed * 0.3f);
            }

            if(Input.GetKey(GetKey(InputType.MoveBackward)))
            {
                cameraTransform.position -= cameraTransform.up * (Time.unscaledDeltaTime * speed * 0.3f);
            }

            if(Input.GetKey(GetKey(InputType.MoveLeft)))
            {
                cameraTransform.position -= cameraTransform.right * (Time.unscaledDeltaTime * speed * 0.3f);
            }

            if(Input.GetKey(GetKey(InputType.MoveRight)))
            {
                cameraTransform.position += cameraTransform.right * (Time.unscaledDeltaTime * speed * 0.3f);
            }
        }

        private void BlockLimitPos()
        {
            if(cameraTransform.position.x < limitMinX)
            {
                cameraTransform.position = new Vector3(limitMinX, cameraTransform.position.y, cameraTransform.position.z);
            }

            if(cameraTransform.position.x > limitMaxX)
            {
                cameraTransform.position = new Vector3(limitMaxX, cameraTransform.position.y, cameraTransform.position.z);
            }

            if(cameraTransform.position.z < limitMinZ)
            {
                cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, limitMinZ);
            }

            if(cameraTransform.position.z > limitMaxZ)
            {
                cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, limitMaxZ);
            }

            if(cameraTransform.position.y < limitMinY)
            {
                cameraTransform.position = new Vector3(cameraTransform.position.x, limitMinY, cameraTransform.position.z);
            }

            if(cameraTransform.position.y > limitMaxY)
            {
                cameraTransform.position = new Vector3(cameraTransform.position.x, limitMaxY, cameraTransform.position.z);
            }
        }

        public void Reset_Camera_Pos()
        {
            cameraTransform.position = firstTR;
            camera.fieldOfView = firstFOV;
        }

        public void FoucsUnit(Unit unit)
        {
            cameraTransform.position = new Vector3(unit.transform.position.x, firstTR.y, unit.transform.position.z);
        }

        private KeyCode GetKey(InputType inputType) => SettingManager.Instance.KeyBindingOption.GetKey(inputType);
    }
}


