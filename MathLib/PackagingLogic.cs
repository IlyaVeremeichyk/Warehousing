using System;
using System.Collections.Generic;
using System.Linq;
using MathLib.Models.Models3D;

namespace MathLib {
    public class PackagingLogic {

        #region Fields

        private int xo = 0, yo = 0, zo = 0;
        private int maxRowWidth = 0;
        private int boxesQuantityBeforePlacing = 0;
        private int boxesQuantityAfterPlacing = 0;
        private int leftToTheCeiling = 0;

        private List<Box> BoxesSet = new List<Box> ();
        private List<Box> Row = new List<Box> ();
        private List<Box> RowsSet = new List<Box> ();
        private List<Box> PlatformsSet = new List<Box> ();

        // private Logger.ILogger logger;

        #endregion

        #region Constructors

        public PackagingLogic () {
            // this.logger = new Logger.NLogger();
        }

        // public PackagingLogic(ILogger logger)
        // {
        //     this.logger = logger;
        // }

        #endregion

        #region Public methods

        public List<Box> PlaceBoxes (List<Box> BoxesSet,
            List<Container> containersSet) {
            if (BoxesSet == null || BoxesSet.Count == 0) {
                // logger.LoggerError("Trying to pass null or empty boxes list");
                throw new ArgumentException ("The list of boxes can not be null or empty");
            }

            if (containersSet == null || containersSet.Count == 0) {
                // logger.LoggerError("Trying to pass null or empty containers list");
                throw new ArgumentException ("The list of containers can not be null or empty");
            }

            //logger.LoggerInfo("Starting counting placing program...");

            this.BoxesSet = BoxesSet;

            CountTotalQuantity (ref boxesQuantityBeforePlacing,
                this.BoxesSet);

            SortVolumeDesc (this.BoxesSet);

            foreach (Container container in containersSet) {
                while (container.Width > 0) {
                    for (int i = 0; i < this.BoxesSet.Count;)
                        if (LeftToTheWall (container) > 0) {
                            if (this.BoxesSet[i].Quantity == 0) i++;
                            else {
                                DefineRowsMaxWidth (this.BoxesSet[i]);
                                PlaceFirstLevel (this.BoxesSet[i],
                                    container);
                                i++;
                            }
                        }
                    else break;

                    DefineSecondLevel (container);
                    PlaceSecondLevel (container);

                    AddRowToRowsSet ();

                    container.Width -= maxRowWidth;
                    yo += maxRowWidth;
                    xo = zo = 0;

                    PlatformsSet.Clear ();
                }
            }

            CountTotalQuantity (ref boxesQuantityAfterPlacing,
                this.BoxesSet);

            // logger.LoggerInfo ("Counting placing program is successfully finished");

            return RowsSet;
        }

        public double CountExecutionPercent () {
            // logger.LoggerInfo ("Counting placing program execution percent");

            return 100 - boxesQuantityAfterPlacing /
                boxesQuantityBeforePlacing * 100;
        }

        #endregion

        #region Private methods

        private void CountTotalQuantity (ref int targetValue,
            List<Box> set) {
            foreach (Box box in set)
                targetValue += box.Quantity;
        }

        private void SortVolumeDesc (List<Box> set) {
            set = set.OrderByDescending (a => a.Volume).ToList ();
        }

        private void RotateBox (Box box) {
            int temp = box.Length;
            box.Length = box.Width;
            box.Width = temp;
        }

        private void RotateAllBoxes (List<Box> set) {
            foreach (Box box in set)
                RotateBox (box);
        }

        private void DefineRowsMaxWidth (Box box) {
            if (Row.Count == 0) {
                if (box.Length > box.Width)
                    RotateBox (box);

                maxRowWidth = box.Width;
            }
        }

        private void RotateToMatchRowsMaxWidth (Box box) {
            if (Math.Abs (box.Length - maxRowWidth) < Math.Abs (box.Width - maxRowWidth))
                RotateBox (box);

            if (box.Width > maxRowWidth)
                maxRowWidth = box.Width;
        }

        private void PlaceTheSameBox (Box box, int x, int y, ref int z) {
            Row.Add (new Box (box.Name, length: box.Length,
                width: box.Width, height: box.Height, x: x, y: y,
                z: z));
            box.Quantity--;
            z += box.Height;
            leftToTheCeiling -= box.Height;
        }

        private void PutTheSameOnTheTop (Box box, Container container,
            int x, int y, int z) {
            bool canPut = true;

            while (canPut) {
                if ((leftToTheCeiling > box.Height) && (box.Quantity > 0) &&
                    ((z + box.Height) < box.Height)) {
                    PlaceTheSameBox (box, x, y, ref z);
                } else canPut = false;
            }
        }

        private int LeftToTheWall (Container container) {
            if (Row.Count == 0) return container.Length;
            else {
                double result = container.Length - (Row.Last ().XPos +
                    Row.Last ().Length);
                return container.Length - (Row.Last ().XPos +
                    Row.Last ().Length);
            }
        }

