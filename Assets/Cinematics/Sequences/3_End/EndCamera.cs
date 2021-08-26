using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCamera : MonoBehaviour
{
    public Animator flashAnim;

    private void Start()
    {
        flashAnim.Play("Flash_Out");
    }
}
