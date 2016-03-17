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
    public class Furnace : Sprite
    {
        public Furnace( Vector2 position )
        {
            mTexture = Nanozin.furnaceTexture;
            mPosition = position;
            mOrigin = new Vector2(0, 0);
            mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSourceRectangle = new Rectangle(0, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mRotation = 0f;
            mScale = 1f;
            mDepth = .5f;
            mTint = Color.White;

            powered = false;
            lastPoweredTime = -10f;
            truePoweredTime = -10f;
            unpoweredTime = -10f;
            durationPowered = .5f;
            unpoweredCD = .5f;
            fireNum = 1;
            lastSounded = 0f;
        }
        ~Furnace() { }

        public bool powered;
        public float lastPoweredTime,
              unpoweredTime,
              durationPowered,
              unpoweredCD,
              truePoweredTime,
              lastSounded;
        public int     fireNum;

        public void update()
        {
            //Fire particles
            if (powered || Nanozin.currentScreenTimer <= unpoweredTime + 2.5f)
            {
                for (int i = 0; i < fireNum; i++)
                {
                    Functions.addTemplateParticle(Nanozin.fireP, Functions.randInRadius(new Vector2(mPosition.X + 32, mPosition.Y + 28), 18));
                }
            }

            if (Nanozin.currentScreenTimer <= unpoweredTime + 2.5f)
            {
                fireNum = 1;
            }

            if (powered)
            {
                //Go to full fire output
                if (fireNum == 1 && Nanozin.currentScreenTimer >= truePoweredTime + 2.5f)
                {
                    fireNum = 2;
                }

                //Unpower after duration
                if (Nanozin.currentScreenTimer >= lastPoweredTime + durationPowered)
                {
                    powered = false;
                    unpoweredTime = Nanozin.currentScreenTimer;
                }

                if (!Nanozin.muted && Nanozin.currentScreenTimer >= lastSounded + 1.3f)
                {
                    Nanozin.soundHumming.Play();
                }
            }

            if (!powered)
            {
                //If off cooldown, look for power source
                if (Nanozin.currentScreenTimer > lastPoweredTime + unpoweredCD)
                {
                    int id = Functions.checkForPowersource(mBoundingBox);

                    if (Functions.checkForPowersource(mBoundingBox) != -1)
                    {
                        powered = true;

                        if (Nanozin.currentScreenTimer > lastPoweredTime + 2.5f)
                        {
                            truePoweredTime = Nanozin.currentScreenTimer;
                        }

                        lastPoweredTime = Nanozin.currentScreenTimer;
                    }
                }
            }
        }

        public new void draw(SpriteBatch sb)
        {
            Vector2 drawLocation = mPosition - (Nanozin.cameraPosition - Nanozin.SCREEN_MID);

            if (new Rectangle((int)drawLocation.X, (int)drawLocation.Y, 64, 64).Intersects(new Rectangle(0, 0, Nanozin.SCREEN_WIDTH, Nanozin.SCREEN_HEIGHT)))
            {
                sb.Draw(mTexture,
                        drawLocation,
                        mSourceRectangle,
                        mTint,
                        mRotation,
                        mOrigin,
                        mScale,
                        SpriteEffects.None,
                        mDepth);
                if (powered)
                {
                    sb.Draw(mTexture,
                            drawLocation,
                            new Rectangle(64, 0, 64, 64),
                            mTint,
                            mRotation,
                            mOrigin,
                            mScale,
                            SpriteEffects.None,
                            .53f);
                }
            }
        }
    }
}
