﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockIndicators.Tests
{
    [TestClass]
    public class ConnorsRsiTests : TestBase
    {

        [TestMethod()]
        public void GetConnorsRsiTest()
        {
            int rsiPeriod = 3;
            int streakPeriod = 2;
            int rankPeriod = 100;
            int startPeriod = Math.Max(rsiPeriod, Math.Max(streakPeriod, rankPeriod)) + 2;

            IEnumerable<ConnorsRsiResult> results1 = Indicator.GetConnorsRsi(history, rsiPeriod, streakPeriod, rankPeriod);

            // assertions

            // proper quantities
            // should always be the same number of results as there is history
            Assert.AreEqual(502, results1.Count());
            Assert.AreEqual(502 - startPeriod + 1, results1.Where(x => x.ConnorsRsi != null).Count());

            // sample value
            ConnorsRsiResult r1 = results1.Where(x => x.Date == DateTime.Parse("12/31/2018")).FirstOrDefault();
            Assert.AreEqual((decimal)68.8087, Math.Round((decimal)r1.RsiClose, 4));
            Assert.AreEqual((decimal)67.4899, Math.Round((decimal)r1.RsiStreak, 4));
            Assert.AreEqual((decimal)88.0000, Math.Round((decimal)r1.PercentRank, 4));
            Assert.AreEqual((decimal)74.7662, Math.Round((decimal)r1.ConnorsRsi, 4));

            // different parameters
            IEnumerable<ConnorsRsiResult> results2 = Indicator.GetConnorsRsi(history, 14, 20, 10);
            ConnorsRsiResult r2 = results2.Where(x => x.Date == DateTime.Parse("12/31/2018")).FirstOrDefault();
            Assert.AreEqual((decimal)42.0773, Math.Round((decimal)r2.RsiClose, 4));
            Assert.AreEqual((decimal)52.7386, Math.Round((decimal)r2.RsiStreak, 4));
            Assert.AreEqual((decimal)90.0000, Math.Round((decimal)r2.PercentRank, 4));
            Assert.AreEqual((decimal)61.6053, Math.Round((decimal)r2.ConnorsRsi, 4));

        }


        /* EXCEPTIONS */

        [TestMethod()]
        [ExpectedException(typeof(BadParameterException), "Bad RSI period.")]
        public void BadRsiPeriod()
        {
            Indicator.GetConnorsRsi(history, 1, 2, 100);
        }

        [TestMethod()]
        [ExpectedException(typeof(BadParameterException), "Bad Streak period.")]
        public void BadStreakPeriod()
        {
            Indicator.GetConnorsRsi(history, 3, 1, 100);
        }

        [TestMethod()]
        [ExpectedException(typeof(BadParameterException), "Bad Rank period.")]
        public void BadPctRankPeriods()
        {
            Indicator.GetConnorsRsi(history, 3, 2, 1);
        }

        [TestMethod()]
        [ExpectedException(typeof(BadHistoryException), "Insufficient history.")]
        public void InsufficientHistory()
        {
            Indicator.GetConnorsRsi(history.Where(x => x.Index < 102), 3, 2, 100);
        }

    }
}