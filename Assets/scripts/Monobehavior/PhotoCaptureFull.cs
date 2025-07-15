/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoCaptureFull : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}*/

using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.Events;

public class PhotoCaptureFull : MonoBehaviour
{
    public Button captureButton;
    public RawImage previewImage;
    public AudioSource shutterSound;
    public GameObject flashOverlay;
    public string fileNamePrefix = "AR_Capture_";

    private string lastSavedPath;

    void Start()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
    {
        Permission.RequestUserPermission(Permission.Camera);
    }
#endif

        captureButton.onClick.AddListener(() => StartCoroutine(CaptureScreenshot()));
    }

    IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        // Effet flash
        if (flashOverlay = null)
        {
            flashOverlay.SetActive(true);
        }

        yield return new WaitForSeconds(0.1f);

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        if (flashOverlay != null)
        {
            flashOverlay.SetActive(false);
        }

        // Son
        shutterSound?.Play();

        // Enregistrement
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = fileNamePrefix + timestamp + ".png";
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(path, screenImage.EncodeToPNG());
        lastSavedPath = path;

        Debug.Log("Image saved to: " + path);

        // Aperçu
        if (previewImage != null)
        {
            Sprite s = Sprite.Create(screenImage, new Rect(0, 0, screenImage.width, screenImage.height), Vector2.one * 0.5f);
            previewImage.texture = screenImage;
        }

#if UNITY_ANDROID
        // Sauvegarder dans la galerie (Android)
        string androidPath = Path.Combine(Application.persistentDataPath, fileName);
        NativeGallery.SaveImageToGallery(androidPath, "AR Filters", fileName);
#endif
    }

    public void ShareLastPhoto()
    {
#if UNITY_ANDROID
        if (!string.IsNullOrEmpty(lastSavedPath))
        {
            new NativeShare().AddFile(lastSavedPath)
                             .SetSubject("Check this out!")
                             .SetText("Test AR filters photo")
                             .Share();
        }
#endif
    }
}

