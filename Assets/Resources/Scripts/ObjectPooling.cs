using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance { get; private set; }
    
    private List<GameObject> pooledBulletHitObjects = new List<GameObject>();
    [SerializeField] private int bulletHitObjectsAmount = 10;
    
    private List<GameObject> pooledBulletEnemyHitObjects = new List<GameObject>();
    [SerializeField] private int bulletEnemyHitObjectsAmount = 10;
    
    private List<GameObject> pooledBulletShellObjects = new List<GameObject>();
    [SerializeField] private int bulletShellObjectsAmount = 10;
    
    private List<GameObject> pooledEnemyBulletObjects = new List<GameObject>();
    [SerializeField] private int enemyBulletObjectsAmount = 10;
    
    [SerializeField] private GameObject bulletHitPrefab;
    [SerializeField] private GameObject bulletEnemyHitPrefab;
    [SerializeField] private GameObject bulletShellPrefab;
    [SerializeField] private GameObject enemyBulletPrefab;

    private void Awake() => instance = this;
    
    private void Start()
    {
        for (int i = 0; i < bulletHitObjectsAmount; i++)
        {
            GameObject obj = Instantiate(bulletHitPrefab);
            obj.SetActive(false);
            pooledBulletHitObjects.Add(obj);
        }
        
        for (int i = 0; i < bulletEnemyHitObjectsAmount; i++)
        {
            GameObject obj = Instantiate(bulletEnemyHitPrefab);
            obj.SetActive(false);
            pooledBulletEnemyHitObjects.Add(obj);
        }
        
        for (int i = 0; i < bulletShellObjectsAmount; i++)
        {
            GameObject obj = Instantiate(bulletShellPrefab);
            obj.SetActive(false);
            pooledBulletShellObjects.Add(obj);
        }
        
        for (int i = 0; i < enemyBulletObjectsAmount; i++)
        {
            GameObject obj = Instantiate(enemyBulletPrefab);
            obj.SetActive(false);
            pooledEnemyBulletObjects.Add(obj);
        }
    }
    
    public GameObject GetPooledBulletHitObject()
    {
        for (int i = 0; i < pooledBulletHitObjects.Count; i++)
        {
            if (!pooledBulletHitObjects[i].activeInHierarchy)
                return pooledBulletHitObjects[i];
        }
        
        GameObject obj = Instantiate(bulletHitPrefab);
        pooledBulletHitObjects.Add(obj);
        return obj;
    }
    
    public GameObject GetPooledBulletEnemyHitObject()
    {
        for (int i = 0; i < pooledBulletEnemyHitObjects.Count; i++)
        {
            if (!pooledBulletEnemyHitObjects[i].activeInHierarchy)
                return pooledBulletEnemyHitObjects[i];
        }
        
        GameObject obj = Instantiate(bulletEnemyHitPrefab);
        pooledBulletEnemyHitObjects.Add(obj);
        return obj;
    }
    
    public GameObject GetPooledShellObject()
    {
        for (int i = 0; i < pooledBulletShellObjects.Count; i++)
        {
            if (!pooledBulletShellObjects[i].activeInHierarchy)
                return pooledBulletShellObjects[i];
        }
        
        GameObject obj = Instantiate(bulletShellPrefab);
        pooledBulletShellObjects.Add(obj);
        return obj;
    }
    
    public GameObject GetPooledEnemyBulletObject()
    {
        for (int i = 0; i < pooledEnemyBulletObjects.Count; i++)
        {
            if (!pooledEnemyBulletObjects[i].activeInHierarchy)
                return pooledEnemyBulletObjects[i];
        }
        
        GameObject obj = Instantiate(enemyBulletPrefab);
        pooledEnemyBulletObjects.Add(obj);
        return obj;
    }
}
