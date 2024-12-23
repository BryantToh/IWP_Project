using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement movement;
    [SerializeField]
    private InputController inputcontroller;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (inputcontroller.TryGetPrimaryAction(out PrimaryActionCommand action))
            movement.ReadPrimaryActionCommand(action);
    }
}
