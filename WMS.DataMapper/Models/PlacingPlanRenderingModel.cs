using System.Collections.Generic;

namespace WMS.DataMapper.Models
{
    public class BoxRenderingModel
    {
        public int LineNumber { get; set; }

        // coordinates of a staring point
        public int Xo { get; set; }
        public int Yo { get; set; }
        public int Zo { get; set; }

        // coordinates of a finishing point
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int Z1 { get; set; }

        public string Color { get; set; }

        public string Name { get; set; }
    }

    public class ContainerRenderingModel
    {
        public string Name { get; set; }

        public int Length { get; set; }

        public int Width { get; set; }
    }

    public class PlacingPlanRenderingModel
    {
        public List<BoxRenderingModel> Boxes { get; set; }

        public ContainerRenderingModel Container { get; set; }

        public double ExecutionPercent { get; set; }

        public PlacingPlanRenderingModel()
        {
            Boxes = new List<BoxRenderingModel>();
        }
    }
}
