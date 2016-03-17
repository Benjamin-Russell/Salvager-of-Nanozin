using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace NanozinProject
{
    public class FuelCell : Sprite
    {
        public FuelCell( Vector2 position )
        {
            mPosition = new Vector2(position.X+32, position.Y+32);
            mTexture = Nanozin.fuelCellTexture;
            mSourceRectangle = new Rectangle(0, 0, 64, 64);
            mOrigin = new Vector2(32, 32);
            mBoundingBox = new Rectangle((int)mPosition.X - 32, (int)mPosition.Y - 32, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mScale = .75f;
            mDepth = .5f;
            mTint = Color.White;

            taken = false;
        }
        ~FuelCell() { }

        public bool taken;

        public bool update()
        {
            mRotation += (float)Math.PI / 90f;
            
            return taken;
        }
    };
}