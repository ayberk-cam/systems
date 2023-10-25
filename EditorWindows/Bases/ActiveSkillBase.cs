using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveSkillLevelBase
{
    [SerializeField]
    private string viewName;
    [SerializeField]
    private int level;
    [SerializeField]
    private string description;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private int amount;
    [SerializeField]
    private float activeTime;
    [SerializeField]
    [NonReorderable]
    private List<ActiveSkillUpgrade> upgrades = new();
    [SerializeField]
    [NonReorderable]
    private List<ActiveSkillEffect> effects = new();

    public string ViewName
    {
        get
        {
            return viewName;
        }
        set
        {
            viewName = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }
    }

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    public float Cooldown
    {
        get
        {
            return cooldown;
        }
    }

    public float ActiveTime
    {
        get
        {
            return activeTime;
        }
    }

    public int Amount
    {
        get
        {
            return amount;
        }
    }

    public List<ActiveSkillUpgrade> Upgrades
    {
        get
        {
            return upgrades;
        }
        set
        {
            upgrades = value;
        }
    }

    public List<ActiveSkillEffect> Effects
    {
        get
        {
            return effects;
        }
        set
        {
            effects = value;
        }
    }
}

[CreateAssetMenu(menuName = "Maestro/ActiveSkillBase")]
public class ActiveSkillBase : ScriptableObject
{
    [SerializeField]
    private string baseName;
    [SerializeField]
    [NonReorderable]
    private ActiveSkillLevelBase[] levels = new ActiveSkillLevelBase[3];

    public string BaseName
    {
        get
        {
            return baseName;
        }
        set
        {
            baseName = value;
        }
    }

    public ActiveSkillLevelBase[] Levels
    {
        get
        {
            return levels;
        }
        set
        {
            levels = value;
        }
    }
}