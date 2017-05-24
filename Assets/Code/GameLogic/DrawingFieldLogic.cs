namespace Assets.Code.GameLogic
{
    using MonoBehaviours.Configuration;
    using Common;
    using DataAccess;
    using DataAccess.DTOs;
    using IoC;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Assets.Code.Utilities;
    using System.Collections.Generic;
    using System.Linq;

    public class DrawingFieldLogic : LogicBase
    {

        private ScreenUtil _screen;

        private Point FieldSize { get; set; }
        private ICollection<Point> DrawingFieldPoints { get; set; }
        private ICollection<Vector3> CurrentFigurePoints { get; set; }

        private struct Point
        {
            public int x;
            public int y;
        }

        public DrawingFieldLogic(IoC container, PrefabManager prefabManager, GlobalConfiguration config) : base(container, prefabManager, config)
        {
            _screen = Container.Resolve<ScreenUtil>();

            var screenSize = _screen.GetScreenSizeInWorld();

            FieldSize = new Point
            {
                x = (int)screenSize.x,
                y = (int)screenSize.y
            };

            DrawingFieldPoints = DrawingFieldPoints == null ? CreateDrawingField(FieldSize.x, FieldSize.y) : DrawingFieldPoints;

            // TODO 1 (DRO): For testing
            //CurrentFigurePoints = CreateStatícFigure();
            CurrentFigurePoints = MapAreasToVector3(DrawingFieldPoints);

            // TODO 1 (DRO): Test
            foreach (var point in CurrentFigurePoints)
            {
                var goo = PrefabManager.GetPrefab(Configuration.prefab_dummy);
                goo.transform.position = point;
                goo.name = string.Format("X: {0} - Y: {1}.", point.x, point.y);
            }

        }


        private ICollection<Point> CreateDrawingField(int width, int height)
        {
            var areas = new List<Point>(width * height);
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    Point area = CreateAndPlaceCellInGrid(x, y);
                    areas.Add(area);
                }
            }
            return areas;
        }

        private Point CreateAndPlaceCellInGrid(int x, int y)
        {

            var area = new Point
            {
                x = x,
                y = y,
            };

            return area;
        }

        /// <summary>
        /// Map horizontal position from a 0-indexed environment
        /// </summary>
        private float MapHorizontalPosition(int x)
        {
            return MathUtil.MapInputFromInputRangeToOutputRange(x, 0, FieldSize.x, _screen.ViewportToWorldBorderMin.x, _screen.ViewportToWorldBorderMax.x);
        }

        /// <summary>
        /// Map vertical position from a 0-indexed environment
        /// </summary>
        private float MapVerticalPosition(int y)
        {
            return MathUtil.MapInputFromInputRangeToOutputRange(y, 0, FieldSize.y, _screen.ViewportToWorldBorderMin.y, _screen.ViewportToWorldBorderMax.y);
        }

        private Vector3 MapAreaToVector3(Point input)
        {
            return new Vector3(MapHorizontalPosition(input.x), MapVerticalPosition(input.y), Camera.main.nearClipPlane);
        }

        private ICollection<Vector3> MapAreasToVector3(ICollection<Point> inputs)
        {
            ICollection<Vector3> result = new List<Vector3>(inputs.Count);
            foreach (var i in inputs)
            {
                result.Add(MapAreaToVector3(i));
            }
            return result;
        }

        private ICollection<Vector3> CreateStatícFigure()
        {
            var figure = new Point[4];
            figure[0] = DrawingFieldPoints.FirstOrDefault(p => p.x == FieldSize.x / 2 && p.y == 0);
            figure[1] = DrawingFieldPoints.FirstOrDefault(p => p.x == 0 && p.y == FieldSize.y / 2);
            figure[2] = DrawingFieldPoints.FirstOrDefault(p => p.x == FieldSize.x / 2 && p.y == FieldSize.y - 1);
            figure[3] = DrawingFieldPoints.FirstOrDefault(p => p.x == FieldSize.x - 1 && p.y == FieldSize.y / 2);
            var res = MapAreasToVector3(figure);
            return res;
        }

        // TODO 1 (DRO): Prototype figure drawing
        //private ICollection<Point> CreateStatícFigure(int count)
        //{
        //    var resultFigure = new Point[count];
        //    var randomIndex = Random.Range(0, DrawingFieldPoints.Count);
        //    var point = DrawingFieldPoints.ElementAt(randomIndex);
        //    resultFigure[0] = point;

        //    for (var i = 1; i < count; i++)
        //    {
        //        randomIndex = GetRandomIndex(randomIndex);
        //        point = DrawingFieldPoints.ElementAt(randomIndex);
        //        resultFigure[0] = point;
        //    }

        //    return null;
        //}

        //private int GetRandomIndex(int index)
        //{
        //    return Random.Range(0, 1) == 0 ? index + FieldSize.y : index + 1;
        //}

    }
}
