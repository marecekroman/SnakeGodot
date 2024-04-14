using Godot;
using System;
using System.Collections.Generic;

public partial class Gameplay : Node2D
{
	private const string Right = "RIGHT";
	private const string Left = "LEFT";
	private const string Up = "UP";
	private const string Down = "DOWN";

	[Export]
	public int ScreenWidth = 1024;
	[Export]
	public int ScreenHeight = 600;

	private Random _random = new Random();
	public int Score { get; private set; } = 5;
	public bool IsGameOver { get; private set; } = false;

	public Vector2 SnakeHead { get; private set; }
	public List<Vector2> SnakeBody { get; private set; } = new List<Vector2>();
	public Vector2 BerryPosition { get; private set; }
	public string CurrentDirection { get; set; } = Right;

	private double _moveDelay = 0.1f;
	private double _timeSinceLastMove = 0f;

	public override void _Ready()
	{
		ResetGame();
	}

	private void ResetGame()
	{
		IsGameOver = false;
		SnakeHead = new Vector2(ScreenWidth / 2, ScreenHeight / 2);
		SnakeBody.Clear();
		for (int i = 1; i < Score; i++)
		{
			SnakeBody.Add(new Vector2(SnakeHead.X - i * 10, SnakeHead.Y));
		}
		BerryPosition = CreateBerry();
		QueueRedraw();
	}

	private Vector2 CreateBerry()
	{
		return new Vector2(
			_random.Next(10, ScreenWidth - 10),
			_random.Next(10, ScreenHeight - 10)
		);
	}

	public override void _Process(double delta)
	{
		if (IsGameOver)
			return;

		_timeSinceLastMove += delta;
		if (_timeSinceLastMove >= _moveDelay)
		{
			HandleInput();
			MoveSnake();
			CheckCollision();
			_timeSinceLastMove = 0f;
		}
	}

	private void HandleInput()
	{
	if (Input.IsActionPressed("ui_up") && CurrentDirection != Down)
		CurrentDirection = Up;
	if (Input.IsActionPressed("ui_down") && CurrentDirection != Up)
		CurrentDirection = Down;
	if (Input.IsActionPressed("ui_left") && CurrentDirection != Right)
		CurrentDirection = Left;
	if (Input.IsActionPressed("ui_right") && CurrentDirection != Left)
		CurrentDirection = Right;
}

	private void MoveSnake()
	{
		Vector2 newHead = new Vector2(SnakeHead.X, SnakeHead.Y);
		switch (CurrentDirection)
		{
			case Up: newHead.Y -= 10; break;
			case Down: newHead.Y += 10; break;
			case Left: newHead.X -= 10; break;
			case Right: newHead.X += 10; break;
		}

		// Store the current position before changing
		SnakeBody.Insert(0, SnakeHead); 
		SnakeHead = newHead;

		if (SnakeHead.DistanceTo(BerryPosition) < 10)
		{
			Score++;
			BerryPosition = CreateBerry();
		}
		else
		{
			SnakeBody.RemoveAt(SnakeBody.Count - 1);
		}
		QueueRedraw();
	}

	private void CheckCollision()
	{
		if (SnakeHead.X < 0 || SnakeHead.X >= ScreenWidth || SnakeHead.Y < 0 || SnakeHead.Y >= ScreenHeight ||
			SnakeBody.Contains(SnakeHead))
		{
			IsGameOver = true;
			GD.Print("Game Over! Score: ", Score);
		}
	}

	public override void _Draw()
	{
		foreach (Vector2 segment in SnakeBody)
		{
			DrawRect(new Rect2(segment.X - 5, segment.Y - 5, 10, 10), Colors.Green);
		}
		DrawRect(new Rect2(SnakeHead.X - 5, SnakeHead.Y - 5, 10, 10), Colors.Red);
		DrawCircle(BerryPosition, 5, Colors.Red);
	}
}
