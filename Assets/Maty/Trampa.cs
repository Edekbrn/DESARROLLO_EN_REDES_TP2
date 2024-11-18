using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampa : MonoBehaviour
{
    public Transform minHeight; // Objeto vacío que marca la posición mínima
    public Transform maxHeight; // Objeto vacío que marca la posición máxima
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float waitTimeAtTop = 2f; // Tiempo que la trampa permanece en la posición superior
    private bool isMoving = false; // Controla si la trampa está en movimiento

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
        // Mover la trampa hacia la posición máxima (hacia arriba)
        while (Vector3.Distance(transform.position, maxHeight.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, maxHeight.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Esperar en la posición superior
        yield return new WaitForSeconds(waitTimeAtTop);

        // Mover la trampa hacia la posición mínima (hacia abajo)
        while (Vector3.Distance(transform.position, minHeight.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, minHeight.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false; // Permitir nuevas activaciones
    }
}
