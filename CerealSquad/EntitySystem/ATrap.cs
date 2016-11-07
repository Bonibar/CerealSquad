using CerealSquad.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    abstract class ATrap : AEntity
    {
        protected int _range;

        public int Range
        {
            get
            {
                return _range;
            }

            set
            {
                _range = value;
            }
        }

        public ATrap(IEntity owner, e_DamageType damage, int range = 1) : base(owner)
        {
            _damageType = damage;
            if (owner.getEntityType() == e_EntityType.Player || owner.getEntityType() == e_EntityType.PlayerTrap)
                _type = e_EntityType.PlayerTrap;
            else
                _type = e_EntityType.EnnemyTrap;
            _range = range;
        }

        public void setPosition(EMovement direction)
        {
            s_position pos = new s_position();

            if (direction == EMovement.Up)
                pos = new s_position(_owner.Pos._x, _owner.Pos._y - 1);
            else if (direction == EMovement.Down)
                pos = new s_position(_owner.Pos._x, _owner.Pos._y + 1);
            else if(direction == EMovement.Right)
                pos = new s_position(_owner.Pos._x + 1, _owner.Pos._y);
            else if(direction == EMovement.Left)
                pos = new s_position(_owner.Pos._x - 1, _owner.Pos._y);

            Pos = pos;
            ressourcesEntity.Position = new SFML.System.Vector2f(Pos._x * 64, Pos._y * 64);
        }

        public override void update(SFML.System.Time deltaTime, AWorld world)
        {
            ressourcesEntity.Update(deltaTime);
            _children.ToList<IEntity>().ForEach(i => i.update(deltaTime, world));
        }
    }
}
