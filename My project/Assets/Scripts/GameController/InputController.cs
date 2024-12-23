using UnityEngine;

public abstract class Command
{
    public float Time { get; private set; }
    public Command(float time) => Time = time;
}

public class PrimaryActionCommand : Command
{
    public int Action { get; private set; }

    public PrimaryActionCommand(float time, int action) : base(time)
    {
        Action = action;
    }
}
public class InputController : MonoBehaviour
{
    public int stepsCount = 0;

    private float holdStartTime;
    private bool isCharging;

    public bool TryGetPrimaryAction(out PrimaryActionCommand primaryActionCommand)
    {
        primaryActionCommand = null;

        // Detect button down and start charging
        if (Input.GetMouseButtonDown(0))
        {
            holdStartTime = Time.time;
            isCharging = true;
        }

        // Detect button release
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            float chargeDuration = Time.time - holdStartTime;
            isCharging = false;

            // Determine the action: regular or charged
            int action = chargeDuration > 1.0f ? 1 : 0; // 1 for charged, 0 for regular
            primaryActionCommand = new PrimaryActionCommand(Time.time, action);
        }

        return primaryActionCommand != null;
    }
}

