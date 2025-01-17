using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public BaseAbility[] abilities;
    public PlayerMovement playerMove;
    private void Update()
    {
        foreach (var ability in abilities)
        {
            if (Input.GetKeyDown(ability.activationKey) && !playerMove.staggered)
            {
                ability.Activate();
            }
        }
    }
}
