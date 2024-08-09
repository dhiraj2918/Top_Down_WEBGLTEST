using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunScript : MonoBehaviour
{
    public float rayDistance = 100f;
    public LayerMask enemyLayerMask;
    public float speed = 20f;
    public Rigidbody bulletPrefab;
    public int poolSize = 20;
    public Transform muzzleTransform;
    public TextMeshPro ammoText;
    public int initialAmmo = 10;
    private int currentAmmo;

    public AudioClip shootSound;
    public AudioClip emptyAmmoSound;

    private Queue<Rigidbody> bulletPool;
    private AudioSource audioSource;

    void Start()
    {
        bulletPool = new Queue<Rigidbody>();
        currentAmmo = initialAmmo;
        audioSource = gameObject.AddComponent<AudioSource>();

        for (int i = 0; i < poolSize; i++)
        {
            Rigidbody bullet = Instantiate(bulletPrefab);
            bullet.gameObject.SetActive(false);
            bulletPool.Enqueue(bullet);
        }

        UpdateAmmoText();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentAmmo > 0)
            {
                FireBullet();
                currentAmmo--;
                UpdateAmmoText();
                PlaySound(shootSound);
            }
            else
            {
                PlaySound(emptyAmmoSound);
                Debug.Log("Out of ammo!");
            }
        }
    }

    void FireBullet()
    {
        Vector3 rayOrigin = muzzleTransform.position;
        Vector3 rayDirection = muzzleTransform.forward;

        if (bulletPool.Count > 0)
        {
            Rigidbody bullet = bulletPool.Dequeue();
            bullet.transform.position = rayOrigin;
            bullet.transform.rotation = Quaternion.LookRotation(rayDirection);
            bullet.gameObject.SetActive(true);
            bullet.velocity = rayDirection * speed;
            StartCoroutine(DisableBulletAfterTime(bullet, 2f));
        }
        else
        {
            Debug.Log("No bullets available in the pool.");
        }
    }

    IEnumerator DisableBulletAfterTime(Rigidbody bullet, float time)
    {
        yield return new WaitForSeconds(time);
        bullet.gameObject.SetActive(false);
        bulletPool.Enqueue(bullet);
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        UpdateAmmoText();
    }

    public void IncreaseAmmo()
    {
        currentAmmo += 25;
    }

    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = "" + currentAmmo.ToString();
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}