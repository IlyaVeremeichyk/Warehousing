using System;
using System.Collections.Generic;
using System.Linq;
using MathLib.Logic.Models;

namespace MathLib.Logic
{
    public class PlacingPlanManager
    {
        #region Fields

        private int _xo, _yo, _zo;
        private int _rowWidth;
        private int _boxesQuantityBeforePlacing;
        private int _boxesQuantityAfterPlacing;
        private int _distanceLeftToTheCeiling;
        private bool _isFirstRow;

        private readonly List<Box> _rowOfBoxes;    // the collection of boxes that forms a 'row' in a container
        private readonly List<List<Box>> _rowsSet; // the collection of 'rows' in a container
        private readonly List<Box> _platformsSet;  // the collection of platforms; a platform is an area, that if formed by top surface of nearby boxes

        private int Delta
        {
            get { return _isFirstRow ? 0 : _rowsSet.Sum(b => b.First().Width); }
        }

        #endregion

        #region Constructors

        public PlacingPlanManager()
        {
            _rowOfBoxes = new List<Box>();
            _rowsSet = new List<List<Box>>();
            _platformsSet = new List<Box>();
            _isFirstRow = true;
        }
        #endregion

        #region Public methods

        public void GetPlacingPlan(List<BoxQunatityPair> boxesSet, List<Container> containersSet)
        {
            if (boxesSet == null || !boxesSet.Any())
            {
                throw new ArgumentException("The list of boxes can not be null or empty");
            }

            if (containersSet == null || !containersSet.Any())
            {
                throw new ArgumentException("The list of containers can not be null or empty");
            }

            if (!IsAllPAssedDataValid(boxesSet, containersSet))
            {
                throw new ArgumentException("Passed data contains invalid content: length, width or height of a box or a container can not be 0 or negative.");
            }

            _boxesQuantityBeforePlacing = CountTotalBoxesQuantity(boxesSet);

            SortBoxesByVolumeDesc(boxesSet);

            foreach (Container container in containersSet)
            {
                var containerLength = container.Length;

                // filling one row
                while (containerLength > 0 && boxesSet.Any(b => b.Quantity > 0))
                {
                    foreach (var placedBox in boxesSet.Where(b => b.Quantity != 0))
                    {
                        while (FindDistanceLeftToTheWallOfContainer(container) >= placedBox.Box.Width && placedBox.Quantity > 0)
                        {
                            DefineRowsWidthOnTheFirstPlacingBox(placedBox.Box);
                            PlaceFirstLevelOfBoxes(placedBox, container);
                        }
                    }

                    DefineTheSecondBoxesLevel(container);
                    PlaceTheSecondLevelOfBoxes(boxesSet);

                    _rowsSet.Add(ReturnDeepListCopy(_rowOfBoxes));
                    
                    containerLength -= _rowWidth;
                    _yo += _rowWidth;
                    _xo = _zo = 0;

                    _platformsSet.Clear();
                    _rowOfBoxes.Clear();

                    _isFirstRow = false;
                }

                container.PlacingPlan = ReturnDeepListCopy(_rowsSet);
                _rowsSet.Clear();
            }

            _boxesQuantityAfterPlacing = CountTotalBoxesQuantity(boxesSet);
        }

        public void GetPlacingPlanForAdaptedRendering(List<BoxQunatityPair> boxesSet, List<Container> containersSet)
        {
            GetPlacingPlan(boxesSet, containersSet);

            foreach (var container in containersSet)
            {
                foreach (var row in container.PlacingPlan)
                {
                    foreach (var box in row)
                    {
                        box.XPos += box.Length / 2;
                        box.YPos += box.Width / 2;
                        box.ZPos += box.Height / 2;
                    }
                }
            }
        }

        public double CountExecutionPercent()
        {
            return 100 - _boxesQuantityAfterPlacing / _boxesQuantityBeforePlacing * 100;
        }

        #endregion

        #region Private methods

        private bool IsAllPAssedDataValid(List<BoxQunatityPair> boxesSet, List<Container> containersSet)
        {
            foreach (var placedBox in boxesSet)
            {
                var box = placedBox.Box;

                if (box.Length <= 0 || box.Width <= 0 || box.Height <= 0 || placedBox.Quantity < 0)
                {
                    return false;
                }
            }

            foreach (var container in containersSet)
            {
                if (container.Length <= 0 || container.Width <= 0 || container.Height <= 0)
                {
                    return false;
                }
            }

            return true;
        }

