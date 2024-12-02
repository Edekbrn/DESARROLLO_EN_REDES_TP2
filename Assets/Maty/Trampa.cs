using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Trampa : NetworkBehaviour
{
    public Transform minHeight; 
    public Transform maxHeight; 
    public float moveSpeed = 5f;
    public float waitTimeAtTop = 2f; 
    private bool isMoving = false; 
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
       
        while (Vector3.Distance(transform.position, maxHeight.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, maxHeight.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

       
        yield return new WaitForSeconds(waitTimeAtTop);

       
        while (Vector3.Distance(transform.position, minHeight.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, minHeight.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false; 
    }
}
