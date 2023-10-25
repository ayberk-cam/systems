using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasicSkillLevelBase
{
    [SerializeField]
    private string viewName;
    [SerializeField]
    private int level;
    [SerializeField]
    private string description;
    [SerializeField]
    private int amount;
    [SerializeField]
    private Sprite icon;

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

    public int Amount
    {
        get
        {
            return amount;
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
}

[CreateAssetMenu(menuName = "Maestro/BasicSkillBase")]
public class BasicSkillBase : ScriptableObject
{
    [SerializeField]
    private string baseName;
    [SerializeField]
    [NonReorderable]
    private BasicSkillLevelBase[] levels = new BasicSkillLevelBase[1];

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

    public BasicSkillLevelBase[] Levels
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