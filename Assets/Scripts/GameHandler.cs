using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    void Start()
    {
        Debug.Log("starrt");
        GameObject snakeHeadGameObject = new GameObject();
        SpriteRenderer snakeSpriteRenderer= snakeHeadGameObject.AddComponent<SpriteRenderer>();
        snakeSpriteRenderer.sprite = GameAssets.i.SnakeHeadSprite;


        
    }

    void Update()
    {
        
    }
}
