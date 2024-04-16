using Godot;
using System;

public partial class GameController : Node
{
	private GameModel model;
	private GameView view;
	private Label gameOverLabel;
	private Button restartButton;
	private string currentDirection = "RIGHT";
	private double moveDelay = 0.1;
	private double timeSinceLastMove = 0;

	public override void _Ready()
	{
		model = new GameModel(1024, 600);
		view = new GameView(model);
		AddChild(view);

		gameOverLabel = GetNode<Label>("GameOverLabel");
		restartButton = GetNode<Button>("ResetButton");

		restartButton.Connect("pressed", new Callable(this, nameof(OnRestartButtonPressed)));

		gameOverLabel.Hide();
		restartButton.Hide();

		SetProcess(true);
	}

	public override void _Process(double delta)
	{
		if (model.IsGameOver)
		{
			GD.Print("Game Over! Score: ", model.Score);
			UpdateGameUI();
			return;
		}

		timeSinceLastMove += delta;
		if (timeSinceLastMove >= moveDelay)
		{
			HandleInput();
			model.MoveSnake(currentDirection);
			view.UpdateView();
			timeSinceLastMove = 0;
		}
	}

	private void OnRestartButtonPressed()
	{
		ResetGame();
	}

	private void ResetGame()
	{
		model.ResetGame();
		UpdateGameUI();
	}

	private void UpdateGameUI()
	{
		if (model.IsGameOver)
		{
			gameOverLabel.Text = $"Game Over! \nScore: {model.Score}";
			gameOverLabel.Show();
			restartButton.Show();
		}
		else
		{
			gameOverLabel.Hide();
			restartButton.Hide();
		}
	}

	private void HandleInput()
	{
		if (Input.IsActionPressed("ui_up") && currentDirection != "DOWN")
			currentDirection = "UP";
		else if (Input.IsActionPressed("ui_down") && currentDirection != "UP")
			currentDirection = "DOWN";
		else if (Input.IsActionPressed("ui_left") && currentDirection != "RIGHT")
			currentDirection = "LEFT";
		else if (Input.IsActionPressed("ui_right") && currentDirection != "LEFT")
			currentDirection = "RIGHT";
	}
}
