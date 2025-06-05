using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode] // Cho phép script chạy trong Editor mà không cần Play
public class RenderText : MonoBehaviour
{
    public Camera captureCamera; // Camera dùng để chụp
    public RenderTexture renderTexture; // RenderTexture để lưu ảnh
    public string savePath = "Assets/Textures/PrefabScreenshot.png"; // Đường dẫn lưu file

    public void TakeScreenshot()
    {
        if (captureCamera == null || renderTexture == null)
        {
            Debug.LogError("Thiếu camera hoặc RenderTexture!");
            return;
        }

        // Gắn RenderTexture vào camera
        captureCamera.targetTexture = renderTexture;

        // Chụp ảnh
        captureCamera.Render();

        // Đọc pixel từ RenderTexture
        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        screenshot.Apply();

        // Lưu ra file PNG
        byte[] bytes = screenshot.EncodeToPNG();
        Debug.Log("Bắt đầu lưu ảnh...");
        File.WriteAllBytes(savePath, bytes);
        Debug.Log("Lưu ảnh xong!");

        Debug.Log("Ảnh đã được lưu tại: " + savePath);
        
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh(); // Để Unity nhận diện file mới
        #endif


        // Dọn dẹp
        RenderTexture.active = null;
        captureCamera.targetTexture = null;
    }
}
