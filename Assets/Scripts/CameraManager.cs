using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public RawImage rawImage;
    public TMPro.TMP_Dropdown dropdownUI;
    WebCamTexture webCamTexture;

    private bool isWebcamPlay = false;
    private string currentWebcamName = "";

    // Start is called before the first frame update
    IEnumerator Start()
    {
        // Request authorization for camera
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            RefreshCameraList();
            
            if (!string.IsNullOrEmpty(currentWebcamName))
            {
                webCamTexture = new WebCamTexture(currentWebcamName);
                rawImage.texture = webCamTexture;
                rawImage.material.mainTexture = webCamTexture;
                webCamTexture.Play();
                isWebcamPlay = true;
                Debug.Log("Web camera play");
            }
            else
            {
                Debug.LogError("DroidCam not found!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(webCamTexture == null) return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            isWebcamPlay = !isWebcamPlay;
            if (isWebcamPlay) webCamTexture.Play();
            else webCamTexture.Stop();
        }
    }

    void RefreshCameraList()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        dropdownUI.ClearOptions();
        List<TMPro.TMP_Dropdown.OptionData> dropodownOptions = new List<TMPro.TMP_Dropdown.OptionData>();
        // Find the DroidCam device
        for (int i = 0; i < devices.Length; i++)
        {
            dropodownOptions.Add(new TMPro.TMP_Dropdown.OptionData(devices[i].name));
            Debug.Log("Web camera name : " + devices[i].name);
        }
        
        currentWebcamName = devices[0].name;
        dropdownUI.AddOptions(dropodownOptions);
        dropdownUI.onValueChanged.AddListener(OnDropdownChanged);
    }

    private void OnDropdownChanged(int index)
    {
        currentWebcamName = dropdownUI.options[index].text;
        webCamTexture = new WebCamTexture(currentWebcamName);
        rawImage.texture = webCamTexture;
        rawImage.material.mainTexture = webCamTexture;
        webCamTexture.Play();
        isWebcamPlay = true;
    }
}
