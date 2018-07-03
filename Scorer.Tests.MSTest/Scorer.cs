using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scorer.Tests.MSTest
{
	[TestClass]
	public class Scorer_MSTest_Tests
	{
		string _player = "Denham";
		Scorer _scorer = null;

		[TestInitialize]
		public void CallBefore()
		{
			_scorer = new Scorer(_player);
		}

		[TestMethod]
		public void ShouldReturnPlayersName()
		{
			Assert.AreEqual(_player, _scorer.Player);
		}

		[DataTestMethod]
		[ExpectedException(typeof(ArgumentException))]
		[DataRow(0, 11)]
		[DataRow(12, 1)]
		[DataRow(6, 6)]
		[DataRow(-5, 8)]
		public void EnterInvalidFrameScores(int Bowl1, int Bowl2)
		{
			_scorer.FrameScore(Bowl1, Bowl2);
		}

		[DataTestMethod]
		[DataRow(0, 0)]
		[DataRow(0, 10)]
		[DataRow(10, 0)]
		[DataRow(5, 5)]
		public void EnterValidFrameScores(int Bowl1, int Bowl2)
		{
			_scorer.FrameScore(Bowl1, Bowl2);
		}

		[TestMethod]
		public void InitialisedScoreShouldBeZero()
		{
			var _currentScore = _scorer.Score;

			Assert.AreEqual(0, _currentScore);
		}

		[TestMethod]
		public void CalculateScoreWithoutSparesOrStrikes()
		{
			_scorer.FrameScore(0, 0);
			Assert.AreEqual(0, _scorer.Score);
			
			_scorer.FrameScore(1, 2);
			Assert.AreEqual(3, _scorer.Score);

			_scorer.FrameScore(3, 4);
			Assert.AreEqual(10, _scorer.Score);

			_scorer.FrameScore(5, 4);
			Assert.AreEqual(19, _scorer.Score);   
		}

		[TestMethod]
		public void CalculateScoreWithSpares()
		{
			_scorer.FrameScore(0, 0);
			Assert.AreEqual(0, _scorer.Score);

			_scorer.FrameSpare(4);
			Assert.AreEqual(10, _scorer.Score);

			_scorer.FrameScore(5, 4);
			Assert.AreEqual(24, _scorer.Score);

			_scorer.FrameSpare(5);
			Assert.AreEqual(34, _scorer.Score);

			_scorer.FrameScore(9, 0);
			Assert.AreEqual(52, _scorer.Score);
		}

		[TestMethod]
		public void CalculateScoreWithStrikes()
		{
			_scorer.FrameScore(0, 0);
			Assert.AreEqual(0, _scorer.Score);

			_scorer.FrameStrike();
			Assert.AreEqual(10, _scorer.Score);

			_scorer.FrameScore(5, 4);
			Assert.AreEqual(28, _scorer.Score);

			_scorer.FrameStrike();
			Assert.AreEqual(38, _scorer.Score);

			_scorer.FrameScore(9, 0);
			Assert.AreEqual(56, _scorer.Score);
		}

		[TestMethod]
		public void PerfectGameReturns300()
		{
			for (int _bowl = 1; _bowl <= 12; _bowl++)
				_scorer.FrameStrike();

			Assert.AreEqual(300, _scorer.Score);
		}

		[TestMethod]
		public void Check10SparesWereThrown()
		{
			for (int _bowl = 1; _bowl <= 10; _bowl++)
				_scorer.FrameSpare(5);

			Assert.AreEqual(10, _scorer.Spares);
		}

		[TestMethod]
		public void Check12StrikesWereThrown()
		{
			for (int _bowl = 1; _bowl <= 12; _bowl++)
				_scorer.FrameStrike();

			Assert.AreEqual(12, _scorer.Strikes);
		}

		[TestMethod]
		public void GetPerfectGameScoreCard()
		{
			for (int _bowl = 1; _bowl <= 12; _bowl++)
				_scorer.FrameStrike();

			var _scoreCard = _scorer.ScoreCard;

			Assert.AreEqual(_player, _scoreCard.Player);
			Assert.AreEqual(10, _scoreCard.Scores.Count);
			Assert.AreEqual(300, _scoreCard.Scores[9].Item3);
			Assert.AreEqual(12, _scoreCard.Strikes);
			Assert.AreEqual(0, _scoreCard.Spares);
		}																		   
	}
}
