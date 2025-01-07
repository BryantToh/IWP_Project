using UnityEngine;

public class PullAbilityObj : BaseAbility
{
    public GameObject playerObj;
    public Camera cam;
    public GameObject pullObj;
    public bool pullOff = false;
    public override void Activate()
    {
        GameObject obj = Instantiate(pullObj, transform.position, Quaternion.identity);
        //obj.transform.parent = transform;
    }
}
