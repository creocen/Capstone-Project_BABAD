using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    private void Start()
    {
    }

    public void NewGame() {

       // SceneManager.LoadScene("Gameplay");
       //I'd change this to index but maybe when we get 
    }

    public void ChapterSelect()
    {
        SceneManager.LoadScene("ChapterSelect");
    }

    public void Settings()
    {

        SceneManager.LoadScene("Settings");
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
