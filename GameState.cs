using System;
using System.Collections.Generic;
using System.Media;

namespace Snake;

public class GameState
{
    public int Rows { get; }
    public int Cols { get; }
    public GridValue[,] Grid { get; }
    public Direction Dir { get; private set; }
    public int Score { get; private set; }
    public bool GameOver { get; private set; }

    private readonly LinkedList<Direction> directionChanges = new ();
    private readonly LinkedList<Position> snakePositions = new ();
    private readonly Random Random = new ();
 
    public GameState(int rows, int cols)
    {
        Rows = rows;
        Cols = cols;
        Grid = new GridValue[rows, cols];
        Dir = Direction.Right;
        
        AddSnake();
        AddFood();
    }

    private void AddSnake()
    {
        int r = Rows / 2;

        for (int c = 1; c <= 3; c++)
        {
            Grid[r, c] = GridValue.Snake;
            snakePositions.AddFirst(new Position(r, c));
        }
    }

    private IEnumerable<Position> EmptyPositions()
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                if (Grid[r, c] == GridValue.Empty)
                {
                    yield return new Position(r, c);
                }
            }
        }
    }

    private void AddFood()
    {
        List<Position> empty = new List<Position>(EmptyPositions());
        if (empty.Count == 0)
        {
            return;
        }

        Position pos = empty[Random.Next(empty.Count)];
        Grid[pos.Row, pos.Col] = GridValue.Food;
    }

    public Position HeadPosition()
    {
        return snakePositions.First.Value;
    }

    public Position TailPosition()
    {
        return snakePositions.Last.Value;
    }

    public IEnumerable<Position> SnakePositions()
    {
        return snakePositions;
    }

    private void AddHead(Position pos)
    {
        snakePositions.AddFirst(pos);
        Grid[pos.Row, pos.Col] = GridValue.Snake;
    }

    private void RemoveTail()
    {
        Position tail = snakePositions.Last.Value;
        Grid[tail.Row, tail.Col] = GridValue.Empty;
        snakePositions.RemoveLast();
    }

    private Direction GetLastDirection()
    {
        if (directionChanges.Count == 0)
        {
            return Dir;
        }

        return directionChanges.Last.Value;
    }

    private bool CanChaneDirection(Direction newDirection)
    {
        if (directionChanges.Count == 2)
        {
            return false;
        }

        Direction lastDirection = GetLastDirection();
        return newDirection != lastDirection && newDirection != lastDirection.Opposite();
    }

    public void ChangeDirection(Direction direction)
    {
        if (CanChaneDirection(direction))
        {
            directionChanges.AddLast(direction);
        }
    }

    private bool OutsideGrid(Position position)
    {
        return position.Row < 0 || position.Row >= Rows || position.Col < 0 || position.Col >= Cols;
    }

    private GridValue WillHit(Position newHeadPosition)
    {
        if (OutsideGrid(newHeadPosition))
        {
            return GridValue.Outside;
        }

        if (newHeadPosition == TailPosition())
        {
            return GridValue.Empty;
        }

        return Grid[newHeadPosition.Row, newHeadPosition.Col];
    }

    public void Move()
    {
        if (directionChanges.Count > 0)
        {
            Dir = directionChanges.First.Value;
            directionChanges.RemoveFirst();
        }

        Position newHeadPosition = HeadPosition().Translate(Dir);
        GridValue hit = WillHit(newHeadPosition);

        if (hit == GridValue.Outside)
        {
            
        } 
        else if (hit == GridValue.Snake)
        {
            GameOver = true;
        } 
        else if (hit == GridValue.Empty)
        {
            RemoveTail();
            AddHead(newHeadPosition);
        } 
        else if (hit == GridValue.Food)
        {
            //Sounds.eatApplePlayer.Load();
            //Sounds.eatApplePlayer.Play();
            AddHead(newHeadPosition);
            Score++;
            AddFood();
        }
    }
}