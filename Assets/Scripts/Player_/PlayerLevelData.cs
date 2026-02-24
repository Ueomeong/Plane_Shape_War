using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "PlayerLevelData", menuName = "Scriptable Objects/PlayerLevelData")]
public class PlayerLevelData : ScriptableObject
{
    public List<int> requiredExp;//레벨업에 필요한 경험치 양

}
