using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillCSVReader : MonoBehaviour
{
    private const string nameString = "Name";
    private const string levelString = "Level";
    private const string descriptionString = "Description";
    private const string cooldownString = "Cooldown";
    private const string baseHpString = "BaseHp";
    private const string baseDamageString = "BaseDamage";
    private const string amountString = "Amount";
    private const string attackDamageString = "AttackDamage";
    private const string attackSpeedString = "AttackSpeed";
    private const string sizeString = "Size";
    private const string hpString = "Hp";
    private const string activeTimeString = "ActiveTime";


    public static List<ActiveSkillCore> ReadActive(TextAsset csv)
    {
        var activeSkills = new List<ActiveSkillCore>();

        List<string> keys = new();

        string[,] grid = CSVReader.SplitCsvGrid(csv);

        for (int ind = 0; ind < grid.GetLength(1); ind++)
        {
            keys.Add(grid[0, ind]);
        }

        for (int rowInd = 1; rowInd < grid.GetLength(0); rowInd++)
        {
            var activeSkill = new ActiveSkillCore
            {
                Upgrades = new List<ActiveSkillUpgrade>(),

                Effects = new List<ActiveSkillEffect>()
            };

            for (int colInd = 0; colInd < grid.GetLength(1); colInd++)
            {
                switch (keys[colInd])
                {
                    case nameString:
                        activeSkill.Name = grid[rowInd, colInd];
                        break;
                    case levelString:
                        activeSkill.Level = int.Parse(grid[rowInd, colInd], System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case descriptionString:
                        activeSkill.Description = grid[rowInd, colInd];
                        break;
                    case cooldownString:
                        activeSkill.Cooldown = float.Parse(grid[rowInd, colInd], System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case amountString:
                        activeSkill.Amount = int.Parse(grid[rowInd, colInd], System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case activeTimeString:
                        if(grid[rowInd, colInd] == "")
                        {
                            activeSkill.ActiveTime = -1.0f;
                        }
                        else
                        {
                            activeSkill.ActiveTime = float.Parse(grid[rowInd, colInd], System.Globalization.CultureInfo.InvariantCulture);
                        }
                        break;
                    case baseHpString:
                        if(grid[rowInd, colInd] != "")
                        {
                            var effect = new ActiveSkillEffect
                            {
                                Effect = (ActiveSkillEffectType)System.Enum.Parse(typeof(ActiveSkillEffectType), baseHpString),
                                Amount = float.Parse(grid[rowInd, colInd], System.Globalization.CultureInfo.InvariantCulture)
                            };
                            activeSkill.Effects.Add(effect);
                        }
                        break;
                    case baseDamageString:
                        if (grid[rowInd, colInd] != "")
                        {
                            var effect = new ActiveSkillEffect
                            {
                                Effect = (ActiveSkillEffectType)System.Enum.Parse(typeof(ActiveSkillEffectType), baseDamageString),
                                Amount = float.Parse(grid[rowInd, colInd], System.Globalization.CultureInfo.InvariantCulture)
                            };
                            activeSkill.Effects.Add(effect);
                        }
                        break;
                    case attackDamageString:
                        if (grid[rowInd, colInd] != "")
                        {
                            var upgrade = new ActiveSkillUpgrade
                            {
                                Upgrade = (ActiveSkillUpgradeType)System.Enum.Parse(typeof(ActiveSkillUpgradeType), attackDamageString),
                                Amount = float.Parse(grid[rowInd, colInd], System.Globalization.CultureInfo.InvariantCulture)
                            };
                            activeSkill.Upgrades.Add(upgrade);
                        }
                        break;
                    case attackSpeedString:
                        if (grid[rowInd, colInd] != "")
                        {
                            var upgrade = new ActiveSkillUpgrade
                            {
                                Upgrade = (ActiveSkillUpgradeType)System.Enum.Parse(typeof(ActiveSkillUpgradeType), attackSpeedString),
                                Amount = float.Parse(grid[rowInd, colInd], System.Globalization.CultureInfo.InvariantCulture)
                            };
                            activeSkill.Upgrades.Add(upgrade);
                        }
                        break;
                    case sizeString:
                        if (grid[rowInd, colInd] != "")
                        {
                            var upgrade = new ActiveSkillUpgrade
                            {
                                Upgrade = (ActiveSkillUpgradeType)System.Enum.Parse(typeof(ActiveSkillUpgradeType), sizeString),
                                Amount = float.Parse(grid[rowInd, colInd], System.Globalization.CultureInfo.InvariantCulture)
                            };
                            activeSkill.Upgrades.Add(upgrade);
                        }
                        break;
                    case hpString:
                        if (grid[rowInd, colInd] != "")
                        {
                            var upgrade = new ActiveSkillUpgrade
                            {
                                Upgrade = (ActiveSkillUpgradeType)System.Enum.Parse(typeof(ActiveSkillUpgradeType), hpString),
                                Amount = float.Parse(grid[rowInd, colInd], System.Globalization.CultureInfo.InvariantCulture)
                            };
                            activeSkill.Upgrades.Add(upgrade);
                        }
                        break;
                    default:
                        Debug.LogWarning("Active Skill Stat with Given Tag Not Found");
                        break;
                }
            }
            activeSkills.Add(activeSkill);
        }
        return activeSkills;
    }
}