        private int CountTotalBoxesQuantity(List<BoxQunatityPair> boxesSet)
        {
            int totalBoxesQuantity = 0;

            foreach (var box in boxesSet)
            {
                totalBoxesQuantity += box.Quantity;
            }

            return totalBoxesQuantity;
        }

        private void SortBoxesByVolumeDesc(List<BoxQunatityPair> boxesSet)
        {
            boxesSet.OrderByDescending(pair => pair.Box.Volume);
        }

        private void RotateBox(Box box)
        {
            int width = box.Length;
            box.Length = box.Width;
            box.Width = width;
        }

        private void DefineRowsWidthOnTheFirstPlacingBox(Box box)
        {
            if (box.Length > box.Width)
            {
                RotateBox(box);
            }

            _rowWidth = box.Width;
        }

        private void RotateBoxToMatchRowsWidth(Box box)
        {
            if (Math.Abs(box.Length - _rowWidth) < Math.Abs(box.Width - _rowWidth))
            {
                RotateBox(box);
            }

            if (box.Width > _rowWidth)
            {
                _rowWidth = box.Width;
            }
        }

        private void PlaceTheSameBox(BoxQunatityPair boxQuantityPair, int destinationXPos, int destinationYPos, ref int destinationZPos)
        {
            var box = boxQuantityPair.Box;

            _rowOfBoxes.Add(new Box(box.Name, length: box.Length, width: box.Width, height: box.Height, 
                                    x: destinationXPos, y: destinationYPos, z: destinationZPos));

            boxQuantityPair.Quantity--;
            destinationZPos += box.Height;
            _distanceLeftToTheCeiling -= box.Height;
        }

        private void PutTheSameBoxOnTheTop(BoxQunatityPair boxQuantityPair, int destinationXPos, int destinationYPos, int destinationZPos)
        {
            var isPossibleToPutAnotherOne = true;

            while (isPossibleToPutAnotherOne)
            {
                if (_distanceLeftToTheCeiling > boxQuantityPair.Box.Height &&
                    boxQuantityPair.Quantity > 0 &&
                    boxQuantityPair.Box.Height < boxQuantityPair.Box.Height + destinationZPos)
                {
                    PlaceTheSameBox(boxQuantityPair, destinationXPos, destinationYPos, ref destinationZPos);
                }
                else
                {
                    isPossibleToPutAnotherOne = false;
                }
            }
        }

        private int FindDistanceLeftToTheWallOfContainer(Container container)
        {
            if (_rowOfBoxes.Count == 0)
            {
                return container.Width;
            }

            return container.Width - (_rowOfBoxes.Last().XPos + _rowOfBoxes.Last().Length);
        }

        private BoxQunatityPair FindTheMostSuitableBoxForThePlatform(List<BoxQunatityPair> boxesSet, Box platform)
        {
            // Box is the most suitable if after its placing, the distance left to the ceiling is minimum

            var heightLeft = 100000000000D;
            BoxQunatityPair theMostSuitableBox = null;

            foreach (var boxQuantityPair in boxesSet.Where(b => b.Quantity != 0))
            {
                var boxesInStackQuantity = Math.Floor((double)platform.Height / boxQuantityPair.Box.Height);

                var heightWillBeLeft = Math.Abs(platform.Height - boxesInStackQuantity * boxQuantityPair.Box.Height);

                if (heightWillBeLeft < heightLeft && 
                    boxQuantityPair.Box.Length <= platform.Length - platform.XPos && boxQuantityPair.Box.Width <= platform.Length - platform.XPos &&
                    boxQuantityPair.Box.Length <= platform.Width - platform.YPos + Delta && boxQuantityPair.Box.Width <= platform.Width - platform.YPos + Delta)
                {
                    heightLeft = heightWillBeLeft;
                    theMostSuitableBox = boxQuantityPair;
                }
            }

            return theMostSuitableBox;
        }

