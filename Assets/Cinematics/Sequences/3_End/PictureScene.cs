using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(FinishCoroutine());
    }

    private IEnumerator FinishCoroutine()
    {
        yield return new WaitForSeconds(5f);

        //Black
        print("Black");

        yield return new WaitForSeconds(1.5f);

        //Credits
        print("Credits");
    }
}
