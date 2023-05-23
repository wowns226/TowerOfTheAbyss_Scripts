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
using UnityEngine;
using UnityEngine.Serialization;

public class CameraManager :  MonoSingleton<CameraManager>
{
    public Transform cameraParentTransform;
    
    public float shakeIntensity = 0.1f;
    private float endTime = 0f;
    
    private Vector3 originalPosition;
    private bool isShaking;
    public bool IsShaking => isShaking;

    [SerializeField]
    private bool isFreeze;
    public bool IsFreeze => isFreeze;

    public void ShakeCamera(float shakeDuration = 1f)
    {
        Debug.Log("CameraManager.ShakeCamera(), Start");
        
        if (cameraParentTransform == null)
        {
            Debug.Log("CameraManager.ShakeCamera(), cameraTransform is null");
            return;
        }
        
        if (IsShaking)
        {
            float newEndTime = Time.time + shakeDuration;

            if (newEndTime > endTime)
                endTime = newEndTime;
            
            return;
        }
        
        StartCoroutine(Shake(shakeDuration));
    }

    // 흔들림 효과 구현
    private IEnumerator Shake(float shakeDuration)
    {
        isShaking = true;
        
        originalPosition = cameraParentTransform.position;
        
        endTime = Time.time + shakeDuration;
        float elapsedTime = Time.time;

        while (elapsedTime < endTime)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            cameraParentTransform.position = originalPosition + randomOffset;

            elapsedTime += Time.deltaTime;

            yield return new WaitForSeconds(0.1f);
        }

        cameraParentTransform.position = originalPosition;

        isShaking = false;
    }

}