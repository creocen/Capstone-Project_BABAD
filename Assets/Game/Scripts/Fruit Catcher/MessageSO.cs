using UnityEngine;
using System.Collections.Generic;

public enum MessageType
{
    Positive,
    Negative
}

[CreateAssetMenu(fileName = "MessageSO", menuName = "ScriptableObjects/FruitCatcher/Message" +
    "")]
public class MessageSO : ScriptableObject
{
    public MessageType messageType;
    public GameObject messagePrefab;
}
