using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampa : MonoBehaviour
{
    public Transform minHeight; // Objeto vac�o que marca la posici�n m�nima
    public Transform maxHeight; // Objeto vac�o que marca la posici�n m�xima
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float waitTimeAtTop = 2f; // Tiempo que la trampa permanece en la posici�n superior
    private bool isMoving = false; // Controla si la trampa est� en movimiento

    public void StartMovement()
    {
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(ActivateTrap());
            print("Moviotrampa");
        }
    }

    private IEnumerator ActivateTrap()
    {
        // Mover la trampa hacia la posici�n m�xima (hacia arriba)
        while (Vector3.Distance(transform.position, maxHeight.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, maxHeight.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Esperar en la posici�n superior
        yield return new WaitForSeconds(waitTimeAtTop);

        // Mover la trampa hacia la posici�n m�nima (hacia abajo)
        while (Vector3.Distance(transform.position, minHeight.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, minHeight.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false; // Permitir nuevas activaciones
    }
}
