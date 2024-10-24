using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool spawnLetter = true;

    [SerializeField] GameObject LetterPrefab, Letters, BombPrefab;

    private LetterFactory letterFactory;
    private RayController rayController;

    public int level = 0;

    [SerializeField] TMP_Text levelText, scoreText, nextLetter;
    [SerializeField] Image nextImage;

    [SerializeField] GameObject WordPanelPrefab, WordPanelContext;

    [SerializeField] ScrollRect scrollRect;

    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject Swiper;

    private char nextLetterChar;

    private int score = 0;

    private bool isCorutineRunning = false;
    public bool isGameOver = false;

    public int minWordLenght = 2;

    [SerializeField] GameObject BombExplodePrefab;

    [SerializeField] AudioClip EraseSound, FallSound;

    private int inGameBlocks = 0;


    void Start()
    {
        rayController = GameObject.Find("RayCasters").GetComponent<RayController>();
        letterFactory = GameObject.Find("LetterFactory").GetComponent<LetterFactory>();

        scoreText.text = score.ToString();
        levelText.text = level.ToString();

        nextLetterChar = letterFactory.GetRandomLetter();
        nextLetter.text = nextLetterChar.ToString();

        StartCoroutine(LineEraser());
    }

    private void Update()
    {
        level = (int)Math.Floor(Time.timeSinceLevelLoad / 30);
        levelText.text = level.ToString();
    }

    public void FalingRutine()
    {
        gameObject.GetComponent<AudioSource>().clip = FallSound;
        gameObject.GetComponent<AudioSource>().Play();
        StartCoroutine(LineEraser());
    }


    public int CheckFalingFigures()
    {
        int falling = 0;
        foreach (var figure in Letters.GetComponentsInChildren<FigureController>())
            {
            if (figure.isFalling)
                { 
                    falling++;
                }
            }
        return falling;
    }

    public void GameOver()
    {
        isGameOver = true;
        GameOverPanel.SetActive(true);
        Swiper.SetActive(false);
    }

    public void ExplodeBomb(Vector3 bombPosition)
    {
        Vector2[] offsets = new Vector2[]
        {
            new(0, -1), new(-1, -1), new(1, -1),
            new(-1, 0), new(1, 0), new(-1, 1),
            new(1, 1), new(0, 0)
        };

        foreach (var offset in offsets)
        {
            FigureController highlighFigure = Letters.GetComponentsInChildren<FigureController>()
                .Where(n => n.transform.position.x == bombPosition.x + offset.x
                         && n.transform.position.y == bombPosition.y + offset.y)
                .FirstOrDefault();

            if (highlighFigure != null)
            {
                Destroy(highlighFigure.gameObject);
            }
        }

        StartCoroutine(BombExlodeRutine(bombPosition));


        LetterSpawner();
    }

    IEnumerator BombExlodeRutine(Vector3 bombPosition)
    {
        var bombExplode = Instantiate(BombExplodePrefab);
        bombExplode.transform.position = bombPosition;
        yield return new WaitForSeconds(2);
        Destroy(bombExplode);
    }



    IEnumerator LineEraser()
    {
        if (!isCorutineRunning && !isGameOver)
        {
            isCorutineRunning = true;

            if (CheckFalingFigures() != 0)
            {
                yield return new WaitForSeconds(0.5f);
                isCorutineRunning = false;
                StartCoroutine(LineEraser());
            }
            else
            {
                rayController.GetRaysToLetters();
                if (FindDictionaryWords() != 0)
                {
                    gameObject.GetComponent<AudioSource>().clip = EraseSound;
                    gameObject.GetComponent<AudioSource>().Play();
                    yield return new WaitForSeconds(0.5f);
                    isCorutineRunning = false;
                    StartCoroutine(LineEraser());
                }
                else
                {
                    yield return new WaitForSeconds(0.5f);
                    isCorutineRunning = false;
                    LetterSpawner();
                }
            }
        }
    }


    public int FindDictionaryWords()
    {
        int count = 0;
        foreach (string wordsLine in rayController.currentGameWords)
        {
            if (letterFactory.WordInDictionary(wordsLine))
            {
                score += wordsLine.Length;
                scoreText.text = score.ToString();
                Debug.Log("Найдено слово " + wordsLine + " Длинной " + wordsLine.Length);
                GetCubes(wordsLine.Length);
                // Tut zapolniaem panel
                Wordpanel(wordsLine);
                count++;

                List<int> positions = rayController.PositionFinder(wordsLine);

                for (int i = 0; i < positions.Count; i += 2)
                {
                    FigureController highlighFigure = Letters.GetComponentsInChildren<FigureController>()
                        .Where(n => n.transform.position.x == (positions[i] - 4) 
                        && n.transform.position.y == (4.5f - positions[i + 1])).First();
                    Destroy(highlighFigure.gameObject);
                }
            }
        }
        Debug.Log("Count: " + count);
        return count;
    }

    public void Wordpanel(string text)
    {
        GameObject wordPanelItem = Instantiate(WordPanelPrefab);
        wordPanelItem.GetComponentInChildren<TMP_Text>().text = text;
        wordPanelItem.transform.SetParent(WordPanelContext.transform, false);

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0;
        Canvas.ForceUpdateCanvases();
    }

    public void LetterSpawner()
    {
        float bombChance = UnityEngine.Random.Range(0, 100);
        GameObject newLetter;

        if (nextLetterChar == ' ')
        {
            newLetter = Instantiate(BombPrefab);
            newLetter.transform.position = new Vector2(0f, 6.5f);
            newLetter.transform.SetParent(Letters.transform, false);
            new WaitForSeconds(0.2f);
        }

        else
        {
            newLetter = Instantiate(LetterPrefab);
            newLetter.transform.position = new Vector2(0f, 6.5f);
            newLetter.GetComponent<FigureController>().figureChar = nextLetterChar;
            newLetter.GetComponent<FigureController>().SetCharToTMP(nextLetterChar);
            newLetter.transform.SetParent(Letters.transform, false);
            new WaitForSeconds(0.2f);
        }

        if (bombChance < 5)
        {
            nextLetter.text = "";
            nextLetterChar = ' ';
            nextImage.gameObject.SetActive(true);
        }
        else
        {
            nextImage.gameObject.SetActive(false);
            nextLetterChar = letterFactory.GetRandomLetter();
            nextLetter.text = nextLetterChar.ToString();
        }
    }


    public void GetCubes(int blocks)
    {
        SaveLoader.SaveResources.Blocks += blocks;
        inGameBlocks += blocks;
        SaveLoader.SaveToJson();
    }
}
