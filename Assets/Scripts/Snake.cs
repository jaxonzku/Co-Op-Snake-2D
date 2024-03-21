using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridPosition;
    private Vector2Int gridMoveDirection;
    private float gridMoveTimerMax;
    private float gridMoveTimer;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<Vector2Int> snakeBodyPositions;
    private List<GameObject> displayedSpriteObjects;


    public void SetUp( LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    private void Awake()
    {
        gridPosition=new Vector2Int(10,10);
        gridMoveDirection=new Vector2Int(0,1);
        gridMoveTimerMax = .5f;
        gridMoveTimer = gridMoveTimerMax;
        snakeBodySize = 0;
        snakeBodyPositions = new List<Vector2Int>();
        displayedSpriteObjects=new List<GameObject>();
    }
    private void Update()
    {
        HandleInput();
        HandleGridMovement();
        Debug.Log(displayedSpriteObjects.Count);
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection.y != -1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = +1;

            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection.y != +1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection.x != +1)
            {
                gridMoveDirection.y = 0;
                gridMoveDirection.x = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection.x != -1)
            {
                gridMoveDirection.y = 0;
                gridMoveDirection.x = +1;
            }
        }

    }
    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer > gridMoveTimerMax)
        {
            
            gridPosition += gridMoveDirection;
            gridMoveTimer -= gridMoveTimerMax;
            snakeBodyPositions.Insert(0, gridPosition);
            Debug.Log(snakeBodyPositions.Count);
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection)-90);
            bool snakeAte=levelGrid.SnakeAtefood(gridPosition);
            if (snakeAte)
            {
                snakeBodySize++;

            }
            if (snakeBodyPositions.Count >= snakeBodySize + 1)
            {
                snakeBodyPositions.RemoveAt(snakeBodyPositions.Count-1);
                if (displayedSpriteObjects.Count != 0)
                {
                    Object.Destroy(displayedSpriteObjects[displayedSpriteObjects.Count - 1]);

                }



            }
            for (int i = 0; i < snakeBodyPositions.Count; i++)
            {
                Vector2Int pos = snakeBodyPositions[i];
                DisplaySpriteAtPosition(pos);

            }


        }
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n=Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public void DisplaySpriteAtPosition(Vector2Int position)
    {

        GameObject spriteObject = new GameObject("Sprite");
        SpriteRenderer spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = GameAssets.i.SnakeBody;
        spriteObject.transform.position = new Vector3(position.x, position.y, 0);
        displayedSpriteObjects.Insert(0, spriteObject);

    }

}
