using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PhotoManager : MonoBehaviour
{
    [SerializeField] private UnityEvent onPhotoRestart;
    [SerializeField] private UnityEvent onPhotoSuccess;

    //Photo
    [SerializeField] private RawImage photoResultUI;
    [SerializeField] private RawImage qrResultUI;

    //Timer
    [SerializeField] private List<GameObject> photoUIToHide;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float photoTimer = 3.5f;
    private float currentPhotoTimer = 3.5f;

    //Photo
    private bool isOnCountdown = false;
    private bool isCapturingImage = false;
    private bool isUploading = false;
    private string filePath;
    private string fileName;

    private Texture2D photoTexture = null;
    private byte[] photoTextureByte = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isOnCountdown) return;

        if(currentPhotoTimer <= 0f) {        
            StartCoroutine(InitiateTakePicture());
        }

        currentPhotoTimer -= Time.deltaTime;
        countdownText.text = currentPhotoTimer.ToString("F0");
    }

    public void RestartPhotoUI()
    {
        onPhotoRestart?.Invoke();
    }
    
    public void TakePicture() {
        if(isCapturingImage)
            return;
        
        countdownText.gameObject.SetActive(true);

        isCapturingImage = true;
        isOnCountdown = true;

        foreach (var ui in photoUIToHide)
            ui.SetActive(false);
    }

    public IEnumerator InitiateTakePicture() {
        print("Taking screenshots...");

        isOnCountdown = false;
        countdownText.gameObject.SetActive(false);

        filePath = Directory.GetCurrentDirectory();
        var foldername = "/Assets/Screenshots/";
        filePath = Path.Join(filePath, foldername);
        if(!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);
        
        fileName = "ScreenCapture_" + System.DateTime.UtcNow.ToString("ydd-MM-yyyy-HH-mm-ss") + ".png";
        filePath = Path.Join(filePath, fileName);

        ScreenCapture.CaptureScreenshot(filePath);
        
        Debug.Log($"File saved at : {filePath}");

        yield return new WaitForSeconds(0.5f);

        photoTextureByte = File.ReadAllBytes(filePath);
        photoTexture = new Texture2D(photoResultUI.texture.width, photoResultUI.texture.height);
        photoTexture.LoadImage(photoTextureByte);
        photoResultUI.texture = photoTexture;

        foreach (var ui in photoUIToHide)
            ui.SetActive(true);

        onPhotoSuccess?.Invoke();

        currentPhotoTimer = photoTimer;
        
        isCapturingImage = false;
    }

    public void CleanCacheTexture() {
        photoTexture = null;
        photoTextureByte = null;
    }
}
