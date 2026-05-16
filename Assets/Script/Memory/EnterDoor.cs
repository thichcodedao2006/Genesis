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
    private Coroutine enter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.PlayOpenDoor();
            enter = StartCoroutine(Teleport(collision.gameObject));
        }
            
    }

    private IEnumerator Teleport(GameObject player)
    {
        yield return null;
        StateControl.instance.IsGamePause = true;
        PlayerController.instance.ResetVelo();
        PlayerController.instance.SetIdleBaseOnMovement();
        E_Hall_Controller.Instance.ShowTransitionPanel(true);
        yield return new WaitForSeconds(1f);
        // Lấy vị trí mới, nhưng giữ nguyên trục Z hiện tại của player
        SoundManager.Instance.PlaySFX(SoundKey.OpenDoor);
        Vector3 targetPosition = new Vector3(
            newPosition.transform.position.x,
            newPosition.transform.position.y,
            player.transform.position.z
        );

        player.transform.position = targetPosition;
        E_Hall_Controller.Instance.SetTriggerShowTransitionPanel();
        yield return new WaitForSeconds(0.8f);
        E_Hall_Controller.Instance.ShowTransitionPanel(false);
        E_Hall_Controller.Instance.ShowNotifyPlace(true, enteringText);
        StateControl.instance.IsGamePause = false;

        if (cinemachineConfiner != null)
        {
            cinemachineConfiner.m_BoundingShape2D = newConfiner;
            cinemachineConfiner.InvalidateCache();
        }
        yield return null;
    }

    private void OnDisable()
    {
        if (enter != null)
        {
            StopCoroutine(enter);
        }
    }
}
