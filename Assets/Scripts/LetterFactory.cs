using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class LetterFactory : MonoBehaviour
{
    private List<char> weightedAlphabet = new();
    public List<string> dictionary = new();

    public static LetterFactory Instance;
    public bool isLoaded = false;

    [SerializeField] GameObject LanguagePanel;

    [Header("Dictionaries")]
    [SerializeField] TextAsset Dictionary_Ru;
    [SerializeField] TextAsset Dictionary_En;

    [Header("Challenges")]
    [SerializeField] TextAsset ChallengeLettersAsset;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        LanguageSwitcher();
    }

    public void LanguageSwitcher()
    {
        switch (SaveLoader.SaveResources.Language)
        {
            case "Ru-ru":
                RandomLetterRu();
                CreateDictionary(Dictionary_Ru);
                break;
            case "En-en":
                RandomLetterEn();
                CreateDictionary(Dictionary_En);
                break;
            default:
                RandomLetterRu();
                CreateDictionary(Dictionary_Ru);
                break;
        }
    }

    public void RandomLetterRu()
    {
        AddLetters('Î', 1097);
        AddLetters('Å', 845);
        AddLetters('À', 801);
        AddLetters('È', 735);
        AddLetters('Í', 670);
        AddLetters('Ò', 626);
        AddLetters('Ñ', 547);
        AddLetters('Ð', 473);
        AddLetters('Â', 454);
        AddLetters('Ë', 440);
        AddLetters('Ê', 349);
        AddLetters('Ì', 321);
        AddLetters('Ä', 298);
        AddLetters('Ï', 281);
        AddLetters('Ó', 262);
        AddLetters('ß', 201);
        AddLetters('Û', 190);
        AddLetters('Ü', 174);
        AddLetters('Ã', 170);
        AddLetters('Ç', 165);
        AddLetters('Á', 159);
        AddLetters('×', 144);
        AddLetters('É', 121);
        AddLetters('Õ', 97);
        AddLetters('Æ', 94);
        AddLetters('Ø', 73);
        AddLetters('Þ', 64);
        AddLetters('Ö', 48);
        AddLetters('Ý', 32);
        AddLetters('Ù', 36);
        AddLetters('Ô', 26);
        AddLetters('¨', 4);
        AddLetters('Ú', 4);
    }

    public void RandomLetterEn()
    {
        AddLetters('E', 1270);
        AddLetters('T', 905);
        AddLetters('A', 816);
        AddLetters('O', 750);
        AddLetters('I', 696);
        AddLetters('N', 674);
        AddLetters('S', 632);
        AddLetters('H', 609);
        AddLetters('R', 598);
        AddLetters('D', 425);
        AddLetters('L', 402);
        AddLetters('C', 278);
        AddLetters('U', 276);
        AddLetters('M', 240);
        AddLetters('W', 236);
        AddLetters('F', 223);
        AddLetters('G', 201);
        AddLetters('Y', 197);
        AddLetters('P', 193);
        AddLetters('B', 149);
        AddLetters('V', 98);
        AddLetters('K', 77);
        AddLetters('J', 15);
        AddLetters('X', 15);
        AddLetters('Q', 10);
        AddLetters('Z', 7);
    }

    private void AddLetters(char letter, int frequency)
    {
        for (int i = 0; i < frequency; i++)
        {
            weightedAlphabet.Add(letter);
        }
    }

    public char GetRandomLetter()
    {
        int index = UnityEngine.Random.Range(0, weightedAlphabet.Count);
        return weightedAlphabet[index];
    }

    public void CreateDictionary(TextAsset Dictionary_Main)
    {
        dictionary = new List<string>(Dictionary_Main.text.Split(new[] { ',', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries));

        for (int i = 0; i < dictionary.Count; i++)
        {
            dictionary[i] = dictionary[i].Trim(new[] { '{', '}', '\"', ' ' });
        }

        isLoaded = true;

        if (SaveLoader.SaveResources.Language == string.Empty)
        {
            LanguagePanel.SetActive(true);
        }
        else
        {
            LanguagePanel.SetActive(false);
            StartCoroutine(Falling());
        }
    }

    public void SetLanguage(string savedLanguage)
    {
        SaveLoader.SaveResources.Language = savedLanguage;
        SaveLoader.SaveToJson();
        LanguagePanel.SetActive(false);
        LanguageSwitcher();
    }


    public bool WordInDictionary(string word)
    {
        return dictionary.Contains(word.ToLower());
    }

    IEnumerator Falling()
    { 
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(1);
    }
}
