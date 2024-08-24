using System.Collections;
using UnityEngine;

public class ExplosionSpawner : MonoBehaviour
{
    [SerializeField] Transform pointA; 
    [SerializeField] Transform pointB; 
    [SerializeField] float range = 25f; 
    [SerializeField] GameObject explosionPrefab; 
    [SerializeField] float explosionDelay = 2f; 
    [SerializeField] LayerMask playerLayer; 
    [SerializeField] LayerMask wondedLayer; 
    [SerializeField] LayerMask enemyLayer; 

    void Start()
    {
        StartCoroutine(SpawnExplosions());
    }

    IEnumerator SpawnExplosions()
    {
        while (true)
        {
            Vector3 explosionPosition = GetRandomPosition();

            if (Physics.CheckSphere(explosionPosition, 2f, playerLayer) || Physics.CheckSphere(explosionPosition, 2f, wondedLayer) 
                || Physics.CheckSphere(explosionPosition, 2f, enemyLayer))
            {
                explosionPosition = GetRandomPosition();
            }

            Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);

            yield return new WaitForSeconds(explosionDelay);
        }
    }

    Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(pointA.position.x - range, pointB.position.x + range);
        float randomZ = Random.Range(pointA.position.z, pointB.position.z);
        return new Vector3(randomX, pointA.position.y, randomZ);
    }
}
