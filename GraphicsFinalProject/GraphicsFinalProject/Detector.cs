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
    public class Detector : Sprite
    {
        public Detector(Vector2 position)
        {
            mTexture = Nanozin.detectorTexture;
            mPosition = new Vector2(position.X + 32, position.Y + 32);
            mOrigin = new Vector2(32, 32);
            mBoundingBox = new Rectangle((int)mPosition.X-32, (int)mPosition.Y-32, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSourceRectangle = new Rectangle(0, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mRotation = 0f;
            mScale = .8f;
            mDepth = .5f;
            mTint = Color.White;

            powered = false;
            lastPoweredTime = 0f;
            durationPowered = .5f;
            lastPulseId = -2;
        }
        ~Detector() { }

        public int lastPulseId;
        public bool powered;
        public float lastPoweredTime,
                     durationPowered;

        public void update()
        {
            if (powered)
            {
                if (Nanozin.currentScreenTimer < lastPoweredTime + .12f)
                    mSourceRectangle.X = 64;
                else if (Nanozin.currentScreenTimer < lastPoweredTime + .25f)
                    mSourceRectangle.X = 128;
                else if (Nanozin.currentScreenTimer < lastPoweredTime + .38f)
                    mSourceRectangle.X = 192;
                else
                    mSourceRectangle.X = 256;

                //Unpower after duration
                if (Nanozin.currentScreenTimer > lastPoweredTime + durationPowered)
                {
                    powered = false;
                    mSourceRectangle.X = 0;
                }
            }
        }
    };
}
