using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SP_Slider : MonoBehaviour
{
    public Player_State PlayerState;
    public float sliderValue;
    public float spRegenTime;
    public float currentTime;
    private UnityEngine.UI.Slider spSlider;

    private void Awake()
    {
        spSlider = GetComponent<UnityEngine.UI.Slider>();
    }
    public void Update() {
        if (PlayerState == null || PlayerState.PlayerData == null) return;
        spRegenTime = PlayerState.PlayerData.regenTimeSP;
        currentTime= PlayerState.regenTimeRemainSP;
        if (spRegenTime > 0)
        {
            spSlider.value = currentTime / spRegenTime;
        }
    }
}
