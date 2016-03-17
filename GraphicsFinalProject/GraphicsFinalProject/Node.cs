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
    public class Node : Sprite
    {
        public Node()
        {
            mTexture = Nanozin.wallsTexture;
            mOrigin = new Vector2(0, 0);
            mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mRotation = 0f;
            mScale = 1f;
            mDepth = .5f;
            mTint = Color.White;
            powered = false;
            lastPoweredTime = -20f;
            durationPowered = .25f;
            unpoweredCD = .5f;
            lastPulseId = -2;
        }
        public Node(Vector2 position)
        {
            mTexture = Nanozin.wallsTexture;
            mPosition = position;
            mOrigin = new Vector2(0, 0);
            mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mRotation = 0f;
            mScale = 1f;
            mDepth = .5f;
            mTint = Color.White;
            powered = false;
            lastPoweredTime = -10f;
            durationPowered = .25f;
            unpoweredCD = .5f;
            lastPulseId = -2;
        }
        ~Node() { }

        public int lastPulseId;
        public bool powered;
        public float lastPoweredTime,
          durationPowered,
          unpoweredCD;

        public void update()
        {
            if (powered)
            {
                //Unpower after duration
                if (Nanozin.currentScreenTimer > lastPoweredTime + durationPowered)
                {
                    powered = false;
                }
            }

            if (!powered)
            {
                //Set to unpowered state
                if (mSourceRectangle.X == Nanozin.SPRITE_LENGTH * 2 && mSourceRectangle.Y == 0)
                    mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
                else if (mSourceRectangle.X == Nanozin.SPRITE_LENGTH * 3 && mSourceRectangle.Y == 0)
                    mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH * 3, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);

                //If off cooldown, look for power source
                if (Nanozin.currentScreenTimer > lastPoweredTime + unpoweredCD)
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
    };
}