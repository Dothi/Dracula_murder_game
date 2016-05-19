using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InvisSkillIcon : MonoBehaviour
{

    public Sprite[] skillIcons;
    GameObject player;
    InvisibilitySkill invis;
    Image currentIcon;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        invis = player.GetComponent<InvisibilitySkill>();
        currentIcon = GetComponent<Image>();
    }

    void Update()
    {
        switch (invis.coolDown)
        {
            case 0:
                currentIcon.sprite = skillIcons[0];
                break;
            case 1:
                currentIcon.sprite = skillIcons[1];
                break;
            case 2:
                currentIcon.sprite = skillIcons[2];
                break;  
            default:
                break;
        }
    }
}
