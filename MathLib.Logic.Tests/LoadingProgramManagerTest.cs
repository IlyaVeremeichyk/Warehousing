using System;
using System.Collections.Generic;
using System.Linq;
using MathLib.Logic.Models;
using Xunit;

namespace MathLib.Logic.Tests
{
    public class LoadingProgramManagerTest
    {
        private readonly LoadingProgramManager _DPLogicManager;

        private readonly Box boxA, boxB, boxC;
        private readonly Container containerA, containerB;

        private readonly List<Box> boxesSet;
        private readonly List<Container> containresSet;

        public LoadingProgramManagerTest()
        {
            boxA = new Box("A", weight: 2D, cost: 65, orderQuantity: 5);
            boxB = new Box("B", weight: 3D, cost: 80, orderQuantity: 5);
            boxC = new Box("C", weight: 1D, cost: 30, orderQuantity: 5);

            containerA = new Container("Name1", capacity: 5D);
            containerB = new Container("Name2", capacity: 5D);

            boxesSet = new List<Box> { boxA, boxB, boxC };
            containresSet = new List<Container> { containerA, containerB };

            _DPLogicManager = new LoadingProgramManager();
        }

        public static IEnumerable<object[]> GetLoadProgram_NullOrEmptyBoxesSet
        {
            get
            {
                yield return new object[] { null, new List<Container>() { new Container() } };
                yield return new object[] { new List<Box>(), new List<Container>() { new Container() } };
            }
        }

        [Theory, MemberData(nameof(GetLoadProgram_NullOrEmptyBoxesSet))]
        public void GetLoadProgram_NullOrEmptyBoxesSet_ThrowsArgumentException(List<Box> boxesSet,
            List<Container> containersSet)
        {
            Assert.Throws<ArgumentException>(() => _DPLogicManager.GetLoadProgram(boxesSet, containersSet));
        }

        public static IEnumerable<object[]> GetLoadProgram_NullOrEmptyContainersSet
        {
            get
            {
                yield return new object[] { new List<Box>() { new Box() }, null };
                yield return new object[] { new List<Box>() { new Box() }, new List<Container>() };
            }
        }

        [Theory, MemberData(nameof(GetLoadProgram_NullOrEmptyContainersSet))]
        public void GetLoadProgram_NullOrEmptyContainersSet_ThrowsArgumentException(List<Box> boxesSet,
            List<Container> containersSet)
        {
            Assert.Throws<ArgumentException>(() => _DPLogicManager.GetLoadProgram(boxesSet, containersSet));
        }

        public static IEnumerable<object[]> GetLoadProgram_InvalidPassedData
        {
            get
            {
                yield return new object[] { new List<Box> { new Box { Length = 0 } }, new List<Container> { new Container {Capacity = 5D}} };
                yield return new object[] { new List<Box> { new Box { Length = -1 } }, new List<Container> { new Container { Capacity = 5D } } };
                yield return new object[] { new List<Box> { new Box { Cost = 10, OrderQuantity = 1, Weight = 10} }, new List<Container> { new Container { Capacity = 0D } } };
                yield return new object[] { new List<Box> { new Box { Cost = 10, OrderQuantity = 1, Weight = 10 } }, new List<Container> { new Container { Capacity = -1D } } };
            }
        }

        // !!! Not tested
        [Theory, MemberData(nameof(GetLoadProgram_InvalidPassedData))]
        public void GetLoadProgram_InvalidPassedData_ThrowsArgumentException(List<Box> boxesSet, List<Container> containersSet)
        {
            Assert.Throws<ArgumentException>(() => _DPLogicManager.GetLoadProgram(boxesSet, containersSet));
        }

        [Theory]
        [InlineData(1)] // check only the first
        [InlineData(-1)] // check all
        public void GetLoadProgram_ValidDataAllChecksPass_ReturnsExpectedResult(int containersNumberToCheck)
        {
            // Arrange
            var containersToCheck = containersNumberToCheck < 0 ? containresSet.Count : containersNumberToCheck;

            containerA.Length = containerA.Width = containerA.Height = 100;
            containerB.Length = containerB.Width = containerB.Height = 100;

            //Act

            var actualResult = _DPLogicManager.GetLoadProgram(boxesSet, containresSet);

            //Assert

            for (int i = 0; i < containersToCheck; i++)
            {
                var actualBoxAResult = actualResult[i].PlacedBoxes.Single(b => b.Box.Equals(boxA)).Quantity;
                var actualBoxBResult = actualResult[i].PlacedBoxes.Single(b => b.Box.Equals(boxB)).Quantity;
                var actualBoxCResult = actualResult[i].PlacedBoxes.Single(b => b.Box.Equals(boxC)).Quantity;

                Assert.Equal(2, actualBoxAResult);
                Assert.Equal(0, actualBoxBResult);
                Assert.Equal(1, actualBoxCResult);
            }
        }

