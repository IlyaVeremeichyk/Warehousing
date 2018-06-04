using System;
using System.Collections.Generic;
using System.Linq;
using MathLib.Logic.MathModelingProviders;
using MathLib.Logic.Models;
using Xunit;

namespace WMS.MathLib.Logic.Tests
{
    public class LoadingProgramProviderTest
    {
        private readonly LoadingProgramProvider _loadingProvider;

        private readonly Box _boxA, _boxB, _boxC;
        private readonly List<Box> _boxesSet;

        public LoadingProgramProviderTest()
        {
            _boxA = new Box("A", weight: 2D, cost: 65, orderQuantity: 5);
            _boxB = new Box("B", weight: 3D, cost: 80, orderQuantity: 5);
            _boxC = new Box("C", weight: 1D, cost: 30, orderQuantity: 5);

            _boxesSet = new List<Box> { _boxA, _boxB, _boxC };

            _loadingProvider = new LoadingProgramProvider();
        }

        public static IEnumerable<object[]> GetLoadProgram_NullOrEmptyBoxesSet
        {
            get
            {
                yield return new object[] { null, new Container() };
                yield return new object[] { new List<Box>(), new Container() };
            }
        }

        [Theory, MemberData(nameof(GetLoadProgram_NullOrEmptyBoxesSet))]
        public void GetLoadProgram_NullOrEmptyBoxesSet_ThrowsArgumentException(List<Box> boxesSet, Container container)
        {
            Assert.Throws<ArgumentException>(() => _loadingProvider.GetLoadProgram(boxesSet, container));
        }

        [Fact]
        public void GetLoadProgram_NullContainer_ThrowsArgumentNullException()
        {
            Container nullContainer = null;

            Assert.Throws<ArgumentNullException>(() => _loadingProvider.GetLoadProgram(_boxesSet, nullContainer));
        }

        public static IEnumerable<object[]> GetLoadProgram_InvalidPassedData
        {
            get
            {
                yield return new object[] { new List<Box> { new Box { Length = 0 } }, new Container(capacity: 5D) };
                yield return new object[] { new List<Box> { new Box { Length = -1 } }, new Container(capacity: 5D) };
                yield return new object[] { new List<Box> { new Box(cost: 10, weight: 10) { OrderQuantity = 1 } }, new Container(capacity: 0D) };
                yield return new object[] { new List<Box> { new Box(cost: 10, weight: 10) { OrderQuantity = 1 } }, new Container(capacity: -1D) };
            }
        }

        [Theory, MemberData(nameof(GetLoadProgram_InvalidPassedData))]
        public void GetLoadProgram_InvalidPassedData_ThrowsArgumentException(List<Box> boxesSet, Container container)
        {
            Assert.Throws<ArgumentException>(() => _loadingProvider.GetLoadProgram(boxesSet, container));
        }

        [Fact]
        public void GetLoadProgram_ValidDataAllChecksPass_ReturnsExpectedResult()
        {
            var container = new Container("Name1", capacity: 5D, length: 100, width: 100, height: 100);

            var actualResult = _loadingProvider.GetLoadProgram(_boxesSet, container);

            var actualBoxAResult = actualResult.Single(b => b.Box.Equals(_boxA)).Quantity;
            var actualBoxBResult = actualResult.Single(b => b.Box.Equals(_boxB)).Quantity;
            var actualBoxCResult = actualResult.Single(b => b.Box.Equals(_boxC)).Quantity;

            Assert.Equal(2, actualBoxAResult);
            Assert.Equal(0, actualBoxBResult);
            Assert.Equal(1, actualBoxCResult);
        }

        [Fact]
        public void GetLoadProgram_CheckingConstrainsForSizeFails_BoxesWithBiggerSizeAreExcludedFormContainer()
        {
            var container = new Container("Name1", capacity: 5D, length: 100, width: 100, height: 100);

            _boxA.Length = container.Length + 1;
            _boxB.Width = container.Width + 1;
            _boxC.Height = container.Height + 1;

            var actualResult = _loadingProvider.GetLoadProgram(_boxesSet, container).Count;

            Assert.Equal(0, actualResult);
        }

        [Fact]
        public void GetLoadProgram_CheckingConstrainsForVolumeFails_TheNumberOfBoxesWithBiggerVolumeIsSetToZero()
        {
            // The Volume of a Container is 6
            // The Volume of each Box is 6
            // Then only one box can be placed in a Container

            _boxA.Length = _boxB.Length = _boxC.Length = 2;
            _boxA.Width = _boxB.Width = _boxC.Width = 2;
            _boxA.Height = _boxB.Height = _boxC.Height = 2;

            var container = new Container("Name1", capacity: 5D, length: 2, width: 2, height: 2);

            var loadingPlan = _loadingProvider.GetLoadProgram(_boxesSet, container);

            int actualResult = loadingPlan.Sum(pair => pair.Quantity);

            Assert.Equal(1, actualResult);
        }

        [Fact]
        public void GetLoadProgram_CheckingConstrainsForOrderFails_TheNumberOfBoxesInContainerIsCahngedAccordingToOrderSize()
        {
            // Expected result without any constrains is A-2, B-0, C-1
            // If the order is A-1, B-1, C-1
            // Then the result will be changed to A-1, B-0, C-1

            var container = new Container("Name1", capacity: 5D, length: 100, width: 100, height: 100);

            _boxA.OrderQuantity = _boxB.OrderQuantity = _boxC.OrderQuantity = 1;

            var loadingPlan = _loadingProvider.GetLoadProgram(_boxesSet, container);

            var actualBoxAResult = loadingPlan.Single(b => b.Box.Equals(_boxA)).Quantity;
            var actualBoxBResult = loadingPlan.Single(b => b.Box.Equals(_boxB)).Quantity;
            var actualBoxCResult = loadingPlan.Single(b => b.Box.Equals(_boxC)).Quantity;

            Assert.Equal(1, actualBoxAResult);
            Assert.Equal(0, actualBoxBResult);
            Assert.Equal(1, actualBoxCResult);
        }
    }
}
