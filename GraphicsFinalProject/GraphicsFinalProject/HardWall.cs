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
    public class HardWall : Wall
    {
        public HardWall(Vector2 position)
            : base(position)
        {
            mSourceRectangle = new Rectangle(Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH, Nanozin.SPRITE_LENGTH);
        }
        ~HardWall() { }
    };
}
