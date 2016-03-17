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
    public class Mirror : Sprite
    {
        public Mirror(Vector2 position)
        {
            mTexture = Nanozin.mirrorTexture;
            mPosition = position;
            mOrigin = new Vector2(0, 0);
            mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSourceRectangle = new Rectangle(0, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mRotation = 0f;
            mScale = 1f;
            mDepth = .5f;
            mTint = Color.White;

            mirrorPos = new Vector2(mPosition.X + 32, mPosition.Y + 32);
            lastPoweredTime = -2f;
            poweredCD = .25f;
            lastPulseId = -1;
            lastHit = -1;
        }

        public Mirror(Vector2 position, int rotation)
        {
            mTexture = Nanozin.mirrorTexture;
            mPosition = position;
            mOrigin = new Vector2(0, 0);
            mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSourceRectangle = new Rectangle(0, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mRotation = 0f;
            mScale = 1f;
            mDepth = .5f;
            mTint = Color.White;

            mirrorPos = new Vector2(mPosition.X + 32, mPosition.Y + 32);
            mirrorRot = (float)((float)rotation * (Math.PI / 2));
            lastPoweredTime = -2f;
            poweredCD = .25f;
            lastPulseId = -1;
            lastHit = -1;
        }

        public Vector2 mirrorPos;
        public float mirrorRot,
             lastHit;
        float lastPoweredTime,
          poweredCD;
        public int lastPulseId;
        public float rotateAmount = 0;

        public void update()
        {
            if (rotateAmount > 0)
            {
                mirrorRot += (float)(Math.PI / 30f);
                rotateAmount -= (float)(Math.PI / 30f);
                if (rotateAmount <= 0)
                    mirrorRot += .001f;
                if (mirrorRot >= Math.PI * 2)
                    mirrorRot = 0;
            }

            if (Nanozin.currentScreenTimer > lastPoweredTime + poweredCD)
            {
                //If off cooldown, look for power source
                if (Nanozin.currentScreenTimer > lastPoweredTime + poweredCD)
                {
                    int id = Functions.checkForPowersource(mBoundingBox);

                    if (id != lastPulseId && id > -1)
                    {
                        lastPulseId = id;
                        lastPoweredTime = Nanozin.currentScreenTimer;
                        rotateAmount += (float)(Math.PI / 2f);

                        if (!Nanozin.muted)
                            Nanozin.soundSwitchOff.Play();
                    }
                }
            }

            if (Nanozin.currentScreenTimer > lastHit + poweredCD)
                mSourceRectangle = new Rectangle(0, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
        }

        public new void draw(SpriteBatch sb)
        {
            Vector2 drawLocation = mPosition - (Nanozin.cameraPosition - Nanozin.SCREEN_MID);
            Vector2 mirrorLocation = mirrorPos - (Nanozin.cameraPosition - Nanozin.SCREEN_MID);

            if (new Rectangle((int)drawLocation.X, (int)drawLocation.Y, 64, 64).Intersects(new Rectangle(0, 0, Nanozin.SCREEN_WIDTH, Nanozin.SCREEN_HEIGHT)))
            {
                sb.Draw(Nanozin.mirrorTexture,
                        drawLocation,
                        mSourceRectangle,
                        mTint,
                        mRotation,
                        mOrigin,
                        mScale,
                        SpriteEffects.None,
                        mDepth);
                sb.Draw(Nanozin.mirrorTexture,
                        mirrorLocation,
                        new Rectangle(Nanozin.SPRITE_LENGTH * 2, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH),
                        mTint,
                        mirrorRot,
                        new Vector2(Nanozin.SPRITE_LENGTH / 2, Nanozin.SPRITE_LENGTH / 2),
                        mScale,
                        SpriteEffects.None,
                        .51f);
            }
        }
    };
}
