using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minigame.Loop_Anomaly
{
    public class BackwardExit : MonoBehaviour
    {
        [Header("LoadScene Config")]
        [SerializeField] private string nextScene;
        [SerializeField] private string baseScene;

        private string playerTag = "Player";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;
            if (ConditionChecker.Instance == null) return;

            bool canProgress = ConditionChecker.Instance.CanProgress(isForwardExit: false);

            if (canProgress)
            {
                LoadNextScene();
            }
            else
            {
                LoadBaseScene();
            }
        }

        private void LoadNextScene()
        {
            SceneManager.LoadScene(nextScene);
        }

        private void LoadBaseScene()
        {
            SceneManager.LoadScene(baseScene);
        }
    }
}