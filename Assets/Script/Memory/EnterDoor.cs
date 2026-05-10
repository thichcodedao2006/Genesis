using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour
{

    [SerializeField] GameObject newPosition;
    [SerializeField] PolygonCollider2D newConfiner;
    [SerializeField] CinemachineConfiner2D cinemachineConfiner;
    [SerializeField] bool isEntering = true;
    [SerializeField] string enteringText;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            StartCoroutine(Teleport(collision.gameObject));
    }

    private IEnumerator Teleport(GameObject player)
    {
        player.transform.position = newPosition.transform.position;

        if (cinemachineConfiner != null)
        {
            cinemachineConfiner.m_BoundingShape2D = newConfiner;
            cinemachineConfiner.InvalidateCache();
        }

        if (E_Hall_Controller.Instance != null && !string.IsNullOrEmpty(enteringText))
            yield return E_Hall_Controller.Instance.StartCoroutine(
                E_Hall_Controller.Instance.Announce(enteringText)
            );
        else
            yield return null;
    }
}