        private void AddSecondRowBox(BoxQunatityPair boxQuantityPair, Box platform)
        {
            var box = boxQuantityPair.Box;

            _rowOfBoxes.Add(new Box(box.Name, length: box.Length, width: box.Width, height: box.Height,
                                    x: platform.XPos, y: platform.YPos, z: platform.ZPos));

            _distanceLeftToTheCeiling = platform.Height - box.Height;

            boxQuantityPair.Quantity--;

            PutTheSameBoxOnTheTop(boxQuantityPair, platform.XPos, platform.YPos, platform.ZPos + box.Height);
        }

        private void PlaceFirstLevelOfBoxes(BoxQunatityPair boxQuantityPair, Container container)
        {
            var box = boxQuantityPair.Box;

            while (box.Length <= FindDistanceLeftToTheWallOfContainer(container) && 
                   boxQuantityPair.Quantity > 0)
            {
                RotateBoxToMatchRowsWidth(box);

                _rowOfBoxes.Add(new Box(box.Name, length: box.Length, width: box.Width, height: box.Height, x: _xo, y: _yo, z: _zo));

                boxQuantityPair.Quantity--;

                _zo += box.Height;
                _distanceLeftToTheCeiling = container.Height - box.Height;

                PutTheSameBoxOnTheTop(boxQuantityPair, _xo, _yo, _zo);

                _xo += box.Length;
                _zo = 0;
            }
        }

        private void DefineTheSecondBoxesLevel(Container container)
        {
            for (int i = 0; i < _rowOfBoxes.Count; i++)
            {
                var currentBox = _rowOfBoxes[i];
                var previousBox = i == 0 ? null : _rowOfBoxes[i - 1];

                // the first box will define the platform parameters

                // if the current box is placed on the previous box top, then the height of the platform is decreased
                if (previousBox!= null && currentBox.XPos == previousBox.XPos)
                {
                    _platformsSet.Last().Height -= previousBox.Height;
                    _platformsSet.Last().ZPos += previousBox.Height;
                }
                // if the current box is has the same height as the previous box + the both boxes are on the same "layer"
                // then the platform length is expanded
                else if (previousBox != null &&
                         currentBox.Height == previousBox.Height &&                            // have the same height?
                         _platformsSet.Last().Height == container.Height - currentBox.Height)  // are on the same layer?
                {
                    _platformsSet.Last().Length += previousBox.Length;
                }
                else
                {
                    _platformsSet.Add(new Box(length: currentBox.Length, width: currentBox.Width, height: container.Height - currentBox.Height,
                                              x: currentBox.XPos, y: currentBox.YPos, z: currentBox.Height));
                }
            }
        }

        private void PlaceTheSecondLevelOfBoxes(List<BoxQunatityPair> boxesSet)
        {
            foreach (var platform in _platformsSet)
            {
                var columnLength = 0;

                while (platform.XPos < platform.Length)
                {
                    var theMostSuitableBox = FindTheMostSuitableBoxForThePlatform(boxesSet, platform);

                    if (theMostSuitableBox != null)
                    {
                        var box = theMostSuitableBox.Box;

                        var defaultYPos = platform.YPos;

                        // theMostSuitableBox == null means that there is not suitable box for the current parameters of a platform
                        while (theMostSuitableBox != null && theMostSuitableBox.Quantity > 0 && platform.YPos <= platform.Width + Delta)
                        {
                            columnLength = columnLength > box.Length ? columnLength : box.Length;

                            if (platform.Height - box.Height > 0)
                            {
                                AddSecondRowBox(theMostSuitableBox, platform);
                            }

                            platform.YPos += box.Width;

                            theMostSuitableBox = FindTheMostSuitableBoxForThePlatform(boxesSet, platform);
                        }

                        platform.XPos += columnLength;
                        platform.YPos = defaultYPos;
                    }
                    else return;
                }
            }
        }

        private List<Box> ReturnDeepListCopy(List<Box> list)
        {
            var result = new List<Box>();

            foreach (var box in list)
            {
                result.Add(box);
            }

            return result;
        }

        private List<List<Box>> ReturnDeepListCopy(List<List<Box>> list)
        {
            var result = new List<List<Box>>();

            foreach (var box in list)
            {
                result.Add(ReturnDeepListCopy(box));
            }

            return result;
        }

        #endregion
    }

}