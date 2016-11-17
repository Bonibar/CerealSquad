using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using static CerealSquad.APlayer;
using CerealSquad.Graphics;
using CerealSquad.GameWorld;
using CerealSquad.Factories;
using SFML.Graphics;

namespace CerealSquad
{
    class JackEnnemy : AEnemy
    {
        protected static JackEnnemyScentMap _scentMap;

        protected class JackEnnemyScentMap : scentMap
        {
            public JackEnnemyScentMap(uint x, uint y) : base(x, y)
            {
            }

            public override int getScent(int x, int y)
            {
                if (x >= 0 && x < _x && y >= 0 && y < _y)
                {
                    int scent = 0;
                    if (_map[x][y][(int)EName.Tchong] != -1)
                        scent += _map[x][y][(int)EName.Tchong];
                    return (scent);
                }
                return (-1);
            }
        }

        public JackEnnemy(IEntity owner, s_position position, ARoom room) : base(owner, position, room)
        {
            _speed = 3;
            _scentMap = new JackEnnemyScentMap(_room.Size.Height, _room.Size.Width);
            _ressources = new EntityResources();
            Factories.TextureFactory.Instance.load("jackHunter", "Assets/Character/jackHunter.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));
            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.IDLE, "jackHunter", new List<uint> { 0 }, new Vector2u(64, 64));
            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.WALKING_DOWN, "jackHunter", new List<uint> { 0, 1, 2 }, new Vector2u(64, 64));
            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.WALKING_LEFT, "jackHunter", new List<uint> { 3, 4, 5 }, new Vector2u(64, 64));
            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.WALKING_RIGHT, "jackHunter", new List<uint> { 6, 7, 8 }, new Vector2u(64, 64));
            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.WALKING_UP, "jackHunter", new List<uint> { 9, 10, 11 }, new Vector2u(64, 64));

            Vector2f pos = _ressources.Position;
            pos.X = position._x * 64;
            pos.Y = position._y * 64;
            _ressources.Position = pos;
            _ressources.CollisionBox = new FloatRect(new Vector2f(12.0f, -20.0f), new Vector2f(12.0f, 27.0f));
        }

        //
        // TODO check egality in scent
        //
        public override void think()
        {
            s_position pos = getCoord(_pos);

            int left = _scentMap.getScent(pos._x - 1, pos._y);
            int right = _scentMap.getScent(pos._x + 1, pos._y);
            int top = _scentMap.getScent(pos._x, pos._y - 1);
            int bottom = _scentMap.getScent(pos._x, pos._y + 1);
            int here = _scentMap.getScent(pos._x, pos._y);
            int maxscent = Math.Max(here, Math.Max(top, Math.Max(bottom, Math.Max(right, left))));

            if (maxscent == 0)
                _move = EMovement.None;
            else if (maxscent == top)
                _move = EMovement.Up;
            else if (maxscent == bottom)
                _move = EMovement.Down;
            else if (maxscent == right)
                _move = EMovement.Right;
            else if (maxscent == left)
                _move = EMovement.Left;
            else if (maxscent == here)
                moveSameTile((WorldEntity)_owner);
        }

        public override void update(Time deltaTime, AWorld world)
        {
            _scentMap.update((WorldEntity)_owner, _room);
            think();
            _ressources.Update(deltaTime);
            move(world, deltaTime);
        }
    }
}
