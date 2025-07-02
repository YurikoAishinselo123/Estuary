using UnityEngine;

public static class PhotoSaveSystem
{

    public static string SavePhoto(Texture2D photo)
    {
        return PhotoSaveHelper.SavePhoto(photo);
    }

    public static Texture2D LoadPhoto(string path)
    {
        return PhotoSaveHelper.LoadPhoto(path);
    }

    public static string[] GetAllPhotoPaths()
    {
        return PhotoSaveHelper.GetAllPhotoPaths();
    }

    public static void DeleteAllPhotos()
    {
        PhotoSaveHelper.DeleteAllPhotos();
    }
}
