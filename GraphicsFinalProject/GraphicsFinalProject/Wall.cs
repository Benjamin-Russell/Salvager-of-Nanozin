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
    public class Wall : Sprite
    {
        public Wall() { }

        public Wall(Vector2 position)
        {
            mTexture = Nanozin.wallsTexture;
            mPosition = position;
            mOrigin = new Vector2(0, 0);
            mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSourceRectangle = new Rectangle(0, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mRotation = 0f;
            mScale = 1f;
            mDepth = .5f;
            mTint = Color.White;
        }
        ~Wall() { }
    };
}
