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
    public class Rifle : Sprite
    {
        public Rifle()
        {
            mTexture = Nanozin.plasmaRifleTexture;
            mPosition = new Vector2(-Nanozin.SPRITE_LENGTH, -Nanozin.SPRITE_LENGTH);
            mOrigin = new Vector2(32, 32);
            mBoundingBox = new Rectangle((int)mPosition.X - 32, (int)mPosition.Y - 32, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSourceRectangle = new Rectangle(0, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mRotation = 0f;
            mScale = 1f;
            mDepth = .51f;
            mTint = Color.White;
        }
        ~Rifle() { }

        public void update()
        {
            mBoundingBox = new Rectangle((int)mPosition.X - 32, (int)mPosition.Y - 32, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
        }

        public new void draw(SpriteBatch sb)
        {
            if (Nanozin.levelEditing == false || !mBoundingBox.Intersects(new Rectangle((int)Nanozin.salvagerEdit.X, (int)Nanozin.salvagerEdit.Y, 64, 64)))
            {
                Vector2 drawLocation = mPosition - (Nanozin.cameraPosition - Nanozin.SCREEN_MID);
                float waterConstant = 1f;

                SpriteEffects mEffect = SpriteEffects.None;

                if (mRotation == (float)Math.PI)
                    mEffect = SpriteEffects.FlipVertically;

                int index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "icePatches", 0);
                if (index != -1)
                    waterConstant = ((float)(Nanozin.icePatches[index].maxHeat - Nanozin.icePatches[index].heatLevel) / (float)(Nanozin.icePatches[index].maxHeat)) + .3f;

                sb.Draw(mTexture,
                        drawLocation,
                        mSourceRectangle,
                        mTint * waterConstant,
                        mRotation,
                        mOrigin,
                        mScale,
                        mEffect,
                        mDepth);
            }
        }
    };
}