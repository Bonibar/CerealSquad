using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Menus
{
    public class MenuManager
    {
        #region Singleton
        private MenuManager() { }

        public static MenuManager Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested() { }

            internal static readonly MenuManager instance = new MenuManager();
        }
        #endregion

        private List<AMenu> Menus = new List<AMenu>();

        public void AddMenu(AMenu menu)
        {
            Menus.Add(menu);
        }
        public void RemoveMenu(AMenu menu)
        {
            Menus.Remove(menu);
        }
        public void removeMenu()
        {
            Menus.Clear();
        }

        /// <summary>
        /// Returns the current displayed menu or null
        /// </summary>
        /// <returns>Current displayed menu or null</returns>
        public AMenu CurrentMenu { get { return Menus.Count > 0 ? Menus[Menus.Count - 1] : null; } }

        /// <summary>
        /// Is a menu currently displayed?
        /// </summary>
        public bool isDisplayed() { return Menus.Count > 0; }
    }
}
