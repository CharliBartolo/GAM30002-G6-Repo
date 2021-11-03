using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Dropdown textureDropdown;
    public Dropdown aaDropdown;

    Resolution[] resolutions;

    // Start is called before the first frame update
    void Start()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
                    currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
    }

    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resToSet = resolutions[resolutionIndex];
        Screen.SetResolution(resToSet.width, resToSet.height, Screen.fullScreen);
    }

    public void SetTextureQuality(int textureIndex)
    {
        QualitySettings.masterTextureLimit = textureIndex;
        qualityDropdown.value = 3;
    }

    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        qualityDropdown.value = 3;
    }

    public void SetQuality(int qualityIndex)
    {
        // If player is not using any presets...
        if (qualityIndex != 3)
            QualitySettings.SetQualityLevel(qualityIndex);

        switch(qualityIndex)
        {
            case 0: //Quawity wevel - Low uwu
                textureDropdown.value = 2;
                aaDropdown.value = 0;
                break;
            case 1: // Quawity wevel - Medium
                textureDropdown.value = 1;
                aaDropdown.value = 1;
                break;
            case 2: // Quawity wevel - High
                textureDropdown.value = 0;
                aaDropdown.value = 2;
                break; 
        }

        qualityDropdown.value = qualityIndex;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("TextureQualityPreference", textureDropdown.value);
        PlayerPrefs.SetInt("AntiAliasingPreference", aaDropdown.value);
        //PlayerPrefs.SetInt("FullscreenPreference", Screen.fullScreen);

    }

    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
            qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        else
            qualityDropdown.value = 2;
        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;
        if (PlayerPrefs.HasKey("TextureQualityPreference"))
            textureDropdown.value = PlayerPrefs.GetInt("TextureQualityPreference");
        else
            textureDropdown.value = 0;
        if (PlayerPrefs.HasKey("AntiAliasingPreference"))
            aaDropdown.value = PlayerPrefs.GetInt("AntiAliasingPreference");
        else
            aaDropdown.value = 2;
        if (PlayerPrefs.HasKey("FullscreenPreference"))
            //Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        //else
            Screen.fullScreen = true;
    }
}
