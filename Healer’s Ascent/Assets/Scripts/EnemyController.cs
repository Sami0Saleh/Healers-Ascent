using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyFOV enemyFOV;
    [SerializeField] Transform pointA; 
    [SerializeField] Transform pointB; 
    [SerializeField] float speed = 2f;
    [SerializeField] private float shootCooldown = 2f; 
    private float nextShootTime = 0f;

    private Vector3 targetPosition; 

    void Start()
    {
        targetPosition = pointA.position;
    }

    void Update()
    {
        if (!enemyFOV.PlayerDetected)
        {
            transform.LookAt(targetPosition);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (transform.position == pointA.position)
            {
                targetPosition = pointB.position;
            }
            else if (transform.position == pointB.position)
            {
                targetPosition = pointA.position;
            }
        }
    }
    
    public void Shoot(PlayerController player)
    {
        if (Time.time >= nextShootTime)
        {
            player.TakeDamage();
            nextShootTime = Time.time + shootCooldown;
        }
    }
}
