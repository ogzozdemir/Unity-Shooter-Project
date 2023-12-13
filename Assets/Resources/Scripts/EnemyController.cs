using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("References")] [Space]
    [SerializeField] private Transform player;
    [SerializeField] private Transform gunBarrel;
    [SerializeField] private GameObject enemyBullet;
    private NavMeshAgent enemy;
    private Animator animator;
    private AudioSource audioSource;

    [Header("Enemy Variables")] [Space]
    public bool isAlerted;
    [SerializeField] private bool isAlive;
    public int enemyHealth;
    [SerializeField] private float seeDistance;
    [SerializeField] private float shootCooldown;
    private float shootCooldownDefault;
    [SerializeField] private int damage;
    [SerializeField] private GameObject[] connectedEnemies;

    private void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        isAlive = true;
        isAlerted = false;
        enemy.updateRotation = false;

        shootCooldownDefault = shootCooldown;
    }

    private void Update()
    {
        if (!isAlerted)
        {
            if (Vector3.Distance(player.position, transform.position) < seeDistance)
                GoAlerted();
        }
        
        if (enemyHealth <= 0 && isAlive)
            Death(UnityEngine.Random.Range(0,2));
    }

    private void FixedUpdate()
    {
        if (isAlerted && PlayerController.instance.playerHealth > 0)
        {
            Turn(player.position);
                
            enemy.SetDestination(player.position);
            
            if (enemy.remainingDistance <= enemy.stoppingDistance)
            {
                if (!enemy.hasPath || enemy.velocity.sqrMagnitude == 0f)
                {
                    if (animator.GetBool("isRunning"))
                        animator.SetBool("isRunning", false);
                    
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Pistol Idle"))
                        animator.Play("Pistol Idle");

                    if (shootCooldown <= 0f)
                        Shoot();
                    else
                        shootCooldown -= Time.deltaTime;
                }
            }
            else
            {
                if (!animator.GetBool("isRunning"))
                    animator.SetBool("isRunning", true);
            }
        }
        else if (isAlerted && PlayerController.instance.playerHealth <= 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                animator.Play("Idle");
        }
    }

    private void Shoot()
    {
        audioSource.Play();

        GameObject bullet = ObjectPooling.instance.GetPooledEnemyBulletObject();
        bullet.transform.position = gunBarrel.position;
        bullet.transform.rotation = gunBarrel.transform.rotation;
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.GetComponent<Rigidbody>().transform.forward * 1500f);
        bullet.GetComponent<EnemyBulletController>().damage = damage;

        shootCooldown = shootCooldownDefault;
    }
    
    private void Turn(Vector3 destination) {
        if ((destination - transform.position).magnitude < 0.1f) return; 
    
        Vector3 direction = (destination - transform.position).normalized;
        Quaternion  qDir= Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, qDir, Time.deltaTime * 20f);
    }
    
    private void Death(int animation)
    {
        GameController.instance.EnemyKilled();
        
        if (animation == 0)
            animator.Play("Death 1");
        else
            animator.Play("Death 2");

        GetComponent<EnemyController>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        enemy.enabled = false;
    }

    public void GoAlerted()
    {
        if (!isAlerted)
        {
            isAlerted = true;

            for (int i = 0; i < connectedEnemies.Length; i++)
                connectedEnemies[i].GetComponent<EnemyController>().isAlerted = true;
        }
    }
}
