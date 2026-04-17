using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StaticOnLook : MonoBehaviour
{
    public RawImage staticRawImage;
    public VideoPlayer staticVideoPlayer;
    public Transform slendermanTransform;

    private float maxStaticAlpha = 0.8f;
    private float increaseRate = 1.0f;
    private float decreaseRate = 0.5f;

    private float viewThreshold = 0.85f;
    private Camera mainCamera;
    private float currentStaticAmount = 0.0f;

    void Start()
    {
        mainCamera = Camera.main;
        if (staticRawImage == null || staticVideoPlayer == null || slendermanTransform == null)
        {
            enabled = false;
            return;
        }
        Color color = staticRawImage.color;
        color.a = 0f;
        staticRawImage.color = color;

    }

    
    void Update()
    {
        if (CheckIfLookingAtSlenderman())
        {
            currentStaticAmount += increaseRate * Time.deltaTime;
        }
        else
        {
            currentStaticAmount -= decreaseRate * Time.deltaTime;
        }
        currentStaticAmount = Mathf.Clamp01(currentStaticAmount);
        AplicarEfectoVisual();

    }

    bool CheckIfLookingAtSlenderman()
        {
        Vector3 directionToSlenderman = (slendermanTransform.position - mainCamera.transform.position).normalized;
        Vector3 cameraForward = mainCamera.transform.forward;
        float dotProduct = Vector3.Dot(cameraForward, directionToSlenderman);
        return dotProduct > viewThreshold;

    }

    void AplicarEfectoVisual()
    {
        float finalAlpha = currentStaticAmount * maxStaticAlpha;
        Color color = staticRawImage.color;
        color.a = finalAlpha;
        staticRawImage.color = color;
    }
}
