using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimesoft.Games.Memory.Domain.Factories;
using NUnit.Framework;

namespace Dimesoft.Games.Memory.Domain.Tests.Factories
{
    [TestFixture]
    public class ColorFactoryTests
    {

        [Test]
        public void BuildEasyBoard_WillReturnCorrectNumberOfSets()
        {
            var factory = new ColorFactory();

            var board = factory.BuildMediumBoard();

            Assert.AreEqual(LevelConstants.EasyLevel, board.Sets.Count);
        }

        [Test]
        public void BuildMediumBoard_WillReturnCorrectNumberOfSets()
        {
            var factory = new ColorFactory();

            var board = factory.BuildMediumBoard();

            Assert.AreEqual(LevelConstants.MediumLevel, board.Sets.Count);
        }

        [Test]
        public void BuildHardBoard_WillReturnCorrectNumberOfSets()
        {
            var factory = new ColorFactory();

            var board = factory.BuildMediumBoard();

            Assert.AreEqual(LevelConstants.HardLevel, board.Sets.Count);
        }
    }
}
