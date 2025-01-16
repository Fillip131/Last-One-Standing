using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;


public class WeaponCrate : MonoBehaviour
{
    [SerializeField]
    private VisualEffect _visualEffect;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private List<GameObject> weaponPrefabs;

    [SerializeField]
    private GameObject interactionTextF;
    [SerializeField]
    private GameObject interactionTextE;
    [SerializeField]
    private GameObject interactionCrossHair;
    [SerializeField]
    private GameObject crossHair;
    [SerializeField]
    private LayerMask interactionLayer;
    [SerializeField]
    private float interactionRange = 3f;

    private Animator _animator;

    private GameObject spawnedWeapon;
    private GameObject selectedWeapon;

    [SerializeField]
    private float scaleDuration = 0.5f;

    private bool isLookingAtCrate = false;
    private bool isAnimatingWeapon = false;

    public GameObject Pistol;
    public GameObject Shotgun;
    public GameObject Ar;

    private string actualTag;

    void Start()
    {
        _animator = GetComponent<Animator>();
        HideInteractionGunBoxUI();
    }

    private void Update()
    {
        UpdateCurrentWeaponTag();

        bool currentlyLooking = IsLookingAtCrate();

        if (currentlyLooking != isLookingAtCrate)
        {
            isLookingAtCrate = currentlyLooking;

            if (isLookingAtCrate && !_animator.GetBool("Open"))
            {
                ShowInteractionGunBoxUI();
            }
            else
            {
                HideInteractionGunBoxUI();
            }
        }


        if (isLookingAtCrate && Input.GetKeyDown(KeyCode.F))
        {
            _animator.SetBool("Open", true);
            HideInteractionGunBoxUI();

            if (spawnedWeapon == null && selectedWeapon == null)
            {
                SelectWeapon();
            }

            if (spawnedWeapon == null && selectedWeapon != null)
            {
                SpawnSelectedWeapon();
            }
        }

        if (spawnedWeapon != null && IsLookingAtWeapon() && Input.GetKeyDown(KeyCode.E))
        {
            GetWeapon();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _animator.SetBool("Open", false);

        if (spawnedWeapon != null)
        {
            StartCoroutine(ReturnWeaponToSpawn());
        }
    }

    private void OnLidLifted()
    {
        _visualEffect.SendEvent("OnPlay");
        AnimateWeapon();
    }

    private void SelectWeapon()
    {
        if (weaponPrefabs.Count == 0)
        {
            Debug.LogWarning("Žádné zbranì nejsou nastaveny v seznamu!");
            return;
        }

        int randomIndex = Random.Range(0, weaponPrefabs.Count);
        selectedWeapon = weaponPrefabs[randomIndex];
        SpawnSelectedWeapon();
    }

    private void SpawnSelectedWeapon()
    {
        spawnedWeapon = Instantiate(selectedWeapon, spawnPoint.position, Quaternion.identity);
        spawnedWeapon.transform.localScale = Vector3.zero;

        PlayerGunController gunController = spawnedWeapon.GetComponent<PlayerGunController>();
        if (gunController != null)
        {
            gunController.ammoText = GameObject.Find("AmmoText").GetComponent<TextMeshProUGUI>();
        }
        AnimateWeapon();
    }


    private void GetWeapon()
    {
        string currentTag = spawnedWeapon.gameObject.tag;
        Debug.Log("Tag nové zbranì: " + currentTag);

        if (actualTag == "Pistol")
        {
            Pistol.SetActive(false);
        }
        else if (actualTag == "Shotgun")
        {
            Shotgun.SetActive(false);
        }
        else if (actualTag == "Ar")
        {
            Ar.SetActive(false);
        }

        if (currentTag == "Pistol")
        {
            Pistol.SetActive(true);
            actualTag = "Pistol";
            ResetWeaponAmmo(Pistol);
        }
        else if (currentTag == "Shotgun")
        {
            Shotgun.SetActive(true);
            actualTag = "Shotgun";
            ResetWeaponAmmo(Shotgun);
        }
        else if (currentTag == "Ar")
        {
            Ar.SetActive(true);
            actualTag = "Ar";
            ResetWeaponAmmo(Ar);
        }

        Destroy(spawnedWeapon);
        spawnedWeapon = null;
        Debug.Log("Nová zbraò aktivována: " + actualTag);

        HideInteractionGunUI();
        crossHair.SetActive(true);
    }

    private void ResetWeaponAmmo(GameObject weapon)
    {
        if (weapon == null) return;

        PlayerGunController gunController = weapon.GetComponent<PlayerGunController>();

        gunController.bulletsLeft = gunController.magazineSize;
        gunController.UpdateAmmoText();

    }

    private void AnimateWeapon()
    {
        if (isAnimatingWeapon || spawnedWeapon == null) return;

        isAnimatingWeapon = true;
        StartCoroutine(AnimateWeaponScaleAndMove());
    }

    private IEnumerator AnimateWeaponScaleAndMove()
    {
        Vector3 startPos = spawnedWeapon.transform.position;
        Vector3 targetPos = startPos + Vector3.up * 1.3f;
        Vector3 targetScale = Vector3.one;

        float elapsed = 0f;

        while (elapsed < scaleDuration)
        {
            elapsed += Time.deltaTime;

            float scaleT = elapsed / scaleDuration;
            spawnedWeapon.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, scaleT);

            float moveT = elapsed / scaleDuration;
            spawnedWeapon.transform.position = Vector3.Lerp(startPos, targetPos, moveT);

            yield return null;
        }

        isAnimatingWeapon = false;

        // Rotace zbranì
        WeaponRotator rotator = spawnedWeapon.AddComponent<WeaponRotator>();
        rotator.rotationSpeed = 50f;
    }

