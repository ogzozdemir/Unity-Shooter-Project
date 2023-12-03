using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    
    [SerializeField] private int fps;
    [SerializeField] private float gravityScale = -30f;
    
    private int enemiesTotal;
    [HideInInspector] public int enemiesKilled;

    private bool death;
    private bool win;

    private void Awake()
    {
        if (instance == null) instance = this;
        
        Application.targetFrameRate = fps; 
        Physics.gravity = new Vector3(0, gravityScale, 0);
    }

    private void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesTotal = enemies.Length;
    }

    private void Update()
    {
        if (PlayerController.instance.playerHealth <= 0 && !death)
        {
            death = true;
            Death();
        }
        if (enemiesKilled >= enemiesTotal && !win)
        {
            win = true;
            Invoke("Win", 1f);
        }
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        UIManager.instance.UpdateEnemyCount(enemiesKilled, enemiesTotal);
    }

    public void Win()
    {
        UIManager.instance.Win();

        if (PlayerController.instance)
        {
            Destroy(PlayerController.instance.GetComponent<Rigidbody>());
            foreach (var boxCollider in PlayerController.instance.GetComponents<BoxCollider>()) Destroy(boxCollider);
            Destroy(PlayerController.instance.GetComponent<PlayerController>());
            Destroy(PlayerController.instance.weaponCamera);
        }
        
    }
    
    private void Death()
    {
        UIManager.instance.Death();

        if (PlayerController.instance)
        {
            PlayerController.instance.transform.position = new Vector3(PlayerController.instance.transform.position.x,
                0.05f, PlayerController.instance.transform.position.z);
            Destroy(PlayerController.instance.GetComponent<Rigidbody>());
            foreach (var boxCollider in PlayerController.instance.GetComponents<BoxCollider>()) Destroy(boxCollider);
            Destroy(PlayerController.instance.GetComponent<PlayerController>());
            Destroy(PlayerController.instance.weaponCamera);
        }
    }
}
