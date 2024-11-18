using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public ObjectManager objectManager;
    public GameObject panelToActivate;
    public List<GameObject> panelsToDeactivate;

    public void ActivatePanel()
    {
        // Llamar al m�todo de ObjectManager para activar y desactivar los objetos
        objectManager.SwitchObjects(panelToActivate, panelsToDeactivate);
    }
}
