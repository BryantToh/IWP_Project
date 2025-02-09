using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class DetailsHandler : MonoBehaviour
{
    [Header("Abilities Data")]
    public List<AbilityDetails> scriptableObj = new List<AbilityDetails>();
    public TMP_Text abilityName;
    public TMP_Text description;
    public TMP_Text abilityKey;
    public Image abilityImage;
    [HideInInspector]
    public int index = 0;

    void ShowAbility()
    {
        if (index < 0 || index >= scriptableObj.Count)
        {
            return;
        }
        var currentAbility = scriptableObj[index];
        abilityImage.sprite = currentAbility.abilityImage;
        abilityKey.text = currentAbility.abilityKey;
        abilityName.text = currentAbility.abilityName;
        description.text = currentAbility.abilityDescription;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PrevAbility();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            NextAbility();
        }
    }

    public void NextAbility()
    {
        if (index < scriptableObj.Count - 1)
        {
            index++;
            ShowAbility();
            AudioManager.instance.PlaySFX("changeability");
        }
    }

    public void PrevAbility()
    {
        if (index > 0)
        {
            index--;
            ShowAbility();
            AudioManager.instance.PlaySFX("changeability");
        }
    }

    private void Start()
    {
        ShowAbility();
    }
}
