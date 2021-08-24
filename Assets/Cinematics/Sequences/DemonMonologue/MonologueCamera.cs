using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologueCamera : MonoBehaviour
{
    public float sensitivity;

    public GameObject monologue;
    public GameObject preNightmare;

    private float rotX;
    private float rotY;

    private void Start()
    {
        rotX = transform.eulerAngles.x;
        rotY = transform.eulerAngles.y;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        rotX -= Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;
        rotY += Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Lerp(rotX, 1f, Time.deltaTime * 5f);
        rotY = Mathf.Lerp(rotY, 180f, Time.deltaTime * 5f);

        transform.eulerAngles = new Vector3(rotX, rotY, 0f);
    }

    public void StartGame()
    {
        preNightmare.SetActive(true);
        monologue.SetActive(false);
    }
}
