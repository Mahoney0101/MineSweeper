class Minesweeper
{
	private const int Size = 8;
	private const int MineCount = 10;
	private char[,] board = new char[Size, Size];
	private bool[,] mines = new bool[Size, Size];
	private bool[,] revealed = new bool[Size, Size];

	public Minesweeper()
	{
		InitializeBoard();
		PlaceMines();
	}

	private void InitializeBoard()
	{
		for (var i = 0; i < Size; i++)
			for (var j = 0; j < Size; j++)
				board[i, j] = '-';
	}

	private void PlaceMines()
	{
		var rand = new Random();
		var placedMines = 0;
		while (placedMines < MineCount)
		{
			var x = rand.Next(Size);
			var y = rand.Next(Size);
			if (!mines[x, y])
			{
				mines[x, y] = true;
				placedMines++;
			}
		}
	}

	private int CountAdjacentMines(
		int x,
		int y)
	{
		var count = 0;
		for (var dx = -1; dx <= 1; dx++)
		{
			for (var dy = -1; dy <= 1; dy++)
			{
				int nx = x + dx, ny = y + dy;
				if (nx >= 0 && ny >= 0 && nx < Size && ny < Size && mines[nx, ny])
					count++;
			}
		}
		return count;
	}

	public void RevealCell(
		int x,
		int y)
	{
		if (x < 0 || y < 0 || x >= Size || y >= Size || revealed[x, y])
			return;

		revealed[x, y] = true;
		if (mines[x, y])
		{
			Console.WriteLine("Game Over! You hit a mine.");
			Environment.Exit(0);
		}
		else
		{
			var adjacentMines = CountAdjacentMines(x, y);
			board[x, y] = adjacentMines > 0 ? (char)('0' + adjacentMines) : ' ';
		}
	}

	public void RevealAll()
	{
		for (var i = 0; i < Size; i++)
		{
			for (var j = 0; j < Size; j++)
			{
				revealed[i, j] = true;
				if (mines[i, j])
				{
					board[i, j] = '*';
				}
				else
				{
					var adjacentMines = CountAdjacentMines(i, j);
					board[i, j] = adjacentMines > 0 ? (char)('0' + adjacentMines) : ' ';
				}
			}
		}
	}

	public void DisplayBoard()
	{
		Console.Clear();
		for (var i = 0; i < Size; i++)
		{
			for (var j = 0; j < Size; j++)
			{
				if (revealed[i, j])
					Console.Write(board[i, j] + " ");
				else
					Console.Write("- ");
			}
			Console.WriteLine();
		}
	}

	public void GiveHint()
	{
		for (var i = 0; i < Size; i++)
		{
			for (var j = 0; j < Size; j++)
			{
				if (!revealed[i, j] && !mines[i, j])
				{
					Console.WriteLine($"Hint: Try revealing cell ({i}, {j})");
					return;
				}
			}
		}
	}

	public void Play()
	{
		while (true)
		{
			DisplayBoard();
			Console.Write("Enter row and column (e.g., 3 4) or type 'hint' or 'reveal all': ");
			string input = Console.ReadLine();
			if (input.ToLower() == "hint")
			{
				GiveHint();
				Console.Write("Enter any value to continue");

				input = Console.ReadLine();
				continue;
			}
			if (input.ToLower() == "reveal all")
			{
				RevealAll();
				DisplayBoard();
				Console.WriteLine("All cells revealed!");
				Console.Write("Enter any value to continue");

				input = Console.ReadLine();
				return;
			}
			var parts = input.Split();
			if (parts.Length == 2 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))
			{
				RevealCell(x, y);
			}
		}
	}

	static void Main()
	{
		var game = new Minesweeper();
		game.Play();
	}
}
