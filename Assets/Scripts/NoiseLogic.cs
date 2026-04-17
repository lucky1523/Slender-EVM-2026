using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class NoiseLogic : MonoBehaviour
{
    public RawImage noiseImage;
    public Transform slenderTransform;
    public VideoPlayer noiseVideoPlayer;
    private Camera mainCamera;
    public SlenderMan slenderman;


    private float maxNoiseAlpha = 0.8f;
    private float increaseRate = 1.0f;
    private float decreaseRate = 0.5f;

    private float viewOfCamera = 0.85f;
    private float currentNoiseAlpha = 0f;


    void Start()
    {
        mainCamera = Camera.main;
        if (noiseImage == null || noiseVideoPlayer == null || slenderTransform == null)
        {
            enabled = false;
            return;
        }
        Color color = noiseImage.color;
        color.a = 0f;
        noiseImage.color = color;
    }

    void Update()
    {
        Debug.Log("Slenderman Visible: " + slenderman.IsVisible());
        Debug.Log("Looking at Slenderman: " + ChechIfLookingAtSlender());
        if (slenderman.IsVisible())
        {
            if (ChechIfLookingAtSlender())
            {
                currentNoiseAlpha += increaseRate * Time.deltaTime;
            }
            else
            {
                currentNoiseAlpha -= decreaseRate * Time.deltaTime;
            }
            currentNoiseAlpha = Mathf.Clamp01(currentNoiseAlpha);
        }
        
        ApplyAlphaFilter();

    }

    void ApplyAlphaFilter()
    {
        float finalAlpha = currentNoiseAlpha * maxNoiseAlpha;
        Color color = noiseImage.color;
        color.a = finalAlpha;
        noiseImage.color = color;
    }

    bool ChechIfLookingAtSlender()
    {
        Vector3 directionToSlender = (slenderTransform.position - mainCamera.transform.position).normalized;
        Vector3 cameraForward = mainCamera.transform.forward;
        float dotProduct = Vector3.Dot(cameraForward, directionToSlender);
        return dotProduct > viewOfCamera;
    }
}
