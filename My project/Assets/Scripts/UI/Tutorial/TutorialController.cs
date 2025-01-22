using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class TutorialController : MonoBehaviour
{
    public PlayerMovement player;
    public TMP_Text instructionText;
    public PullAbilityLogic pullAbilityLogic;
    public PushAbilityEffect pushAbilityEffect;
    public UltiLogic ultiLogic;
    public DeathLogic deathLogic;
    public SurgeLogic surgeLogic;

    private int index = 0;
    private float horizontalInput;
    private float verticalInput;
    private bool normalAttack = false;
    private bool isDisplayingText = false;

    private void Start()
    {
        instructionText.text = "Hello, use WASD to move around";
    }

    private void FixedUpdate()
    {
        if (index == 0)
        {
            CheckMovementInput();
        }
        else if (index == 1)
        {
            CheckMouseInput();
        }
        else if (index == 2)
        {
            CheckAbilityUse();

        }
        else if (index == 3)
        {
            CheckBuffUse();
        }
    }

    bool CheckMovementInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 && verticalInput != 0)
        {
            instructionText.text = "Click left click for normal attack 5 times";
            index++;
            return true;
        }
        else return false;
    }

    bool CheckMouseInput()
    {
        if (player.kickSteps == 4)
        {
            instructionText.text = "Niceu, now hold down left click for heavy attack";
            normalAttack = true;
        }

        if (player.chargeAttack && normalAttack)
        {
            instructionText.text = "Press numbers 1, 2 and 3 to use your abilities";
            index++;
            return true;
        }
        else return false;
    }

    bool CheckAbilityUse()
    {
        if (/*pullAbilityLogic.inUse && */pushAbilityEffect.inUse && ultiLogic.inUse)
        {
            UpdateInstructionText("Q is an active buff while the passive buff only occurs when you dodge a enemy attack");
            index++;
            return true;
        }
        else return false;
    }

    bool CheckBuffUse()
    {
        if (surgeLogic.isUse && deathLogic.isUse)
        {
            UpdateInstructionText("Your Q will only work successfully when you die, so use it at the right time");
            index++;
            return true;
        }
        else return false;
    }

    private void UpdateInstructionText(string newText)
    {
        if (!isDisplayingText)
        {
            StartCoroutine(DisplayTextCoroutine(newText));
        }
    }

    private IEnumerator DisplayTextCoroutine(string newText)
    {
        isDisplayingText = true;
        instructionText.text = newText;
        yield return new WaitForSeconds(5f);
        isDisplayingText = false;
    }
}
