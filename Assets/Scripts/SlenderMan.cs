using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SlenderMan : MonoBehaviour
{
    private NavMeshAgent navMeshAgentSlender;
    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    private SkinnedMeshRenderer slenderMeshRenderer;
    private Animator slenderAnimator;

    private float baseSpeed = 0.5f;
    private float catchDistance = 2f;
    private bool isGameOver = false;

    public AudioClip slenderSound;
    private AudioSource audioSource;

    public Transform[] teleportDestinations;
    private bool isTeleporting = false;


    void Start()
    {
        navMeshAgentSlender = GetComponent<NavMeshAgent>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        playerLook = FindAnyObjectByType<PlayerLook>();
        slenderMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        slenderAnimator = GetComponent<Animator>();

        navMeshAgentSlender.speed = baseSpeed;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = slenderSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        // Ajustes para Sonido 3D
        audioSource.spatialBlend = 1.0f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 2.0f;
        audioSource.maxDistance = 10.0f;
        audioSource.spatialize = true;

        CambiarDificultad();

    }

    void Update()
    {
        if (isGameOver) return;
        if (navMeshAgentSlender.enabled)
        {
            navMeshAgentSlender.destination = playerMovement.transform.position;
            float currentVelocity = navMeshAgentSlender.velocity.magnitude;
            slenderAnimator.SetFloat("speed", currentVelocity);
            VerificarDistanciaConJugador();
        }
        float distanceToPlayer = Vector3.Distance(transform.position, playerMovement.transform.position);

        if (!isTeleporting && distanceToPlayer > 25f)
        {
            Debug.Log("ENTRE A TELEPORT EN UPDATE()");
            StartCoroutine(Teleport());
        }
        CambiarDificultad();

    }

    public void VerificarDistanciaConJugador()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerMovement.transform.position);
        if (distanceToPlayer <= catchDistance)
        {
            AtraparJugador();
        }
    }

    public void AtraparJugador()
    {
        isGameOver = true;
        // Detener a Slender
        navMeshAgentSlender.isStopped = true;
        navMeshAgentSlender.velocity = Vector3.zero;

        // Hacer que Slender mire al jugador
        Vector3 slenderlookAt = new Vector3(playerMovement.transform.position.x, transform.position.y, playerMovement.transform.position.z);
        transform.LookAt(slenderlookAt);
        slenderAnimator.SetTrigger("jumpscare");

        // Desactivar controles del jugador
        playerMovement.enabled = false;
        playerLook.enabled = false;

        // Forzar a la cámara del jugador a mirar a la cara de Slender
        Vector3 playerLookAtSlender = transform.position + (Vector3.up * 2f);
        playerLook.playerCamera.LookAt(playerLookAtSlender);
        GameManager.Instance.Invoke(nameof(GameManager.Instance.PerderJuego), 3f);

    }

    public void CambiarDificultad()
    {
        int notas = GameManager.Instance.GetNotesCount();
        if (notas < 1)
        {
            navMeshAgentSlender.enabled = false;
            slenderMeshRenderer.enabled = false;
            if (audioSource.isPlaying) audioSource.Stop();
            return;
        } else
        {
            navMeshAgentSlender.enabled = true;
            slenderMeshRenderer.enabled = true;
            if (!audioSource.isPlaying) audioSource.Play();
        }
        navMeshAgentSlender.speed = baseSpeed + (notas * 0.5f);
        if (notas >= 5)
        {
            slenderAnimator.SetBool("isRunning", true);
            navMeshAgentSlender.acceleration = 12f;
        }

    }

    public bool IsVisible()
    {
        return slenderMeshRenderer.enabled;
    }

    IEnumerator Teleport()
    {
        isTeleporting = true;
        Transform closestPoint = null;
        float minDistance = float.MaxValue;
        for (int i = 0; i < teleportDestinations.Length; i++) 
        {
            
            float distance = Vector3.Distance(teleportDestinations[i].position, playerMovement.transform.position);
            Debug.Log("Distance to point " + i + ": " + distance);

            if (distance > 7f && distance < minDistance)
            {
                minDistance = distance;
                closestPoint = teleportDestinations[i];
                Debug.Log("Closest point is now: " + closestPoint.position);
            }   
        }
        if (closestPoint != null)
        {
            navMeshAgentSlender.Warp(closestPoint.position);
        }
        isTeleporting = false;
        yield return new WaitForSeconds(5f);
    }
}
