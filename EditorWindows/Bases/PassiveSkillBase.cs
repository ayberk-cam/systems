using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PassiveSkillLevelBase
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
    [NonReorderable]
    private PassiveSkillEffect effect;

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

    public PassiveSkillEffect Effect
    {
        get
        {
            return effect;
        }
    }
}

[CreateAssetMenu(menuName = "Maestro/PassiveSkillBase")]
public class PassiveSkillBase : ScriptableObject
{
    [SerializeField]
    private string baseName;
    [SerializeField]
    [NonReorderable]
    private PassiveSkillLevelBase[] levels = new PassiveSkillLevelBase[3];

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

    public PassiveSkillLevelBase[] Levels
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