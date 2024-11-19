using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public GameObject winCanvas; // El Canvas de ganar
    public GameObject loseCanvas; // El Canvas de perder

    // Llamado cuando el jugador gana
    public void ShowWinCanvas()
    {
        winCanvas.SetActive(true);
    }

    // Llamado cuando el jugador pierde
    public void ShowLoseCanvas()
    {
        loseCanvas.SetActive(true);
    }
}
