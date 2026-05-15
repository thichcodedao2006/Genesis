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
            SceneTransitionManager.TargetSpawn = KeyData.SpawnFromB;
            SceneManager.LoadScene("Outside");
        }
    }
}
