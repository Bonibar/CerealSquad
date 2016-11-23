using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.TrapEntities
{
    class CoffeePool : ATrap
    {
        public CoffeePool(IEntity owner) : base(owner, e_DamageType.COFFE_DAMAGE, 0)
        {
            Factories.TextureFactory.Instance.load("CoffeeStaying", "Assets/Enemies/Boss/CoffeeStaying.png");
            Factories.TextureFactory.Instance.load("CoffeeSpreading", "Assets/Enemies/Boss/CoffeeSpreading.png");
            Factories.TextureFactory.Instance.load("CoffeeThrowed", "Assets/Enemies/Boss/CoffeeThrowed.png");
        }
    }
}
