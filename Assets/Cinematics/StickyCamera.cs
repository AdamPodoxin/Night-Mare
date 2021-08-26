using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyCamera : MonoBehaviour
{
    public float sensitivity = 30f;

    [Space]

    public Vector2 lerpRot = new Vector2(1f, 180f);

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

        rotX = Mathf.Lerp(rotX, lerpRot.x, Time.deltaTime * 5f);
        rotY = Mathf.Lerp(rotY, lerpRot.y, Time.deltaTime * 5f);

        transform.eulerAngles = new Vector3(rotX, rotY, 0f);
    }
}
