using CodeMonkey;
using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    private Vector2Int gridPosition;
    private Direction gridMoveDirection;
    private float gridMoveTimerMax;
    private float gridMoveTimer;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeBodyPositions;
    private List<GameObject> displayedSpriteObjects;
    private List<SnakeBodyPart> snakeBodyPartList;


    public void SetUp( LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    private void Awake()
    {
        gridPosition=new Vector2Int(10,10);
        gridMoveDirection=Direction.Right;
        gridMoveTimerMax = .5f;
        gridMoveTimer = gridMoveTimerMax;
        snakeBodySize = 0;
        snakeBodyPositions = new List<SnakeMovePosition>();
        displayedSpriteObjects=new List<GameObject>();
        snakeBodyPartList = new List<SnakeBodyPart>();
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
            if (gridMoveDirection != Direction.Down)
            {
                gridMoveDirection = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Direction.Up)
            {
                gridMoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection !=Direction.Right)
            {
                gridMoveDirection = Direction.Left;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection!=Direction.Left)
            {
                gridMoveDirection = Direction.Right;
            }
        }

    }
    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer > gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;
            SnakeMovePosition previousSnakeMovePosition=null;
            if (snakeBodyPositions.Count > 0)
            {
                previousSnakeMovePosition = snakeBodyPositions[0];
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition,gridPosition, gridMoveDirection);
           snakeBodyPositions.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector;
            switch (gridMoveDirection)
            {
                default:
                case Direction.Right: gridMoveDirectionVector = new Vector2Int(+1, 0);break;
                case Direction.Left:gridMoveDirectionVector = new Vector2Int(-1, 0);break;
                case Direction.Up:gridMoveDirectionVector = new Vector2Int(0, +1); break;
                case Direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
            }

            bool snakeAteFood = levelGrid.SnakeAtefood(gridPosition);
            if (snakeAteFood)
            {
                snakeBodySize++;
                CreateSnakeBody();
            }

            gridPosition += gridMoveDirectionVector;
            if (snakeBodyPositions.Count >= snakeBodySize + 1)
            {
                snakeBodyPositions.RemoveAt(snakeBodyPositions.Count - 1);
            }
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90);
            UpdateSnakeBodyParts();
            
        }
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].setSnakeMovePosition(snakeBodyPositions[i].GetGridPosition());
        }
    }

    private void CreateSnakeBody()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));

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
    public List<Vector2Int> getFullSnakeGridPosition()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        foreach(SnakeMovePosition snakeMovePosition in snakeBodyPositions)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }

        return gridPositionList;

    }
    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("snakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.SnakeBody;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }
        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);
            float angle;
            switch (snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up:
                    angle = 0;
                    break;
                case Direction.Down:
                    angle = 180;
                    break;
                case Direction.Left:
                    angle = -90;
                    break;
                case Direction.Right:
           
               
                    switch (snakeMovePosition.GetPreviousDirection()){
                        default:
                            angle = 90; break;
                        case Direction.Down:
                            angle = 45;break;



                    }break;


            }
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

         

    }

    private class SnakeMovePosition
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;
        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition,Vector2Int gridPosition,Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;

        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }
        public Direction GetDirection()
        {
            return direction;

        }
        public Direction GetPreviousDirection()
        {
            if (previousSnakeMovePosition == null)
            {
                return Direction.Right;
            }else
            {
                return previousSnakeMovePosition.direction;
            }
        }




    }

}
