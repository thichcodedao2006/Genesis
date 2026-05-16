using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FromBToOutside : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySFX(SoundKey.CloseDoor);
            StateControl.instance.IsGamePause = true;
            PlayerController.instance.ResetVelo();
            PlayerController.instance.SetIdleBaseOnMovement();

            StartCoroutine(Transition());
        }
    }

    IEnumerator Transition()
    {
        yield return null;
        UI_BHall_Controller.instance.ShowTransitionPanel(true);

        yield return new WaitForSeconds(0.5f);

        SceneTransitionManager.TargetSpawn = KeyData.SpawnFromB;
        SceneManager.LoadSceneAsync("Outside");
    }


}
