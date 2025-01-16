using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIconsUI : MonoBehaviour
{
    public GameObject PlayerPistol;
    public GameObject PlayerShotgun;
    public GameObject PlayerAr;

    public GameObject IconPistol;
    public GameObject IconShotgun;
    public GameObject IconAr;

    void Update()
    {
        if (PlayerPistol.activeInHierarchy == true)
        {
            IconShotgun.SetActive(false);
            IconAr.SetActive(false);


            IconPistol.SetActive(true);
        }
        else if (PlayerShotgun.activeInHierarchy == true)
        {
            IconPistol.SetActive(false);
            IconAr.SetActive(false);


            IconShotgun.SetActive(true);
        }
        else if (PlayerAr.activeInHierarchy == true)
        {
            IconShotgun.SetActive(false);
            IconPistol.SetActive(false);


            IconAr.SetActive(true);
        }
    }
}
