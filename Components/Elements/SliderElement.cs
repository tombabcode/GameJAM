using GameJAM.Services;
using Microsoft.Xna.Framework;
using System;
using TBEngine.Types;
using DH = TBEngine.Utils.DisplayHelper;
using LANG = TBEngine.Utils.TranslationService;

namespace GameJAM.Components.Elements {
    public sealed class SliderElement {

        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }
        public float Minimum { get; set; }
        public float Maximum { get; set; }
        public string Name { get; set; }

        private ContentService _content;
        private InputService _input;

        private Action<float> _onValueChange;
        private float _value;

        public SliderElement(ContentService content, InputService input, float initValue, Action<float> onValueChange) {
            _content = content;
            _input = input;

            _value = initValue;

            _onValueChange = onValueChange;
        }

        public void Update( ) {
            if (_input.IsOver(AbsoluteX - 120, AbsoluteY - 16, 240, 20) && _input.IsLMBPressed( )) {
                _value = (_input.MouseX - (AbsoluteX - 120)) / 240f;
                _value = _value < 0 ? 0 : _value > 1 ? 1 : _value;
                _onValueChange(_value);
            }
        }

        public void Display( ) {
            DH.Box(AbsoluteX, AbsoluteY, 240, 2, align: AlignType.CB);
            DH.Box((int)(AbsoluteX - 120 + _value * 240), AbsoluteY + 4, 6, 20, color: Color.Red, align: AlignType.CB);

            DH.Text(_content.FontTiny, $"{Minimum:0.0}", AbsoluteX - 120, AbsoluteY + 8, translate: false, color: Color.DarkGray, align: AlignType.CT);
            DH.Text(_content.FontTiny, $"{Maximum:0.0}", AbsoluteX + 120, AbsoluteY + 8, translate: false, color: Color.DarkGray, align: AlignType.CT);
            DH.Text(_content.FontSmall, LANG.Get(Name) + $" {(_value * (Maximum - Minimum) + Minimum):0.0}", AbsoluteX, AbsoluteY + 8, translate: false, align: AlignType.CT);
        }

    }
}
