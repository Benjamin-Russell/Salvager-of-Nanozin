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
    public abstract class Sprite
    {
        public Sprite()
        {
            mTexture = Nanozin.dustTexture;
        }
        ~Sprite() { }

        public Texture2D mTexture;
        public Rectangle mSourceRectangle,
                 mBoundingBox;
        public Vector2 mPosition,
               mOrigin;
        public float mRotation,
             mScale,
             mDepth;
        public Color mTint;

        public void draw(SpriteBatch sb)
        {
            Vector2 drawLocation = mPosition - (Nanozin.cameraPosition - Nanozin.SCREEN_MID);

            if (new Rectangle((int)drawLocation.X - (int)mOrigin.X, (int)drawLocation.Y - (int)mOrigin.Y, 64, 64).Intersects(new Rectangle(0, 0, Nanozin.SCREEN_WIDTH, Nanozin.SCREEN_HEIGHT)))
            sb.Draw(mTexture,
                    drawLocation,
                    mSourceRectangle,
                    mTint,
                    mRotation,
                    mOrigin,
                    mScale,
                    SpriteEffects.None,
                    mDepth);
        }
    };
}