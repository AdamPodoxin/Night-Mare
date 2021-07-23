using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float maxDistance = 2f;

    public Text interactText;

    private Transform playerCamera;

    private PlayerInputActions playerInputActions;

    private Interactable focus;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Interact.performed += DoInteract;
        playerInputActions.Player.Interact.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Interact.Disable();
    }

    private void Start()
    {
        playerCamera = Camera.main.transform;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, maxDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null) SetFocus(interactable);
            else ResetFocus();
        }
        else ResetFocus();
    }

    private void DoInteract(InputAction.CallbackContext obj)
    {
        if (focus != null)
        {
            focus.Interact();
            ResetFocus();
        }
    }

    public void SetFocus(Interactable newFocus)
    {
        focus = newFocus;

        interactText.gameObject.SetActive(true);
        interactText.text = focus.interactText;
    }

    public void ResetFocus()
    {
        focus = null;

        interactText.gameObject.SetActive(false);
    }
}
