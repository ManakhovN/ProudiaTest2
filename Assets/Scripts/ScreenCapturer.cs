using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class captures screenshot and saves it in following path.
/// I wasn't sure about the task, so i used side asset to open Windows File Manager.
/// </summary>
/// 
public class ScreenCapturer : MonoBehaviour {
    ICapturerInput capturerInput;
    string path;
    [SerializeField] Camera screenshotCamera;
    [SerializeField] int height = 2048;
    [SerializeField] int width = 2048;

	void Awake () {
        path = Application.dataPath;
        capturerInput = GetComponent<ICapturerInput>();
        capturerInput.OnChoosePathClick += ChoosePath;
        capturerInput.OnSaveScreenshot += MakeScreenshot;
	}

    private void MakeScreenshot()
    {
        Texture2D screenShot = ReadTextureFromCamera();
        if (SaveScreenshot(screenShot, path))
            OpenImage(path + "/screenshot.png");
        else
            SaveScreenshotAs(screenShot, path); 
    }

    ExtensionFilter[] extensions = new ExtensionFilter[] {
        new SFB.ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
        new SFB.ExtensionFilter("All Files", "*" ),
    };

    private void SaveScreenshotAs(Texture2D screenShot, string path)
    {
        SFB.StandaloneFileBrowser.SaveFilePanelAsync("Default name is busy choose another path", path, "screenshot.png", extensions,  (string p) => {
            SaveCertainScreenshot(screenShot, p);
            OpenImage(p);
        });
    }

    private Texture2D ReadTextureFromCamera() {
        RenderTexture rt = new RenderTexture(width, height, 24);
        screenshotCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenshotCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        return screenShot;
    }

    private bool SaveScreenshot(Texture2D texture, string path) {
        string filename = path + "/screenshot.png";

        if (System.IO.File.Exists(filename))
            return false;
        SaveCertainScreenshot(texture, filename);
        return true;
    }

    private void SaveCertainScreenshot(Texture2D texture, string fullPath) {
        byte[] bytes = texture.EncodeToPNG();
        if (String.IsNullOrEmpty(fullPath))
            return;
        System.IO.File.WriteAllBytes(fullPath, bytes);
    }

    private void ChoosePath()
    {
        SFB.StandaloneFileBrowser.OpenFolderPanelAsync("Pick Screenshot folder", "", true, (string[] paths) =>
        {
            path = paths[0];
        });
    }

    public void OpenImage(string path)
    {
        System.Diagnostics.Process.Start(path);
    }
}
