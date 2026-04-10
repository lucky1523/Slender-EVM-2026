using UnityEngine;
using UnityEngine.AI;

public class SlenderMan : MonoBehaviour
{
    private NavMeshAgent navMeshAgentSlender;
    private PlayerMovement player;
    private SkinnedMeshRenderer slenderMeshRenderer;
    private Animator slenderAnimator;

    private float baseSpeed = 0.5f;


    void Start()
    {
        navMeshAgentSlender = GetComponent<NavMeshAgent>();
        player = FindAnyObjectByType<PlayerMovement>();
        slenderMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        slenderAnimator = GetComponent<Animator>();

        navMeshAgentSlender.speed = baseSpeed;

    }

    void Update()
    {
        navMeshAgentSlender.destination = player.transform.position;

        float currentVelocity = navMeshAgentSlender.velocity.magnitude;
        slenderAnimator.SetFloat("speed", currentVelocity);
        cambiarDificultad();

    }

    public void cambiarDificultad()
    {
        int notas = GameManager.Instance.GetNotesCount();
        Debug.Log("Notas recogidas: " + notas);
        navMeshAgentSlender.speed = baseSpeed + (notas * 0.5f);
        Debug.Log("Velocidad actual: " + navMeshAgentSlender.speed);

    }
}
