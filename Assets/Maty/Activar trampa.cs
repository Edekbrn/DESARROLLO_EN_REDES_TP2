using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


public class Activartrampa : NetworkBehaviour
{
    public GameObject targetObject;
    public float cooldownTime = 5f; // Tiempo de espera antes de reactivar el bot�n
    private bool isTrapActive = false; // Estado de la trampa
    private bool isButtonActive = true; // Estado del bot�n

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isButtonActive)
        {
            if (!isTrapActive)
            {
                print("ActivoTrampa1");
                isTrapActive = true;
                isButtonActive = false; // Desactivar el bot�n
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
                StartCoroutine(ReactivateButton()); // Reactivar el bot�n tras un tiempo
                print("ActivoTrampaReseteo");
            }
        }
    }

    private IEnumerator ReactivateButton()
    {
        yield return new WaitForSeconds(cooldownTime);
        isTrapActive = false;
        isButtonActive = true; // Reactivar el bot�n
    }
}
