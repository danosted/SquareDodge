namespace Assets.Code.MonoBehaviours.Drawing
{
    using UnityEngine;
    using IoC;
    using Common;
    using Assets.Code.GameLogic;
    using System;

    public class DrawingField : PrefabBase
    {
        private DrawingFieldLogic _logic;
        private SpriteRenderer _childSprite;
        private bool _isTarget;
        public int Order { get; private set; }

        private bool _isLowestOrder;
        public bool IsLowestOrder
        {
            get
            {
                return _isLowestOrder;
            }
            set
            {
                _isLowestOrder = value;
                SetColour(true);
            }
        }

        public void Activate(IoC container, int order)
        {
            Order = order;
            Activate(container);
        }

        public override void Activate(IoC container)
        {
            base.Activate(container);

            // Collider size
            //AreaSize = ;
            _logic = Container.Resolve<DrawingFieldLogic>();
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void IsTarget(bool isTarget)
        {
            _isTarget = isTarget;
            SetSpriteEnabled(isTarget);
            _childSprite = _childSprite == null ? GetComponentInChildren<SpriteRenderer>() : _childSprite;
            SetColour(isTarget);
            //_childSprite.color = isTarget ? Color.red : new Color(1, 1, 1, 0.2f);
        }


        void OnMouseEnter()
        {
            // TODO 1 (DRO): Register drawing
            //Debug.LogFormat("Entered drawing field {0}.", name);
            if (_isTarget)
            {
                if (!_logic.DrawingFieldRegistered(this))
                {
                    return;
                }
                IsTarget(false);
                SetColour(false);
                return;
            }

            // Debug
            //SetColour(true);
        }

        void OnMouseExit()
        {
            // TODO 1 (DRO): Register drawing
            //Debug.LogFormat("Entered drawing field {0}.", name);
            if (_isTarget)
            {
                return;
            }

            // Debug
            //SetColour(false);
        }

        void OnMouseDown()
        {
            var index = _logic.GetDrawingFieldIndex(this);
            Debug.LogFormat("drawing field {0} with index {1}", name, index);
        }

        private void SetColour(bool isOn)
        {
            _childSprite = _childSprite == null ? GetComponentInChildren<SpriteRenderer>() : _childSprite;
            var onColor = IsLowestOrder ? Color.green : new Color(0f, 0f, 0f, 0f);
            _childSprite.color = isOn ? onColor : Color.white;
        }

        void Update()
        {
            if (_isTarget)
            {
                return;
            }
            SetSpriteEnabled(Configuration.param_debug_enabled);
        }

        private void SetSpriteEnabled(bool isEnabled)
        {
            _childSprite = _childSprite == null ? GetComponentInChildren<SpriteRenderer>() : _childSprite;
            _childSprite.gameObject.SetActive(isEnabled);
        }
    }
}
