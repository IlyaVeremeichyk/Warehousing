using System.Collections.Generic;
using System.Linq;
using MathLib.Logic.Models;
using WMS.DataMapper.Models;

namespace WMS.DataMapper
{
    public class MapperFor3DVisualization
    {
        private List<string> _colors;
        private Dictionary<string,string> _boxColorDictionary;

        private const int Interval = 6;

        public MapperFor3DVisualization()
        {
            _colors = new List<string>
            {
                "#00897b", "#ffb300", "#00acc1", "#e53935",
                "#43a047", "#d81b60", "#8e24aa", "#f4511e",
                "#3949ab", "#1e88e5", "#c0ca33", "#fdd835"
            };

            _boxColorDictionary = new Dictionary<string, string>();
        }

        public PlacingPlanRenderingModel GetPlacingPlanAdaptedForSeenjsRendering(ModelingResult modelingResult)
        {
            var result = new PlacingPlanRenderingModel();

            var container = modelingResult.Container;
            result.Container = new ContainerRenderingModel { Name = container.Name, Width = container.Width + Interval, Length = container.Length + Interval};

            result.ExecutionPercent = modelingResult.ExecutionPercent;

            foreach(var line in modelingResult.PlacingPlan)
            {
                foreach(var box in line)
                {
                    var item = new BoxRenderingModel
                    {
                        LineNumber = modelingResult.PlacingPlan.IndexOf(line) + 1,
                        Xo = box.XPos + Interval,
                        X1 = box.XPos + box.Length + Interval,
                        Yo = box.YPos + Interval,
                        Y1 = box.YPos + box.Width + Interval,
                        Zo = box.ZPos + Interval,
                        Z1 = box.ZPos + box.Height + Interval,
                        Color = DefineBoxColor(box.Name),
                        Name = box.Name
                    };

                    result.Boxes.Add(item);
                }
            }

            return result;
        }

        #region Private methods

        private string DefineBoxColor(string boxName)
        {
            if (_boxColorDictionary.Keys.Contains(boxName))
            {
                return _boxColorDictionary[boxName];
            }

            var freeColor = _colors.First(c => !_boxColorDictionary.Values.Contains(c));

            _boxColorDictionary.Add(boxName, freeColor);

            return freeColor;
        }

        #endregion
    }
}
