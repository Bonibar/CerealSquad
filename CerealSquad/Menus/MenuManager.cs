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

        private List<Menu> Menus = new List<Menu>();

        public void AddMenu(Menu menu)
        {
            if (Menus.Count > 0)
                Menus[Menus.Count - 1].Hide();
            Menus.Add(menu);
            menu.Initialize();
            menu.Show();
        }
        public void RemoveMenu(Menu menu)
        {
            Menus.Remove(menu);
            if (Menus.Count > 0)
                Menus[Menus.Count - 1].Show();
        }

        /// <summary>
        /// Returns the current displayed menu or null
        /// </summary>
        /// <returns>Current displayed menu or null</returns>
        public Menu CurrentMenu { get { return Menus.Count > 0 ? Menus[Menus.Count - 1] : null; } }

        /// <summary>
        /// Is a menu currently displayed?
        /// </summary>
        public bool isDisplayed() { return Menus.Count > 0; }
    }
}
