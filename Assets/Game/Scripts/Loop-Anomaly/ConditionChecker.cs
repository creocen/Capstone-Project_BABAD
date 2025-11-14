using UnityEngine;

namespace Minigame.Loop_Anomaly
{
    public class ConditionChecker : MonoBehaviour
    {
        public static ConditionChecker Instance { get; private set; }

        [Header("Flags")]
        [SerializeField] private bool foundAnomaly = false;
        [SerializeField] private bool hasAnomalyInScene = false;
        [SerializeField] private bool wentBackCorrectly = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            } 
            else
            {
                Destroy(this);
                Debug.LogWarning("Duplicate instance found, bozo :p");
            }
        }

        public void SetFoundAnomaly(bool value)
        {
            foundAnomaly = value;
        }

        public void SetWentBackCorrectly(bool value)
        {
            wentBackCorrectly = value;
        }

        public bool CanProgress(bool isForwardExit)
        {
            if (hasAnomalyInScene)
            {
                if (isForwardExit && foundAnomaly)
                {
                    // CAN PROGRESS : has anomaly but player identified anomaly
                    return true;
                }
                else if (isForwardExit)
                {
                    
                    // CANNOT PROGRESS : can't go forward when anomaly is present
                    return false;
                }
                else
                {
                    // CAN PROGRESS : current scene has (non-interactable) anomaly
                    wentBackCorrectly = true;
                    return true;
                }
            } 
            else
            {
                if (isForwardExit)
                {
                    // CAN PROGRESS : no anomaly so slay
                    return true;
                }
                else
                {
                    // CANNOT PROGRESS : bozo, there is no anomaly, why go back? :pensive:
                    return false;
                }
            } 
        }

        public void ResetFlags()
        {
            foundAnomaly = false;
            wentBackCorrectly = false;
        }
    }
}
