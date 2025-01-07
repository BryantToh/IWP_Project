using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public BaseAbility[] abilities;

    private void Update()
    {
        foreach (var ability in abilities)
        {
            if (Input.GetKeyDown(ability.activationKey))
            {
                ability.Activate();
            }
        }
    }
}
