using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float life_time;

    private void Awake()
    {
        Destroy(gameObject, life_time);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
