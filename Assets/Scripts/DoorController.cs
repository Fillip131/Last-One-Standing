using UnityEngine;
using TMPro; 
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public Transform leftDoor;  
    public Transform rightDoor; 
    public Vector3 leftOpenPosition; 
    public Vector3 rightOpenPosition; 
    public float openSpeed = 2f; 
    public GameObject interactionText;
    public GameObject interactionCrossHair; 
    public GameObject crossHair; 
    public float autoCloseDelay = 5f; 

    private bool isPlayerNearby = false;
    private bool isOpen = false;
    private float openTimer = 0f;

    private Vector3 leftClosedPosition;
    private Vector3 rightClosedPosition;

    void Start()
    {

        leftClosedPosition = leftDoor.localPosition;
        rightClosedPosition = rightDoor.localPosition;

 
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

    void Update()
    {

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            isOpen = !isOpen; 

            if (isOpen)
            {
                openTimer = autoCloseDelay;

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

        if (isOpen)
        {
            openTimer -= Time.deltaTime;
            if (openTimer <= 0f)
            {
                isOpen = false; 
            }
        }

        if (isOpen)
        {
            leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, leftOpenPosition, Time.deltaTime * openSpeed);
            rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, rightOpenPosition, Time.deltaTime * openSpeed);
        }
        else
        {
            leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, leftClosedPosition, Time.deltaTime * openSpeed);
            rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, rightClosedPosition, Time.deltaTime * openSpeed);

            if (isPlayerNearby)
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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;

            if (!isOpen)
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;


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
}
