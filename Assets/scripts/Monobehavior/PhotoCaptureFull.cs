using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation; // Add this at the top

public class PhotoCaptureFull : MonoBehaviour
{
    [Header("Capture")]
    public Button captureButton;
    public Image previewImage;
    public GameObject previewFrame;
    public AudioSource shutterSound;
    private Texture2D screenImage;
    private bool viewingPreview;
    private string lastSavedPath;
    public Animator fadingAnimation;

    public GameObject moveCanvas;


    public string fileNamePrefix = "Capture_";

    [Header("Gallery")]
    public Button galleryButton;
    public GameObject galleryPanel;
    public Transform galleryContent; // Parent for thumbnails
    public GameObject thumbnailPrefab; // Prefab with Image component

    [Header("AR Camera")]
    public ARCameraManager arCameraManager; // Assign this in the Inspector
    public Button switchCameraButton;

    void Start()
    {
        screenImage = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
     #if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
        Permission.RequestUserPermission(Permission.Camera);
        }
     #endif

    captureButton.onClick.AddListener(() =>
    {
        if (!viewingPreview)
        {
            StartCoroutine(CaptureScreenshot());
        }
       /* else
        {
            RemoveImagePreview();
        }*/
        });
            
        galleryButton.onClick.AddListener(OpenNativeGallery);
        switchCameraButton.onClick.AddListener(SwitchCameraFacingDirection);
    }

    IEnumerator CaptureScreenshot()
    {
        moveCanvas.SetActive(false);
        viewingPreview = true;
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;

        Rect rect = new Rect(0, 0, width, height);
        screenImage.ReadPixels(rect, 0, 0);
        screenImage.Apply();

        // Son
        shutterSound?.Play();
        PreviewImage();

        //Save the image
        SaveImage();

        // Remove preview after 15 seconds
        StartCoroutine(RemovePreviewAfterDelay(7f));

        //yield return new WaitForSeconds(0.1f);
    }

    IEnumerator RemovePreviewAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RemoveImagePreview();
    }

    void PreviewImage()
    {
        moveCanvas?.SetActive(true);
        // Convert Texture2D to Sprite
        Sprite previewSprite = Sprite.Create(screenImage, new Rect(0, 0, screenImage.width, screenImage.height), new Vector2(0.5f, 0.5f), 100.0f);
        previewImage.sprite = previewSprite;

        previewFrame.SetActive(true);
        fadingAnimation.Play("ImageFade");
        
    }

    void RemoveImagePreview()
    {
        previewFrame.SetActive(false);
        viewingPreview = false;
        // Reset the preview image
        previewImage.sprite = null;
    }

    void SaveImage()
    {
        byte[] byteArray = screenImage.EncodeToPNG();

        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = fileNamePrefix + timestamp + ".png";
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(path, byteArray);
        lastSavedPath = path;

     #if UNITY_ANDROID
        // Sauvegarder dans la galerie (Android)
        string androidPath = Path.Combine(Application.persistentDataPath, fileName);
        NativeGallery.SaveImageToGallery(androidPath, "AR Filters", fileName);
     #endif
    }

    void OpenNativeGallery()
    {
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                // Clear previous thumbnails
                foreach (Transform child in galleryContent)
                {
                    Destroy(child.gameObject);
                }

                // Load the selected image
                Texture2D tex = NativeGallery.LoadImageAtPath(path, 512, false);
                if (tex == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                // Instantiate thumbnail
                GameObject thumb = Instantiate(thumbnailPrefab, galleryContent);
                Image img = thumb.GetComponent<Image>();
                img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

                galleryPanel.SetActive(true);
            }
        }, "Select an image", "image/*");
    }

    public void ShareLastPhoto()
    {
     #if UNITY_ANDROID
        if (!string.IsNullOrEmpty(lastSavedPath))
        {
            new NativeShare().AddFile(lastSavedPath)
                             .SetSubject("Check this out!")
                             .SetText("AR ####")
                             .Share();
        }
     #endif
    }

    void SwitchCameraFacingDirection()
    {
        if (arCameraManager == null)
            return;

        // Toggle between World and User
        if (arCameraManager.requestedFacingDirection == CameraFacingDirection.World)
        {
            arCameraManager.requestedFacingDirection = CameraFacingDirection.User;
        }
        else
        {
            arCameraManager.requestedFacingDirection = CameraFacingDirection.World;
        }
    }
}




















