using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestroyExplosions());
    }

    IEnumerator DestroyExplosions()
    {
        yield return new WaitForSeconds(1.8f);
        Destroy(gameObject);
        
    }
}
