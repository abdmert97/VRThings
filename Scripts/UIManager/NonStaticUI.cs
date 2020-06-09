using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonStaticUI : MonoBehaviour
{

    public SceneIndex sceneIndex;
   
    private void Start()
    {
        sceneIndex = Instantiate(sceneIndex);
    }

    // Start is called before the first frame update
    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        StartCoroutine(ClosePanelWithDelay(panel, 2));
    }

   

    public void QuitGame()
    {
        SceneManager.LoadScene((int) UIManager.UIManager.SCENES.STARTSCENE, LoadSceneMode.Single);

    }

    public void QuitApplication()
    {
        Application.Quit();
    }
    public void LoadScene(string sceneName)
    {
        UIManager.UIManager.Instance.Load(sceneIndex.FindSceneIndex(sceneName));
    }
    IEnumerator ClosePanelWithDelay(GameObject panel, int seconds)
    {
        yield return new WaitForSeconds(2);
        panel.SetActive(false);
    }
}
