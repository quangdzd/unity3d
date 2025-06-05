using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    void Awake()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
    }

    void Start()
    {
        List<String> reresolutionOptions = new List<String>();
        int resolutionsIndex = 0; 

        for (int i = 0; i < resolutions.Length ; i++)
        {
            String option = resolutions[i].width + " x " + resolutions[i].height;
            if(resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                resolutionsIndex = i;
            }
            reresolutionOptions.Add(option);
        }
        resolutionDropdown.AddOptions(reresolutionOptions);
        resolutionDropdown.value = resolutionsIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        Debug.Log("is " + volume);
    }

    public void QualitySetting(int qualityIndex)
    {

        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width ,  resolution.height , false);


    }
}
