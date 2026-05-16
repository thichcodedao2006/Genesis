using System.Collections;
using UnityEngine;

public class FromB136ToB : MonoBehaviour
{
    public GameObject InFrontB316;
    private Coroutine enter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySFX(SoundKey.CloseDoor);
            UI_BHall_Controller.instance.ShowNotifyPlace(false, "");
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
        UI_BHall_Controller.instance.ShowTransitionPanel(true);
        yield return new WaitForSeconds(1f);
        PlayerController.instance.transform.position = InFrontB316.transform.position;
        PlayerController.instance.SetPlayerIdle(0, -1);
        UI_BHall_Controller.instance.SetTriggerShowTransitionPanel();
        yield return new WaitForSeconds(0.8f);
        UI_BHall_Controller.instance.ShowTransitionPanel(false);
        UI_BHall_Controller.instance.ShowNotifyPlace(true, "Tòa B");
        StateControl.instance.IsGamePause = false;
    }


}