        private int FindTheMostSuitableBoxIndex (Box platform) {
            double heightLeft = 100000000000;
            int theMostSuitableBoxIndex = -1;

            foreach (Box box in BoxesSet)
                if ((platform.Height - (Math.Floor ((double) (platform.Height / box.Height)) *
                        box.Height)) < heightLeft &&
                    box.Quantity != 0) {
                    heightLeft = platform.Height - (Math.Floor ((double) (platform.Height / box.Height)) *
                        box.Height);
                    theMostSuitableBoxIndex = BoxesSet.IndexOf (box);
                }

            return theMostSuitableBoxIndex;
        }

        private void AddSecondRowBox (Box box, Box platform,
            Container container) {
            Row.Add (new Box (box.Name, length: box.Length,
                width: box.Width, height: box.Height,
                x: platform.XPos, y: platform.YPos,
                z: platform.ZPos));

            leftToTheCeiling = platform.Height;
            BoxesSet[FindTheMostSuitableBoxIndex (platform)].Quantity--;
            PutTheSameOnTheTop (box, container, platform.XPos,
                platform.YPos, platform.ZPos + box.Height);
        }

        private double CountApproximateQuantity (Box platform, Box box) {
            return Math.Floor ((double) (platform.Length / box.Length)) *
                Math.Floor ((double) (platform.Width / box.Width));
        }

        private double CountRatio (Box platform, Box box) {
            double result = Math.Floor ((double) (platform.BaseArea /
                box.BaseArea));
            return Math.Floor ((double) (platform.BaseArea / box.BaseArea));
        }

        private int FillPlatform (ref Box platform, Box box,
            Container container) {
            int placedBoxesNumber = 0;
            int platformWidth = platform.Width;

            for (int width = platform.YPos; platformWidth - box.Width >= 0; width += box.Width) {
                for (int length = platform.XPos; platform.Length - box.Length >= 0; length += box.Length) {
                    if (box.Quantity > 0) {
                        Row.Add (new Box (box.Name, length: box.Length,
                            width: box.Width, height: box.Height, x: length,
                            y: width, z: platform.ZPos));

                        leftToTheCeiling = platform.Height - box.Height;

                        PutTheSameOnTheTop (box, container, platform.XPos,
                            width, leftToTheCeiling);

                        platform.Length -= box.Length;
                        platform.XPos += box.Length;

                        box.Quantity--;
                        placedBoxesNumber++;
                    } else break;
                }

                platformWidth -= box.Width;
            }

            return placedBoxesNumber;
        }

        private bool IsPlatformFilled (Box platform, Box box,
            Container container) {
            bool condition = true;
            double ratio = CountRatio (platform, box);

            if (box.Width > maxRowWidth) RotateBox (box);

            while (condition) {
                if (CountApproximateQuantity (platform, box) == ratio && box.Quantity > 0) {
                    if (FillPlatform (ref platform, box, container) == ratio) return true;
                    // condition = false; // Don't compile
                    else RotateBox (box);
                }
            }

            return false;
        }

        private void PlaceFirstLevel (Box box, Container container) {
            while ((box.Length <= LeftToTheWall (container)) &&
                (box.Quantity > 0)) {
                RotateToMatchRowsMaxWidth (box);
                Row.Add (new Box (box.Name, length: box.Length,
                    width: box.Width, height: box.Height, x: xo, y: yo,
                    z: zo));
                box.Quantity--;
                zo += box.Height;
                leftToTheCeiling = container.Height - box.Height;
                PutTheSameOnTheTop (box, container, xo, yo, zo);
                xo += box.Length;
                zo = 0;
            }
        }

        private void DefineSecondLevel (Container container) {
            for (int i = 0; i < Row.Count; i++) {
                Box box = Row[i];
                Box prevBox = i == 0 ? null : Row[i - 1];

                if (i == 0) PlatformsSet.Add (new Box (box.Name, box.Length,
                    box.Width, container.Height - box.Height,
                    x: box.XPos, y: box.YPos, z: box.Height));
                else if (box.XPos == prevBox.XPos && (i != 0)) {
                    PlatformsSet.Last ().Height -= prevBox.Height;
                    PlatformsSet.Last ().ZPos += prevBox.Height;
                } else if ((box.Height == prevBox.Height) && (i != 0) &&
                    (PlatformsSet.Last ().Height == container.Height - box.Height)) {
                    PlatformsSet.Last ().Length += prevBox.Length;
                } else {
                    PlatformsSet.Add (new Box (box.Name, box.Length,
                        box.Width, container.Height - box.Height,
                        x: box.XPos, y: box.YPos, z: box.Height));
                }
            }
        }

        private void PlaceSecondLevel (Container container) {
            foreach (Box platform in PlatformsSet) {
                int theMostSuitableBoxIndex =
                    FindTheMostSuitableBoxIndex (platform);

                if (theMostSuitableBoxIndex != -1) {
                    Box box = BoxesSet[theMostSuitableBoxIndex];

                    if (CountRatio (platform, box) == 1 &&
                        box.Quantity > 0) {
                        if (platform.Height - box.Height > 0)
                            AddSecondRowBox (box, platform, container);
                        else continue;
                    } else if (!IsPlatformFilled (platform, box, container)) {
                        BoxesSet.Remove (BoxesSet[theMostSuitableBoxIndex]);
                        PlaceSecondLevel (container);

                        return;
                    }
                } else return;
            }
        }

        private void AddRowToRowsSet () {
            foreach (Box box in Row)
                RowsSet.Add (box);

            Row.Clear ();
        }

        #endregion
    }

}