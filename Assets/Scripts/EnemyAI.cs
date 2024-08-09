using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Audio;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float followDistance = 10f;
    public float attackDistance = 2f;
    public float moveSpeed = 3.5f;
    public int health = 100;
    public Slider slider;

    public AudioClip attackSound;
    public AudioClip hurtSound;
    public AudioMixer audioMixer;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private EnemyPool enemyPool;
    private AudioSource audioSource;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyPool = FindObjectOfType<EnemyPool>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnEnable()
    {
        health = 100;
        if (slider != null)
        {
            slider.value = health;
        }
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        navMeshAgent.updateRotation = true;

        if (distanceToPlayer <= attackDistance)
        {
            navMeshAgent.isStopped = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);
            PlaySound(attackSound, "SoundFXVolume"); // Play attack sound
        }
        else if (distanceToPlayer <= followDistance)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            navMeshAgent.isStopped = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
        }

        SetHealth();
        if (health <= 0)
        {
            ReturnToPool();
        }
    }

    public void Hurt()
    {
        health -= 25;
        Debug.Log("Hurt called");
        animator.SetTrigger("Hurt");
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        PlaySound(hurtSound, "SoundFXVolume"); // Play hurt sound
    }

    public void SetHealth()
    {
        if (slider != null)
        {
            slider.value = health;
        }
    }

    private void ReturnToPool()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        gameObject.SetActive(false);
        enemyPool.ReturnEnemyToPool(gameObject);
    }

    private void PlaySound(AudioClip clip, string volumeParameter)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(volumeParameter)[0];
            audioSource.Play();
        }
    }
}
