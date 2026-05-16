using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoBackFromA : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.PlayOpenDoor();
            StateControl.instance.IsGamePause = true;
            PlayerController.instance.ResetVelo();
            PlayerController.instance.SetIdleBaseOnMovement();

            StartCoroutine(Transition());
        }
    }

    IEnumerator Transition()
    {
        yield return null;
        UI_AHall_Controller.instance.ShowTransitionPanel(true);

        yield return new WaitForSeconds(0.5f);

        SceneTransitionManager.TargetSpawn = KeyData.SpawnFromA;
        SceneManager.LoadSceneAsync("Outside");
    }
}
