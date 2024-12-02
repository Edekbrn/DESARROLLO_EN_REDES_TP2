using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


public class Activartrampa : NetworkBehaviour
{
    public GameObject targetObject;
    public float cooldownTime = 5f; 
    private bool isTrapActive = false; 
    private bool isButtonActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;

        if (other.CompareTag("Player") && isButtonActive)
        {
            if (!isTrapActive)
            {
                print("ActivoTrampa1");
                isTrapActive = true;
                isButtonActive = false;
                if (targetObject != null)
                {
                    print("ActivoTrampa2");
                    Trampa Trampa = targetObject.GetComponent<Trampa>();
                    if (Trampa != null)
                    {
                        Trampa.StartMovement();
                        print("ActivoTrampa3");
                    }
                }
                StartCoroutine(ReactivateButton()); 
                print("ActivoTrampaReseteo");
            }
        }
    }

    private IEnumerator ReactivateButton()
    {
        yield return new WaitForSeconds(cooldownTime);
        isTrapActive = false;
        isButtonActive = true;
    }
}
