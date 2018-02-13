using System;
using MathLib.Logic;
using WMS.BL.Interfaces;

namespace WMS.BL
{
    public class PlacingService : IPlacingService
    {
        private PlacingPlanManager _manager;

        public PlacingService()
        {
            _manager = null;
        }

        public string GetMessage()
        {
            return "HEre";
        }
    }
}
