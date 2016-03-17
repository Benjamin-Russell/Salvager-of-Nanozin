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
    public class PressurePlate : Sprite
    {
        public PressurePlate(Vector2 position)
        {
            mTexture = Nanozin.pressurePlateTexture;
            mPosition = position;
            mOrigin = new Vector2(0, 0);
            mBoundingBox = new Rectangle((int)mPosition.X, (int)mPosition.Y, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mSourceRectangle = new Rectangle(0, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mRotation = 0f;
            mScale = 1f;
            mDepth = .5f;
            mTint = Color.White;
        }
        ~PressurePlate() { }

        public bool powering;

        public void update()
        {
            mSourceRectangle = new Rectangle(0, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            mTint = Color.White;

            if (Nanozin.theSalvager != null && ((mBoundingBox.Intersects(Nanozin.theSalvager.mBoundingBox) && !Nanozin.theSalvager.dead) || mBoundingBox.Intersects(Nanozin.theRifle.mBoundingBox)))
            {
                if (!powering && !Nanozin.muted)
                    Nanozin.soundSwitchOn.Play();

                powering = true;
                mSourceRectangle = new Rectangle(64, 0, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
            }
            else
            {
                if (powering && !Nanozin.muted)
                    Nanozin.soundSwitchOff.Play();

                powering = false;
            }
        }
    };
}
