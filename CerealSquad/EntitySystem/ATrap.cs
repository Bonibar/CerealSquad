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
        public float Range { get; protected set; }
        public bool Triggered { get; protected set; }

        public abstract void Trigger();

        public ATrap(IEntity owner, e_DamageType damage, float range = 0) : base(owner)
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
            var size = _owner.ressourcesEntity.Size;

            var TruePosition = new SFML.System.Vector2f();
            if (direction == EMovement.Up)
                TruePosition = new SFML.System.Vector2f(pos.X, pos.Y - size.Y);
            else if (direction == EMovement.Down)
                TruePosition = new SFML.System.Vector2f(pos.X, pos.Y + size.Y);
            else if (direction == EMovement.Right)
                TruePosition = new SFML.System.Vector2f(pos.X + size.X, pos.Y);
            else if (direction == EMovement.Left)
                TruePosition = new SFML.System.Vector2f(pos.X - size.X, pos.Y);

            // Subtracte half size because origin not good
            TruePosition = new SFML.System.Vector2f(TruePosition.X - ressourcesEntity.Size.X / 2.0f, TruePosition.Y - ressourcesEntity.Size.Y / 2.0f);
            // Divide by 64.0f to get the real size grid
            TruePosition /= 64.0f;

            Pos = new s_position(TruePosition.X, TruePosition.Y);
        }

        public override void update(SFML.System.Time deltaTime, AWorld world)
        {
            ressourcesEntity.Update(deltaTime);
            _children.ToList<IEntity>().ForEach(i => i.update(deltaTime, world));
        }
    }
}
