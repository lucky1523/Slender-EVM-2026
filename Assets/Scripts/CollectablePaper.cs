using UnityEngine;

public class CollectablePaper : MonoBehaviour
{

    private MeshRenderer[] meshRenderers;
    private Material[] originalMaterials;
    public Material highlightMaterial;
    public float lookRange = 3f;

    private PlayerLook player;
    private Camera playerCam;
    private bool isLookedAt;
    void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        originalMaterials = new Material[meshRenderers.Length];
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            originalMaterials[i] = meshRenderers[i].material;
        }
        player = FindAnyObjectByType<PlayerLook>();
         playerCam = player.GetComponentInChildren<Camera>();

    }

    void Update()
    {
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        if ( Physics.Raycast(ray, out RaycastHit hit, lookRange))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                if (!isLookedAt)
                {
                    isLookedAt = true;
                    SetLookedAt(isLookedAt);
                }
                return;
            }
        }
        if (isLookedAt)
        {
            isLookedAt = false;
            SetLookedAt(isLookedAt);
        }

    }

    void SetLookedAt(bool lookedAt)
    {
        isLookedAt = lookedAt;

        if (lookedAt)
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
}
