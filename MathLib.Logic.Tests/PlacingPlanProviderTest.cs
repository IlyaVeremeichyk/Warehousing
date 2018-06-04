using System;
using System.Collections.Generic;
using MathLib.Logic.MathModelingProviders;
using MathLib.Logic.Models;
using Xunit;

namespace WMS.MathLib.Logic.Tests
{
    public class PlacingPlanProviderTest
    {
        private readonly PlacingPlanProvider _placingPlanProvider;

        private readonly List<BoxQuantityPair> _boxesSet;
        private readonly Container _container;

        public PlacingPlanProviderTest()
        {
            var boxA = new BoxQuantityPair { Box = new Box("A", length: 60, width: 60, height: 200), Quantity = 15 };
            var boxB = new BoxQuantityPair { Box = new Box("B", length: 55, width: 60, height: 80), Quantity = 20 };
            var boxC = new BoxQuantityPair { Box = new Box("C", length: 20, width: 20, height: 30), Quantity = 40 };

            _container = new Container("Name1", length: 400, width: 300, height: 400);

            _boxesSet = new List<BoxQuantityPair> { boxA, boxB, boxC };

            _placingPlanProvider = new PlacingPlanProvider();
        }

        public static IEnumerable<object[]> PlaceBoxes_NullOrEmptyBoxesSet
        {
            get
            {
                yield return new object[] { null, new Container() };
                yield return new object[] { new List<BoxQuantityPair>(), new Container() };
            }
        }

        [Theory, MemberData(nameof(PlaceBoxes_NullOrEmptyBoxesSet))]
        public void GetPlacingPlan_NullOrEmptyBoxesSet_ThrowsArgumentException(List<BoxQuantityPair> boxesSet, Container container)
        {
            Assert.Throws<ArgumentException>(() => _placingPlanProvider.GetPlacingPlan(boxesSet, _container));
        }

        [Fact]
        public void GetPlacingPlan_NullContainer_ThrowsArgumentNullException()
        {
            Container nullContainer = null;

            Assert.Throws<ArgumentNullException>(() => _placingPlanProvider.GetPlacingPlan(_boxesSet, nullContainer));
        }

        public static IEnumerable<object[]> PlaceBoxes_InvalidPassedData
        {
            get
            {
                yield return new object[] { new List<BoxQuantityPair> { new BoxQuantityPair { Box = new Box { Length = 0 } } }, new Container(length: 10, width: 10, height: 10) };
                yield return new object[] { new List<BoxQuantityPair> { new BoxQuantityPair { Box = new Box { Length = -1 } } }, new Container(length: 10, width: 10, height: 10) };
                yield return new object[] { new List<BoxQuantityPair> { new BoxQuantityPair { Box = new Box { Length = 10, Width = 10, Height = 10 }, Quantity = -5 } }, new Container(length: 10, width: 10, height: 10) };
                yield return new object[] { new List<BoxQuantityPair> { new BoxQuantityPair { Box = new Box { Length = 10, Width = 10, Height = 10 }, Quantity = 10 } }, new Container(length: 0) };
                yield return new object[] { new List<BoxQuantityPair> { new BoxQuantityPair { Box = new Box { Length = 10, Width = 10, Height = 10 }, Quantity = 10 } }, new Container(length: -1) };
            }
        }

        [Theory, MemberData(nameof(PlaceBoxes_InvalidPassedData))]
        public void GetPlacingPlan_InvalidPassedData_ThrowsArgumentException(List<BoxQuantityPair> boxesSet, Container container)
        {
            Assert.Throws<ArgumentException>(() => _placingPlanProvider.GetPlacingPlan(boxesSet, container));
        }

        [Fact]
        public void GetPlacingPlan_ValidData_RetursPlacingProgram()
        {
            var packagingPlan = _placingPlanProvider.GetPlacingPlan(_boxesSet, _container);
            var executionPercent = _placingPlanProvider.CountExecutionPercent();

            Assert.NotEmpty(packagingPlan);
            Assert.InRange(executionPercent, 90, 100); // is more than 90%
        }
    }
}