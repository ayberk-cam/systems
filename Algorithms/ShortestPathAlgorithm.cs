using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingItem
{
    public int x;
    public int y;
    public int distance;

    public MovingItem(int x, int y, int distance)
    {
        this.x = x;
        this.y = y;
        this.distance = distance;
    }
}

//Finding the shortest distance from a source cell to a destination cell.

public static class ShortestDistanceAlgorithm
{
    public static int width;
    public static int height;

    public static int MinDistance(int[,] grid, int gridWidth, int gridHeight)
    {
        width = gridWidth;
        height = gridHeight;

        MovingItem source = new(0, 0, 0);

        bool[,] visited = new bool[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grid[i, j] == 3)
                {
                    visited[i, j] = true;
                }
                else
                {
                    visited[i, j] = false;
                }

                if (grid[i, j] == 0)
                {
                    source.x = i;
                    source.y = j;
                }
            }
        }

        Queue<MovingItem> itemsQ = new Queue<MovingItem>();
        itemsQ.Enqueue(source);
        visited[source.x, source.y] = true;

        //0 = source, 1 = destination, 2 = not matched, 3 = matched

        while (itemsQ.Count > 0)
        {
            MovingItem item = itemsQ.Peek();
            itemsQ.Dequeue();

            // Destination found;
            if (grid[item.x, item.y] == 1)
            {
                return item.distance;
            }

            // moving up
            if (item.x - 1 >= 0 && visited[item.x - 1, item.y] == false)
            {
                itemsQ.Enqueue(new MovingItem(item.x - 1, item.y, item.distance + 1));
                visited[item.x - 1, item.y] = true;
            }

            // moving down
            if (item.x + 1 < width && visited[item.x + 1, item.y] == false)
            {
                itemsQ.Enqueue(new MovingItem(item.x + 1, item.y, item.distance + 1));
                visited[item.x + 1, item.y] = true;
            }

            // moving left
            if (item.y - 1 >= 0 && visited[item.x, item.y - 1] == false)
            {
                itemsQ.Enqueue(new MovingItem(item.x, item.y - 1, item.distance + 1));
                visited[item.x, item.y - 1] = true;
            }

            // moving right
            if (item.y + 1 < height && visited[item.x, item.y + 1] == false)
            {
                itemsQ.Enqueue(new MovingItem(item.x, item.y + 1, item.distance + 1));
                visited[item.x, item.y + 1] = true;
            }
        }

        return -1;
    }
}
