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
    public class Toggler : Receiver
    {
        public Toggler(Vector2 position) : base(position)
        {
            powering = false;
            toggleCD = .5f;
        }
        ~Toggler() { }

        public bool powering;
        public float toggleCD;

        public new void update()
        {
            if (!Nanozin.levelEditing)
            {
                if (powering)
                {
                    mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH * 3, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);

                    if (Nanozin.currentScreenTimer > lastPoweredTime + unpoweredCD)
                        mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH * 3, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
                }

                if (powered)
                {
                    //Unpower after duration
                    if (Nanozin.currentScreenTimer >= lastPoweredTime + durationPowered)
                    {
                        powered = false;
                    }
                }

                if (!powered)
                {
                    //Set to unpowered state
                    if (mSourceRectangle.X == Nanozin.SPRITE_LENGTH * 3 && mSourceRectangle.Y == 0 && Nanozin.currentScreenTimer > lastPoweredTime + durationPowered)
                        mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH * 3, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);

                    //If off cooldown, look for power source
                    if (powering && Nanozin.currentScreenTimer >= lastPoweredTime + toggleCD)
                    {
                        lastPulseId = Nanozin.pulseIds++;
                        powered = true;
                        lastPoweredTime = Nanozin.currentScreenTimer;
                        if (mSourceRectangle.X == Nanozin.SPRITE_LENGTH && mSourceRectangle.Y == 0)
                            mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH * 2, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
                        else
                            mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH * 3, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
                    }
                    else if (Nanozin.currentScreenTimer >= lastPoweredTime + unpoweredCD)
                    {
                        int id = Functions.checkForPowersource(mBoundingBox);

                        if (id != lastPulseId && id > -1)
                        {
                            lastPulseId = id;
                            powered = true;
                            lastPoweredTime = Nanozin.currentScreenTimer;
                            if (mSourceRectangle.X == Nanozin.SPRITE_LENGTH && mSourceRectangle.Y == 0)
                                mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH * 2, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
                            else
                                mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH * 3, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
                        }
                    }
                }
            }
        }

        public new void draw(SpriteBatch sb)
        {
            Vector2 drawLocation = mPosition - (Nanozin.cameraPosition - Nanozin.SCREEN_MID);
            Rectangle lightSource = new Rectangle(Nanozin.SPRITE_LENGTH * 4, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);

            if (new Rectangle((int)drawLocation.X, (int)drawLocation.Y, 64, 64).Intersects(new Rectangle(0, 0, Nanozin.SCREEN_WIDTH, Nanozin.SCREEN_HEIGHT)))
            {
                if (powering)
                    lightSource.X = Nanozin.SPRITE_LENGTH * 5;

                sb.Draw(mTexture,
                        drawLocation,
                        mSourceRectangle,
                        mTint,
                        mRotation,
                        mOrigin,
                        mScale,
                        SpriteEffects.None,
                        mDepth);
                sb.Draw(mTexture,
                        drawLocation,
                        lightSource,
                        mTint,
                        mRotation,
                        mOrigin,
                        mScale,
                        SpriteEffects.None,
                        .51f);
            }
        }
    }
}
