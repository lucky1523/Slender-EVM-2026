using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LookAtSlender : MonoBehaviour
{
    public RawImage staticRawImage;         
    public VideoPlayer staticVideoPlayer;    

    public float maxStaticAlpha = 0.8f;     
    public float increaseRate = 1.0f;        
    public float decreaseRate = 0.5f;       

    public Transform slenderTransform;  
    public float viewThreshold = 0.85f;

    private Camera mainCamera;
    private float currentStaticAmount = 0f;

    void Start()
    {
        mainCamera = Camera.main;

        if (staticRawImage == null || staticVideoPlayer == null || slenderTransform == null)
        {
            enabled = false;
        }

        Color color = staticRawImage.color;
        color.a = 0f;
        staticRawImage.color = color;
    }

    void Update()
    {
        bool isLookingAtSlender = CheckIfLookingAtSlender();

        if (isLookingAtSlender)
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

    bool CheckIfLookingAtSlender()
    {
        Vector3 directionToSlender = (slenderTransform.position - mainCamera.transform.position).normalized;

        Vector3 cameraForward = mainCamera.transform.forward;

        float dotProduct = Vector3.Dot(cameraForward, directionToSlender);

        return dotProduct > viewThreshold;
    }

    void AplicarEfectoVisual()
    {
        float finalAlpha = currentStaticAmount * maxStaticAlpha;

        Color c = staticRawImage.color;
        c.a = finalAlpha;
        staticRawImage.color = c;

    }
}