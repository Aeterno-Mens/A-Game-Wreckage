using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System.IO;

public class MenuController : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    public int resolutionIndex;

    [Header("Save&Load")]
    SaveSettings save = new SaveSettings();

    [Header("GameParameters")]
    [SerializeField] bool bot;
    [SerializeField] int map;
    [SerializeField] int levelToLoad;
    [Header("Loading Screen")]
    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private Slider LoadingBarFill;
    private void Start()
    {
        if (File.Exists(Application.dataPath + "/settings.json"))
        {
            string text = File.ReadAllText(Application.dataPath + "/settings.json");
            SaveSettings load = JsonUtility.FromJson<SaveSettings>(text);
            AudioListener.volume = load.masterVolume;
            volumeSlider.value = load.masterVolume;
            volumeTextValue.text = load.masterVolume.ToString("0.0");
            Screen.SetResolution((int)load.width, (int)load.height, Screen.fullScreen);
        }
        resolutions = Screen.resolutions.Where(resolution => (resolution.refreshRate == 60 && (resolution.width == 800 || (resolution.width == 1280 && resolution.height == 1024) || resolution.width == 1920))).ToArray();
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
    }

    public void SetResolution(int index)
    {
        resolutionIndex = index;
    }

    public void GraphicsApply()
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
        save.masterVolume = AudioListener.volume;
        save.width = resolutions[resolutionIndex].width;
        save.height = resolutions[resolutionIndex].height;
        string json = JsonUtility.ToJson(save);
        File.WriteAllText(Application.dataPath + "/settings.json", json);
        StartCoroutine(ConfirmationBox());
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        save.masterVolume = AudioListener.volume;
        save.width = Screen.width;
        save.height = Screen.height;
        string json = JsonUtility.ToJson(save);
        File.WriteAllText(Application.dataPath + "/settings.json", json);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }

        if(MenuType == "Graphics")
        {
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

    public void Game_Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBarFill.value = progressValue;
            yield return null;
        }
    }
    private class SaveSettings{
        public float masterVolume;
        public int width;
        public int height;
    }
}


