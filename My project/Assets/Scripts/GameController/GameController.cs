using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement movement;
    [SerializeField]
    private InputController inputcontroller;

    // Update is called once per frame
    private void Update()
    {
        if (movement == null && inputcontroller == null)
            return;

        if (inputcontroller.TryGetPrimaryAction(out PrimaryActionCommand action))
            movement.ReadPrimaryActionCommand(action);
    }
}
