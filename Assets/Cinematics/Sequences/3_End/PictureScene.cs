using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PictureScene : MonoBehaviour
{
    public GameObject black;

    private void Start()
    {
        StartCoroutine(FinishCoroutine());
    }

    private IEnumerator FinishCoroutine()
    {
        yield return new WaitForSeconds(5f);
        black.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync("4_Credits", LoadSceneMode.Single);
    }
}
