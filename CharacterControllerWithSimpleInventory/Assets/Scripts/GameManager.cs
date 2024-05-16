using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    private InputAction exitAction;

    private void Awake()
    {
        exitAction = playerInput.actions["Exit"];
    }

    private void OnEnable()
    {
        exitAction.started += ExitAction_started;
    }

    private void OnDisable()
    {
        exitAction.started -= ExitAction_started;
    }

    private void ExitAction_started(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }
}
