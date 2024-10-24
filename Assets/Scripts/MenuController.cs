using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] TMP_Text CubesAmount;

    private void Start()
    {
        CubesAmount.text = SaveLoader.SaveResources.Blocks.ToString();
    }

    public void LoadMainGame()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenButton(GameObject Panel)
    {
        Panel.SetActive(!Panel.activeInHierarchy);
    }

    public void ChallengeStart(int number)
    { 
    
    }
}
