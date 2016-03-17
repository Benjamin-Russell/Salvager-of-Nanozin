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
    public class Ice : Sprite
    {
        public Ice(Vector2 position)
        {
            mTexture = Nanozin.iceTexture;
            mPosition = position;
            mOrigin = new Vector2(0, 0);
            mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSourceRectangle = new Rectangle(64 * ((Nanozin.rand.Next() % 7) + 1), 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mRotation = 0f;
            mScale = 1f;
            mDepth = .5f;
            mTint = Color.White;

            beingHeated = false;
            heatLevel = 0;
            maxHeat = 200f;
            durationHeated = .2f;
            lastHeatedTime = -10f;
            lastTimeAnimated = 0f;
            furnaceRange = 256f;
        }
        ~Ice() { }

        public float heatLevel,
                     maxHeat,
                     durationHeated,
                     lastHeatedTime,
                     lastTimeAnimated,
                     furnaceRange;
        public bool beingHeated;


        public void update()
        {
            //Animate water
            if (Nanozin.currentScreenTimer >= lastTimeAnimated + .2f)
            {
                int startX = mSourceRectangle.X,
                    newX = 64 * ((Nanozin.rand.Next() % 7) + 1);

                while (startX == newX)
                {
                    newX = 64 * ((Nanozin.rand.Next() % 7) + 1);
                }

                mSourceRectangle.X = newX;
                lastTimeAnimated = Nanozin.currentScreenTimer;
            }

            beingHeated = false;

            //Check for powered furnace
            for (int i = 0; i < Nanozin.furnaces.Count && !beingHeated; i++)
            {
                if (Nanozin.furnaces[i].powered && (Math.Sqrt( (Math.Pow(Nanozin.furnaces[i].mPosition.Y - mPosition.Y, 2)) + (Math.Pow(Nanozin.furnaces[i].mPosition.X - mPosition.X, 2)))) <= furnaceRange)
                {
                    beingHeated = true;
                    lastHeatedTime = Nanozin.currentScreenTimer;
                    if (heatLevel < maxHeat)
                    {
                        //add heat based on proximity
                        heatLevel += 1.5f - ((float)Math.Sqrt( (Math.Pow(Nanozin.furnaces[i].mPosition.Y - mPosition.Y, 2)) + (Math.Pow(Nanozin.furnaces[i].mPosition.X - mPosition.X, 2))) / furnaceRange);
                        if (heatLevel > maxHeat)
                            heatLevel = maxHeat;
                    }
                }
            }

            //Splash particles
            if (heatLevel >= maxHeat * .8 && Nanozin.rand.Next() % 18 == 0)
            {
                Functions.addTemplateParticle(Nanozin.splashP, Functions.randInRadius(new Vector2(mPosition.X+32, mPosition.Y+32), 32));
                Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.Y += ((Nanozin.rand.Next() % 10f) - 5f) / 10f;
                Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.X += ((Nanozin.rand.Next() % 10f) - 5f) / 10f;
            }

            if (!beingHeated)
            {
                //cool
                if (heatLevel > 0)
                {
                    heatLevel -= 1;
                    if (heatLevel < 0)
                        heatLevel = 0;
                }
            }
        }

        public new void draw(SpriteBatch sb)
        {
            Vector2 drawLocation = mPosition - (Nanozin.cameraPosition - Nanozin.SCREEN_MID);

            if (new Rectangle((int)drawLocation.X, (int)drawLocation.Y, 64, 64).Intersects(new Rectangle(0, 0, Nanozin.SCREEN_WIDTH, Nanozin.SCREEN_HEIGHT)))
            {
                //draw water
                sb.Draw(mTexture,
                        drawLocation,
                        mSourceRectangle,
                        mTint * ((heatLevel / maxHeat) + .45f),
                        mRotation,
                        mOrigin,
                        mScale,
                        SpriteEffects.None,
                        .45f);

                //draw ice
                sb.Draw(mTexture,
                        drawLocation,
                        new Rectangle(0, 0, 64, 64),
                        mTint * .6f * ((maxHeat - heatLevel) / maxHeat),
                        mRotation,
                        mOrigin,
                        mScale,
                        SpriteEffects.None,
                        mDepth);
            }
        }
    };
}