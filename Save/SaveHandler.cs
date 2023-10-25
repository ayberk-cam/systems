using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class LevelSave : SaveObj
{
    public int levelNumber;
    public int difficulty;
    public int gridSize;
    public int pieceAmount;
    public List<TileSave> tiles = new();

    public void SetLevelSave(int levelNumber, int difficulty, int gridSize, int pieceAmount)
    {
        this.levelNumber = levelNumber;
        this.difficulty = difficulty;
        this.gridSize = gridSize;
        this.pieceAmount = pieceAmount;
    }
}

[System.Serializable]
public class TileSave
{
    public int xCoordinate;
    public int yCoordinate;
    public int groupNumber;
    public Color32 color;

    public void SetTileSave(int x, int y, int groupNumber, Color32 color)
    {
        this.xCoordinate = x;
        this.yCoordinate = y;
        this.groupNumber = groupNumber;
        this.color = color;
    }
}

public class SaveHandler : MonoBehaviour
{
    private readonly string levelSaveFileName = "/LevelSave_";
    private string levelSavePath;

    void Awake()
    {
        levelSavePath = Application.persistentDataPath + levelSaveFileName;
    }

    private void SaveLevel(LevelSave level, int levelNumber)
    {
        SaveController.SaveObject(level, levelSavePath + levelNumber.ToString());
    }

    private void LoadLevel(int levelNumber)
    {
        var levelSaveObj = SaveController.LoadObject<LevelSave>(levelSavePath + levelNumber.ToString(), out bool fileExists);

        if (fileExists)
        {
            //load
        }
        else
        {
            Debug.LogWarning("There is no level on save");
        }
    }
}
