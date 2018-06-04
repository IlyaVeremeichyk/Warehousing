using System.Collections.Generic;

namespace MathLib.Logic.Models
{
    public class ModelingResult
    {
        public Container Container { get; internal set; }

        // The collection of boxes to be placed in this container
        // Box - a box object, int - boxes quantity
        public List<BoxQuantityPair> LoadingProgram { get; internal set; }

        // Each internal list is one row of placing
        public List<List<Box>> PlacingPlan { get; internal set; }

        public double ExecutionPercent { get; set; }
    }
}
