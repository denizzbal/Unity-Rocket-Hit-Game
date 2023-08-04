using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Main : MonoBehaviour
{
    [SerializeField] Image SceneLoadImage;
    [SerializeField] GameObject SceneLoadPanel;
    [SerializeField] TMP_Text PercentText;

    void Start()
    {
        SceneLoadPanel.SetActive(false);
    }


    public void Level()
    {
        StartCoroutine(AsyncLevel());
    }

    IEnumerator AsyncLevel()
    {
        SceneLoadPanel.SetActive(true);
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("Level1");
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            SceneLoadImage.fillAmount = progressValue;
            //PercentText.text = Mathf.RoundToInt(progressValue * 100) + "%";
            PercentText.text = progressValue * 100 + "%";
            yield return null;

        }
    }









    //Image SceneLoadImage;
    //GameObject SceneLoadPanel;
    //TMP_Text PercentText;


    IEnumerator SceneAsyncc()
    {
        SceneLoadPanel.SetActive(true);
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("Level1");
        while (!operation.isDone)
        {
            float progressBar = Mathf.Clamp01(operation.progress / 0.9f);
            SceneLoadImage.fillAmount = progressBar;
            PercentText.text = progressBar * 100 + "%";
            yield return null;
        }
    }
}
