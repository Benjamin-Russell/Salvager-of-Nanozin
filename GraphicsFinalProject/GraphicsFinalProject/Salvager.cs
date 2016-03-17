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
    public class Salvager : Sprite
    {
        public Salvager()
        {
            mTexture = Nanozin.salvagerRifleTexture;
            mPosition = new Vector2(-128, -128);
            mOrigin = new Vector2(32, 32);
            mBoundingBox = new Rectangle((int)mPosition.X - 32, (int)mPosition.Y - 32, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSmallerBox = new Rectangle((int)mPosition.X - 8, (int)mPosition.Y - 8, Nanozin.SPRITE_LENGTH / 4, Nanozin.SPRITE_LENGTH / 4);
            mSourceRectangle = new Rectangle(0, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mScale = .8f;
            mDepth = .8f;
            mTint = Color.White;

            mVelocity = new Vector2(0, 0);
            mAmmo = 999;
            moveSpeed = 4f;
            lastEquipped = 0;
            targetRotation = mRotation;
            firing = false;
            onIce = false;
            firingTime = 1f;
            rifleCD = 2f;
            lastFired = -rifleCD;
            movedUpLast = false;
            movedRightLast = false;
            movedVertical = false;
            movedHorizontal = false;
            sinking = false;
            dead = false;
            lastAnimated = -10f;
            mAlpha = 1f;
            lastFootStep = 0f;
            screamed = false;
        }
        ~Salvager() { }

        public bool firing,
                    onIce,
                    movedUpLast,
                    movedRightLast,
                    movedVertical,
                    movedHorizontal,
                    sinking,
                    dead,
                    screamed;
        float moveSpeed,
              lastEquipped,
              lastFired,
              firingTime,
              rifleCD,
              lastAnimated,
              lastFootStep,
              mAlpha;
        public float targetRotation;
        public int mAmmo;
        public Vector2 mVelocity;
        Rectangle mSmallerBox;

        public void update()
        {
            bool alignedX = false,
                 alignedY = false;
            Vector2 mStartPosition = mPosition;
            int index,
                directionTried = -1;
            
            //Add velocity
            mPosition += mVelocity;

            //Check alignment
            if ((mPosition.X - 32) % Nanozin.SPRITE_LENGTH == 0)
                alignedX = true;
            if ((mPosition.Y - 32) % Nanozin.SPRITE_LENGTH == 0)
                alignedY = true;

            //Update bounding boxes
            mBoundingBox = new Rectangle((int)mPosition.X - 32, (int)mPosition.Y - 32, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSmallerBox = new Rectangle((int)mPosition.X - 8, (int)mPosition.Y - 8, Nanozin.SPRITE_LENGTH / 4, Nanozin.SPRITE_LENGTH / 4);

            //Ice
            onIce = false;
            for (int i = 0; i < Nanozin.icePatches.Count && !onIce; i++)
            {
                if (mBoundingBox.Intersects(Nanozin.icePatches[i].mBoundingBox) && (Nanozin.icePatches[i].heatLevel < (Nanozin.icePatches[i].maxHeat * .7f)))
                {
                    onIce = true;
                }
            }
           
            //When to lose velocity from ices
            if (mVelocity != new Vector2(0, 0) && (!onIce
               || mPosition.Y + mVelocity.Y < 32
               || mPosition.Y + mVelocity.Y > Nanozin.levelHeight-32
               || mPosition.X + mVelocity.X < 32
               || mPosition.X + mVelocity.X > Nanozin.levelWidth - 32
               || Functions.checkObjectCollision(mBoundingBox, (int)mVelocity.X, (int)mVelocity.Y, "walls", 0) != -1
               || Functions.checkObjectCollision(mBoundingBox, (int)mVelocity.X, (int)mVelocity.Y, "hardWalls", 0) != -1
               || Functions.checkObjectCollision(mBoundingBox, (int)mVelocity.X, (int)mVelocity.Y, "crackedWalls", 0) != -1
               || Functions.checkObjectCollision(mBoundingBox, (int)mVelocity.X, (int)mVelocity.Y, "wallExplosions", 0) != -1
               || Functions.checkObjectCollision(mBoundingBox, (int)mVelocity.X, (int)mVelocity.Y, "nodes", 0) != -1
               || Functions.checkObjectCollision(mBoundingBox, (int)mVelocity.X, (int)mVelocity.Y, "receivers", 0) != -1
               || Functions.checkObjectCollision(mBoundingBox, (int)mVelocity.X, (int)mVelocity.Y, "mirrors", 0) != -1
               || Functions.checkObjectCollision(mBoundingBox, (int)mVelocity.X, (int)mVelocity.Y, "ammos", 0) != -1
               || Functions.checkObjectCollision(mBoundingBox, (int)mVelocity.X, (int)mVelocity.Y, "bombWalls", 0) != -1
               || Functions.checkObjectCollision(mBoundingBox, (int)mVelocity.X, (int)mVelocity.Y, "plasmaLancers", 0) != -1
               || Functions.checkObjectCollision(mBoundingBox, (int)mVelocity.X, (int)mVelocity.Y, "togglers", 0) != -1
               || Functions.checkObjectCollision(mBoundingBox, (int)mVelocity.X, (int)mVelocity.Y, "furnaces", 0) != -1))
                mVelocity = new Vector2(0, 0);

            //Pick up and drop rifle
            if (Keyboard.GetState().IsKeyDown(Keys.E) && alignedX && alignedY && !firing 
                && !Nanozin.transitioning && Nanozin.currentScreenTimer > lastEquipped + .5f)
            {
                if (mTexture == Nanozin.salvagerRifleTexture)
                {
                    mTexture = Nanozin.salvagerTexture;
                    Nanozin.theRifle.mPosition = mPosition;
                    Nanozin.theRifle.mRotation = mRotation;
                    lastEquipped = Nanozin.currentScreenTimer;

                    if (!Nanozin.muted)
                        Nanozin.soundDrop.Play();
                }
                else if (mTexture == Nanozin.salvagerTexture && mBoundingBox.Intersects(Nanozin.theRifle.mBoundingBox))
                {
                    mTexture = Nanozin.salvagerRifleTexture;
                    Nanozin.theRifle.mPosition = new Vector2(-64, -64);
                    lastEquipped = Nanozin.currentScreenTimer;

                    if (!Nanozin.muted)
                        Nanozin.soundPickUp.Play();
                }
            }

            //Firing
            if (Nanozin.currentScreenTimer >= lastFired + firingTime)
            {
                //Waits until player is aligned with grid
                if (firing && alignedX && alignedY)
                {
                    Nanozin.plasmas.Add(new Plasma(mPosition, 3, "Salvager", 0));
                    mAmmo--;
                    firing = false;

                    if (!Nanozin.muted)
                        Nanozin.soundPlasmaFire.Play();
                }
            }

            //Initiate firing of rifle
            if (Keyboard.GetState().IsKeyDown(Keys.Space)
                && ((alignedX && alignedY) || mVelocity != new Vector2(0, 0))
                && !firing && mTexture == Nanozin.salvagerRifleTexture
                && Nanozin.currentScreenTimer > lastFired + rifleCD
                && !Nanozin.transitioning)
            {
                if (mAmmo > 0)
                {
                    firing = true;
                    lastFired = Nanozin.currentScreenTimer;

                    if (!Nanozin.muted)
                        Nanozin.soundPlasmaCharge.Play();

                    for (int i = 0; i < 200; i++)
                    {
                        Functions.addTemplateParticle(Nanozin.chargeP, mPosition);
                    }
                }
                else
                {
                    //Can't fire particles
                    lastFired = Nanozin.currentScreenTimer - (rifleCD * .6f);

                    if (!Nanozin.muted)
                        Nanozin.soundNoAmmo.Play();

                    for (int i = 0; i < 9; i++)
                    {
                        Functions.addTemplateParticle(Nanozin.trailP, mPosition);
                        Nanozin.particles[Nanozin.particles.Count - 1].mDepth = .83f;
                    }

                }
            }

            //Set rotation and directionTried on ice regardless of alignment
            if (onIce && mVelocity != new Vector2(0, 0) && !firing && !Nanozin.transitioning)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    targetRotation = (float)Math.PI * 1.5f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    targetRotation = (float)Math.PI * .5f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    targetRotation = (float)Math.PI * 1f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    targetRotation = 0f;
                }
            }

            if (sinking && mSourceRectangle.Width > 0)
            {
                mSourceRectangle.Width -= 1;
                mAlpha -= .015f;
                mScale -= .002f;

                if (!screamed && mSourceRectangle.Width < 40 && !Nanozin.muted)
                {
                    screamed = true;
                    Nanozin.soundScream.Play();
                }

                for (int i = 0; i < (Nanozin.rand.Next() % 2) + 1; i++)
                    Functions.addTemplateParticle(Nanozin.oozeP, Functions.randInRadius(new Vector2(Nanozin.wastes[Functions.checkObjectCollision(mBoundingBox, 0, 0, "wastes", 0)].mPosition.X+32, Nanozin.wastes[Functions.checkObjectCollision(mBoundingBox, 0, 0, "wastes", 0)].mPosition.Y+32), 24));

                if (mSourceRectangle.Width == 0)
                {
                    Nanozin.transitioning = true;
                    Nanozin.timeTransitionStarted = Nanozin.currentScreenTimer;

                    for (int i = 0; i < Nanozin.particlesPerDeath; i++)
                    {
                        Functions.addTemplateParticle(Nanozin.deathP, mPosition);
                        Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.X = (((float)Nanozin.rand.Next() % 300f) - 150f) / 100f;
                        Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.Y = (((float)Nanozin.rand.Next() % 300f) - 150f) / 100f;
                    }
                }

                //Stop velocity from ice
                if (mVelocity.Y != 0 && alignedY)
                    mVelocity.Y = 0;
                if (mVelocity.X != 0 && alignedX)
                    mVelocity.X = 0;
            }

            if (!sinking)
            {
                if (Functions.checkObjectCollision(mBoundingBox, 0, 0, "wastes", 0) != -1)
                {
                    sinking = true;
                }
            }

            //Move
            if (!firing)
            {
                movedVertical = false;
                movedHorizontal = false;

                if (Keyboard.GetState().IsKeyDown(Keys.W) && alignedX && !Nanozin.transitioning)
                {
                    targetRotation = (float)Math.PI * 1.5f;
                    if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        directionTried = 3;

                    if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift) && mPosition.Y - 32 >= moveSpeed && mVelocity == new Vector2(0, 0)
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)-moveSpeed * 2, "walls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)-moveSpeed * 2, "hardWalls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)-moveSpeed * 2, "crackedWalls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)-moveSpeed * 2, "wallExplosions", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)-moveSpeed * 2, "nodes", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)-moveSpeed * 2, "receivers", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)-moveSpeed * 2, "mirrors", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)-moveSpeed * 2, "ammos", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)-moveSpeed * 2, "bombWalls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)-moveSpeed * 2, "plasmaLancers", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)-moveSpeed * 2, "togglers", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)-moveSpeed * 2, "furnaces", 0) == -1
                        && (Functions.checkObjectCollision(mBoundingBox, 0, (int)-moveSpeed * 2, "wastes", 0) == -1 || onIce))
                    {
                        mPosition.Y -= moveSpeed;
                        movedUpLast = true;
                        movedVertical = true;

                        //Form velocity on ice
                        if (onIce)
                            mVelocity = new Vector2(0, (int)-moveSpeed);
                        //Splash particles in water
                        else if (Functions.checkObjectCollision(mBoundingBox, 0, 0, "icePatches", 0) != -1)
                        {
                            for (int i = 0; i < (Nanozin.rand.Next() % 10) + 2; i++)
                            {
                                Functions.addTemplateParticle(Nanozin.splashP, Functions.randInRadius(new Vector2(mPosition.X, mPosition.Y), 16));
                                Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.Y -= (Nanozin.rand.Next() % 60f) / 10f;
                            }
                        }
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.S) && alignedX && !Nanozin.transitioning)
                {
                    targetRotation = (float)Math.PI / 2;
                    if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        directionTried = 1;

                    if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift) && mPosition.Y + 32 <= Nanozin.levelHeight - moveSpeed && mVelocity == new Vector2(0, 0)
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)moveSpeed * 2, "walls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)moveSpeed * 2, "hardWalls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)moveSpeed * 2, "crackedWalls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)moveSpeed * 2, "wallExplosions", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)moveSpeed * 2, "nodes", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)moveSpeed * 2, "receivers", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)moveSpeed * 2, "mirrors", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)moveSpeed * 2, "ammos", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)moveSpeed * 2, "bombWalls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)moveSpeed * 2, "plasmaLancers", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)moveSpeed * 2, "togglers", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, 0, (int)moveSpeed * 2, "furnaces", 0) == -1
                        && (Functions.checkObjectCollision(mBoundingBox, 0, (int)moveSpeed * 2, "wastes", 0) == -1 || onIce))
                    {
                        mPosition.Y += moveSpeed;
                        movedUpLast = false;
                        movedVertical = true;

                        //Form velocity on ice
                        if (onIce)
                            mVelocity = new Vector2(0, (int)moveSpeed);
                        //Splash particles in water
                        else if (Functions.checkObjectCollision(mBoundingBox, 0, 0, "icePatches", 0) != -1)
                        {
                            for (int i = 0; i < (Nanozin.rand.Next() % 10) + 2; i++)
                            {
                                Functions.addTemplateParticle(Nanozin.splashP, Functions.randInRadius(new Vector2(mPosition.X, mPosition.Y), 16));
                                Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.Y += (Nanozin.rand.Next() % 60f) / 10f;
                            }
                        }
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A) && alignedY && !Nanozin.transitioning)
                {
                    targetRotation = (float)Math.PI;
                    if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        directionTried = 2;

                    if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift) && mPosition.X - 32 >= moveSpeed && mVelocity == new Vector2(0, 0)
                        && Functions.checkObjectCollision(mBoundingBox, (int)-moveSpeed * 2, 0, "walls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)-moveSpeed * 2, 0, "hardWalls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)-moveSpeed * 2, 0, "crackedWalls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)-moveSpeed * 2, 0, "wallExplosions", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)-moveSpeed * 2, 0, "nodes", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)-moveSpeed * 2, 0, "receivers", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)-moveSpeed * 2, 0, "mirrors", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)-moveSpeed * 2, 0, "ammos", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)-moveSpeed * 2, 0, "bombWalls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)-moveSpeed * 2, 0, "plasmaLancers", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)-moveSpeed * 2, 0, "togglers", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)-moveSpeed * 2, 0, "furnaces", 0) == -1
                        && (Functions.checkObjectCollision(mBoundingBox, (int)-moveSpeed * 2, 0, "wastes", 0) == -1 || onIce))
                    {
                        mPosition.X -= moveSpeed;
                        movedRightLast = false;
                        movedHorizontal = true;

                        //Form velocity on ice
                        if (onIce)
                            mVelocity = new Vector2((int)-moveSpeed, 0);
                        //Splash particles in water
                        else if (Functions.checkObjectCollision(mBoundingBox, 0, 0, "icePatches", 0) != -1)
                        {
                            for (int i = 0; i < (Nanozin.rand.Next() % 10) + 2; i++)
                            {
                                Functions.addTemplateParticle(Nanozin.splashP, Functions.randInRadius(new Vector2(mPosition.X, mPosition.Y), 16));
                                Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.X -= (Nanozin.rand.Next() % 60f) / 10f;
                            }
                        }
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D) && alignedY && !Nanozin.transitioning)
                {
                    targetRotation = 0f;
                    if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        directionTried = 0;

                    if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift) && mPosition.X + 32 <= Nanozin.levelWidth - moveSpeed && mVelocity == new Vector2(0, 0)
                        && Functions.checkObjectCollision(mBoundingBox, (int)moveSpeed * 2, 0, "walls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)moveSpeed * 2, 0, "hardWalls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)moveSpeed * 2, 0, "crackedWalls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)moveSpeed * 2, 0, "wallExplosions", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)moveSpeed * 2, 0, "nodes", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)moveSpeed * 2, 0, "receivers", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)moveSpeed * 2, 0, "mirrors", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)moveSpeed * 2, 0, "ammos", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)moveSpeed * 2, 0, "bombWalls", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)moveSpeed * 2, 0, "plasmaLancers", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)moveSpeed * 2, 0, "togglers", 0) == -1
                        && Functions.checkObjectCollision(mBoundingBox, (int)moveSpeed * 2, 0, "furnaces", 0) == -1
                        && (Functions.checkObjectCollision(mBoundingBox, (int)moveSpeed * 2, 0, "wastes", 0) == -1 || onIce))
                    {
                        mPosition.X += moveSpeed;
                        movedRightLast = true;
                        movedHorizontal = true;

                        //Form velocity on ice
                        if (onIce)
                            mVelocity = new Vector2((int)moveSpeed, 0);
                        //Splash particles in water
                        else if (Functions.checkObjectCollision(mBoundingBox, 0, 0, "icePatches", 0) != -1)
                        {
                            for (int i = 0; i < (Nanozin.rand.Next() % 10) + 2; i++)
                            {
                                Functions.addTemplateParticle(Nanozin.splashP, Functions.randInRadius(new Vector2(mPosition.X, mPosition.Y), 16));
                                Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.X += (Nanozin.rand.Next() % 60f) / 10f;
                            }
                        }
                    }
                }

                //Check alignment
                if ((mPosition.X - 32) % Nanozin.SPRITE_LENGTH == 0)
                    alignedX = true;
                if ((mPosition.Y - 32) % Nanozin.SPRITE_LENGTH == 0)
                    alignedY = true;

                //Check for ammo without collision
                if (alignedX && alignedY)
                {
                    index = -1;

                    switch (directionTried)
                    {
                        case 0:
                            index = Functions.checkObjectCollision(mBoundingBox, (int)moveSpeed, 0, "ammos", 0);
                            break;
                        case 1:
                            index = Functions.checkObjectCollision(mBoundingBox, 0, (int)moveSpeed, "ammos", 0);
                            break;
                        case 2:
                            index = Functions.checkObjectCollision(mBoundingBox, -(int)moveSpeed, 0, "ammos", 0);
                            break;
                        case 3:
                            index = Functions.checkObjectCollision(mBoundingBox, 0, -(int)moveSpeed, "ammos", 0);
                            break;
                    }

                    if (index != -1)
                    {
                        if (Nanozin.ammos[index].mSourceRectangle == new Rectangle(0, 0, 64, 64))
                        {
                            Nanozin.ammos[index].mSourceRectangle = new Rectangle(64, 0, 64, 64);
                            mAmmo += Nanozin.ammos[index].supply;

                            if (!Nanozin.muted)
                                Nanozin.soundAmmo.Play();

                            for (int i = 0; i < 30; i++)
                            {
                                Functions.addTemplateParticle(Nanozin.trailP, Functions.randInRadius(new Vector2(Nanozin.ammos[index].mPosition.X+32, Nanozin.ammos[index].mPosition.Y+32), 16));
                            }

                            Nanozin.floaters.Add(new Floater(new Vector2(Nanozin.ammos[index].mPosition.X + 8, Nanozin.ammos[index].mPosition.Y - 8), "+" + Nanozin.ammos[index].supply.ToString(), Nanozin.fontFloater, new Color(0, 255, 255), 80f, 2f));
                        }
                    }
                }

                //Continue to align to grid
                bool aligning = false;
                if (mVelocity == new Vector2(0, 0))
                {
                    if (!movedVertical && !alignedY)
                    {
                        if (movedUpLast)
                        {
                            mPosition.Y -= moveSpeed;
                            aligning = true;
                            //Form velocity on ice
                            if (onIce)
                                mVelocity = new Vector2(0, (int)-moveSpeed);
                        }
                        else
                        {
                            mPosition.Y += moveSpeed;
                            aligning = true;
                            //Form velocity on ice
                            if (onIce)
                                mVelocity = new Vector2(0, (int)moveSpeed);
                        }
                    }
                    else if (!movedHorizontal && !alignedX)
                    {
                        if (movedRightLast)
                        {
                            mPosition.X += moveSpeed;
                            aligning = true;
                            //Form velocity on ice
                            if (onIce)
                                mVelocity = new Vector2((int)moveSpeed, 0);
                        }
                        else
                        {
                            mPosition.X -= moveSpeed;
                            aligning = true;
                            //Form velocity on ice
                            if (onIce)
                                mVelocity = new Vector2((int)-moveSpeed, 0);
                        }
                    }
                }

                //Animate
                if (!firing && (movedVertical || movedHorizontal || aligning || (mVelocity != new Vector2(0, 0) && (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.D)))))
                {
                    if (Nanozin.currentScreenTimer > lastAnimated + .09f)
                    {
                        lastAnimated = Nanozin.currentScreenTimer;
                        mSourceRectangle.Y += 64;
                        if (mSourceRectangle.Y >= 512)
                            mSourceRectangle.Y = 64;
                    }

                    if (!onIce && Nanozin.currentScreenTimer >= lastFootStep + .35f && !Nanozin.muted)
                    {
                        lastFootStep = Nanozin.currentScreenTimer;

                        if (Functions.checkObjectCollision(mBoundingBox, 0, 0, "icePatches", 0) == -1)
                        {
                            switch (Nanozin.rand.Next() % 3)
                            {
                                case 0:
                                    Nanozin.soundFootstep2.Play();
                                    break;
                                case 1:
                                    Nanozin.soundFootstep3.Play();
                                    break;
                                case 2:
                                    Nanozin.soundFootstep4.Play();
                                    break;
                            }
                        }
                        else
                        {
                            Nanozin.soundSplash.Play();
                        }
                    }
                }
                else
                {
                    mSourceRectangle.Y = 0;
                }

                //If aligning into water, make splash particles
                if (aligning || mVelocity != new Vector2(0, 0))
                {
                    int iceIndex = 0;
                    bool foundWater = false;

                    for (int i = 0; i < Nanozin.icePatches.Count && !foundWater; i++)
                    {
                        iceIndex = Functions.checkObjectCollision(mSmallerBox, 0, 0, "icePatches", i);
                        if (iceIndex != -1 && Nanozin.icePatches[iceIndex].heatLevel >= Nanozin.icePatches[iceIndex].maxHeat * .9f)
                        {
                            foundWater = true;

                            //Splash particles
                            for (int j = 0; j < (Nanozin.rand.Next() % 10) + 2; j++)
                            {
                                Functions.addTemplateParticle(Nanozin.splashP, Functions.randInRadius(new Vector2(mPosition.X, mPosition.Y), 16));

                                //assign direction
                                if (movedHorizontal)
                                {
                                    if (movedRightLast)
                                    {
                                        Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.X += (Nanozin.rand.Next() % 60f) / 10f;
                                    }
                                    else
                                    {
                                        Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.X -= (Nanozin.rand.Next() % 60f) / 10f;
                                    }
                                }
                                else if (movedVertical)
                                {
                                    if (movedUpLast)
                                    {
                                        Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.Y -= (Nanozin.rand.Next() % 60f) / 10f;
                                    }
                                    else
                                    {
                                        Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.Y += (Nanozin.rand.Next() % 60f) / 10f;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //if firing:
                mSourceRectangle.Y = 0;
            }

            //Rotate towards target Rotation
            if (mRotation != targetRotation)
            {
                float turnSpeed = ((float)Math.PI / 20f);

                //Fix for right-facing jitter
                if (targetRotation == 0f && (mRotation < ((float)Math.PI * .1f) || mRotation > ((float)Math.PI * 1.90f)))
                    mRotation = 0f;

                if ((mRotation - targetRotation + ((float)Math.PI * 2f)) % ((float)Math.PI * 2f) >= (float)Math.PI)
                {
                    mRotation += turnSpeed;

                    if (((mRotation + ((float)Math.PI * 2f)) % ((float)Math.PI * 2f)) > ((targetRotation + ((float)Math.PI * 2f)) % ((float)Math.PI * 2f)) && !(targetRotation == 0f))
                        mRotation = targetRotation;
                }
                else
                {
                    mRotation -= turnSpeed;

                    if ((((mRotation + ((float)Math.PI * 2f)) % ((float)Math.PI * 2f)) < ((targetRotation + ((float)Math.PI * 2f)) % ((float)Math.PI * 2f))) && !(targetRotation == ((float)Math.PI * 1.5f) && mRotation < ((float)Math.PI * .5f) && mRotation > 0f))
                        mRotation = targetRotation;

                    //Fix for right-facing rotation
                    if (targetRotation == 0f && mRotation > -.158f && mRotation < -.156f)
                        mRotation = 0f;
                }
            }

            //Check for collision with plasmas
            index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "plasmas", 0);
            if (!Nanozin.transitioning && index != -1 && Nanozin.currentScreenTimer > Nanozin.plasmas[index].timeCreated + .04f)
            {
                dead = true;
                Nanozin.plasmas[index].collided = true;
                Nanozin.transitioning = true;
                Nanozin.timeTransitionStarted = Nanozin.currentScreenTimer;

                for (int i = 0; i < Nanozin.particlesPerDeath; i++)
                {
                    Functions.addTemplateParticle(Nanozin.deathP, mPosition);
                    float speedX = Nanozin.plasmas[Nanozin.plasmas.Count - 1].mVelocity.X,
                          speedY = Nanozin.plasmas[Nanozin.plasmas.Count - 1].mVelocity.Y;
                    int   pDirection;

                    if (speedX > 0)
                        pDirection = 0;
                    else if (speedY > 0)
                        pDirection = 1;
                    else if (speedX < 0)
                        pDirection = 2;
                    else
                        pDirection = 3;

                    switch (pDirection)
                    {
                        case 0:
                            speedX = (((float)Nanozin.rand.Next() % 100f) + 50f) / 100f;
                            speedY = (((float)Nanozin.rand.Next() % 120f) - 60f) / 100f;
                            break;
                        case 1:
                            speedY = (((float)Nanozin.rand.Next() % 100f) + 50f) / 100f;
                            speedX = (((float)Nanozin.rand.Next() % 120f) - 60f) / 100f;
                            break;
                        case 2:
                            speedX = (((float)Nanozin.rand.Next() % 100f) + 50f) / -100f;
                            speedY = (((float)Nanozin.rand.Next() % 120f) - 60f) / 100f;
                            break;
                        case 3:
                            speedY = (((float)Nanozin.rand.Next() % 100f) + 50f) / -100f;
                            speedX = (((float)Nanozin.rand.Next() % 120f) - 60f) / 100f;
                            break;
                    }

                    Nanozin.particles[Nanozin.particles.Count - 1].mVelocity = new Vector2(speedX, speedY);
                }
            }

            //Check for fuel cells
            index = Functions.checkObjectCollision(mBoundingBox, 0, 0, "fuelCells", 0);
            if (index != -1)
            {
                Nanozin.fuelCells[index].taken = true;
                Nanozin.numFuelCells--;

                if (!Nanozin.muted)
                    Nanozin.soundFuelGet.Play();

                //Fuel Particles
                for(int i = 0; i < 40; i++)
                {
                    Functions.addTemplateParticle(Nanozin.fuelP, Nanozin.fuelCells[index].mPosition);
                    Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.X = 20f - (40f * (float)i / 39f);
                    Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.Y = -20f + Math.Abs(Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.X);
                }
                for (int i = 0; i < 40; i++)
                {
                    Functions.addTemplateParticle(Nanozin.fuelP, Nanozin.fuelCells[index].mPosition);
                    Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.X = 20f - (40f * (float)i / 39f);
                    Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.Y = 20f - Math.Abs(Nanozin.particles[Nanozin.particles.Count - 1].mVelocity.X);
                }

                if (Nanozin.numFuelCells <= 0)
                {
                    //Win Level
                    Nanozin.beatLevel = true;
                    Nanozin.transitioning = true;
                    Nanozin.timeTransitionStarted = Nanozin.currentScreenTimer;
                }
            }
        }

        public new void draw(SpriteBatch sb)
        {
            Vector2 drawLocation = mPosition - (Nanozin.cameraPosition - Nanozin.SCREEN_MID);

            sb.Draw(mTexture,
                    drawLocation,
                    mSourceRectangle,
                    mTint * mAlpha,
                    mRotation,
                    mOrigin,
                    mScale,
                    SpriteEffects.None,
                    mDepth);
        }
    }
}