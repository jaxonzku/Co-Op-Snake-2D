using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private Snake snake;

    private LevelGrid levelGrid;
    void Start()
    {
        levelGrid = new LevelGrid(20, 20);
        snake.SetUp(levelGrid);
        levelGrid.SetUp(snake);



    }

    void Update()
    {
        
    }
}
