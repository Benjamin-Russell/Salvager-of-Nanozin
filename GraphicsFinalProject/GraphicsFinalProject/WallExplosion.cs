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
    public class WallExplosion : Sprite
    {
        public WallExplosion(Vector2 position)
        {
            mAlpha = 1;
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
        ~WallExplosion() { }

        float mAlpha;

        public bool update()
        {
            bool done = false;

            mAlpha -= .02f;
            if (mAlpha <= 0)
            {
                done = true;
            }

            if (mAlpha >= .98f)
            {
                int index;
                index = Functions.checkObjectCollision(mBoundingBox, Nanozin.SPRITE_LENGTH / 2, 0, "crackedWalls", 0);
                if (index != -1 && Nanozin.crackedWalls[index].timeTriggered == -1)
                    Nanozin.crackedWalls[index].timeTriggered = Nanozin.currentScreenTimer;
                index = Functions.checkObjectCollision(mBoundingBox, Nanozin.SPRITE_LENGTH / -2, 0, "crackedWalls", 0);
                if (index != -1 && Nanozin.crackedWalls[index].timeTriggered == -1)
                    Nanozin.crackedWalls[index].timeTriggered = Nanozin.currentScreenTimer;
                index = Functions.checkObjectCollision(mBoundingBox, 0, Nanozin.SPRITE_LENGTH / 2, "crackedWalls", 0);
                if (index != -1 && Nanozin.crackedWalls[index].timeTriggered == -1)
                    Nanozin.crackedWalls[index].timeTriggered = Nanozin.currentScreenTimer;
                index = Functions.checkObjectCollision(mBoundingBox, 0, Nanozin.SPRITE_LENGTH / -2, "crackedWalls", 0);
                if (index != -1 && Nanozin.crackedWalls[index].timeTriggered == -1)
                    Nanozin.crackedWalls[index].timeTriggered = Nanozin.currentScreenTimer;
            }

            return done;
        }

        public new void draw(SpriteBatch sb)
        {
            Vector2 drawLocation = mPosition - (Nanozin.cameraPosition - Nanozin.SCREEN_MID);

            if (new Rectangle((int)drawLocation.X, (int)drawLocation.Y, 64, 64).Intersects(new Rectangle(0, 0, Nanozin.SCREEN_WIDTH, Nanozin.SCREEN_HEIGHT)))
            {
                sb.Draw(Nanozin.wallsTexture,
                        drawLocation,
                        mSourceRectangle,
                        mTint,
                        mRotation,
                        mOrigin,
                        mScale,
                        SpriteEffects.None,
                        mDepth);
                sb.Draw(Nanozin.wallsTexture,
                        drawLocation,
                        new Rectangle(0, 64, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH),
                        Color.White * (1 - mAlpha),
                        mRotation,
                        mOrigin,
                        mScale,
                        SpriteEffects.None,
                        .52f);
            }
        }
    };
}