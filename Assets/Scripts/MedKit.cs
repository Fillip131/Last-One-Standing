using UnityEngine;

public class MedKit : MonoBehaviour
{
    public float healAmount = 50f;
    public LayerMask interactionLayer; 
    public float interactionRange; 

    public GameObject interactionText; 
    public GameObject interactionCrossHair; 
    public GameObject crossHair; 

    private bool isLookingAtMedkit = false; 

    private void Update()
    {

        bool currentlyLooking = IsLookingAtMedkit();

        if (currentlyLooking != isLookingAtMedkit)
        {
            isLookingAtMedkit = currentlyLooking;

            if (isLookingAtMedkit)
            {
                ShowInteractionUI();
            }
            else
            {
                HideInteractionUI();
            }
        }


        if (isLookingAtMedkit && Input.GetKeyDown(KeyCode.E))
        {
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.RestoreHealth(healAmount);
            }

            Destroy(gameObject);
            HideInteractionUI();
        }
    }

    private bool IsLookingAtMedkit()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactionLayer))
        {
            return hit.collider.gameObject == gameObject;
        }

        return false;
    }

    private void ShowInteractionUI()
    {
        if (interactionText != null)
        {
            interactionText.SetActive(true);
        }

        if (interactionCrossHair != null)
        {
            interactionCrossHair.SetActive(true); 
        }

        if (crossHair != null)
        {
            crossHair.SetActive(false); 
        }
    }

    private void HideInteractionUI()
    {
        if (interactionText != null)
        {
            interactionText.SetActive(false); 
        }

        if (interactionCrossHair != null)
        {
            interactionCrossHair.SetActive(false); 
        }

        if (crossHair != null)
        {
            crossHair.SetActive(true); 
        }
    }
}
