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

#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using UnityEditor;

namespace ProjectL
{
    public class ScreenShotEditor : EditorWindow
    {
        RecorderController controller;

        string captureFileName = "";

        [MenuItem("Tools/Custom/Capture Game View")]
        static void Init()
        {
            ScreenShotEditor window = GetWindow<ScreenShotEditor>();
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Capture Settings", EditorStyles.boldLabel);

            captureFileName = EditorGUILayout.TextField("File Name : ", captureFileName);

            var controllerSettings = CreateInstance<RecorderControllerSettings>();
            controller = new RecorderController(controllerSettings);
            var mediaOutputFolder = Path.Combine(Application.dataPath, "ScreenShot");

            var imageRecorder = CreateInstance<ImageRecorderSettings>();
            imageRecorder.name = "My Image Recorder";
            imageRecorder.Enabled = true;
            imageRecorder.OutputFormat = ImageRecorderSettings.ImageRecorderOutputFormat.PNG;
            imageRecorder.CaptureAlpha = false;

            if(string.IsNullOrEmpty(captureFileName))
                imageRecorder.OutputFile = Path.Combine(mediaOutputFolder, "image_") + DefaultWildcard.Take;
            else
                imageRecorder.OutputFile = Path.Combine(mediaOutputFolder, captureFileName);

            imageRecorder.imageInputSettings = new GameViewInputSettings
            {
                OutputWidth = 1920,
                OutputHeight = 1080,
            };

            controllerSettings.AddRecorderSettings(imageRecorder);
            controllerSettings.SetRecordModeToSingleFrame(0);

            if(GUILayout.Button("Capture"))
            {
                if(File.Exists(imageRecorder.OutputFile + ".png"))
                {
                    Debug.Log($"Already Exists (File Name : {imageRecorder.OutputFile + ".png"}");
                    return;
                }

                controller.PrepareRecording();
                controller.StartRecording();

                Debug.Log($"Capture Complete (File Name : {imageRecorder.OutputFile + ".png"})");
            }
        }
    }
}
#endif
