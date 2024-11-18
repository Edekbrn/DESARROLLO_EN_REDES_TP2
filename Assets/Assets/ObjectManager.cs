using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // Método para activar un GameObject y desactivar una lista de otros GameObjects
    public void SwitchObjects(GameObject objectToActivate, List<GameObject> objectsToDeactivate)
    {
        // Activar el GameObject deseado
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        // Desactivar los GameObjects en la lista
        foreach (var obj in objectsToDeactivate)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
