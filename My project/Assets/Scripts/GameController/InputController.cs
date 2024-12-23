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
    //public int stepsCount = 0;
    //public bool TryGetPrimaryAction(out PrimaryActionCommand primaryActionCommand)
    //{
    //    primaryActionCommand = null;

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        primaryActionCommand = new PrimaryActionCommand(Time.time, 0); // Use a suitable action code
    //    }
    //    return primaryActionCommand != null;
    //}

    public int stepsCount = 0;

    private float holdStartTime;
    private bool isCharging;

    public bool TryGetPrimaryAction(out PrimaryActionCommand primaryActionCommand)
    {
        primaryActionCommand = null;

        if (Input.GetMouseButtonDown(0))
        {
            holdStartTime = Time.time;
            isCharging = true;
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            float chargeDuration = Time.time - holdStartTime;
            isCharging = false;

            int action = chargeDuration > 1.0f ? 1 : 0;
            primaryActionCommand = new PrimaryActionCommand(Time.time, action);
        }

        return primaryActionCommand != null;
    }
}
