using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager instance;

    float dayCycleMinute = 1f;
    public EnvLightingData[] lightingData;
    public Material[] envObjectMaterials;
    public TextMeshPro[] textMeshes;

    
    //[Header("Sounds")]
    //public AudioClip dayCripingSound;
    //public AudioClip nightCripingSound;

    //[Local Variables]
    bool startTime;
    CurrentTimeSpan timeSpan = CurrentTimeSpan.Day;
    enum CurrentTimeSpan
    {
        Day,
        Night
    }

    private void Awake()
    {
        instance = this;
    }
    public void SetEnvironmentLights()
    {
        //GameSoundController.instance.PlayEnvironmentSound(dayCripingSound);
        for(int _outer = 0; _outer < lightingData.Length; _outer++)
        {
            lightingData[_outer].lightMaps = LightmapSettings.lightmaps;
            for (int i = 0; i < lightingData[_outer].lightMaps.Length; i++)
            {
                if (i < lightingData[_outer].lightMapDirs.Length)
                    lightingData[_outer].lightMaps[i].lightmapDir = lightingData[_outer].lightMapDirs[i];
                if (i < lightingData[_outer].lightMapColors.Length)
                    lightingData[_outer].lightMaps[i].lightmapColor = lightingData[_outer].lightMapColors[i];
            }
        }
        ChangeMaps();
    }
    public void StartEnvironmentCycle()
    {
        dayCycleMinute = GameController.instance.dayCycleMinutes;
        startTime = true;
        
    }
    void Update()
    {
        if (!startTime)
        {
            return;
        }
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    UpdateTimeSpan(CurrentTimeSpan.Night);
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    UpdateTimeSpan(CurrentTimeSpan.Day);
        //}
        UpdateGameTime();
    }
   
    void UpdateGameTime()
    {
        GameController.instance.gameData.currentTime += Time.deltaTime;
        float _currentTime = GameController.instance.gameData.currentTime;

        if (_currentTime > (dayCycleMinute * 60f))
        {
            GameController.instance.gameData.currentTime = 0f;
            GameController.instance.gameData.currentDay++;
            _currentTime = 0f;

        }
        CurrentTimeSpan _timeSpan = CurrentTimeSpan.Day;
        //if (GameController.instance.gameData.currentTime > (((dayCycleMinute / 3f) * 2f)*60))
        //{
        //    _timeSpan = CurrentTimeSpan.Night;
        //}
        UpdateTimeSpan(_timeSpan);
        UIController.instance.UpdateGameTime(_currentTime, GameController.instance.gameData.currentDay);
    }
    void UpdateTimeSpan(CurrentTimeSpan _timeSpan)
    {
        if (timeSpan == _timeSpan)
        {
            return;
        }
        timeSpan = _timeSpan;
        ChangeMaps();
    }
    public void ChangeMaps()
    {

        int _timeIndex = timeSpan ==CurrentTimeSpan.Day ? 0 : 1;
        EnvLightingData _lights = lightingData[_timeIndex];
        LightmapSettings.lightmaps = _lights.lightMaps;
        RenderSettings.skybox = _lights.skyboxMaterial;
        //RenderSettings.fogColor = _lights.fogColor;
        print(envObjectMaterials.Length);
        for(int i = 0; i < envObjectMaterials.Length; i++)
        {
            print(_lights.objectsEmissionColor[i].emissionColor);
            envObjectMaterials[i].SetColor("_EmissionColor", _lights.objectsEmissionColor[i].emissionColor);

            /*if (_lights.objectsEmissionColor.Length <= i)
                continue;
            envObjectMaterials[i].SetColor("_EmissionColor", _lights.objectsEmissionColor[i].emissionColor);*/
        }
        for (int i = 0; i < textMeshes.Length; i++)
        {
            print(textMeshes[i]);
            textMeshes[i].GetComponent<MeshRenderer>().material = _lights.textMaterials[i].meshMaterial;
        }
    }
}
[System.Serializable]
public class EnvLightingData
{
    public string timeStatus;
    public Texture2D[] lightMapDirs;
    public Texture2D[] lightMapColors;
    public LightmapData[] lightMaps;
    public Material skyboxMaterial;
    public Color fogColor;
    public ObjectMaterials[] objectsEmissionColor;
    public TextMeshMaterials[] textMaterials;
 //   public ObjectMaterials[] textMeshGlowColors;

}
[System.Serializable]
public class ObjectMaterials
{
    public string materialName;
    public Color emissionColor;
}
[System.Serializable]
public class TextMeshMaterials
{
    public string textMeshName;
    public Material meshMaterial;
}