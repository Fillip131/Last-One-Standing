using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public GameObject PressESC;
    public GameObject Arrow;

    bool ESCused = false;

    void Start()
    {
        Invoke("EnablePressESC", 3f);
    }

    void Update()
    {
        if (!ESCused && Input.GetKeyDown(KeyCode.Escape))
        {
            ESCused = true; 
            PressESC.SetActive(false); 
            StartCoroutine(EnableArrowAfterDelay(1.5f)); 
        }
    }

    void EnablePressESC()
    {
        if (!ESCused) 
        {
            PressESC.SetActive(true);
        }
    }

    IEnumerator EnableArrowAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); 
        Arrow.SetActive(true); 
    }
}
