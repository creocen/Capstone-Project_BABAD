using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterSelect : MonoBehaviour
{
    public void LoadSideScrolling()
    {
        Debug.Log("Loading SideScrolling");
        SceneManager.LoadScene("SideScrolling");
    }

    public void LoadFruitCatcher()
    {
        Debug.Log("Loading FruitCatcher");
        SceneManager.LoadScene("FruitCatcher");
    }

    public void LoadPingPong()
    {
        Debug.Log("Loading PingPong");
        SceneManager.LoadScene("PingPong");
    }

    public void LoadTowerBuilding()
    {
        Debug.Log("Loading TowerBuilding");
        SceneManager.LoadScene("TowerBuilding");
    }

    public void LoadAnomaly()
    {
        Debug.Log("Loading Anomaly");
        SceneManager.LoadScene("Anomaly");
    }

    public void Back()
    {
        Debug.Log("Loading Title Menu");
        SceneManager.LoadScene("TitleScreen");
    }

}
