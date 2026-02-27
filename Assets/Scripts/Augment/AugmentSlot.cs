using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AugmentSlot : MonoBehaviour
{
    [Header("UI Components")]
    public Image iconImage;       // 아이콘 이미지
    public TMP_Text nameText;         // 이름 텍스트
    public TMP_Text descText;         // 설명 텍스트
    public Image panelBackground; // (선택사항) 등급에 따라 색을 바꿀 배경 이미지

    public AugmentData _data;    // 현재 담고 있는 증강 데이터

    public void Setup(AugmentData data)
    {
        _data = data;

        if (_data != null)
        {
            iconImage.sprite = _data.icon;
            nameText.text = _data.augmentName;
            descText.text = _data.description;
            UpdateColor(_data.tier);
        }
    }
    public void OnClickSelect()
    {
        if (_data != null)
        {
            _data.ApplyAugment(); // 증강 효과 적용

            GameManager.Instance.AddAcquiredAugment(_data.augmentName);
            InGameManager.Instance.ResumeGame(); // 게임 재개
        }
    }
    private void UpdateColor(AugmentTier tier)
    {
        switch (tier)
        {
            case AugmentTier.Silver: panelBackground.color = Color.gray; break;
            case AugmentTier.Gold: panelBackground.color = Color.yellow; break;
            case AugmentTier.Platinum: panelBackground.color = Color.cyan; break;
        }
    }
}
