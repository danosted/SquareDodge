namespace Assets.Code.GameLogic
{
    using MonoBehaviours.Configuration;
    using Common;
    using DataAccess;
    using IoC;
    using UnityEngine;
    using Assets.Code.Utilities;
    using System.Collections.Generic;
    using System.Linq;
    using Assets.Code.MonoBehaviours.Drawing;

    public class DrawingFieldLogic : LogicBase
    {

        private ScreenUtil _screen;
        private float _initDrawingTime;

        private Point FieldSize { get; set; }
        private ICollection<Point> DrawingFieldPoints { get; set; }
        private ICollection<Vector3> CurrentFigurePoints { get; set; }
        private ICollection<DrawingField> DrawingFields { get; set; }
        private ICollection<DrawingField> CurrentDrawingPoints { get; set; }
        private const string activePrefix = "active ";

        private ScoreLogic ScoreLogic { get; set; }

        private struct Point
        {
            public int x;
            public int y;
        }

        public DrawingFieldLogic(IoC container, PrefabManager prefabManager, GlobalConfiguration config) : base(container, prefabManager, config)
        {
            _screen = Container.Resolve<ScreenUtil>();
            ScoreLogic = Container.Resolve<ScoreLogic>();

        }

        public void InitializeDrawingFields()
        {
            // First calculate the field size based on the screen resolution
            var screenSize = _screen.GetScreenSizeInWorld();

            FieldSize = new Point
            {
                x = (int)screenSize.x,
                y = (int)screenSize.y
            };

            // Initialize the drawingfield locations based on points
            DrawingFieldPoints = CreateDrawingField(FieldSize.x, FieldSize.y);

            // Map the points to Vector3 in world coordinates
            CurrentFigurePoints = MapAreasToVector3(DrawingFieldPoints);

            // Initialize the drawing fields in the game world
            DrawingFields = new List<DrawingField>(CurrentFigurePoints.Count);
            CurrentDrawingPoints = new List<DrawingField>();
            var fieldOrder = 1;
            foreach (var point in CurrentFigurePoints)
            {
                var dField = PrefabManager.GetPrefab(Configuration.prefab_drawing_field);
                dField.transform.position = point;
                dField.name = string.Format("Order: {0}, X: {1} - Y: {2}.", fieldOrder, point.x, point.y);
                dField.Order = fieldOrder;
                DrawingFields.Add(dField);
                fieldOrder++;
            }
        }

        // TODO 1 (DRO): Test
        public void InitializeTestDrawing()
        {
            if (!DrawingFields.Any())
            {
                Debug.LogWarning("No drawing fields found.");
                return;
            }
            _initDrawingTime = Time.time;
            var randomIndex = Random.Range(0, DrawingFields.Count() - 1);
            var field = DrawingFields.ElementAt(randomIndex);
            field.name = activePrefix + field.name;
            CurrentDrawingPoints.Add(field);
            field.IsTarget(true);
        }

        public int GetDrawingFieldIndex(DrawingField drawingField)
        {
            return DrawingFields.ToList().IndexOf(drawingField);
        }

        /// <summary>
        ///  Check if the registered drawingfield is taken in the correct order
        /// </summary>
        /// <param name="drawingField"></param>
        /// <returns></returns>
        public bool DrawingFieldRegistered(DrawingField drawingField)
        {

            //if (CurrentDrawingPoints.Any(x => x.Equals(drawingField)))
            // Are there any drawing fields with lower order, then it is not a success
            if (drawingField.IsLowestOrder)
            {
                var activeSeconds = Time.time - _initDrawingTime;
                ScoreLogic.AddDrawingFieldPoints(activeSeconds);
                Debug.LogFormat("DrawingField registered. Order {0}. ActiveSeconds {1}.", drawingField.Order, activeSeconds);
                CurrentDrawingPoints.Remove(drawingField);
                drawingField.name = drawingField.name.Replace(activePrefix, "");
                drawingField.IsTarget(false);
                if (!CurrentDrawingPoints.Any())
                {
                    Debug.LogFormat("All drawing points registered. Hurray!!.");
                    InitializeTestDrawing();
                }
                //UpdateLowestOrderDrawingField();
                return true;
            }
            else
            {
                // We might decide to do something on unsuccesfull drawing field selection
                return false;
            }
        }

        //public void UpdateLowestOrderDrawingField()
        //{
        //    var lowestOrderDrawingField = CurrentDrawingPoints.FirstOrDefault(df => CurrentDrawingPoints.Min(x => x.Order) == df.Order);
        //    if(lowestOrderDrawingField == null)
        //    {
        //        throw new System.Exception("Could not find lowest order drawing field.");
        //    }
        //    lowestOrderDrawingField.IsLowestOrder = true;
        //}

        #region Create Drawing Fields
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
        #endregion

    }
}
