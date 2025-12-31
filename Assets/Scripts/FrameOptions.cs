using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameOptions : MonoBehaviour
{
    public List<Sprite> frameBorders = new List<Sprite>();
    public Image borderUI;

    public int currentBorderIndex = 0;

    void Start()
    {
        ClearFrame();
    }

    public void NextFrame()
    {
        currentBorderIndex++;
        if (currentBorderIndex >= frameBorders.Count) currentBorderIndex = 0;
        ClearFrame();
        borderUI.sprite = frameBorders[currentBorderIndex];
    }

    public void PreviousFrame()
    {
        currentBorderIndex--;
        if (currentBorderIndex < 0) currentBorderIndex = frameBorders.Count - 1;
        ClearFrame();
        borderUI.sprite = frameBorders[currentBorderIndex];
    }

    void ClearFrame()
    {
        if(currentBorderIndex == 0) borderUI.gameObject.SetActive(false);
        else borderUI.gameObject.SetActive(true);
    }

}
