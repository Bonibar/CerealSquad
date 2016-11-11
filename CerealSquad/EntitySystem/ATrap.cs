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
        public e_TrapType TrapType { get; protected set; }
        public int Range { get; protected set; }

        public ATrap(IEntity owner, e_DamageType damage, int range = 1) : base(owner)
        {
            _damageType = damage;
            if (owner.getEntityType() == e_EntityType.Player || owner.getEntityType() == e_EntityType.PlayerTrap)
                _type = e_EntityType.PlayerTrap;
            else
                _type = e_EntityType.EnnemyTrap;
            Range = range;
        }

        public void setPosition(EMovement direction)
        {
            var pos = _owner.ressourcesEntity.Position;
            var size = _owner.ressourcesEntity.sprite.Size;

            if (direction == EMovement.Up)
                ressourcesEntity.Position = new SFML.System.Vector2f(pos.X, pos.Y - size.Y);
            else if (direction == EMovement.Down)
                ressourcesEntity.Position = new SFML.System.Vector2f(pos.X, pos.Y + size.Y);
            else if (direction == EMovement.Right)
                ressourcesEntity.Position = new SFML.System.Vector2f(pos.X + size.X, pos.Y);
            else if (direction == EMovement.Left)
                ressourcesEntity.Position = new SFML.System.Vector2f(pos.X - size.X, pos.Y);
            else
                ressourcesEntity.Position = new SFML.System.Vector2f(pos.X, pos.Y);
        }

        public override void update(SFML.System.Time deltaTime, AWorld world)
        {
            ressourcesEntity.Update(deltaTime);
            _children.ToList<IEntity>().ForEach(i => i.update(deltaTime, world));
        }
    }
}
