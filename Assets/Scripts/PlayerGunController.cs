using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerGunController : MonoBehaviour
{
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    public int bulletsLeft, bulletsShot;
    private bool shooting, readyToShoot, reloading;

    public Camera fpsCam;
    public Transform attackPoint;
    public GameObject bulletPrefab;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    [SerializeField]
    public TextMeshProUGUI ammoText;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;

        UpdateAmmoText();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            StartReloading();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        Vector3 direction = fpsCam.transform.forward;
        direction += new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);
        direction = direction.normalized;

        GameObject bullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.LookRotation(direction));
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = direction * range;

        bulletsLeft--;
        bulletsShot--;
        UpdateAmmoText();

        Invoke("ResetShot", timeBetweenShooting);
        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }



    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void StartReloading()
    {
        reloading = true;

        ammoText.text = "Reloading...";
        Invoke("FinishReloading", reloadTime);
    }

    private void FinishReloading()
    {
        bulletsLeft = magazineSize;
        reloading = false;

        UpdateAmmoText();
    }

    public void UpdateAmmoText()
    {
        ammoText.text = $"{bulletsLeft} / {magazineSize}";
    }
}
