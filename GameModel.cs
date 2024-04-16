//This project is done by Roman Marecek for Tomas Bata University in Zlin, Czech Republic
//This code is refactored and modified fom the original code from https://codereview.stackexchange.com/questions/127515/first-c-program-snake-game

using Godot;
using System;
using System.Collections.Generic;

public partial class GameModel
{
	public Vector2 SnakeHead { get; private set; }
	public List<Vector2> SnakeBody { get; private set; } = new List<Vector2>();
	public Vector2 BerryPosition { get; private set; }
	
	// Initial score and game state flags.
	public int Score { get; private set; } = 5;
	public bool IsGameOver { get; private set; } = false;

	public int screenWidth;
	public int screenHeight;
	
	// Random number generator for placing the berryPosition randomly.
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
		
		// Initialize the snake body based on the initial score.
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

		// Move the snake head and adjust the body accordingly.
		SnakeBody.Insert(0, newHead);
		SnakeHead = newHead;

		// If the new head position is on the berry, eat the berry and grow.
		if (SnakeHead.DistanceTo(BerryPosition) < 10) {
			Score++;
			BerryPosition = CreateBerryPosition();
		} else {
			// Remove the tail segment if no berry was eaten
			SnakeBody.RemoveAt(SnakeBody.Count - 1);
		}

		CheckCollision();
	}

	private Vector2 GetDirectionVector(string direction)
	{
		// Calculate new head position based on current direction.
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
		// Place a new berry at a random position within the game boundaries.
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
