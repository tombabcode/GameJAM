using System;
using GameJAM.Services;
using Microsoft.Xna.Framework;
using TBEngine.Types;

using DH = TBEngine.Utils.DisplayHelper;
using AH = TBEngine.Utils.AlignmentHelper;

namespace GameJAM.Components.Elements {
    public sealed class Button {

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public string Text { get; set; }
        public bool TextTranslate { get; set; } = true;

        public AlignType ButtonAlign { get; set; } = AlignType.LT;

        public Color? TextColor { get; set; }
        public Color? BackgroundColor { get; set; }

        public Action OnHover { get; set; }
        public Action OnClick { get; set; }


        private bool _isOver;

        public void Update(InputService input) {
            Vector2 position = AH.Position(ButtonAlign, X, Y, Width, Height);
            _isOver = input.IsOver((int)position.X, (int)position.Y, Width, Height);
            if (input.HasSwitchedArea((int)position.X, (int)position.Y, Width, Height)) OnHover?.Invoke( );
            if (_isOver && input.IsLMBPressedOnce( )) OnClick?.Invoke( );
        }

        public void Display(ContentService content) {
            Vector2 position = AH.Position(ButtonAlign, X, Y, Width, Height);
            if (BackgroundColor != null) DH.Box((int)position.X, (int)position.Y, Width, Height, BackgroundColor ?? Color.White);

            DH.Text(
                _isOver ? content.FontRegular : content.FontSmall, 
                Text, 
                (int)position.X + Width / 2, 
                (int)position.Y + Height / 2, 
                translate: TextTranslate, color: TextColor, align: AlignType.CM
            );
        }

    }
}