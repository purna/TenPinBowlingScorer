using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorer
{
	public class Scorer
	{
		public int Strikes { private set; get; } = 0;
		public int Spares { private set; get; } = 0;

		private string _player;
		private int _frameCount = 0;
		private List<Tuple<int, int>> _frameScores = new List<Tuple<int, int>>();
		private List<Tuple<int, int, int>> _scoreCard = new List<Tuple<int, int, int>>();

		public Scorer(string Player)
		{
			this._player = Player;
		}

		public string Player
		{
			get { return _player; }
		}

		public void FrameScore(int Bowl1, int Bowl2)
		{
			if (Bowl1 < 0 || Bowl1 > 10)
				throw new ArgumentException("Bowl1 must be between 0 and 10");

			if (Bowl2 < 0 || Bowl2 > 10)
				throw new ArgumentException("Bowl2 must be between 0 and 10");

			var _score = Bowl1 + Bowl2;

			if (_score < 0 || _score > 10)
				throw new ArgumentException("Invalid scores entered");

			_frameScores.Add(new Tuple<int, int>(Bowl1, Bowl2));
			_frameCount++;
		}

		public void FrameSpare(int Bowl1)
		{
			Spares++;
			FrameScore(Bowl1, 10 - Bowl1);
		}

		public void FrameStrike()
		{
			Strikes++;
			FrameScore(10, 0);
		}

		public ScoreCard ScoreCard
		{
			get {
				_scoreCard = new List<Tuple<int, int, int>>();
				var _score = Score;
				return new ScoreCard(Player, Strikes, Spares, _scoreCard);
			}
		}

		public int Score
		{
			get
			{
				// Set Framecount to 10 if the 10th Frame had a strike or spare
				if (_frameCount > 10)
					_frameCount = 10;

				Tuple<int, int> _thisFramePlus1 = null;
				Tuple<int, int> _thisFramePlus2 = null;

				int _currentScore = 0;

				for (int _bowl = 0; _bowl < _frameCount; _bowl++)
				{
					var _currentFrame = _bowl + 1;
					var _thisFrame = _frameScores[_bowl];

					if (_currentFrame == _frameScores.Count)
					{
						_thisFramePlus1 = new Tuple<int, int>(0, 0);
						_thisFramePlus2 = new Tuple<int, int>(0, 0);
					}
					else if (_currentFrame == _frameScores.Count - 1)
					{
						_thisFramePlus1 = _frameScores[_bowl + 1];
						_thisFramePlus2 = new Tuple<int, int>(0, 0);
					} else
					{
						_thisFramePlus1 = _frameScores[_bowl + 1];
						_thisFramePlus2 = _frameScores[_bowl + 2];
					}

					var _bowl1 = _thisFrame.Item1;
					var _bowl2 = _thisFrame.Item2;

					if (_bowl1 == 10 && _thisFramePlus1.Item1 == 10)
					{
						_currentScore += 10 + 10 + _thisFramePlus2.Item1;
						UpdateScoreCard(_bowl1, _bowl2, _currentScore);
						continue;
					}

					if (_bowl1 == 10 && _thisFramePlus1.Item1 != 10)
					{
						_currentScore += 10 + _thisFramePlus1.Item1 + _thisFramePlus1.Item2;
						UpdateScoreCard(_bowl1, _bowl2, _currentScore);
						continue;
					}

					if (_bowl1 + _bowl2 == 10)
					{
						_currentScore += 10 + _thisFramePlus1.Item1;
						UpdateScoreCard(_bowl1, _bowl2, _currentScore);
						continue;
					}

					_currentScore += _bowl1;
					_currentScore += _bowl2;

					UpdateScoreCard(_bowl1, _bowl2, _currentScore);
				}

				return _currentScore;
			}
		}

		private void UpdateScoreCard(int Bowl1, int Bowl2, int CurrentScore)
		{
			_scoreCard.Add(new Tuple<int, int, int>(Bowl1, Bowl2, CurrentScore));
		}
	}
}
