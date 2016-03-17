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
    public class Plasma : Sprite
    {
        public Plasma(Vector2 position, int max, string source, int index)
        {
            mPosition = position;
            mTexture = Nanozin.plasmaProjectileTexture;
            mOrigin = new Vector2(32, 32);
            mBoundingBox = new Rectangle((int)mPosition.X - 32, (int)mPosition.Y - 32, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSourceRectangle = new Rectangle(0, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mScale = .1f;
            mDepth = .53f;
            mTint = Color.White;
            mSpeed = 32;
            wallsHit = 0;
            maxSize = 1;
            maxWalls = max;
            mID = Nanozin.plasmaIds++;
            collided = false;
            timeCreated = Nanozin.currentScreenTimer;
            mSource = source;
            sourceIndex = index;

            mRotation = Nanozin.theSalvager.mRotation;
            mVelocity = getVelocityFromRot(mRotation);

        }
        ~Plasma() { }

        public Vector2 mVelocity;
        public string mSource;
        public int sourceIndex;
        int wallsHit,
            maxWalls,
            mID;
        float mSpeed,
          maxSize;

        public float timeCreated;
        public bool collided;

        public Vector2 getVelocityFromRot(float rot)
        {
            Vector2 result = Vector2.Zero;
            int tmp = (int)(rot / (Math.PI / 2));
            switch (tmp)
            {
                case 0:
                    result = new Vector2(mSpeed, 0);
                    break;
                case 1:
                    result = new Vector2(0, mSpeed);
                    break;
                case 2:
                    result = new Vector2(-mSpeed, 0);
                    break;
                case 3:
                    result = new Vector2(0, -mSpeed);
                    break;
            }

            return result;
        }

        public bool update()
        {
            bool done = false;

            mPosition += mVelocity;

            //Trail Particles
            for (int i = 0; i < (maxWalls - wallsHit) * 4; i++)
            {
                Functions.addTemplateParticle(Nanozin.trailP, Functions.randInRadius(mPosition, 16));
            }

            mBoundingBox = new Rectangle((int)mPosition.X - 32, (int)mPosition.Y - 32, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);

            int index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "walls", 0);
            if (index != -1)
            {
                wallsHit++;
                maxSize -= (1f / (float)maxWalls) * .7f;
                Nanozin.wallExplosions.Add(new WallExplosion(Nanozin.walls[index].mPosition));
                Nanozin.walls.RemoveAt(index);

                Nanozin.soundPlasmaFadeOut.Play();

                for (int i = 0; i < (Nanozin.rand.Next() % 30) + 20; i++)
                {
                    Functions.addTemplateParticle(Nanozin.burstP, Functions.randInRadius(mPosition, 16));
                }
            }

            index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "plasmaFilms", 0);
            if (index != -1)
            {
                if (Nanozin.plasmaFilms[index].lastPlasmaId != mID && Nanozin.plasmaFilms[index].timeKilled < 0 && !Nanozin.plasmaFilms[index].mBoundingBox.Intersects(Nanozin.theSalvager.mBoundingBox))
                {
                    Nanozin.plasmaFilms[index].timeKilled = Nanozin.currentScreenTimer;

                    if (!Nanozin.muted)
                        Nanozin.soundSizzle.Play();
                }
            }

            index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "detectors", 0);
            if (index != -1)
            {
                if (Nanozin.detectors[index].powered == false && !Nanozin.detectors[index].mBoundingBox.Intersects(Nanozin.theSalvager.mBoundingBox))
                {
                    Nanozin.detectors[index].lastPoweredTime = Nanozin.currentScreenTimer;
                    Nanozin.detectors[index].powered = true;
                    Nanozin.detectors[index].lastPulseId = Nanozin.pulseIds++;
                }
            }

            if (mScale < maxSize)
            {
                mScale += .1f;
            }
            if (mScale > maxSize)
            {
                mScale = maxSize;
            }

            index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "crackedWalls", 0);
            if (index != -1)
            {
                Nanozin.crackedWalls[index].timeTriggered = Nanozin.currentScreenTimer;
                Nanozin.soundSizzle.Play();

                for (int i = 0; i < (Nanozin.rand.Next() % 10) + 15; i++)
                {
                    Functions.addTemplateParticle(Nanozin.burstP, Functions.randInRadius(mPosition, 32));
                }
            }

            index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "icePatches", 0);
            if (index != -1)
            {
                Nanozin.icePatches[index].heatLevel = Nanozin.icePatches[index].maxHeat * .89f;
            }

            //Melt ice in range
            if (mPosition.Y % 64 == 0)
            {
                index = Functions.checkObjectCollision(mBoundingBox, -64, 0, "icePatches", 0);
                if (index != -1)
                {
                    if (Nanozin.icePatches[index].heatLevel < Nanozin.icePatches[index].maxHeat * .6f)
                        Nanozin.icePatches[index].heatLevel = Nanozin.icePatches[index].maxHeat * .6f;
                }

                index = Functions.checkObjectCollision(mBoundingBox, 64, 0, "icePatches", 0);
                if (index != -1)
                {
                    if (Nanozin.icePatches[index].heatLevel < Nanozin.icePatches[index].maxHeat * .6f)
                        Nanozin.icePatches[index].heatLevel = Nanozin.icePatches[index].maxHeat * .6f;
                }
            }

            //Melt ice in range
            if (mPosition.X % 64 == 0)
            {
                index = Functions.checkObjectCollision(mBoundingBox, 0, -64, "icePatches", 0);
                if (index != -1)
                {
                    if (Nanozin.icePatches[index].heatLevel < Nanozin.icePatches[index].maxHeat * .6f)
                        Nanozin.icePatches[index].heatLevel = Nanozin.icePatches[index].maxHeat * .6f;
                }

                index = Functions.checkObjectCollision(mBoundingBox, 0, 64, "icePatches", 0);
                if (index != -1)
                {
                    if (Nanozin.icePatches[index].heatLevel < Nanozin.icePatches[index].maxHeat * .6f)
                        Nanozin.icePatches[index].heatLevel = Nanozin.icePatches[index].maxHeat * .6f;
                }
            }

            index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "mirrors", 0);
            if (index != -1)
            {
                mPosition = Nanozin.mirrors[index].mirrorPos;
                mRotation = Nanozin.mirrors[index].mirrorRot;
                Nanozin.mirrors[index].mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
                Nanozin.mirrors[index].lastHit = Nanozin.currentScreenTimer;

                mVelocity = getVelocityFromRot(mRotation);
                mPosition += mVelocity;

                if (!Nanozin.muted)
                    Nanozin.soundPlasmaAbsorb.Play();

                for (int i = 0; i < 10; i++)
                {
                    Functions.addTemplateParticle(Nanozin.trailP, Functions.randInRadius(Nanozin.mirrors[index].mirrorPos, 64));
                }
            }
            
            for (int i = 0; i < Nanozin.plasmas.Count; i++)
            {
                if (mID != Nanozin.plasmas[i].mID && mBoundingBox.Intersects(Nanozin.plasmas[i].mBoundingBox))
                {
                    done = true;
                    Nanozin.plasmas[i].collided = true;
                }
            }

            index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "bombWalls", 0);
            if (index != -1)
            {
                done = true;
                Nanozin.bombWalls[index].triggeredTime = Nanozin.currentScreenTimer;

                if (!Nanozin.muted)
                {
                    Nanozin.soundBombCharge.Play();
                    Nanozin.soundPlasmaHardWall.Play();
                }
            }

            index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "ammos", 0);
            if (index != -1)
            {
                //Particles
                if (Nanozin.ammos[index].mSourceRectangle.X == 0)
                {
                    done = true;
                    Nanozin.ammos[index].mSourceRectangle.X = 64;
                    for (int j = 0; j < 100; j++)
                    {
                        Functions.addTemplateParticle(Nanozin.burstP, Functions.randInRadius(Nanozin.ammos[index].mPosition, 24));
                    }
                }
            }

            index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "fuelCells", 0);
            if (index != -1)
            {
                done = true;
                Nanozin.transitioning = true;
                Nanozin.timeTransitionStarted = Nanozin.currentScreenTimer;

                if (!Nanozin.muted)
                    Nanozin.soundFuelCellBroken.Play();

                for (int j = 0; j < 500; j++)
                {
                    Functions.addTemplateParticle(Nanozin.burstP, Functions.randInRadius(Nanozin.fuelCells[index].mPosition, 24));
                }
                Nanozin.fuelCells.RemoveAt(index);
            }

            index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "plasmaLancers", 0);
            if (index != -1)
            {
                if (!(mSource == "plasmaLancer" && index == sourceIndex && Nanozin.currentScreenTimer < timeCreated + .04f))
                {
                    done = true;
                    Nanozin.plasmaLancers[index].destroyed = true;
                }
            }
            
            if (!done && (
                !mBoundingBox.Intersects(new Rectangle((int)mSpeed, (int)mSpeed, Nanozin.levelWidth - (int)mSpeed, Nanozin.levelHeight - (int)mSpeed))
                || (Functions.checkObjectCollision(mBoundingBox, 0, 0, "hardWalls", 0) != -1)
                || (Functions.checkObjectCollision(mBoundingBox, 0, 0, "nodes", 0) != -1)
                || wallsHit > maxWalls - 1
                || collided))
                done = true;

            index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "receivers", 0);
            int index2 = Functions.checkObjectCollision(mBoundingBox, 0, 0, "togglers", 0);
            if (index != -1)
            {
                done = true;
                if (Nanozin.currentScreenTimer > Nanozin.receivers[index].lastPoweredTime + Nanozin.receivers[index].unpoweredCD)
                {
                    Nanozin.receivers[index].powered = true;
                    Nanozin.receivers[index].mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH * 3, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
                    Nanozin.receivers[index].lastPoweredTime = Nanozin.currentScreenTimer;
                    Nanozin.receivers[index].lastPulseId = Nanozin.pulseIds++;

                    if (!Nanozin.muted)
                        Nanozin.soundPlasmaAbsorb.Play();

                    for (int i = 0; i < 60; i++)
                    {
                        Functions.addTemplateParticle(Nanozin.trailP, Functions.randInRadius(new Vector2(Nanozin.receivers[index].mPosition.X + 32, Nanozin.receivers[index].mPosition.Y + 32), 56));
                    }
                }
            }
            else if (index2 != -1)
            {
                done = true;
                Nanozin.togglers[index2].lastPoweredTime = Nanozin.currentScreenTimer;

                if (!Nanozin.muted)
                    Nanozin.soundPlasmaAbsorb.Play();

                if (Nanozin.togglers[index2].powering)
                {
                    Nanozin.togglers[index2].powering = false;
                    Nanozin.togglers[index2].mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH * 3, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
                }
                else
                {
                    Nanozin.togglers[index2].powering = true;
                    Nanozin.togglers[index2].powered = true;
                    Nanozin.togglers[index2].lastPoweredTime = Nanozin.currentScreenTimer - 20f;
                    Nanozin.togglers[index2].mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH * 3, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
                }

                for (int i = 0; i < 60; i++)
                {
                    Functions.addTemplateParticle(Nanozin.trailP, Functions.randInRadius(new Vector2(Nanozin.togglers[index2].mPosition.X + 32, Nanozin.togglers[index2].mPosition.Y + 32), 56));
                }
            }
            else if (done)
            {
                //Ending Sound Effects
                if (Functions.checkObjectCollision(mBoundingBox, 0, 0, "hardWalls", 0) != -1
                    || Functions.checkObjectCollision(mBoundingBox, 0, 0, "nodes", 0) != -1
                    || !mBoundingBox.Intersects(new Rectangle((int)mSpeed, (int)mSpeed, Nanozin.levelWidth - (int)mSpeed, Nanozin.levelHeight - (int)mSpeed)))
                {
                    Nanozin.soundPlasmaHardWall.Play();
                }
                else
                {
                    Nanozin.soundPlasmaFadeOut.Play();
                }

                mPosition += mVelocity;

                //Fade effect
                Nanozin.fadeLevel.SetValue(1.75f);

                int num = (Nanozin.rand.Next() % 100) + (85 * maxWalls) - (wallsHit * 50);
                for (int i = 0; i < num; i++)
                {
                    Functions.addTemplateParticle(Nanozin.burstP, Functions.randInRadius(mPosition, 16));
                }
            }

            return done;
        }
        
        public new void draw(SpriteBatch sb)
        {
            Vector2 drawLocation = mPosition - (Nanozin.cameraPosition - Nanozin.SCREEN_MID);

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
                    drawLocation + new Vector2(mVelocity.X * .25f, mVelocity.Y * .25f),
                    new Rectangle(0, 0, 64, 64),
                    Color.White * .5f,
                    mRotation,
                    mOrigin,
                    mScale,
                    SpriteEffects.None,
                    mDepth - .02f);
            sb.Draw(mTexture,
                    drawLocation + new Vector2(mVelocity.X * -.25f, mVelocity.Y * -.25f),
                    new Rectangle(0, 0, 64, 64),
                    Color.White * .5f,
                    mRotation,
                    mOrigin,
                    mScale,
                    SpriteEffects.None,
                    mDepth - .02f);
        }
    };
}
