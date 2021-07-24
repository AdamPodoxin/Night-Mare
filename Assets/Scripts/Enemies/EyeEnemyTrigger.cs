using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeEnemyTrigger : MonoBehaviour
{
    public EyeEnemy eyeEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            eyeEnemy.PlayerEnterVision();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            eyeEnemy.PlayerExitVision();
        }
    }
}