        [Fact]
        public void GetLoadProgram_CheckingConstrainsForSizeFails_BoxesWithBiggerSizeAreExcludedFormContainer()
        {
            //Arrange

            boxA.Length = containresSet.First().Length + 1;
            boxB.Width = containresSet.First().Width + 1;
            boxC.Height = containresSet.First().Height + 1;

            //Act

            var actualResult = _DPLogicManager.GetLoadProgram(boxesSet, containresSet).First().PlacedBoxes.Count;

            //Assert

            Assert.Equal(0, actualResult);
        }

        [Fact]
        public void GetLoadProgram_CheckingConstrainsForVolumeFails_TheNumberOfBoxesWithBiggerVolumeIsSetToZero()
        {
            // The Volume of a Container is 6
            // The Volume of each Box is 6
            // Then only one box can be placed in a Container

            //Arrange

            boxA.Length = boxB.Length = boxC.Length = 2;
            boxA.Width = boxB.Width = boxC.Width = 2;
            boxA.Height = boxB.Height = boxC.Height = 2;

            containerA.Length = containerA.Width = containerA.Height = 2;
            containerB.Length = containerB.Width = containerB.Height = 2;

            int actualResult = 0;

            //Act 

            var loadingPlan = _DPLogicManager.GetLoadProgram(boxesSet, containresSet);

            foreach (var placedBox in loadingPlan.First().PlacedBoxes)
            {
                actualResult += placedBox.Quantity;
            }

            //Assert

            Assert.Equal(1, actualResult);
        }

        [Fact]
        public void GetLoadProgram_CheckingConstrainsForOrderFails_TheNumberOfBoxesInContainerIsCahngedAccordingToOrderSize()
        {
            // Expected result without any constrains is A-2, B-0, C-1
            // If the order is A-1, B-1, C-1
            // Then the result will be changed to A-1, B-0, C-1

            //Arrange
            boxA.OrderQuantity = boxB.OrderQuantity = boxC.OrderQuantity = 1;

            containerA.Length = containerA.Width = containerA.Height = 100;
            containerB.Length = containerB.Width = containerB.Height = 100;

            // Act
            var loadingPlan = _DPLogicManager.GetLoadProgram(boxesSet, containresSet);

            //Assert

            var actualBoxAResult = loadingPlan.First().PlacedBoxes.Single(b => b.Box.Equals(boxA)).Quantity;
            var actualBoxBResult = loadingPlan.First().PlacedBoxes.Single(b => b.Box.Equals(boxB)).Quantity;
            var actualBoxCResult = loadingPlan.First().PlacedBoxes.Single(b => b.Box.Equals(boxC)).Quantity;

            Assert.Equal(1, actualBoxAResult);
            Assert.Equal(0, actualBoxBResult);
            Assert.Equal(1, actualBoxCResult);
        }

        [Fact]
        public void GetLoadProgram_CheckOrderPlanIsFulfiled_TheNumberOfEachBoxInBothContainersIsEqualToOrderSize()
        {
            // Expected result without any constrains is A-2, B-0, C-1
            // If the order is A-1, B-1, C-1
            // Then the result for the first container: A-1, B-0, C-1
            // Then the result for the second container: A-0, B-1, C-0

            //Arrange
            boxA.OrderQuantity = boxB.OrderQuantity = boxC.OrderQuantity = 1;

            containerA.Length = containerA.Width = containerA.Height = 100;
            containerB.Length = containerB.Width = containerB.Height = 100;

            int actualBoxAResult = 0;
            int actualBoxBResult = 0;
            int actualBoxCResult = 0;

            // Act
            var loadingPlan = _DPLogicManager.GetLoadProgram(boxesSet, containresSet);

            //Assert

            foreach (var container in loadingPlan)
            {
                actualBoxAResult += container.PlacedBoxes.Single(b => b.Box.Equals(boxA)).Quantity;
                actualBoxBResult += container.PlacedBoxes.Single(b => b.Box.Equals(boxB)).Quantity;
                actualBoxCResult += container.PlacedBoxes.Single(b => b.Box.Equals(boxC)).Quantity;
            }

            Assert.Equal(1, actualBoxAResult);
            Assert.Equal(1, actualBoxBResult);
            Assert.Equal(1, actualBoxCResult);
        }
    }
}
