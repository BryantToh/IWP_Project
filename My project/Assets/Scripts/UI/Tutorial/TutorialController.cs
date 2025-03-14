using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;

public class TutorialController : MonoBehaviour
{
    public PlayerMovement player;
    public TMP_Text instructionText;
    public PullAbilityObj pullAbilityObj;
    public PushAbilityEffect pushAbilityEffect;
    public UltiLogic ultiLogic;
    public DeathLogic deathLogic;
    public SurgeLogic surgeLogic;
    public GameObject teleporter;
    public GameObject senObj;
    SentinelHealth health;
    public NavMeshAgent agent;
    private int index = 0;
    private float horizontalInput;
    private float verticalInput;
    private bool normalAttack = false;
    float ogSpeed = 0;

    private void Start()
    {
        teleporter.SetActive(false);
        instructionText.text = "Hello, use WASD to move around";
    }

    private void FixedUpdate()
    {
        if (senObj == null)
        {
            senObj = GameObject.FindGameObjectWithTag("EnemyObj");
            if (senObj == null)
                return;
        }

        agent = senObj.GetComponent<NavMeshAgent>();
        health = senObj.GetComponent<SentinelHealth>();
        ogSpeed = agent.speed;
        agent.speed = 0;
        health.Unit.Damage = 0;

        if (index == 0)
        {
            CheckMovementInput();
        }
        else if (index == 1)
        {
            CheckMouseInput();
            agent.speed = ogSpeed;
        }
        else if (index == 2)
        {
            CheckAbilityUse();
        }
        else if (index == 3)
            teleporter.SetActive(true);
    }

    bool CheckMovementInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 && verticalInput != 0 && -horizontalInput != 0 && -verticalInput != 0)
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
        bool canUsePush = false;
        bool canUseUlti = false;

        if (pullAbilityObj.inUse)
        {
            canUsePush = true;
        }
        if (pushAbilityEffect.inUse && canUsePush)
        {
            canUseUlti = true;
        }
        if (ultiLogic.inUse && pullAbilityObj.inUse && pushAbilityEffect.inUse && canUseUlti)
        {
            instructionText.text = "Congratz you done with tutorial. Touch the ball to exit tutorial";
            index++;
            return true;
        }
        else return false;
    }
}
