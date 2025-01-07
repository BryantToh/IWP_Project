using UnityEngine;

public class PullAbilityObj : BaseAbility
{
    public Camera cam;
    public GameObject pullObj;
    public bool pullOff = false;
    public override void Activate()
    {
        Instantiate(pullObj, cam.transform.forward * 6f, Quaternion.identity);
    }
}
