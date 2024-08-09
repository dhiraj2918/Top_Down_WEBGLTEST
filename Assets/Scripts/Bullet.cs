using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 2f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        rb.isKinematic = false;
        rb.velocity = -transform.forward * speed;
        Invoke(nameof(Deactivate), lifetime);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Assuming the enemy has a script with a Hurt method
            other.GetComponent<EnemyAI>()?.Hurt();
            Deactivate();
        }
    }

    void Deactivate()
    {
        rb.isKinematic = true; // Set to kinematic to prevent physics interactions while deactivated
        gameObject.SetActive(false);
    }
}