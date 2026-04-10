using UnityEngine;
using UnityEngine.AI;

public class Slender : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private PlayerMovement player;
    private Animator anim;
    private SkinnedMeshRenderer meshRenderer;
    private AudioSource slenderSound;

    private float baseSpeed = 0.5f;
    private float speedMultiplierPerNote = 0.5f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = FindAnyObjectByType<PlayerMovement>();
        anim = GetComponentInChildren<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        slenderSound = GetComponent<AudioSource>();
       

        ActualizarDificultad();
    }

    void Update()
    {
        if (navMeshAgent.enabled)
        {
            navMeshAgent.destination = player.transform.position;

            float currentVelocity = navMeshAgent.velocity.magnitude;
            anim.SetFloat("speed", currentVelocity);
            if (!slenderSound.isPlaying)
            {
                slenderSound.Play();
                slenderSound.loop = true;
            }
        }

        ActualizarDificultad();
    }

    void ActualizarDificultad()
    {
        int notas = GameManager.Instance.GetNotesCount();

        if (notas < 1)
        {
            meshRenderer.enabled = false;
            navMeshAgent.enabled = false; 
            return;
        }
        else
        {
            meshRenderer.enabled = true;
            navMeshAgent.enabled = true;
        }

        navMeshAgent.speed = baseSpeed + (notas * speedMultiplierPerNote);

        if (notas >= 5)
        {
            anim.SetBool("isRunning", true);
            navMeshAgent.acceleration = 12f;
        }
    }
}