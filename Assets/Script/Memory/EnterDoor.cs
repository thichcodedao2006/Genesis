using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour
{

    [SerializeField] GameObject newPosition;
    [SerializeField] PolygonCollider2D newConfiner;
    [SerializeField] CinemachineConfiner2D cinemachineConfiner; 



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           StartCoroutine(Teleport(collision.gameObject));
            var confiner = cinemachineConfiner;
            confiner.m_BoundingShape2D = newConfiner;
            confiner.InvalidateCache();
        }
    }

    private IEnumerator Teleport(GameObject player) 
    {
        player.transform.position = newPosition.transform.position;
        yield return null;  
    }
}
