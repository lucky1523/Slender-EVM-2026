using UnityEngine;

public class Note : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private Material[] originalMaterials;
    public Material highlightMaterial;
    private float lookRange = 3f;

    private PlayerLook player;
    private Camera playerCamPosition;
    private bool isLookedAt = false;
    void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        originalMaterials = new Material[meshRenderers.Length];
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            originalMaterials[i] = meshRenderers[i].material;
        }
        player = FindAnyObjectByType<PlayerLook>();
        playerCamPosition = player.GetComponentInChildren<Camera>();

    }

    void Update()
    {
        Ray ray = new Ray(playerCamPosition.transform.position, playerCamPosition.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, lookRange))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                isLookedAt = true;
                Debug.Log("Looking at note" + isLookedAt);
                IsLookedAt(isLookedAt);
            }
            return;

        } else
        {
            isLookedAt = false;
            Debug.Log("Not looking at note" + isLookedAt);
            IsLookedAt(isLookedAt);
        }

    }

    public void IsLookedAt(bool isLookAt)
    {
        isLookedAt = isLookAt;
        if (isLookedAt)
        {
            foreach (MeshRenderer mr in meshRenderers)
            {
                mr.material = highlightMaterial;
            }
        }
        else
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material = originalMaterials[i];
            }
        }
    }

    public void OnCollect()
    {
        Destroy(gameObject);
    }
}
