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
    public class Particle
    {
        //constructors / destructor
        public Particle()
        {
            isTrash = true;
            mGlow = ((float)Nanozin.rand.Next() % 50f) / 100;
            mGlowDir = Nanozin.rand.Next() % 2;
            if (mGlowDir == 0)
                mGlowDir = -1;
        }
        public Particle(int textureIndex, Vector2 origin, Vector2 position, Vector2 velocity, Vector2 acceleration, float friction, float rotation, float startScale, float endScale, Color startColor, Color endColor, float duration, float startAlpha, float endAlpha, float depth)
        {
            isTrash = false;
            mSourceRectangle = new Rectangle(0, 0, 64, 64);
            mTextureIndex = textureIndex;
            mOrigin = origin;
            mPosition = position;
            mVelocity = velocity;
            mStartVelocity = mVelocity;
            mAcceleration = acceleration;
            mFriction = friction;
            mRotation = rotation;
            mStartScale = startScale;
            mCurScale = startScale;
            mEndScale = endScale;
            mStartColor = startColor;
            mCurColor = startColor;
            mEndColor = endColor;
            mDuration = duration;
            mStartAlpha = startAlpha;
            mCurAlpha = mStartAlpha;
            mEndAlpha = endAlpha;
            mAge = 0;
            mDepth = depth;
        }
        ~Particle() { }

        public Vector2 mPosition,
               mVelocity,
               mStartVelocity,
               mAcceleration,
               mOrigin;
        public Rectangle mSourceRectangle;
        public float mAge,
             mDuration,
             mFriction,
             mRotation,
             mCurRotation,
             mDepth,
             mCurScale,
             mStartScale,
             mEndScale,
             mCurAlpha,
             mStartAlpha,
             mEndAlpha,
             mGlow;
        public Color mCurColor,
             mStartColor,
             mEndColor;
        public int mTextureIndex,
                   mGlowDir;
        public bool isTrash;

        public void replicate(Particle p)
        {
            mAge = 0;
            mSourceRectangle = p.mSourceRectangle;
            isTrash = false;
            mTextureIndex = p.mTextureIndex;
            mOrigin = p.mOrigin;
            mPosition = p.mPosition;
            mVelocity = p.mVelocity;
            mStartVelocity = p.mStartVelocity;
            mAcceleration = p.mAcceleration;
            mFriction = p.mFriction;
            mStartScale = p.mStartScale;
            mCurScale = mStartScale;
            mEndScale = p.mEndScale;
            mStartColor = p.mStartColor;
            mCurColor = mStartColor;
            mEndColor = p.mEndColor;
            mDuration = p.mDuration;
            mStartAlpha = p.mStartAlpha;
            mCurAlpha = mStartAlpha;
            mEndAlpha = p.mEndAlpha;
            mDepth = p.mDepth;
            mRotation = p.mRotation;

            if (mRotation != 0)
                mCurRotation = Nanozin.rand.Next() % (float)(Math.PI * 2);
        }

        public void update()
        {
            if (mAge >= mDuration)
            {
                isTrash = true;
            }
            else
            {
                float ageFactor = (float)mAge / (float)mDuration;

                //Add rotation
                mCurRotation += mRotation;

                //Add acceleration to velocity
                mVelocity = Vector2.Add(mVelocity, mAcceleration);

                //Add friction to velocity
                mVelocity = new Vector2(mVelocity.X * mFriction, mVelocity.Y * mFriction);

                //Add velocity to position
                mPosition = Vector2.Add(mPosition, mVelocity);

                //update scale
                mCurScale = mStartScale + ((mEndScale - mStartScale) * ageFactor);

                //Explosion particle specific
                if (mTextureIndex == 8)
                    ageFactor += .5f;

                //update color
                mCurColor = Color.Lerp(mStartColor, mEndColor, ageFactor);

                //Explosion particle specific
                if (mTextureIndex == 8)
                    ageFactor -= .5f;

                //Update alpha
                mCurColor *= ((ageFactor * mEndAlpha) + ((1 - ageFactor) * mStartAlpha));

                //Charge particle specific
                if (mTextureIndex == 1)
                {
                    mSourceRectangle = new Rectangle(64, 0, 64, 64);

                    if (Nanozin.currentScreen == 2)
                        mCurRotation = (float)Math.Atan2(mPosition.Y - Nanozin.theSalvager.mPosition.Y, mPosition.X - Nanozin.theSalvager.mPosition.X);
                    else
                        mCurRotation = (float)Math.Atan2(mPosition.Y - Nanozin.SCREEN_HEIGHT / 2f, mPosition.X - Nanozin.SCREEN_WIDTH / 2f);

                    if (ageFactor >= .4 && ageFactor <= .8)
                    {
                        mSourceRectangle = new Rectangle(0, 0, 64, 64);
                        mFriction = 1f;
                        mVelocity = new Vector2(0, 0);
                    }
                    else if (ageFactor > .8)
                    {
                        if (Nanozin.currentScreen == 1)
                            mVelocity = new Vector2(mStartVelocity.X * -1.3f, mStartVelocity.Y * -1.3f);
                        else if (Nanozin.currentScreen == 2)
                            mVelocity = new Vector2(((Nanozin.theSalvager.mPosition.X + (Nanozin.theSalvager.mVelocity.X * 8)) - mPosition.X) * .23f, ((Nanozin.theSalvager.mPosition.Y + (Nanozin.theSalvager.mVelocity.Y * 8)) - mPosition.Y) * .23f);
                    }
                }
                //Death particle specific
                else if (mTextureIndex == 3)
                {
                    mGlow += (float)mGlowDir / 75f;
                    if (mGlow >= .5f || mGlow <= 0 )
                        mGlowDir *= -1;
                }
                //Cloud particle specific
                else if (mTextureIndex == 9)
                {
                    //Beginning Shrink effect
                    if (mStartScale > mEndScale)
                        mStartScale -= .75f;

                    //If out of level, die
                    if (mAge > 100f && !new Rectangle((int)mPosition.X - (int)(32 * mCurScale), (int)mPosition.Y - (int)(32 * mCurScale), (int)(32 * mCurScale), (int)(32 * mCurScale)).Intersects(new Rectangle(0, 0, Nanozin.levelWidth, Nanozin.levelHeight)))
                    {
                        isTrash = true;
                    }
                }
            }
        }

        public void draw(SpriteBatch sb)
        {
            Vector2 drawLocation = mPosition - (Nanozin.cameraPosition - Nanozin.SCREEN_MID);

            sb.Draw(Nanozin.particleTextures[mTextureIndex],
                drawLocation,
                mSourceRectangle,
                mCurColor,
                mCurRotation,
                mOrigin,
                mCurScale,
                SpriteEffects.None,
                mDepth);

            //Motion Blur
            if (mTextureIndex == 1 || mTextureIndex == 2 || mTextureIndex == 6)
            {
                sb.Draw(Nanozin.particleTextures[mTextureIndex],
                    drawLocation + new Vector2(mVelocity.X * .3f, mVelocity.Y * .3f),
                    mSourceRectangle,
                    mCurColor * (mGlow * .8f),
                    mCurRotation - (float)(Math.PI / 20),
                    mOrigin,
                    mCurScale,
                    SpriteEffects.None,
                    mDepth - .05f);
                sb.Draw(Nanozin.particleTextures[mTextureIndex],
                    drawLocation + new Vector2(mVelocity.X * -.3f, mVelocity.Y * -.3f),
                    mSourceRectangle,
                    mCurColor * (mGlow * .8f),
                    mCurRotation + (float)(Math.PI / 20),
                    mOrigin,
                    mCurScale,
                    SpriteEffects.None,
                    mDepth - .05f);
            }
            //Death particle specific
            if (mTextureIndex == 3)
            {
                sb.Draw(Nanozin.particleTextures[4],
                    drawLocation,
                    mSourceRectangle,
                    Color.White * mGlow,
                    mCurRotation,
                    mOrigin,
                    mCurScale,
                    SpriteEffects.None,
                    mDepth + .02f);
            }
        }
    };
}