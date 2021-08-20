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

    private RaycastHit _hit;
    private Interactable _hitInteractable;

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
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out _hit, maxDistance, ~LayerMask.GetMask("Player")))
        {
            _hitInteractable = _hit.collider.GetComponent<Interactable>();

            if (_hitInteractable != null) SetFocus(_hitInteractable);
            else ResetFocus();
        }
        else ResetFocus();
    }

    private void DoInteract(InputAction.CallbackContext obj)
    {
        if (focus != null)
        {
            focus.Interact(this);
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
