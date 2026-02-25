using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class AugmentManager : MonoBehaviour
{
    [Header("AugmentSlots")]
    public AugmentSlot[] augmentSlots;
    [Header("Silver Augment Data")]
    public List<AugmentData> Silver_Augments;
    [Header("Gold Augment Data")]
    public List<AugmentData> Gold_Augments;
    [Header("Platinum Augment Data")]
    public List<AugmentData> Platinum_Augments;
    [Header("Probability for Augments")]

    public int SilverProb = 30;
    public int GoldProb = 40;
    public int PlatProb = 30;


    private void OnEnable()
    {
        // 패널이 켜질 때마다 자동으로 카드를 섞고 배치합니다.
        ShowRandomAugments();
    }
    public void ShowRandomAugments()
    {
        AugmentTier selectedTier = RandomSelectAugmentTier();
        List<AugmentData> targetList = null;
        switch (selectedTier)
        {
            case AugmentTier.Silver: targetList = Silver_Augments; break;
            case AugmentTier.Gold: targetList = Gold_Augments; break;
            case AugmentTier.Platinum: targetList = Platinum_Augments; break;
        }

        List<AugmentData> availablePool = new List<AugmentData>();
        if (targetList != null)//선택 가능한 증강 모으기
        {
            foreach (var augment in targetList)
            {
                // "중복 가능"하거나, "아직 획득한 적 없는" 증강만 후보에 넣음
                if (!augment.isNotMultiable || !GameManager.Instance.HasAcquired(augment.augmentName))
                {
                    availablePool.Add(augment);
                }
            }
        }

        for (int i = 0; i < availablePool.Count; i++)
        {
            AugmentData temp = availablePool[i];
            int randomIndex = Random.Range(i, availablePool.Count);
            availablePool[i] = availablePool[randomIndex];
            availablePool[randomIndex] = temp;
        }

        // 5. UI 슬롯에 적용 (최대 3개, 혹은 가능한 개수만큼)
        for (int i = 0; i < augmentSlots.Length; i++)
        {
            if (i < availablePool.Count)
            {
                augmentSlots[i].gameObject.SetActive(true);
                augmentSlots[i].Setup(availablePool[i]);
            }
            else
            {
                // 후보가 부족하면 슬롯 끄기 (예: 남은 증강이 1개뿐일 때)
                augmentSlots[i].gameObject.SetActive(false);
            }
        }

    }

    public AugmentTier RandomSelectAugmentTier()
    {
        int rValue = Random.Range(0, 100);

        if (rValue < SilverProb)
        {
            return AugmentTier.Silver;
        }
        else if (rValue < GoldProb + SilverProb)
        {
            return AugmentTier.Gold;
        }
        else 
        {
            return AugmentTier.Platinum;
        }
    }
}
