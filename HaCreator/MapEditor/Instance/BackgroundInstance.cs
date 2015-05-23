﻿using HaCreator.MapEditor.Info;
using MapleLib.WzLib.WzStructure.Data;
using XNA = Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaCreator.MapEditor.Instance
{
    public class BackgroundInstance : BoardItem, IFlippable
    {
        private BackgroundInfo baseInfo;
        private bool flip;
        private int _a; //alpha
        private int _cx; //copy x
        private int _cy; //copy y
        private int _rx;
        private int _ry;
        private bool _front;
        private BackgroundType _type;

        public BackgroundInstance(BackgroundInfo baseInfo, Board board, int x, int y, int z, int rx, int ry, int cx, int cy, BackgroundType type, int a, bool front, bool flip)
            : base(board, x, y, z)
        {
            this.baseInfo = baseInfo;
            this.flip = flip;
            _rx = rx;
            _ry = ry;
            _cx = cx;
            _cy = cy;
            _a = a;
            _type = type;
            _front = front;
            if (flip)
                BaseX -= Width - 2 * Origin.X;
        }

        public override ItemTypes Type
        {
            get { return ItemTypes.Backgrounds; }
        }

        public bool Flip
        {
            get
            {
                return flip;
            }
            set
            {
                if (flip == value) return;
                flip = value;
                int xFlipShift = Width - 2 * Origin.X;
                if (flip) BaseX -= xFlipShift;
                else BaseX += xFlipShift;
            }
        }

        public int UnflippedX
        {
            get
            {
                return flip ? (BaseX + Width - 2 * Origin.X) : BaseX;
            }
        }

        public override void Draw(SpriteBatch sprite, XNA.Color color, int xShift, int yShift)
        {
            XNA.Rectangle destinationRectangle;
            /*if (ApplicationSettings.emulateParallax)
                destinationRectangle = new Rectangle((int)X - Origin.X, (int)Y - Origin.Y, Width, Height);
            else */
            destinationRectangle = new XNA.Rectangle((int)X + xShift - Origin.X, (int)Y + yShift - Origin.Y, Width, Height);
            sprite.Draw(baseInfo.GetTexture(sprite), destinationRectangle, null, color, 0f, new XNA.Vector2(0f, 0f), Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1);
            base.Draw(sprite, color, xShift, yShift);
        }

        public override MapleDrawableInfo BaseInfo
        {
            get { return baseInfo; }
        }

        public override System.Drawing.Bitmap Image
        {
            get
            {
                return baseInfo.Image;
            }
        }

        public override int Width
        {
            get { return baseInfo.Width; }
        }

        public override int Height
        {
            get { return baseInfo.Height; }
        }

        public override System.Drawing.Point Origin
        {
            get
            {
                return baseInfo.Origin;
            }
        }

        //parallax + undo\redo is shit. I don't like this way either.
        public int BaseX { get { return (int)base.position.X; } set { base.position.X = value; } }
        public int BaseY { get { return (int)base.position.Y; } set { base.position.Y = value; } }

        public int rx
        {
            get { return _rx; }
            set { _rx = value; }
        }

        public int ry
        {
            get { return _ry; }
            set { _ry = value; }
        }

        public int cx
        {
            get { return _cx; }
            set { _cx = value; }
        }

        public int cy
        {
            get { return _cy; }
            set { _cy = value; }
        }

        public int a
        {
            get { return _a; }
            set { _a = value; }
        }

        public BackgroundType type
        {
            get { return _type; }
            set { _type = value; }
        }

        public bool front
        {
            get { return _front; }
            set { _front = value; }
        }

        public int CalculateBackgroundPosX()
        {
            return (rx * (Board.hScroll - Board.CenterPoint.X + 400) / 100) + base.X /*- Origin.X*/ + 400 - Board.CenterPoint.X + Board.hScroll;
        }

        public int CalculateBackgroundPosY()
        {
            return (ry * (Board.vScroll - Board.CenterPoint.Y + 300) / 100) + base.Y /*- Origin.X*/ + 300 - Board.CenterPoint.Y + Board.vScroll;
        }

        public int ReverseBackgroundPosX(int bgPos)
        {
            return bgPos - Board.hScroll + Board.CenterPoint.X - 400 - (rx * (Board.hScroll - Board.CenterPoint.X + 400) / 100);
        }

        public int ReverseBackgroundPosY(int bgPos)
        {
            return bgPos - Board.vScroll + Board.CenterPoint.Y - 300 - (ry * (Board.vScroll - Board.CenterPoint.Y + 300) / 100);
        }

        public override int X
        {
            get
            {
                if (UserSettings.emulateParallax)
                    return CalculateBackgroundPosX();
                else return base.X;
            }
            set
            {
                int newX;
                if (UserSettings.emulateParallax)
                    newX = ReverseBackgroundPosX(value);
                else newX = value;
                base.Move(newX, base.Y);
            }
        }

        public override int Y
        {
            get
            {
                if (UserSettings.emulateParallax)
                    return CalculateBackgroundPosY();
                else return base.Y;
            }
            set
            {
                int newY;
                if (UserSettings.emulateParallax)
                    newY = ReverseBackgroundPosY(value);
                else newY = value;
                base.Move(base.X, newY);
            }
        }

        public override void Move(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void MoveBase(int x, int y)
        {
            BaseX = x;
            BaseY = y;
        }
    }
}