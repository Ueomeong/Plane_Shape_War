using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GoalText : MonoBehaviour
{
    private TextMeshProUGUI spawnerLeftText;
    private void Awake()
    {
        spawnerLeftText = GetComponent<TextMeshProUGUI>();
    }
    public void goalTextUpdate()
    {
        spawnerLeftText.text = $"Spawners Left: {InGameManager.Instance.totalSpawnerCount - InGameManager.Instance.currentSpawnerCount}";
        return;
    }
}
