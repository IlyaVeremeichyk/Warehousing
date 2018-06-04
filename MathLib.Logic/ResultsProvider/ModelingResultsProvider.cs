using System;
using System.Collections.Generic;
using MathLib.Logic.MathModelingProviders;
using MathLib.Logic.Models;

namespace MathLib.Logic.ResultsProvider
{
    public class ModelingResultsProvider
    {
        private readonly LoadingProgramProvider _loadingProgramProvider;
        private readonly PlacingPlanProvider _placingPlanProvider;

        public ModelingResultsProvider()
        {
            _loadingProgramProvider = new LoadingProgramProvider();
            _placingPlanProvider = new PlacingPlanProvider();
        }

        public ModelingResult GetLoadingAndPlacingPrograms(List<Box> boxesForPlacing, Container container)
        {
            if (boxesForPlacing == null)
            {
                throw new ArgumentNullException(nameof(boxesForPlacing));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            var loadingProgram = _loadingProgramProvider.GetLoadProgram(boxesForPlacing, container);
            var placingPlan = _placingPlanProvider.GetPlacingPlan(loadingProgram, container);
            var executionPercent = _placingPlanProvider.CountExecutionPercent();

            return new ModelingResult
            {
                Container = container,
                LoadingProgram = loadingProgram,
                PlacingPlan = placingPlan,
                ExecutionPercent = executionPercent
            };
        }
    }
}
