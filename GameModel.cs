using Godot;
using System;
using System.Collections.Generic;

public partial class GameModel
{
	public Vector2 SnakeHead { get; private set; }
	public List<Vector2> SnakeBody { get; private set; } = new List<Vector2>();
	public Vector2 BerryPosition { get; private set; }
	public int Score { get; private set; } = 5;
	public bool IsGameOver { get; private set; } = false;

	public int screenWidth;
	public int screenHeight;
	private Random random = new Random();

	public GameModel(int width, int height)
	{
		screenWidth = width;
		screenHeight = height;
		ResetGame();
	}

	public void ResetGame()
	{
		SnakeHead = new Vector2(screenWidth / 2, screenHeight / 2);
		SnakeBody.Clear();
		SnakeBody.Add(SnakeHead);  
		
		for (int i = 1; i < Score; i++) {
			Vector2 newSegment = new Vector2(SnakeHead.X - i * 10, SnakeHead.Y);
			if (!SnakeBody.Contains(newSegment)) {
				SnakeBody.Add(newSegment);
			}
		}
		BerryPosition = CreateBerryPosition();
		IsGameOver = false;
		while (SnakeHead.DistanceTo(BerryPosition) < 20) {
			BerryPosition = CreateBerryPosition();
		}
	}

	public void MoveSnake(string direction)
	{
		if (IsGameOver) return;

		Vector2 directionVector = GetDirectionVector(direction);
		Vector2 newHead = SnakeHead + directionVector;
		
		if (SnakeBody.Contains(newHead)) {
			IsGameOver = true;
			return;
		}

		SnakeBody.Insert(0, newHead);
		SnakeHead = newHead;

		if (SnakeHead.DistanceTo(BerryPosition) < 10) {
			Score++;
			BerryPosition = CreateBerryPosition();
		} else {
			SnakeBody.RemoveAt(SnakeBody.Count - 1);
		}

		CheckCollision();
	}

	private Vector2 GetDirectionVector(string direction)
	{
		switch (direction) {
			case "UP": return new Vector2(0, -10);
			case "DOWN": return new Vector2(0, 10);
			case "LEFT": return new Vector2(-10, 0);
			case "RIGHT": return new Vector2(10, 0);
			default: return new Vector2();
		}
	}

	private Vector2 CreateBerryPosition()
	{
		return new Vector2(
			random.Next(10, screenWidth - 10),
			random.Next(10, screenHeight - 10)
		);
	}

	private void CheckCollision()
	{
		if (SnakeHead.X < 0 || SnakeHead.X >= screenWidth || SnakeHead.Y < 0 || SnakeHead.Y >= screenHeight) {
			IsGameOver = true;
		}
	}
}
