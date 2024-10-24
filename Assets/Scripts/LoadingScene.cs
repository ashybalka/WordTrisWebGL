using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    void Start()
    {
        SaveLoader.LoadFromJson();
        SaveLoader.SaveToJson();
    }

    public void CheckLanguage()
    {
        if (SaveLoader.SaveResources.Language == string.Empty)
        {
            //LanguagePanel.SetActive(true);
        }
        else
        {
            //LanguagePanel.SetActive(false);
        }
    }
}
