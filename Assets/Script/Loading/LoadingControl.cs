using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingControl : MonoBehaviour
{
    public Image FillBar;
    public TextMeshProUGUI LoadText;

    private void Start()
    {
        FillBar.fillAmount = 0f;
        LoadText.text = "Loading 0% ...";
        StartCoroutine(LoadProcess());
    }

    IEnumerator LoadProcess()
    {
        yield return new WaitForSeconds(1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync("B_hall");

        while( !operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // làm tròn

            float realvalue = progress * 100;

            FillBar.fillAmount = progress;


            LoadText.text = "Loading " + realvalue.ToString("F0") + "% ...";
            yield return null;
        }
    }
}
