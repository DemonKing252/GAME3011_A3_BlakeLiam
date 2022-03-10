using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public LayerMask gemMask;
    public Vector3 spawnLoc;
    public GemType gemType;
    public int row = -1;
    public int col = -1;

    public Rigidbody2D rb;
    public bool gravityEnabled = true;

    public bool IsStill 
    { 
        get
        {
            return Mathf.Abs(deltaMovementNow.y) < 0.2f;
        }
            
    }

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
    public void OnMouseDown()
    {
        Debug.Log("Clicked on gem row: " + row.ToString() + " col: " + col.ToString());
    }
    private Vector3 posPreviousFrame;
    private Vector3 posThisFrame;
    // Checking velocity won't work because it still reads a value < 0 even when on the ground because gravity is always applied.
    
    [SerializeField]
    private Vector3 deltaMovementNow;

    private void Awake()
    {
        GridManager.Instance.onTurnOffKinematics += OnKinematicsOff;
    }
    private void OnDestroy()
    {
        GridManager.Instance.onTurnOffKinematics -= OnKinematicsOff;
    }

    public void OnKinematicsOff()
    {
        gravityEnabled = true;
        //rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        posPreviousFrame = posThisFrame;
        posThisFrame = transform.position;

        deltaMovementNow = posThisFrame - posPreviousFrame;
    
        /*
        if (gravityEnabled)
        {
            transform.position -= Vector3.up * 19.6f * Time.deltaTime;
        }
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector3.down, 0.28f);
        
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.transform.name != transform.name)
                {
                    //Debug.Log(hit.transform.name + " != " + transform.name);
                    gravityEnabled = false;

                    Vector3 p = transform.position;
                    p.y = -2.0f + (float)(1.0f * (7 - row));
                    transform.position = p;
                }
            }
        }
        */
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, transform.position - new Vector3(0f, 0.48f, 0f));
    //}
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Gem"))
    //    {
    //        if (collision.transform.position.y > transform.position.y /*|| rb.bodyType == RigidbodyType2D.Dynamic*/)
    //            return;
    //
    //        //
    //        //
    //        //
    //
    //        //rb.bodyType = RigidbodyType2D.Static;
    //
    //        Debug.Log("Collided with object at col: ");
    //    }
    //}
}
