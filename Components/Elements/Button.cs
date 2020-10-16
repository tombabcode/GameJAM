using System;
using GameJAM.Services;
using Microsoft.Xna.Framework;
using TBEngine.Services;
using TBEngine.Types;
using TBEngine.Utils;
using DH = TBEngine.Utils.DisplayHelper;

namespace GameJAM.Components.Elements {
    public sealed class Button {

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public string Text { get; set; }

        public AlignType ButtonAlign { get; set; }

        public Color? TextColor { get; set; }
        public Color? BackgroundColor { get; set; }

        public Action OnHover { get; set; }
        public Action OnClick { get; set; }

        public void Update(InputService input) {
            if (input.HasSwitchedArea(X, Y, Width, Height)) OnHover?.Invoke( );
            if (input.IsOver(X, Y, Width, Height) && input.IsLMBPressedOnce( )) OnClick?.Invoke( );
        }

        public void Display(ContentDataService content) {
            Vector2 position = AlignmentHelper.Position(ButtonAlign, X, Y, Width, Height);
            if (BackgroundColor != null) DH.Box((int)position.X, (int)position.Y, Width, Height, BackgroundColor ?? Color.White);
            DH.Text(content.FontRegular, Text, (int)position.X + Width / 2, (int)position.Y + Height / 2, TextColor, AlignType.CM);
        }

    }
}