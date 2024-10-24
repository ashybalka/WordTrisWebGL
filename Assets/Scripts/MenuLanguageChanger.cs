using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLanguageChanger : MonoBehaviour
{
    public void ChangeLanguage(string Language)
    {
        SaveLoader.SaveResources.Language = Language;
        SaveLoader.SaveToJson();
        Destroy(GameObject.Find("LetterFactory"));
        SceneManager.LoadScene(0);
    }
}
