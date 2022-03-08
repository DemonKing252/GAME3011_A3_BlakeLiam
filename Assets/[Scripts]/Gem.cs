using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public GemType gemType;
    public int row = -1;
    public int col = -1;

    public Gem GetLeftNeighbor()
    {
        if (col - 1 >= 0)
        {
            return GridManager.Instance.gems[row, col - 1];
        }
        else
            return null;
    }
    public Gem GetRightNeighbor()
    {
        if (col + 1 <= 7)
        {
            return GridManager.Instance.gems[row, col + 1];
        }
        else
            return null;
    }
    public Gem GetUpNeighbor()
    {
        if (row - 1 >= 0)
        {
            return GridManager.Instance.gems[row - 1, col];
        }
        else
            return null;
    }
    public Gem GetDownNeighbor()
    {
        if (row + 1 <= 7)
        {
            return GridManager.Instance.gems[row + 1, col];
        }
        else
            return null;
    }
}
