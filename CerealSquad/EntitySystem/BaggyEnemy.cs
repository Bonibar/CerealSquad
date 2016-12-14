using CerealSquad.EntitySystem.Projectiles;
using CerealSquad.GameWorld;
using CerealSquad.Graphics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.EntitySystem
{
    class BaggyEnemy : AEnemy
    {
        private double _attackCoolDown;
        private int _hp;
        private double _invuln;
        private double _autoattack;
        private bool _takingDamage;
        private double _summonTime;
        private double _rushTime = 0;

        protected scentMap _scentMap;

        protected enum SBaggyState
        {
            IDLE = 0,
            WALKING_UP,
            WALKING_DOWN,
            WALKING_RIGHT,
            WALKING_LEFT,
            PHASE_ONE_TO_TWO,
            DYING,
            SUMMONING,
            SUMMONINGBOSS,
        }

        public BaggyEnemy(IEntity owner, s_position position, ARoom room) : base(owner, position, room)
        {
            _speed = 2;
            _attackCoolDown = 5; // 5sec
            _summonTime = 10 + _rand.Next() % 10;
            _hp = 3;
            _invuln = 0;
            _autoattack = 7;
            _scentMap = new scentMap(room.Size.Height, room.Size.Width);
            ressourcesEntity = new EntityResources();
            Factories.TextureFactory.Instance.load("BaggyHiding", "Assets/Enemies/Boss/BaggyHiding.png");
            Factories.TextureFactory.Instance.load("BaggyPhase1toPhase2", "Assets/Enemies/Boss/BaggyPhase1toPhase2.png");
            Factories.TextureFactory.Instance.load("BaggyPhase1Walking", "Assets/Enemies/Boss/BaggyPhase1Walking.png");
            Factories.TextureFactory.Instance.load("BaggyPhase2Walking", "Assets/Enemies/Boss/BaggyPhase2Walking.png");
            Factories.TextureFactory.Instance.load("BaggySummoning", "Assets/Enemies/Boss/BaggySummoning.png");
            Factories.TextureFactory.Instance.load("BaggyDying", "Assets/Enemies/Boss/BaggyDying.png");


            _ressources.InitializationAnimatedSprite(new Vector2u(128, 128));
            ChangingAnimationPhase(1);
            
            Pos = Pos; // very important
        }

        private void ChangingAnimationPhase(int phase)
        {
            if (phase == 1)
            {
                ressourcesEntity.AddAnimation((uint)SBaggyState.IDLE, "BaggyPhase1Walking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_DOWN, "BaggyPhase1Walking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_UP, "BaggyPhase1Walking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_RIGHT, "BaggyPhase1Walking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_LEFT, "BaggyPhase1Walking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.SUMMONING, "BaggySummoning", Enumerable.Range(0, 10).Select(i => (uint)i).ToList(), new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.SUMMONINGBOSS, "BaggySummoning", Enumerable.Range(0, 10).Select(i => (uint)i).ToList(), new Vector2u(128, 128));
            }
            else
            {
                _autoattack = 1;
                _attackCoolDown = 0.5;
                ressourcesEntity.AddAnimation((uint)SBaggyState.IDLE, "BaggyPhase2Walking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_DOWN, "BaggyPhase2Walking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_UP, "BaggyPhase2Walking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_RIGHT, "BaggyPhase2Walking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_LEFT, "BaggyPhase2Walking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.SUMMONING, "BaggySummoning", Enumerable.Range(11, 21).Select(i => (uint)i).ToList(), new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.SUMMONINGBOSS, "BaggySummoning", Enumerable.Range(11, 21).Select(i => (uint)i).ToList(), new Vector2u(128, 128));
            }
            ressourcesEntity.AddAnimation((uint)SBaggyState.PHASE_ONE_TO_TWO, "BaggyPhase1toPhase2", Enumerable.Range(0, 5).Select(i => (uint)i).ToList(), new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)SBaggyState.DYING, "BaggyDying", Enumerable.Range(0, 32).Select(i => (uint)i).ToList(), new Vector2u(128, 128));

        }

        public override bool IsCollidingEntity(AWorld World, List<AEntity> CollidingEntities)
        {
            bool baseResult = base.IsCollidingEntity(World, CollidingEntities);
            bool result = false;

            CollidingEntities.ForEach(i =>
            {
                if (i.getEntityType() == e_EntityType.PlayerTrap && ((ATrap)i).TrapType == e_TrapType.WALL)
                    result = true;
            });


            return result || baseResult;
        }

        public override void die()
        {
            if (!Die)
            {
                PlayAnimation((uint)SBaggyState.DYING);
                ressourcesEntity.Loop = false;
                base.die();
            }
        }

        public override void update(Time deltaTime, AWorld world)
        {
            if (_invuln > 0 && _children.Count == 0)
                _invuln -= deltaTime.AsSeconds();
            if (_attackCoolDown > 0)
                _attackCoolDown -= deltaTime.AsSeconds();
            if (_summonTime > 0)
                _summonTime -= deltaTime.AsSeconds();
            if (_rushTime > 0)
                _rushTime -= deltaTime.AsSeconds();
            if (_autoattack > 0)
                _autoattack -= deltaTime.AsSeconds();

            if (_rushTime < 0)
                _speed = 2;

            if (Die)
            {
                if (ressourcesEntity.Pause)
                    if (_children.Count == 0)
                        destroy();
            }
            else
            {
                if (ressourcesEntity.Animation == (uint)SBaggyState.SUMMONINGBOSS && ressourcesEntity.Pause)
                {
                    if (_hp == 1)
                        ChangingAnimationPhase(2);
                    else
                        PlayAnimation((uint)SBaggyState.IDLE);
                    _invuln = 1; //switching to unlimited invuln(10 sec lol) to 1 sec of invuln after the dead of his children
                }
                else if (ressourcesEntity.Animation == (uint)SBaggyState.DYING && ressourcesEntity.Pause)
                {
                    destroy();
                }
                else if (_takingDamage && ressourcesEntity.Animation != (uint)SBaggyState.PHASE_ONE_TO_TWO && _hp == 1)
                {
                    PlayAnimation((uint)SBaggyState.PHASE_ONE_TO_TWO);
                }
                else if (_takingDamage && ressourcesEntity.Pause)
                {
                    ressourcesEntity.Loop = true;
                    PlayAnimation((uint)SBaggyState.IDLE);
                    _takingDamage = false;
                }
                else if (!_takingDamage)
                {
                    if (Active)
                    {
                        _scentMap.update((WorldEntity)_owner.getOwner(), _room);
                        think(world, deltaTime);
                    }
                    move(world, deltaTime);
                }
            }
            _ressources.Update(deltaTime);
            _children.ToList().ForEach(i => i.update(deltaTime, world));
        }

        private bool detect_player(WorldEntity world)
        {
            double deltaX = 0;
            double deltaY = 0;
            bool result = false;
            bool end = false;
            s_position pos = getCoord(HitboxPos); switch (_move[0])
            {
                case EMovement.Down:
                    deltaY = 0.5;
                    break;
                case EMovement.Up:
                    deltaY = -0.5;
                    break;
                case EMovement.Right:
                    deltaX = 0.5;
                    break;
                case EMovement.Left:
                    deltaX = -0.5;
                    break;
            }
            world.GetAllEntities().ForEach(ent =>
            {
                if (ent.getEntityType() == e_EntityType.Player)
                {
                    for (int i = 0; i < 30; i += 1)
                    {
                        if (InCircleRange(HitboxPos.X + deltaX * i, HitboxPos.Y + deltaY * i, ent, 0.5f))
                            result = true;
                        if (shootDebug)
                        {
                            EllipseShapeSprite sprite = new EllipseShapeSprite(new Vector2f(0.5f * 64f, 0.5f * 64f), new Color(255, (byte)(10 * i), 0, 255), new Color(255, 0, 0, 0));
                            var posSprite = sprite.EllipseShape.Position;
                            posSprite.X = (float)(deltaX * i * 64);
                            posSprite.Y = (float)(deltaY * i * 64);
                            sprite.EllipseShape.Position = posSprite;
                            ressourcesEntity.secondarySprite.Add(sprite);
                        }
                        if (_room.getPosition((uint)(pos.X + deltaX * i), (uint)(pos.Y + deltaY * i)) == RoomParser.e_CellType.Wall
                        || _room.getPosition((uint)(pos.X + deltaX * i), (uint)(pos.Y + deltaY * i)) == RoomParser.e_CellType.Void)
                            end = true;
                        if (result || end)
                            break;
                    }
                }
            });
            return (result);


        }

        private void summon()
        {
            _summonTime = 10 + _rand.Next() % 10;
            PlayAnimation((uint)SBaggyState.SUMMONING);
            Console.WriteLine("Summon");
            AEnemy c = new GhostEnemy(_owner, new s_position(Pos.X - _room.Position.X, Pos.Y - _room.Position.Y), _room);
            c.Active = true;
        }

        private void summonBoss()
        {
            PlayAnimation((uint)SBaggyState.SUMMONINGBOSS);
            AEnemy c = new CoffeeMachineEnemy(this, new s_position(Pos.X - _room.Position.X, Pos.Y - _room.Position.Y), _room);
            c.Active = true;
        }

        private bool canAttack(WorldEntity world)
        {
            if (_hp > 1)
                return (false);

            if (_autoattack <= 0)
                return (true);

            if (_attackCoolDown > 0)
                return (false);

            return (detect_player(world));
        }

        private bool canRush(WorldEntity world)
        {
            if (_hp <= 1)
                return (false);

            if (_autoattack <= 0)
                return (true);

            if (_attackCoolDown > 0)
                return (false);

            return (detect_player(world));
        }

        protected void rush()
        {
            _speed = 10;
            _rushTime = 0.5;
            _attackCoolDown = 5.5f;
            _autoattack = 7.5;
        }

        protected void attack()
        {
            _attackCoolDown = 0.5f;
            _autoattack = 1f;
            s_position pos = Pos;
            pos.X += ressourcesEntity.Size.X / 128 / 2;
            pos.Y += ressourcesEntity.Size.Y / 128 / 2;
            EMovement move = _move[0];
            if (_move[0] == EMovement.None)
                move = EMovement.Down;
            new BaggyProjectile(_owner, move, pos);
        }

        protected bool canSummon()
        {
            return (_summonTime < 0);
        }

        public override void think(AWorld world, Time deltaTime)
        {
            if (canSummon())
                summon();
            if (canAttack((WorldEntity)_owner.getOwner()))
                attack();
            bool result = true;
            result &= executeUpMove(world, Speed * deltaTime.AsSeconds());
            result &= executeDownMove(world, Speed * deltaTime.AsSeconds());
            result &= executeLeftMove(world, Speed * deltaTime.AsSeconds());
            result &= executeRightMove(world, Speed * deltaTime.AsSeconds());
            if (_r > 0 && result)
                _r -= 1;
            else
            {
                _r = 0;
                s_position pos = getCoord(HitboxPos);
                var position = ressourcesEntity.Position;
                EMovement lastMove = _move[0];
                _move = new List<EMovement> { EMovement.Up, EMovement.Down, EMovement.Right, EMovement.Left };
                int left = executeLeftMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent((int)pos.X - 1, (int)pos.Y) : 0;
                ressourcesEntity.Position = position;
                int right = executeRightMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent((int)pos.X + 1, (int)pos.Y) : 0;
                ressourcesEntity.Position = position;
                int top = executeUpMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent((int)pos.X, (int)pos.Y - 1) : 0;
                ressourcesEntity.Position = position;
                int bottom = executeDownMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent((int)pos.X, (int)pos.Y + 1) : 0;
                ressourcesEntity.Position = position;
                int here = _scentMap.getScent((int)pos.X, (int)pos.Y);
                int maxscent = Math.Max(top, Math.Max(bottom, Math.Max(right, left)));
                _move = new List<EMovement> { EMovement.None };

                if (maxscent == 0 && here == 0)
                {
                    _move = new List<EMovement> { EMovement.Down, EMovement.Left, EMovement.Right, EMovement.Up };
                    _move = new List<EMovement> { _move[_rand.Next() % _move.Count] };
                    _r = 30;
                }
                else if (maxscent <= here && moveSameTile(world, (WorldEntity)getRootEntity(), deltaTime))
                {
                    if (_attackCoolDown <= 0 && _hp <= 1)
                        attack();
                }
                else
                {
                    if (maxscent == top)
                        _move.Add(EMovement.Up);
                    if (maxscent == bottom)
                        _move.Add(EMovement.Down);
                    if (maxscent == right)
                        _move.Add(EMovement.Right);
                    if (maxscent == left)
                        _move.Add(EMovement.Left);
                    _move.Remove(EMovement.None);
                    if (_move.Contains(lastMove))
                    {
                        _move = new List<EMovement> { lastMove };
                    }
                    else
                    {
                        _move = new List<EMovement> { _move[_rand.Next() % _move.Count] };
                        _r = _speed == 10 ? 1 : 10;
                    }
                }
                if (canRush((WorldEntity)_owner.getOwner()))
                    rush();
            }
        }

        public override bool attemptDamage(IEntity Sender, e_DamageType damage, bool isHitBox = false)
        {
            bool result = false;
            if (_invuln <= 0 && !_takingDamage)
                switch (damage)
                {
                    case e_DamageType.BOMB_DAMAGE:
                    case e_DamageType.TRUE_DAMAGE:
                        _hp -= 1;
                        if (_hp == 0)
                            die();
                        else
                            receiveDamage();
                        result = true;
                        break;
                }

            return result;
        }

        private void receiveDamage()
        {
            summonBoss();
            _invuln = 10;
            ressourcesEntity.Loop = false;
            _takingDamage = true;
        }
    }
}