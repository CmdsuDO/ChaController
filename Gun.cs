using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 0.2f;
    public int maxAmmo = 10;
    public int currentAmmo;
    public float reloadTime = 1.5f;

    public Camera fpsCamera;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public AudioSource gunSound;
    public AudioSource reloadSound;

    private float nextTimeToFire = 0f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading) return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;

        if (muzzleFlash != null) muzzleFlash.Play();
        if (gunSound != null) gunSound.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            if (impactEffect != null)
            {
                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        if (reloadSound != null) reloadSound.Play();

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
