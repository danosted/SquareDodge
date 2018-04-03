namespace Assets.Code.MonoBehaviours.UserInterface
{
    using IoC;
    using Common;
    using UnityEngine;
    using UnityEngine.UI;
    using Assets.Code.Utilities;

    [RequireComponent(typeof(Text))]
    public class FloatingText : PrefabBase
    {
        private RectTransform RectTransform { get; set; }
        private Text Text { get; set; }
        private float _showTextForSeconds;
        private float _screenVerticalSize { get; set; }
        private ScreenUtil _screenUtil { get; set; }

        public override void Activate(IoC container)
        {
            base.Activate(container);
            Text = GetComponent<Text>();
            RectTransform = GetComponent<RectTransform>();
            _screenUtil = Container.Resolve<ScreenUtil>();
        }

        public void ShowTextForSeconds(string textValue, int showTextForSeconds, Color textColor)
        {
            Text.text = textValue;
            _showTextForSeconds = showTextForSeconds;
            _screenVerticalSize = _screenUtil.GetScreenSize().y;
            Text.color = textColor;
        }

        private void Update()
        {
            if (_showTextForSeconds <= 0f)
            {
                Text.color = new Color(0, 0, 0, 0); // hide the text
                return;
            }
            var pointerPosition = Input.mousePosition;
            RectTransform.anchoredPosition3D = new Vector3(pointerPosition.x, pointerPosition.y - _screenVerticalSize + 120f + (-80f * (_showTextForSeconds)), pointerPosition.z);
            _showTextForSeconds -= Time.deltaTime;
        }
    }
}
