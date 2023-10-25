using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillRepo", menuName = "AyberkRepos/ActiveSkillRepo")]
public class ActiveSkillsRepo : ScriptableObject
{
    [SerializeField] List<ActiveSkillCore> list;

    private Dictionary<ActiveSkillCore, int> dict;

    private void List2Dict()
    {
        dict = new();

        foreach (var unit in list)
        {
            if (!dict.ContainsKey(unit))
            {
                dict.Add(unit, unit.Level);
            }
            else
            {
                Debug.LogWarning(unit.Name + " " + unit.Level.ToString() + "Duplicate");
            }
        }
    }

    public ActiveSkillCore GetUnit(string name, int level)
    {
        ActiveSkillCore unit = null;

        if (dict == null)
        {
            List2Dict();
        }

        foreach (var skill in list)
        {
            if (skill.Name == name && skill.Level == level)
            {
                unit = skill;
            }
        }

        if (unit == null)
        {
            Debug.LogWarning(name + " " + level.ToString() + " Does not exist");
        }

        return unit;
    }

    public ActiveSkillCore GetRandomUnit()
    {
        ActiveSkillCore unit = null;

        if (list != null)
        {
            if (list.Count > 0)
            {
                unit = list[Random.Range(0, list.Count)];
            }
        }
        return unit;
    }

    public List<ActiveSkillCore> GetSkillsWithLevel(int level)
    {
        var units = new List<ActiveSkillCore>();

        foreach (var unit in list)
        {
            if (unit.Level == level)
            {
                units.Add(unit);
            }
        }

        return units;
    }

    public void SetRepo(List<ActiveSkillCore> skills)
    {
        list = new();

        list.AddRange(skills);

        List2Dict();
    }

    public List<ActiveSkillCore> GetAllList()
    {
        return list;
    }

    public bool MaxLevelChecker(string name, int level)
    {
        if(Container(name, level+1))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool CheckExistance(string name)
    {
        foreach (var skill in list)
        {
            if (skill.Name == name)
            {
                return true;
            }
        }

        return false;
    }

    public bool Container(string name, int level)
    {
        foreach(var skill in list)
        {
            if(skill.Name == name && skill.Level == level)
            {
                return true;
            }
        }
        return false;
    }
}
