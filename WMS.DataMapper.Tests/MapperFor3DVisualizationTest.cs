using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathLib.Logic.MathModelingProviders;
using MathLib.Logic.Models;
using MathLib.Logic.ResultsProvider;
using Xunit;

using Newtonsoft.Json;

namespace WMS.DataMapper.Tests
{
    public class MapperFor3DVisualizationTest
    {
        private readonly MapperFor3DVisualization _mapperFor3DVisualization;
        private readonly ModelingResultsProvider _resultsProvider;

        private readonly List<Box> _boxesSet;
        private readonly Container _container;

        public MapperFor3DVisualizationTest()
        {
            var boxA = new Box("A", length: 60, width: 60, height: 200, weight: 2D, cost: 65, orderQuantity: 5);
            var boxB = new Box("B", length: 55, width: 100, height: 80, weight: 3D, cost: 80, orderQuantity: 5);
            var boxC = new Box("C", length: 20, width: 20, height: 30, weight: 1D, cost: 30, orderQuantity: 5);

            _container = new Container("Name1", length: 300, width: 280, height: 200, capacity: 5D);

            _boxesSet = new List<Box> { boxA, boxB, boxC };

            _resultsProvider = new ModelingResultsProvider();
            _mapperFor3DVisualization = new MapperFor3DVisualization();
        }

        [Fact]
        public void GetPlacingPlanAdaptedForSeenjsRendering_ReturnsRenderedData()
        {
            // Act

            var result = _resultsProvider.GetLoadingAndPlacingPrograms(_boxesSet, _container);

            var renderedResult = _mapperFor3DVisualization.GetPlacingPlanAdaptedForSeenjsRendering(result);

            File.WriteAllText(@"D:\placingPlan.json", JsonConvert.SerializeObject(renderedResult));

            //Assert
            Assert.NotNull(renderedResult.Container);
            Assert.NotEmpty(renderedResult.Boxes);
            Assert.NotEqual(0D, renderedResult.ExecutionPercent);
        }
    }
}
