using System;
using System.Collections.Generic;

namespace MathLib.Models
{
    public class FilledContainer
    {
        #region Properties

        public Container container { get; internal set; }
        public List<Result> placedBoxes { get; internal set; }

        #endregion

        #region Constructors

        internal FilledContainer() { }

        internal FilledContainer(Container container, 
   List<Result> placedBoxes)
        {
            this.container = container;
            this.placedBoxes = placedBoxes;
        }

        #endregion
    }

}