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
        // Lấy vị trí mới, nhưng giữ nguyên trục Z hiện tại của player
        SoundManager.Instance.PlaySFX(SoundKey.OpenDoor);
        Vector3 targetPosition = new Vector3(
            newPosition.transform.position.x,
            newPosition.transform.position.y,
            player.transform.position.z
        );

        player.transform.position = targetPosition;

        yield return null;

        if (cinemachineConfiner != null)
        {
            cinemachineConfiner.m_BoundingShape2D = newConfiner;
            cinemachineConfiner.InvalidateCache();
        }

        if (E_Hall_Controller.Instance != null && !string.IsNullOrEmpty(enteringText))
            yield return E_Hall_Controller.Instance.StartCoroutine(
                E_Hall_Controller.Instance.Announce(enteringText)
            );
    }
}
