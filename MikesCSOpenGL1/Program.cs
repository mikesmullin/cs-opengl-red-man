using System;

namespace MikesCSOpenGL1
{
	public class Program
	{
		static int Main (string[] args)
		{
			using (Game game = new Game ()) {
				game.Run (30.0);
			}
			return 0;
		}
	}
}

