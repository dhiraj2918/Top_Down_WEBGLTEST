using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public EnemyAI Enemy;
    public GameObject player;
    public Slider slider;
    public int health = 100;
    public GunScript gunScript;

    public AudioClip hurtSound;
    public AudioClip PowerUpSound;

    private Rigidbody rb;
    private Animator animator;
    private bool isAttacking;
    public float turnSpeed;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
        }

        float y = Input.GetAxis("Mouse X") * turnSpeed;
        player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y + y, 0);

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = player.transform.TransformDirection(movement);
        player.transform.position += movement * moveSpeed * Time.deltaTime;

        animator.SetFloat("moveHorizontal", moveHorizontal);
        animator.SetFloat("moveVertical", moveVertical);

        SetHealth();
        Mathf.Clamp(health, 0, 100);
        if (health <= 0)
        {
            animator.SetTrigger("Dead");
        }
    }

    public void SetHealth()
    {
        slider.value = health;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Collided");
            health -= 25;

            PlaySound(hurtSound, "SFXVolume");
            animator.SetTrigger("Hurt");
        }

        if (other.gameObject.CompareTag("Medi"))
        {
            Debug.Log("Medi");
            if (health < 100)
            {
                health += 25;
                PlaySound(PowerUpSound, "SFXVolume");
            }

            Destroy(other.gameObject);
            Debug.Log("Medi Destroy: ");
        }

        if (other.gameObject.CompareTag("Ammo"))
        {
            gunScript.IncreaseAmmo();
            Destroy(other.gameObject);
            Debug.Log("Ammo Destroy: ");
        }
    }
    private void PlaySound(AudioClip clip, string volumeParameter)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
