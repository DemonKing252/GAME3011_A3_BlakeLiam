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

    [SerializeField]
    private Vector3 deltaMovementNow;
    private Vector3 posPreviousFrame;
    private Vector3 posThisFrame;


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

        GridManager.Instance.OnGemSelected(this);
    }
    // Checking velocity won't work because it still reads a value < 0 even when on the ground because gravity is always applied.
    

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
    
    }
    
}
