using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class ChallengeController : MonoBehaviour
{
    public string selectedLanguage = "en";
    public List<Challenge> _challenges = new();

    [System.Serializable]
    public class Challenge
    {
        public int number;
        public bool unlocked;
        public bool cleared;
        public string description;
        public List<ChallengeLetters> letters;
    }

    [System.Serializable]
    public class ChallengeLetters
    {
        public char letter;
        public Vector2 position;

        public ChallengeLetters(char letter, Vector2 position)
        {
            this.letter = letter;
            this.position = position;
        }
    }

    // ����� ��� �������� ��������� ��� ����������� ����� �� JSON
    public void LoadChallengesFromJson(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            // �������������� JSON � ������ � ��������� �������
            LanguageWrapper languageWrapper = JsonUtility.FromJson<LanguageWrapper>(json);

            // ��������, ���������� �� ��������� ����
            if (languageWrapper.languages.ContainsKey(selectedLanguage))
            {
                _challenges = new List<Challenge>(languageWrapper.languages[selectedLanguage]);
                Debug.Log("Challenges for language " + selectedLanguage + " loaded successfully.");
            }
            else
            {
                Debug.LogError("Language not found: " + selectedLanguage);
            }
        }
        else
        {
            Debug.LogError("File not found: " + path);
        }
    }

    // ����� ��� ���������� ��������� �� ������ � JSON
    public void SaveChallengesToJson(string path)
    {
        // �������� ������� ��� ������������
        LanguageWrapper languageWrapper = new();
        languageWrapper.languages = new Dictionary<string, List<Challenge>>();
        languageWrapper.languages[selectedLanguage] = _challenges;

        string json = JsonUtility.ToJson(languageWrapper, true);
        File.WriteAllText(path, json);
        Debug.Log("Challenges saved to: " + path);
    }

    // ������� ��� �������� ������ �� ������
    [System.Serializable]
    public class LanguageWrapper
    {
        public Dictionary<string, List<Challenge>> languages; // ������� � ����������� �� ������
    }

    private void Start()
    {
        string path = Application.persistentDataPath + "/challenges.json";
        LoadChallengesFromJson(path); // �������� ������ ��� ������
    }
}
