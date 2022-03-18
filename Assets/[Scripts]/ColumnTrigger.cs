using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnTrigger : MonoBehaviour
{
    public int col = -1;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gem"))
        {
            Gem g = collision.GetComponent<Gem>();
            g.col = col;
            GridManager.Instance.gems[g.row, g.col] = g;
        }
    }
}
