using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorToA : MonoBehaviour
{
    private Coroutine enter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySFX(SoundKey.OpenDoor);
            UI_AHall_Controller.instance.ShowNotifyPlace(false, "");
            enter = StartCoroutine(Enter());
        }
    }

    private void OnDisable()
    {
        if (enter != null)
        { 
            StopCoroutine(enter);
        }
    }

    IEnumerator Enter()
    {
        yield return null;
        StateControl.instance.IsGamePause = true;
        PlayerController.instance.ResetVelo();
        PlayerController.instance.SetIdleBaseOnMovement();
        UI_AHall_Controller.instance.ShowTransitionPanel(true);
        yield return new WaitForSeconds(1f);
        PlayerController.instance.transform.position = Game_AHall_Controller.instance.LibraryToA.transform.position;
        UI_AHall_Controller.instance.SetTriggerShowTransitionPanel();
        yield return new WaitForSeconds(0.8f);
        UI_AHall_Controller.instance.ShowTransitionPanel(false);
        UI_AHall_Controller.instance.ShowNotifyPlace(true, "Tòa A");
        StateControl.instance.IsGamePause = false;
    }
}
