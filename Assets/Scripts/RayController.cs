using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;


public class RayController : MonoBehaviour
{
    public List<string> currentGameWords = new();
    public char[,] gameField = new char[9,9];

    [SerializeField] GameObject Letters;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    public void GetRaysToLetters()
    {
        currentGameWords.Clear();

        //Initialize empty char array
        for (int i = 0; i < gameField.GetLength(0); i++)
        {
            for (int j = 0; j < gameField.GetLength(1); j++)
            {
                gameField[i, j] = ' ';
            }
        }

        // Find All Letters
        foreach (var letterFigure in Letters.GetComponentsInChildren<BoxCollider2D>().Where(n => n.transform.position.y< 3))
        {
            var pos = letterFigure.transform.position;
            gameField[(int)(pos.x + 4f), (int)(4.5f - pos.y)] = letterFigure.GetComponent<FigureController>().figureChar;
        }

        // Find All Substrings
        currentGameWords = FindSubstrings(gameField);
    }

    public static string Reverse(string s)
    {
        return new string(s.Reverse().ToArray());
    }


    public static List<string> FindSubstrings(char[,] array)
    {
        HashSet<string> substrings = new HashSet<string>();

        // Слева направо и справа налево
        for (int i = 0; i < array.GetLength(0); i++)
        {
            string substring = "";

            for (int j = 0; j < array.GetLength(1); j++)
            {
                substring += array[i, j];
            }

            foreach (var str in GetAllSubstringsWithoutSpaces(substring, 2))
            {
                substrings.Add(str);
                substrings.Add(new string(Reverse(str)));
            }
        }

        // Сверху вниз и снизу вверх
        for (int j = 0; j < array.GetLength(1); j++)
        {
            string substring = "";
            for (int i = 0; i < array.GetLength(0); i++)
            {
                substring += array[i, j];
            }

            foreach (var str in GetAllSubstringsWithoutSpaces(substring, 2))
            {
                substrings.Add(str);
                substrings.Add(new string(Reverse(str)));
            }
        }

        return substrings.ToList();
    }

    public List<int> PositionFinder(string word)
    {
        return FindSubstringPositions(gameField, word);
    }

    public static List<int> FindSubstringPositions(char[,] array, string substring)
    {
        List<int> positions = new List<int>();

        // Поиск слева направо
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j <= array.GetLength(1) - substring.Length; j++)
            {
                bool match = true;
                for (int k = 0; k < substring.Length; k++)
                {
                    if (array[i, j + k] != substring[k])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    for (int k = 0; k < substring.Length; k++)
                    {
                        positions.Add(i);
                        positions.Add(j + k);
                    }
                }
            }

        }

        // Поиск сверху вниз
        for (int j = 0; j < array.GetLength(1); j++)
        {
            for (int i = 0; i <= array.GetLength(0) - substring.Length; i++)
            {
                bool match = true;
                for (int k = 0; k < substring.Length; k++)
                {
                    if (array[i + k, j] != substring[k])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    for (int k = 0; k < substring.Length; k++)
                    {
                        positions.Add(i + k);
                        positions.Add(j);
                    }
                }
            }
        }

        
        // Поиск справа налево
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = array.GetLength(1) - 1; j >= substring.Length - 1; j--)
            {
                bool match = true;
                for (int k = 0; k < substring.Length; k++)
                {
                    if (array[i, j - k] != substring[k])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    for (int k = 0; k < substring.Length; k++)
                    {
                        positions.Add(i);
                        positions.Add(j - k);
                    }
                }
            }
        }

        // Поиск снизу вверх
        for (int j = 0; j < array.GetLength(1); j++)
        {
            for (int i = array.GetLength(0) - 1; i >= substring.Length - 1; i--)
            {
                bool match = true;
                for (int k = 0; k < substring.Length; k++)
                {
                    if (array[i - k, j] != substring[k])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    for (int k = 0; k < substring.Length; k++)
                    {
                        positions.Add(i - k);
                        positions.Add(j);
                    }
                }
            }
        }

        return positions;
    }

    public static List<string> GetAllSubstringsWithoutSpaces(string input, int lenght)
    {
        List<string> substrings = new List<string>();

        // Проход по всем символам строки
        for (int i = 0; i < input.Length; i++)
        {
            // Сбор подстрок начиная с позиции i
            for (int j = i + 1; j <= input.Length; j++)
            {
                string substring = input.Substring(i, j - i);

                // Добавляем только подстроки длиной больше 1 символа и без пробелов
                if (substring.Length > lenght && !substring.Contains(' '))
                {
                    substrings.Add(substring);
                }
            }
        }

        return substrings;
    }
}
