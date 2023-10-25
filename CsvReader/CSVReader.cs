using UnityEngine;
using System.Collections;

public class CSVReader
{
	static public string DebugOutputGrid(string[,] grid)
	{
		string textOutput = "";
		for (int y = 0; y < grid.GetUpperBound(1); y++)
		{
			for (int x = 0; x < grid.GetUpperBound(0); x++)
			{

				textOutput += grid[x, y];
				textOutput += "|";
			}
			textOutput += "\n";
		}
		return textOutput;
	}

	static public string[,] SplitCsvGrid(TextAsset csvTextAsset)
	{
		string[] lines = csvTextAsset.text.Split("\n"[0]);

		int width = 0;
		for (int i = 0; i < lines.Length; i++)
		{
			string[] row = lines[i].Trim().Split(',');
			width = Mathf.Max(width, row.Length);
		}

		string[,] outputGrid = new string[lines.Length, width];
		for (int y = 0; y < lines.Length; y++)
		{
			string[] row = lines[y].Trim().Split(',');
			for (int x = 0; x < row.Length; x++)
			{
				outputGrid[y, x] = row[x].Trim(new char[] { '\"', ' ' });

				outputGrid[y, x] = outputGrid[y, x].Replace("\"\"", "\"");
			}
		}

		return outputGrid;
	}
}