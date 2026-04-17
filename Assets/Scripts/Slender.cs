using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Slender : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    private Animator anim;
    private SkinnedMeshRenderer meshRenderer;

    public AudioClip slenderSound;
    private AudioSource audioSource;
    private bool isTeleporting = false;

    [Header("Configuración de Velocidad")]
    public float baseSpeed = 1.0f;
    public float speedMultiplierPerNote = 1.3f;

    [Header("Ataque")]
    public float catchDistance = 2.0f;
    private bool isGameOver = false;
    public Transform[] destinations;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        playerLook = FindAnyObjectByType<PlayerLook>();
        anim = GetComponentInChildren<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        // CONFIGURACIÓN DEL AUDIO
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

        ActualizarDificultad();
    }

    void Update()
    {
        if (isGameOver) return;

        if (navMeshAgent.enabled)
        {
            navMeshAgent.destination = playerMovement.transform.position;
            float currentVelocity = navMeshAgent.velocity.magnitude;
            anim.SetFloat("speed", currentVelocity);

            CheckDistanceToPlayer();
        }
        float distanceToPlayer = Vector3.Distance(transform.position, playerMovement.transform.position);

        if (distanceToPlayer > 25f && !isTeleporting)
        {
            StartCoroutine(Teleport());
        }
        ActualizarDificultad();
    }

    void CheckDistanceToPlayer()
    {
        float distance = Vector3.Distance(transform.position, playerMovement.transform.position);
        

        if (distance <= catchDistance)
        {
            AtraparJugador();
        }
    }

    void AtraparJugador()
    {
        isGameOver = true; 

        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;

        Vector3 lookAtPos = new Vector3(playerMovement.transform.position.x, transform.position.y, playerMovement.transform.position.z);
        transform.LookAt(lookAtPos);

        anim.SetTrigger("jumpscare");

        playerMovement.enabled = false;
        playerLook.enabled = false;

        Rigidbody playerRigidbody = playerMovement.GetComponent<Rigidbody>();
        playerRigidbody.angularVelocity = Vector3.zero;
        playerRigidbody.isKinematic = true;


        Vector3 slenderFace = transform.position + (Vector3.up * 2.5f); 
        playerLook.playerCamera.LookAt(slenderFace);

        GameManager.Instance.Invoke(nameof(GameManager.Instance.PerderJuego), 4f);

    }


    void ActualizarDificultad()
    {
        int notas = GameManager.Instance.GetNotesCount();

        if (notas < 1)
        {
            meshRenderer.enabled = false;
            navMeshAgent.enabled = false;


            if (audioSource.isPlaying) audioSource.Stop();
            return;
        }
        else
        {
            meshRenderer.enabled = true;
            navMeshAgent.enabled = true;

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        navMeshAgent.speed = baseSpeed + (notas * speedMultiplierPerNote);

        if (notas >= 5)
        {
            anim.SetBool("isRunning", true);
            navMeshAgent.acceleration = 12f;
            audioSource.pitch = 1.2f;
        }
    }

    IEnumerator Teleport()
    {
        isTeleporting = true;

        Transform closestPoint = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < destinations.Length; i++)
        {
            float dist = Vector3.Distance(destinations[i].position, playerMovement.transform.position);

            if (dist > 7f && dist < minDistance)
            {
                minDistance = dist;
                closestPoint = destinations[i];
                Debug.Log("Punto mas cercano es..." + closestPoint.position + destinations[i]);
            }
        }

        if (closestPoint != null)
        {
            navMeshAgent.Warp(closestPoint.position);
            Debug.Log("Punto mas cercano FINAL es..." + closestPoint.position);
        }
        yield return new WaitForSeconds(5f);
        isTeleporting = false;
    }
}