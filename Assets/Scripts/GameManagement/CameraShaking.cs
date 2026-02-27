using Unity.Cinemachine;
using UnityEngine;
using System.Collections;
public class CameraShaking : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin shake;
    void Awake()
    {
        var cam = GetComponent<CinemachineCamera>();
        if (cam != null)
        {
            shake = cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }
        if (shake != null)
        {
            shake = GetComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }
    public void ShakeCamera(float intensity,float shakingTime)
    {
        if (shake == null)
        {
            return;
        }
        StopAllCoroutines();
        StartCoroutine(ShakeProcess(intensity, shakingTime));
    }

    public IEnumerator ShakeProcess(float intensity, float time)
    {
        shake.AmplitudeGain = intensity;
        yield return new WaitForSeconds(time);
        shake.AmplitudeGain = 0f;
    }
}
