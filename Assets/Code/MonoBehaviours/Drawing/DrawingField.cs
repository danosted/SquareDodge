namespace Assets.Code.MonoBehaviours.Drawing
{
    using System.Collections.Generic;
    using UnityEngine;
    using IoC;
    using Utilities;
    using Common;
    using System.Linq;
    using Extensions;
    using Assets.Code.GameLogic;

    public class DrawingField : PrefabBase
    {
        private DrawingFieldLogic _logic;

        private struct Point
        {
            public int x;
            public int y;
        }

        public override void Activate(IoC container)
        {
            base.Activate(container);
            _logic = Container.Resolve<DrawingFieldLogic>();

            // Collider size
            //AreaSize = ;

            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        
        void OnMouseEnter()
        {
            // TODO 1 (DRO): Register drawing
            Debug.LogFormat("Entered drawing field {0}.", name);
            SetColour(true);
        }

        void OnMouseExit()
        {
            // TODO 1 (DRO): Register drawing
            Debug.LogFormat("Entered drawing field {0}.", name);
            SetColour(false);
        }

        private void SetColour(bool isOn)
        {
            var spriteTexture = GetComponentInChildren<SpriteRenderer>();
            spriteTexture.color = isOn ? Color.yellow : new Color (1,1,1,0.2f);
            //spriteTexture.color = 0.2f;
        }
    }
}
