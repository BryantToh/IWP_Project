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
        if (CheckMovementInput())
        {
            instructionText.text = "Click left click for normal attack 5 times";
            if (CheckMouseInput())
            {
                instructionText.text = "Press numbers 1, 2 and 3 to use your abilities";
                if (CheckAbilityUse())
                {
                    instructionText.text = "Q is an active buff";
                    //CheckBuffUse();
                }
            }
        }
    }

    bool CheckMovementInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 && verticalInput != 0)
        {
            UpdateInstructionText("Niceu");
            return true;
        }
        else return false;
    }

    bool CheckMouseInput()
    {
        if (player.kickSteps == 4 && !normalAttack)
        {
            instructionText.text = "Niceu, now hold down left click for heavy attack";
            normalAttack = true;
        }

        if (player.chargeAttack && normalAttack)
        {
            UpdateInstructionText("Nice, let's try using your abilities");
            return true;
        }
        else return false;
    }

    bool CheckAbilityUse()
    {
        if (/*pullAbilityLogic.inUse && */pushAbilityEffect.inUse && ultiLogic.inUse)
        {
            UpdateInstructionText("Even nicer, now let's introduce you to your buffs");
            return true;
        }
        else return false;
    }

    bool CheckBuffUse()
    {
        if (surgeLogic.isUse && deathLogic.isUse)
        {
            UpdateInstructionText("Nice. Remember that your Q buff's effect will only activate when you die. Use it wisely");
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
