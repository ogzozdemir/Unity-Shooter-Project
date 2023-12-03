using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    
    [Header("Text References")] [Space]
    [SerializeField] private TMP_Text interactMessage;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text enemiesText;
    
    [Header("Screen References")] [Space]
    [SerializeField] private GameObject aliveScreen;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject hitRed;

    private float waitTime;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        aliveScreen.SetActive(true);
        deathScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    private void Update()
    {
        if (waitTime > 0)
            waitTime -= Time.deltaTime;
        
        if ((deathScreen.activeSelf || winScreen.activeSelf) && waitTime <= 0f)
        {
            if (Input.anyKeyDown)
                SceneManager.LoadScene("Game Scene");
        }
    }

    public void UpdateEnemyCount(int enemiesKilled, int enemiesTotal)
    {
        enemiesText.text = "Kill them all! (" + enemiesKilled + "/" + enemiesTotal + ")";
    }
    
    public void UpdateInteractMessage(string message)
    {
        interactMessage.text = message;
    }
    
    public void UpdateHealth()
    {
        healthText.text = PlayerController.instance.playerHealth.ToString();
    }

    public void UpdateAmmo()
    {
        ammoText.text = GunController.instance.currentAmmo + "/" + GunController.instance.totalAmmo;
    }

    public void GetHit()
    {
        GameObject hit = Instantiate(hitRed, transform, false);
        hit.GetComponent<Animator>().Play("Fade Out");
        UpdateHealth();
    }

    public void Win()
    {
        aliveScreen.SetActive(false);
        winScreen.SetActive(true);

        waitTime = .5f;
    }
    
    public void Death()
    {
        aliveScreen.SetActive(false);
        winScreen.SetActive(false);
        deathScreen.SetActive(true);
        
        waitTime = .5f;
    }
}
