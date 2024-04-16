using Godot;
using System.Collections.Generic;

public partial  class GameView : Node2D
{
	private GameModel model;
	private Color snakeColor = Colors.Green;
	private Color berryColor = Colors.Red;
	private Color borderColor = Colors.Red;

	public GameView(GameModel gameModel)
	{
		model = gameModel;
	}

	public override void _Draw()
	{
		foreach (Vector2 segment in model.SnakeBody)
		{
			DrawRect(new Rect2(segment.X - 5, segment.Y - 5, 10, 10), Colors.Green);
		}
		DrawRect(new Rect2(model.SnakeHead.X - 5, model.SnakeHead.Y - 5, 10, 10), Colors.Red);
		DrawCircle(model.BerryPosition, 5, Colors.Red);
		DrawBorders();
	}

	private void DrawBorders()
	{
		DrawLine(new Vector2(0, 0), new Vector2(model.screenWidth, 0), borderColor, 6);
		DrawLine(new Vector2(0, model.screenHeight), new Vector2(model.screenWidth, model.screenHeight), borderColor, 6);
		DrawLine(new Vector2(0, 0), new Vector2(0, model.screenHeight), borderColor, 6);
		DrawLine(new Vector2(model.screenWidth, 0), new Vector2(model.screenWidth, model.screenHeight), borderColor, 6);
	}

	public void UpdateView()
	{
		QueueRedraw();
	}
}
