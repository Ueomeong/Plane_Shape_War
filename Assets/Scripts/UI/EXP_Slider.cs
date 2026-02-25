using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EXP_Slider : MonoBehaviour
{
    public PlayerLevelData playerLevelData;
    public float sliderValue;
    public int max_EXP;
    public int current_EXP;
    private UnityEngine.UI.Slider expSlider;
    [SerializeField] private UnityEngine.UI.Image fillImage;
    private Color Empty_Color;
    private Color LevelUp_Color;
    private Color General_Color;
    private void Awake()
    {
        expSlider = GetComponent<UnityEngine.UI.Slider>();
        Empty_Color = new Color(0, 1f, 1f);
        LevelUp_Color = new Color(0.9f,1f,0f);
    }
    public void SliderAdjust(int current_EXP, int max_EXP)
    {
        expSlider.value = (float)current_EXP / max_EXP;
        General_Color = Color.Lerp(Empty_Color, LevelUp_Color, expSlider.value);
        if (expSlider.value == 1)
        {
            fillImage.color = LevelUp_Color;
        }
        else
        {
            fillImage.color = General_Color;
        }
    }
}
