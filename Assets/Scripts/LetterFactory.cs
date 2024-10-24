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
        AddLetters('�', 1097);
        AddLetters('�', 845);
        AddLetters('�', 801);
        AddLetters('�', 735);
        AddLetters('�', 670);
        AddLetters('�', 626);
        AddLetters('�', 547);
        AddLetters('�', 473);
        AddLetters('�', 454);
        AddLetters('�', 440);
        AddLetters('�', 349);
        AddLetters('�', 321);
        AddLetters('�', 298);
        AddLetters('�', 281);
        AddLetters('�', 262);
        AddLetters('�', 201);
        AddLetters('�', 190);
        AddLetters('�', 174);
        AddLetters('�', 170);
        AddLetters('�', 165);
        AddLetters('�', 159);
        AddLetters('�', 144);
        AddLetters('�', 121);
        AddLetters('�', 97);
        AddLetters('�', 94);
        AddLetters('�', 73);
        AddLetters('�', 64);
        AddLetters('�', 48);
        AddLetters('�', 32);
        AddLetters('�', 36);
        AddLetters('�', 26);
        AddLetters('�', 4);
        AddLetters('�', 4);
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
