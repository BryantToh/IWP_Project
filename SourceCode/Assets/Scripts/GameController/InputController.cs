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
    public bool TryGetPrimaryAction(out PrimaryActionCommand primaryActionCommand)
    {
        primaryActionCommand = null;

        if (Input.GetMouseButtonDown(0))
        {
            primaryActionCommand = new PrimaryActionCommand(Time.time, 0); // Use a suitable action code
        }
        return primaryActionCommand != null;
    }
}
