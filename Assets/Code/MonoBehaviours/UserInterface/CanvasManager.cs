namespace Assets.Code.MonoBehaviours.UserInterface
{
    using IoC;
    using Common;
    using UnityEngine;
    
    [RequireComponent(typeof(Canvas))]
    public class CanvasManager : PrefabBase
    {
        public override void Activate(IoC container)
        {
            base.Activate(container);

            var canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;
            gameObject.SetActive(true);

            // Activate all UI parts
            var uiprefabs = GetComponentsInChildren<PrefabBase>();
            foreach(var p in uiprefabs)
            {
                if (p == this) continue;
                p.Activate(container);
            }
        }
    }
}
