using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromB136ToB : MonoBehaviour
{
    public GameObject InFrontB316;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySFX(SoundKey.CloseDoor);
            PlayerController.instance.transform.position = InFrontB316.transform.position;
            PlayerController.instance.SetPlayerIdle(0, -1);
        }
    }
}
