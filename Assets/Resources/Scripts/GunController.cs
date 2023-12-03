using UnityEngine;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    public static GunController instance { get; private set;  }
    
    [Header("References")] [Space]
    private Animator animator;
    private AudioSource audioSource;
    
    [Header("Gun References")] [Space]
    [SerializeField] private Transform gunBarrel;
    [SerializeField] private Transform ejectionPort;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject bulletCasing;
    [SerializeField] private GameObject bulletImpactPrefab;
    [SerializeField] private GameObject enemyImpactPrefab;
    
    [Header("Gun")] [Space]
    public int damage;
    public int currentAmmo;
    public int totalAmmo;
    [SerializeField] private float bulletDistance;

    [Header("Audio Clips")] [Space]
    public AudioClip gunShotSFX;
    public AudioClip dryFireSFX;
    public AudioClip reloadSFX;
    
    private bool canShoot;

    private void Awake()
    {
        if (instance == null) instance = this;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        UIManager.instance.UpdateAmmo();
        canShoot = true;
        animator.SetInteger("currentAmmo", currentAmmo);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot)
        {
            if (currentAmmo > 0)
                Shoot();
            else
            {
                audioSource.clip = dryFireSFX;
                audioSource.volume = .15f;
                audioSource.PlayOneShot(audioSource.clip);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < 10 && totalAmmo > 0)
        {
            audioSource.clip = reloadSFX;
            audioSource.volume = .25f;
            audioSource.PlayOneShot(audioSource.clip);
            
            canShoot = false;
            
            int remainingBullet = 10 - currentAmmo;
            remainingBullet = (totalAmmo - remainingBullet) >= 0 ? remainingBullet : totalAmmo;
            
            if (currentAmmo > 0)
                animator.Play("GunReloadAnimation");
            else
                animator.Play("GunReloadOutOfAmmoAnimation");

            currentAmmo += remainingBullet;
            totalAmmo -= remainingBullet;
        }
        
        float horizontalInput = Input.GetAxisRaw("Horizontal");   
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0f || verticalInput != 0f)
        {
            if (PlayerController.instance.isRunning)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
            }
            else
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }

    private void Shoot()
    {
        audioSource.clip = gunShotSFX;
        audioSource.volume = .25f;
        audioSource.PlayOneShot(audioSource.clip);
        
        muzzleFlash.Play();
        
        currentAmmo--;
        animator.SetInteger("currentAmmo", currentAmmo);
        
        UIManager.instance.UpdateAmmo();
        
        CameraShake.Invoke();
        
        canShoot = false;
        
        animator.Play("GunShootAnimation");

        RaycastHit hit;
        if (Physics.Raycast(gunBarrel.position, transform.TransformDirection(Vector3.forward), out hit, bulletDistance))
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            {
                GameObject impact = Instantiate(bulletImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 1f);
            }
            else
            {
                GameObject impact = Instantiate(enemyImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 1f);
                
                hit.transform.gameObject.GetComponent<EnemyController>().GoAlerted();
                hit.transform.gameObject.GetComponent<EnemyController>().enemyHealth -= damage;
            }
        }
    }

    private void EjectCasing()
    {
        GameObject casing = Instantiate(bulletCasing, ejectionPort.position, Quaternion.identity);
        casing.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.up * Random.Range(50f, 100f)) + transform.TransformDirection(Vector3.right) * Random.Range(50f, 100f));
        Destroy(casing, 1f);
    }

    private void ShootReset()
    {
        canShoot = true;
    }

    private void Reload()
    {
        animator.SetInteger("currentAmmo", currentAmmo);
        UIManager.instance.UpdateAmmo();
    }
}
