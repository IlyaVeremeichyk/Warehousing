using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathLib.Logic;
using MathLib.Logic.Models;
using Xunit;

using Newtonsoft.Json;

namespace WMS.DataMapper.Tests
{
    public class MapperFor3DVisualizationTest
    {
        private MapperFor3DVisualization _mapperFor3DVisualization;
        private readonly PlacingPlanManager _placingPlanManager;

        private readonly BoxQunatityPair boxA, boxB, boxC;
        private readonly Container containerA;

        private readonly List<BoxQunatityPair> boxesSet;
        private readonly List<Container> containersSet;

        public MapperFor3DVisualizationTest()
        {
            boxA = new BoxQunatityPair { Box = new Box("A", length: 60, width: 60, height: 200), Quantity = 6 };
            boxB = new BoxQunatityPair { Box = new Box("B", length: 55, width: 60, height: 80), Quantity = 12};
            boxC = new BoxQunatityPair { Box = new Box("C", length: 20, width: 20, height: 30), Quantity = 100 };

            containerA = new Container("Name1", length: 300, width: 250, height: 200);

            boxesSet = new List<BoxQunatityPair> { boxA, boxB, boxC };
            containersSet = new List<Container> { containerA };

            _placingPlanManager = new PlacingPlanManager();
            _mapperFor3DVisualization = new MapperFor3DVisualization();
        }

        [Fact]
        public void GetPlacingPlanAdaptedForSeenjsRendering_ReturnsRenderedData()
        {
            // Act
            var packagingPlan = _placingPlanManager.GetPlacingPlan(boxesSet, containersSet);
            var executionPercent = _placingPlanManager.CountExecutionPercent();

            var renderedResult = _mapperFor3DVisualization.GetPlacingPlanAdaptedForSeenjsRendering(packagingPlan.First(), executionPercent);

            File.WriteAllText(@"D:\placingPlan.json", JsonConvert.SerializeObject(renderedResult));

            //Assert
            Assert.NotNull(renderedResult.Container);
            Assert.NotEmpty(renderedResult.Boxes);
            Assert.NotEqual(0D, renderedResult.ExecutionPercent);
        }
    }
}
