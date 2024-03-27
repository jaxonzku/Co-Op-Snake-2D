using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid
{
    private Vector2Int foodGridPosition;
    private int width;
    private int height;
    private GameObject foodGameObject;
    private Snake snake;
    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void SetUp(Snake snake)
    {
        this.snake = snake;
        spawnFood();

    }


    private void spawnFood()
    {
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.getFullSnakeGridPosition().IndexOf(foodGridPosition)!=-1);



        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodsprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    public bool SnakeAtefood(Vector2Int snakeGridPosition)
    {
        if (foodGridPosition == snakeGridPosition)
        {
            Debug.Log("snake Ate Food");
            Object.Destroy(foodGameObject);
            spawnFood();
            return true;
        }
        else
        {
            return false;
        }
    }



}
