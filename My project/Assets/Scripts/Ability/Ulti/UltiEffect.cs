using UnityEngine;

public class UltiEffect : BaseAbility
{
    public UltiLogic ultiLogic;
    public override void Activate()
    {
        ultiLogic.activated = true;
    }
}
