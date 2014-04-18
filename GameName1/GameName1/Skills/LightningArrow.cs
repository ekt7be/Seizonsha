using GameName1.Effects;
using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class LightningArrow : Skill, Unlockable
    {

        private int damage;
        private List<GameEntity> hit;

        Rectangle? arrowSource;

        private float elapsed;
        private float delay = 60f;
        private int currentFrame = 0;
        private static readonly int arrowFrames = 8;
        private int recharge_time;


        public LightningArrow(Seizonsha game, GameEntity user, int damage, int recharge_time)
            : base(game, user, Static.LIGHTNING_ARROW_COST, Static.LIGHTNING_ARROW_RECHARGE, 30, 30)
        {
            this.damage = damage;
            hit = new List<GameEntity>();
        }



        public override string getDescription()
        {
            return "Pierce Enemies with an Arrow of Lightning";
        }

        public override string getName()
        {
            return Static.LIGHTNING_ARROW_NAME;
        }

        public override void affect(GameEntity affected)
        {
           if(game.ShouldDamage(this.damageType, affected.getTargetType()) && !this.hit.Contains(affected)) {
               this.hit.Add(affected);
               LightningEnchant le = new LightningEnchant(game, user, 25, 0);
               le.affect(affected);
               //game.damageEntity(user, affected, this.damage, this.damageType);
            }
        }

        public void UpdateAnimation(GameTime gameTime, Texture2D sprite)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed > delay)
            {
                if (currentFrame >= arrowFrames - 1)
                {
                    currentFrame = 0;
                }

                else
                {
                    currentFrame++;
                }

                elapsed = 0;
            }


            arrowSource = new Rectangle(256 * currentFrame, 0 * 64, 64, 64);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle hitbox, float pDirection)
        {
            spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_LIGHTNING_ARROW], hitbox, arrowSource, Color.White, pDirection, new Vector2(0, 0), SpriteEffects.None, 1f);
        }

        protected override void UseSkill()
        {
            game.lightningArrowSound.Play(1f, 1f, 1f);
            this.hit = new List<GameEntity>();
            int width = 50;
            int length = 125;
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX()), (int)(user.getCenterY() - 5), length, width);
            List<PolygonIntersection.Vector> points = new List<PolygonIntersection.Vector>();
            float theta = bufferedDirection;
            //if(theta < (float)(2.0*Math.PI)) theta += 2.0f*(float)Math.PI;
            float ox = user.getCenterX();
            float oy = user.getCenterY() - (int)(width / 2);
            float px1 = user.getCenterX();
            float py1 = user.getCenterY();
            float px2 = user.getCenterX();
            float py2 = user.getCenterY() + width;
            float px3 = user.getCenterX() + length;
            float py3 = user.getCenterY() + width;
            float px4 = user.getCenterX() + length;
            float py4 = user.getCenterY();
            points.Add(new PolygonIntersection.Vector((int)(Math.Cos(theta) * (px1 - ox) - Math.Sin(theta) * (py1 - oy) + ox), (int)(Math.Sin(theta) * (px1 - ox) + Math.Cos(theta) * (py1 - oy) + oy)));
            points.Add(new PolygonIntersection.Vector((int)(Math.Cos(theta) * (px2 - ox) - Math.Sin(theta) * (py2 - oy) + ox), (int)(Math.Sin(theta) * (px2 - ox) + Math.Cos(theta) * (py2 - oy) + oy)));
            points.Add(new PolygonIntersection.Vector((int)(Math.Cos(theta) * (px3 - ox) - Math.Sin(theta) * (py3 - oy) + ox), (int)(Math.Sin(theta) * (px3 - ox) + Math.Cos(theta) * (py3 - oy) + oy)));
            points.Add(new PolygonIntersection.Vector((int)(Math.Cos(theta) * (px4 - ox) - Math.Sin(theta) * (py4 - oy) + ox), (int)(Math.Sin(theta) * (px4 - ox) + Math.Cos(theta) * (py4 - oy) + oy)));
            /* p'x = cos(theta) * (px-ox) - sin(theta) * (py-oy) + ox
             p'y = sin(theta) * (px-ox) + cos(theta) * (py-oy) + oy
             */

            PolygonIntersection.Polygon polygon = new PolygonIntersection.Polygon(points);
            //game.Spawn(new SwordSlash(game, user, Static.PIXEL_THIN, slashBounds, damage, damageType, 10, user.vectorDirection), slashBounds.Left, slashBounds.Top);
            Arrow attack = new Arrow(game, user, null, this, slashBounds, polygon, damage, damageType, 50, new Vector2(bufferedVectorDirection.X * 25, bufferedVectorDirection.Y * 25), bufferedDirection, true);
            attack.rotateToAngle(this.bufferedDirection);

            game.Spawn(attack, slashBounds.Left, slashBounds.Top);
        }


        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }
    }
}
