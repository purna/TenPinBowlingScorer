using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Scorer.Tests.NUnit
{
	[TestFixture]
	public class Scorer_NUnit_Tests
	{
		string _player = "Denham";
		Scorer _scorer = null;

		[SetUp]
		public void CallBefore()
		{
			_scorer = new Scorer(_player);
		}

		[Test]
		public void ShouldReturnPlayersName()
		{
			Assert.That<string>(_scorer.Player, Is.EqualTo(_player));
		}

		[TestCase(0, 11)]
		[TestCase(12, 1)]
		[TestCase(6, 6)]
		[TestCase(-5, 8)]
		public void EnterInvalidFrameScores(int Bowl1, int Bowl2)
		{
			Assert.That(() => _scorer.FrameScore(Bowl1, Bowl2), Throws.TypeOf<ArgumentException>());
		}

		[TestCase(0, 0)]
		[TestCase(0, 10)]
		[TestCase(10, 0)]
		[TestCase(5, 5)]
		public void EnterValidFrameScores(int Bowl1, int Bowl2)
		{
			_scorer.FrameScore(Bowl1, Bowl2);
		}

		[Test]
		public void InitialisedScoreShouldBeZero()
		{
			var _currentScore = _scorer.Score;

			Assert.That(_currentScore, Is.EqualTo(0));
		}

		[Test]
		public void CalculateScoreWithoutSparesOrStrikes()
		{
			_scorer.FrameScore(0, 0);
			Assert.That(_scorer.Score, Is.EqualTo(0));

			_scorer.FrameScore(1, 2);
			Assert.That(_scorer.Score, Is.EqualTo(3));

			_scorer.FrameScore(3, 4);
			Assert.That(_scorer.Score, Is.EqualTo(10));

			_scorer.FrameScore(5, 4);
			Assert.That(_scorer.Score, Is.EqualTo(19));
		}

		[Test]
		public void CalculateScoreWithSpares()
		{
			_scorer.FrameScore(0, 0);
			Assert.That(_scorer.Score, Is.EqualTo(0));

			_scorer.FrameSpare(4);
			Assert.That(_scorer.Score, Is.EqualTo(10));

			_scorer.FrameScore(5, 4);
			Assert.That(_scorer.Score, Is.EqualTo(24));

			_scorer.FrameSpare(5);
			Assert.That(_scorer.Score, Is.EqualTo(34));

			_scorer.FrameScore(9, 0);
			Assert.That(_scorer.Score, Is.EqualTo(52));
		}

		[Test]
		public void CalculateScoreWithStrikes()
		{
			_scorer.FrameScore(0, 0);
			Assert.That(_scorer.Score, Is.EqualTo(0));

			_scorer.FrameStrike();
			Assert.That(_scorer.Score, Is.EqualTo(10));

			_scorer.FrameScore(5, 4);
			Assert.That(_scorer.Score, Is.EqualTo(28));

			_scorer.FrameStrike();
			Assert.That(_scorer.Score, Is.EqualTo(38));

			_scorer.FrameScore(9, 0);
			Assert.That(_scorer.Score, Is.EqualTo(56));
		}

		[Test]
		public void PerfectGameReturns300()
		{
			for (int _bowl = 1; _bowl <= 12; _bowl++)
				_scorer.FrameStrike();

			Assert.That(_scorer.Score, Is.EqualTo(300));
		}

		[Test]
		public void Check10SparesWereThrown()
		{
			for (int _bowl = 1; _bowl <= 10; _bowl++)
				_scorer.FrameSpare(5);

			Assert.That(_scorer.Spares, Is.EqualTo(10));
		}

		[Test]
		public void Check12StrikesWereThrown()
		{
			for (int _bowl = 1; _bowl <= 12; _bowl++)
				_scorer.FrameStrike();

			Assert.That(_scorer.Strikes, Is.EqualTo(12));
		}

		[Test]
		public void GetPerfectGameScoreCard()
		{
			for (int _bowl = 1; _bowl <= 12; _bowl++)
				_scorer.FrameStrike();

			var _scoreCard = _scorer.ScoreCard;

			Assert.That(_scoreCard.Player, Is.EqualTo(_player));
			Assert.That(_scoreCard.Scores.Count, Is.EqualTo(10));
			Assert.That(_scoreCard.Scores[9].Item3, Is.EqualTo(300));
			Assert.That(_scoreCard.Strikes, Is.EqualTo(12));
			Assert.That(_scoreCard.Spares, Is.EqualTo(0));
		}
	}
}