    private IEnumerator ReturnWeaponToSpawn()
    {
        if (spawnedWeapon == null) yield break;

        float scaleElapsed = 0f;
        Vector3 startScale = spawnedWeapon.transform.localScale;
        Vector3 targetScale = Vector3.zero;

        Vector3 startPos = spawnedWeapon.transform.position;
        Vector3 targetPos = spawnPoint.position;

        float moveElapsed = 0f;
        float totalDuration = 0.5f;

        while (scaleElapsed < totalDuration)
        {
            scaleElapsed += Time.deltaTime;
            float t = scaleElapsed / totalDuration;

            spawnedWeapon.transform.position = Vector3.Lerp(startPos, targetPos, t);
            spawnedWeapon.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }

        Destroy(spawnedWeapon);
        spawnedWeapon = null;
    }

    private bool IsLookingAtCrate()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactionLayer))
        {
            return hit.collider.gameObject == gameObject;
        }

        return false;
    }

    private void UpdateCurrentWeaponTag()
    {
        if (Pistol.activeInHierarchy)
        {
            actualTag = "Pistol";
        }
        else if (Shotgun.activeInHierarchy)
        {
            actualTag = "Shotgun";
        }
        else if (Ar.activeInHierarchy)
        {
            actualTag = "Ar";
        }
    }

    private void ShowInteractionGunBoxUI()
    {
        interactionTextF.SetActive(true);
        interactionCrossHair.SetActive(true);
        crossHair.SetActive(false);
    }

    private void HideInteractionGunBoxUI()
    {
        interactionTextF.SetActive(false);
        interactionCrossHair.SetActive(false);
        crossHair.SetActive(true);
    }

    private void ShowInteractionGunUI()
    {
        interactionTextE.SetActive(true);
        interactionCrossHair.SetActive(true);
        crossHair.SetActive(false);
    }

    private void HideInteractionGunUI()
    {
        interactionTextE.SetActive(false);
        interactionCrossHair.SetActive(false);
        crossHair.SetActive(true);
    }

    private bool IsLookingAtWeapon()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactionLayer))
        {
            if (hit.collider.gameObject == spawnedWeapon)
            {
                ShowInteractionGunUI();
                return true;
            }
        }

        HideInteractionGunUI();
        return false;
    }

    private void UpdateAmmoText(GameObject weapon)
    {
        if (weapon == null)
        {
            Debug.LogWarning("Weapon objekt je null!");
            return;
        }

        PlayerGunController gunController = weapon.GetComponent<PlayerGunController>();
        if (gunController == null)
        {
            Debug.LogWarning("PlayerGunController nebyl nalezen na aktuální zbrani!");
            return;
        }


        TextMeshProUGUI ammoText = GameObject.Find("AmmoText")?.GetComponent<TextMeshProUGUI>();
        if (ammoText == null)
        {
            Debug.LogWarning("AmmoText nebyl nalezen ve scénì!");
            return;
        }

        ammoText.text = $"{gunController.bulletsLeft} / {gunController.magazineSize}";
    }

}
