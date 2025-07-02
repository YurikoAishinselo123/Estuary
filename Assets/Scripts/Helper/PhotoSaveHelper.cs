using System.IO;
using UnityEngine;

public static class PhotoSaveHelper
{
    private static readonly string photoFolder = Path.Combine(Application.persistentDataPath, "Photos");

    static PhotoSaveHelper()
    {
        if (!Directory.Exists(photoFolder))
        {
            Directory.CreateDirectory(photoFolder);
        }
    }

    public static string SavePhoto(Texture2D photo)
    {
        string fileName = GenerateFileName();
        string filePath = Path.Combine(photoFolder, fileName);
        byte[] bytes = photo.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);
        Debug.Log($"[PhotoSaveHelper] Photo saved to: {filePath}");
        return filePath;
    }

    public static Texture2D LoadPhoto(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning($"[PhotoSaveHelper] File not found: {path}");
            return null;
        }

        byte[] bytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        return tex;
    }

    public static void DeleteAllPhotos()
    {
        if (Directory.Exists(photoFolder))
        {
            var files = Directory.GetFiles(photoFolder);
            foreach (var file in files)
            {
                File.Delete(file);
            }
            Debug.Log("[PhotoSaveHelper] All saved photos deleted.");
        }
    }

    public static string[] GetAllPhotoPaths()
    {
        if (Directory.Exists(photoFolder))
            return Directory.GetFiles(photoFolder, "*.png");

        return new string[0];
    }

    private static string GenerateFileName()
    {
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        return $"Photo_{timestamp}.png";
    }
}
