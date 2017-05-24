namespace Assets.Code.MonoBehaviours.Drawing
{
    using System.Collections.Generic;
    using UnityEngine;
    using IoC;
    using Utilities;
    using Common;
    using System.Linq;
    using Extensions;

    public class DrawingField : PrefabBase
    {
        private ScreenUtil _screen;

        private Point FieldSize { get; set; }
        private ICollection<Point> DrawingFieldPoints { get; set; }
        private ICollection<Vector3> CurrentFigurePoints { get; set; }
        private int AreaSize { get; set; }

        private struct Point
        {
            public int x;
            public int y;
        }

        public override void Activate(IoC container)
        {
            base.Activate(container);
            _screen = Container.Resolve<ScreenUtil>();

            var screenSize = _screen.GetScreenSizeInWorld();

            AreaSize = 1;
            FieldSize = new Point
            {
                x = (int)screenSize.x / AreaSize,
                y = (int)screenSize.y / AreaSize
            };

            DrawingFieldPoints = DrawingFieldPoints == null ? CreateDrawingField(FieldSize.x, FieldSize.y) : DrawingFieldPoints;

            CurrentFigurePoints = CreateStatícFigure();

            // TODO 1 (DRO): Test
            foreach (var point in CurrentFigurePoints)
            {
                var goo = PrefabManager.GetPrefab(Configuration.prefab_dummy);
                goo.transform.position = point;
                goo.name = string.Format("X: {0} - Y: {1}.", point.x, point.y);
            }

            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private ICollection<Point> CreateDrawingField(int width, int height)
        {
            var areas = new List<Point>(width * height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
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
            return MathUtil.MapInputFromInputRangeToOutputRange(x, 0, FieldSize.x, _screen.ViewportToWorldBorderMin.x + AreaSize, _screen.ViewportToWorldBorderMax.x - AreaSize);
        }

        /// <summary>
        /// Map vertical position from a 0-indexed environment
        /// </summary>
        private float MapVerticalPosition(int y)
        {
            return MathUtil.MapInputFromInputRangeToOutputRange(y, 0, FieldSize.y, _screen.ViewportToWorldBorderMin.y + AreaSize, _screen.ViewportToWorldBorderMax.y - AreaSize);
        }

        private Vector3 MapAreaToVector3(Point input)
        {
            return new Vector3(MapHorizontalPosition(input.x), MapVerticalPosition(input.y), Camera.main.nearClipPlane);
        }

        private ICollection<Vector3> MapAreasToVector3(ICollection<Point> inputs)
        {
            ICollection<Vector3> result = new List<Vector3>(inputs.Count);
            foreach(var i in inputs)
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

        void Update()
        {
            // TODO 1 (DRO): Register drawing
            if (Input.GetMouseButtonUp(0)) return;
            //Debug.LogFormat("Input.mousePosition. {0}.", Input.mousePosition.ToVector2());
            var pointerPosition = _screen.GetScreenPositionOnWorldPlane(Input.mousePosition).ToVector2();
            var insideArea = CurrentFigurePoints.Any(p => Vector2.Distance(p, pointerPosition) <= AreaSize * 0.5f);
            //Debug.LogFormat("pointerPosition. {0}.", pointerPosition);
            if (!insideArea) return;
            //Debug.LogFormat("insideArea. {0}.", insideArea);
        }
    }
}
