using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorer
{
	public class ScoreCard
	{
		public string Player {private set; get;}
		public int Strikes { private set; get; }
		public int Spares { private set; get; }
		public List<Tuple<int, int, int>> Scores { private set; get; }

		public ScoreCard(string Player, int Strikes, int Spares, List<Tuple<int, int, int>> Scores)
		{
			this.Player = Player;
			this.Strikes = Strikes;
			this.Spares = Spares;
			this.Scores = Scores;
		}
	}
}
