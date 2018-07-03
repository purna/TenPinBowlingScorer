using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Scorer.Tests.XUnit
{
	public class Scorer_XUnit_Tests
	{
		string _player = "Denham";
		Scorer _scorer = null;

		public Scorer_XUnit_Tests()
		{
			_scorer = new Scorer(_player);
		}

		[Fact]
		public void ShouldReturnPlayersName()
		{
			Assert.Equal(_player, _scorer.Player);
		}

		[Theory]
		[InlineData(0, 11)]
		[InlineData(12, 1)]
		[InlineData(6, 6)]
		[InlineData(-5, 8)]
		public void EnterInvalidFrameScores(int Bowl1, int Bowl2)
		{
			Assert.Throws<ArgumentException>( () => _scorer.FrameScore(Bowl1, Bowl2));
		}

		[Theory]
		[InlineData(0, 0)]
		[InlineData(0, 10)]
		[InlineData(10, 0)]
		[InlineData(5, 5)]
		public void EnterValidFrameScores(int Bowl1, int Bowl2)
		{
			_scorer.FrameScore(Bowl1, Bowl2);
		}

		[Fact]
		public void InitialisedScoreShouldBeZero()
		{
			var _currentScore = _scorer.Score;

			Assert.Equal(0, _currentScore);
		}

		[Fact]
		public void CalculateScoreWithoutSparesOrStrikes()
		{
			_scorer.FrameScore(0, 0);
			Assert.Equal(0, _scorer.Score);

			_scorer.FrameScore(1, 2);
			Assert.Equal(3, _scorer.Score);

			_scorer.FrameScore(3, 4);
			Assert.Equal(10, _scorer.Score);

			_scorer.FrameScore(5, 4);
			Assert.Equal(19, _scorer.Score);
		}

		[Fact]
		public void CalculateScoreWithSpares()
		{
			_scorer.FrameScore(0, 0);
			Assert.Equal(0, _scorer.Score);

			_scorer.FrameSpare(4);
			Assert.Equal(10, _scorer.Score);

			_scorer.FrameScore(5, 4);
			Assert.Equal(24, _scorer.Score);

			_scorer.FrameSpare(5);
			Assert.Equal(34, _scorer.Score);

			_scorer.FrameScore(9, 0);
			Assert.Equal(52, _scorer.Score);
		}

		[Fact]
		public void CalculateScoreWithStrikes()
		{
			_scorer.FrameScore(0, 0);
			Assert.Equal(0, _scorer.Score);

			_scorer.FrameStrike();
			Assert.Equal(10, _scorer.Score);

			_scorer.FrameScore(5, 4);
			Assert.Equal(28, _scorer.Score);

			_scorer.FrameStrike();
			Assert.Equal(38, _scorer.Score);

			_scorer.FrameScore(9, 0);
			Assert.Equal(56, _scorer.Score);
		}

		[Fact]
		public void PerfectGameReturns300()
		{
			for (int _bowl = 1; _bowl <= 12; _bowl++)
				_scorer.FrameStrike();

			Assert.Equal(300, _scorer.Score);
		}

		[Fact]
		public void Check10SparesWereThrown()
		{
			for (int _bowl = 1; _bowl <= 10; _bowl++)
				_scorer.FrameSpare(5);

			Assert.Equal(10, _scorer.Spares);
		}

		[Fact]
		public void Check12StrikesWereThrown()
		{
			for (int _bowl = 1; _bowl <= 12; _bowl++)
				_scorer.FrameStrike();

			Assert.Equal(12, _scorer.Strikes);
		}

		[Fact]
		public void GetPerfectGameScoreCard()
		{
			for (int _bowl = 1; _bowl <= 12; _bowl++)
				_scorer.FrameStrike();

			var _scoreCard = _scorer.ScoreCard;

			Assert.Equal(_player, _scoreCard.Player);
			Assert.Equal(10, _scoreCard.Scores.Count);
			Assert.Equal(300, _scoreCard.Scores[9].Item3);
			Assert.Equal(12, _scoreCard.Strikes);
			Assert.Equal(0, _scoreCard.Spares);
		}
	}
}
