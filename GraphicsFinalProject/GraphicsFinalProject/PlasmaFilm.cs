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
    public class PlasmaFilm : Sprite
    {
        public PlasmaFilm(Vector2 position)
        {
            mTexture = Nanozin.plasmaFilmTexture;
            mPosition = new Vector2(position.X + 32, position.Y + 32);
            mOrigin = new Vector2(32, 32);
            mBoundingBox = new Rectangle((int)mPosition.X - 32, (int)mPosition.Y - 32, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSourceRectangle = new Rectangle(0, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mScale = 1f;
            mDepth = .5f;
            mTint = Color.White;
            powered = false;
            lastPoweredTime = 0f;
            durationPowered = .25f;
            unpoweredCD = .5f;
            lastPulseId = -2;
            lastPlasmaId = -1;
            mRotation = (float)(Nanozin.rand.Next() % 4) * (float)(Math.PI) / 2f;
            timeKilled = -1f;
        }
        ~PlasmaFilm() { }

        public int lastPulseId,
                   lastPlasmaId;
        public bool powered;
        public float lastPoweredTime,
                     durationPowered,
                     unpoweredCD,
                     timeKilled;

        public bool update()
        {
            if (timeKilled > 0 && Nanozin.currentScreenTimer > timeKilled + 1f)
            {
                return true;
            }
            else
            {
                if (timeKilled > 0)
                {
                    //death particles
                    Functions.addTemplateParticle(Nanozin.trailP, Functions.randInRadius(new Vector2(mPosition.X, mPosition.Y), 22));
                    Nanozin.particles[Nanozin.particles.Count - 1].mStartScale -= .2f;
                    Nanozin.particles[Nanozin.particles.Count - 1].mEndScale -= .2f;
                    Nanozin.particles[Nanozin.particles.Count - 1].mDuration -= .1f;

                    powered = false;
                    mSourceRectangle = new Rectangle(0, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
                }
                else
                {
                    if (powered)
                    {
                        //Unpower after duration
                        if (Nanozin.currentScreenTimer > lastPoweredTime + durationPowered)
                        {
                            powered = false;
                        }
                        else
                        {
                            if (Nanozin.rand.Next() % 3 == 0)
                                Functions.addTemplateParticle(Nanozin.powerP, Functions.randInRadius(new Vector2(mPosition.X, mPosition.Y + 6), 20));

                            //Set animation
                            if (Nanozin.currentScreenTimer < lastPoweredTime + (durationPowered * .3))
                                mSourceRectangle = new Rectangle(64, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
                            else if (Nanozin.currentScreenTimer <= lastPoweredTime + (durationPowered * .6))
                                mSourceRectangle = new Rectangle(128, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
                            else
                                mSourceRectangle = new Rectangle(192, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
                        }
                    }
                    if (!powered)
                    {
                        //Set to unpowered state
                        mSourceRectangle = new Rectangle(0, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);

                        //If off cooldown, look for power source
                        if (Nanozin.currentScreenTimer > lastPoweredTime + unpoweredCD)
                        {
                            int id = Functions.checkForPowersource(mBoundingBox);

                            if (id != lastPulseId && id > -1)
                            {
                                lastPulseId = id;
                                powered = true;
                                lastPoweredTime = Nanozin.currentScreenTimer;
                            }
                        }
                    }
                }

                return false;
            }
        }
    };
}
