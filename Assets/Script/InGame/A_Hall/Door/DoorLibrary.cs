using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLibrary : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySFX(SoundKey.OpenDoor);
            PlayerController.instance.transform.position = Game_AHall_Controller.instance.AToLibrary.transform.position;
        }
    }
}
