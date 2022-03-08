using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowTrigger : MonoBehaviour
{
    public int row = -1;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gem"))
        {
            Gem g = collision.GetComponent<Gem>();
            g.row = row;
            GridManager.Instance.gems[g.row, g.col] = g;
        }
    }
}
