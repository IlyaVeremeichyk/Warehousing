using System;
using System.Collections.Generic;
using System.Linq;
using MathLib.Logic;
using MathLib.Logic.Models;
using Xunit;

namespace WMS.MathLib.Logic.Tests
{
    public class PlacingPlanManagerTest
    {
        private readonly PlacingPlanManager _PackagingLogicManager;

        private readonly BoxQunatityPair boxA, boxB, boxC;
        private readonly Container containerA;

        private readonly List<BoxQunatityPair> boxesSet;
        private readonly List<Container> containersSet;

        public PlacingPlanManagerTest()
        {
            boxA = new BoxQunatityPair { Box = new Box("A", length: 60, width: 60, height: 200), Quantity = 15 };
            boxB = new BoxQunatityPair { Box = new Box("B", length: 55, width: 60, height: 80), Quantity = 20 };
            boxC = new BoxQunatityPair { Box = new Box("C", length: 20, width: 20, height: 30), Quantity = 40 };

            containerA = new Container("Name1", length: 400, width: 300, height: 400);

            boxesSet = new List<BoxQunatityPair> { boxA, boxB, boxC };
            containersSet = new List<Container> { containerA };

            _PackagingLogicManager = new PlacingPlanManager();
        }

        public static IEnumerable<object[]> PlaceBoxes_NullOrEmptyBoxesSet
        {
            get
            {
                yield return new object[] { null, new List<Container> { new Container() } };
                yield return new object[] { new List<BoxQunatityPair>(), new List<Container> { new Container() } };
            }
        }

        [Theory, MemberData(nameof(PlaceBoxes_NullOrEmptyBoxesSet))]
        public void GetPlacingPlan_NullOrEmptyBoxesSet_ThrowsArgumentException(List<BoxQunatityPair> boxesSet, List<Container> containersSet)
        {
            Assert.Throws<ArgumentException>(() => _PackagingLogicManager.GetPlacingPlan(boxesSet, containersSet));
        }

        public static IEnumerable<object[]> PlaceBoxes_NullOrEmptyContainersSet
        {
            get
            {
                yield return new object[] { new List<BoxQunatityPair> { new BoxQunatityPair { Box = new Box { Length = 10, Width = 10, Height = 10 }, Quantity = 10 } }, null };
                yield return new object[] { new List<BoxQunatityPair> { new BoxQunatityPair { Box = new Box { Length = 10, Width = 10, Height = 10 }, Quantity = 10 } }, new List<Container>() };
            }
        }

        [Theory, MemberData(nameof(PlaceBoxes_NullOrEmptyContainersSet))]
        public void GetPlacingPlan_NullOrEmptyContainersSet_ThrowsArgumentException(List<BoxQunatityPair> boxesSet, List<Container> containersSet)
        {
            Assert.Throws<ArgumentException>(() => _PackagingLogicManager.GetPlacingPlan(boxesSet, containersSet));
        }

        public static IEnumerable<object[]> PlaceBoxes_InvalidPassedData
        {
            get
            {
                yield return new object[] { new List<BoxQunatityPair> { new BoxQunatityPair { Box = new Box { Length = 0 } } }, new List<Container> { new Container { Length = 10, Width = 10, Height = 10 } } };
                yield return new object[] { new List<BoxQunatityPair> { new BoxQunatityPair { Box = new Box { Length = -1 } } }, new List<Container> { new Container { Length = 10, Width = 10, Height = 10 } } };
                yield return new object[] { new List<BoxQunatityPair> { new BoxQunatityPair { Box = new Box { Length = 10, Width = 10, Height = 10 }, Quantity = -5 } }, new List<Container> { new Container { Length = 10, Width = 10, Height = 10 } } };
                yield return new object[] { new List<BoxQunatityPair> { new BoxQunatityPair { Box = new Box { Length = 10, Width = 10, Height = 10 }, Quantity = 10 } }, new List<Container> { new Container { Length = 0 } } };
                yield return new object[] { new List<BoxQunatityPair> { new BoxQunatityPair { Box = new Box { Length = 10, Width = 10, Height = 10 }, Quantity = 10 } }, new List<Container> { new Container { Length = -1 } } };
            }
        }

        [Theory, MemberData(nameof(PlaceBoxes_InvalidPassedData))]
        public void GetPlacingPlan_InvalidPassedData_ThrowsArgumentException(List<BoxQunatityPair> boxesSet, List<Container> containersSet)
        {
            Assert.Throws<ArgumentException>(() => _PackagingLogicManager.GetPlacingPlan(boxesSet, containersSet));
        }

        [Fact]
        public void GetPlacingPlan_ValidData_RetursPlacingProgram()
        {
            // Act

            var packagingPlan = _PackagingLogicManager.GetPlacingPlan(boxesSet, containersSet);
            var executionPercent = _PackagingLogicManager.CountExecutionPercent();

            //Assert

            Assert.NotEmpty(packagingPlan.First().PlacingPlan);
            Assert.InRange(executionPercent, 90, 100); // is more than 90%
        }
    }
}