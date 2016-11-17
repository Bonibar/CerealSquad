using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using CerealSquad.Graphics;
using CerealSquad.GameWorld;
using CerealSquad.Factories;
/*
namespace CerealSquad
{
    class Ennemy : AEnemy
    {
        protected static scentMap _scentMap;

        public Ennemy(IEntity owner, s_position position, ARoom room) : base(owner, position, room)
        {
            _speed = 0.1;
            _scentMap = new scentMap(_room.Size.Height, _room.Size.Width);
            _ressources = new EntityResources();
            Factories.TextureFactory.Instance.load("basicEnnemy", "Assets/Character/basicEnnemy.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));
            Vector2f pos = _ressources.Position;
            pos.X = position._x * 64;
            pos.Y = position._y * 64;
            _ressources.Position = pos;
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
            int maxScent = Math.Max(top, Math.Max(bottom, Math.Max(right, left)));

            if (maxScent == 0)
                _move = EMovement.None;
            else if (maxScent == top)
            {
                _move = EMovement.Up;
                if (Math.Max(bottom, Math.Max(right, left)) == maxScent)
                    check_egality(left, right, top, bottom, maxScent);
            }
            else if (maxScent == bottom)
            {
                _move = EMovement.Down;
                if (Math.Max(top, Math.Max(right, left)) == maxScent)
                    check_egality(left, right, top, bottom, maxScent);
            }
            else if (maxScent == right)
            {
                _move = EMovement.Right;
                if (Math.Max(top, Math.Max(bottom, left)) == maxScent)
                    check_egality(left, right, top, bottom, maxScent);
            }
            else
            {
                _move = EMovement.Left;
                if (Math.Max(top, Math.Max(bottom, right)) == maxScent)
                    check_egality(left, right, top, bottom, maxScent);
            }
        }

        private void check_egality(int left, int right, int top, int bottom, int maxScent)
        {
            int maxCharacterScent = 0;
            s_position pos = getCoord(_pos);

            if (left == maxScent)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (_scentMap.Map[pos._x - 1][pos._y][i] > maxCharacterScent)
                    {
                        _move = EMovement.Left;
                        maxCharacterScent = _scentMap.Map[pos._x - 1][pos._y][i];
                    }
                }
            }

            if (right == maxScent)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (_scentMap.Map[pos._x + 1][pos._y][i] > maxCharacterScent)
                    {
                        _move = EMovement.Right;
                        maxCharacterScent = _scentMap.Map[pos._x + 1][pos._y][i];
                    }
                }
            }

            if (top == maxScent)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (_scentMap.Map[pos._x][pos._y - 1][i] > maxCharacterScent)
                    {
                        _move = EMovement.Up;
                        maxCharacterScent = _scentMap.Map[pos._x][pos._y - 1][i];
                    }
                }
            }

            if (bottom == maxScent)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (_scentMap.Map[pos._x][pos._y + 1][i] > maxCharacterScent)
                    {
                        _move = EMovement.Down;
                        maxCharacterScent = _scentMap.Map[pos._x][pos._y + 1][i];
                    }
                }
            }
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
*/