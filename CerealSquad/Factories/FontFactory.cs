using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Factories.FontFactory
{
    /// <summary>
    /// (THREAD SAFE) Manage all Fonts you need
    /// </summary>
    public sealed class FontFactory
    {
        #region Singleton
        private FontFactory() {}

        public static FontFactory Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested() {}

            internal static readonly FontFactory instance = new FontFactory();
        }
        #endregion

        public enum Font
        {
            XirodRegular,
            ReenieBeanie
        }

        private Object _Locker = new Object();

        private Dictionary<Font, SFML.Graphics.Font> _Fonts = null;

        /// <summary>
        /// Load all the needed fonts in memory
        /// </summary>
        private void initFonts()
        {
            _Fonts = new Dictionary<Font, SFML.Graphics.Font>();
            if (_Fonts == null)
                throw new OutOfMemoryException("Impossible de charger les polices");
            _Fonts.Add(Font.XirodRegular, new SFML.Graphics.Font("Fonts/xirod.regular.ttf"));
            _Fonts.Add(Font.ReenieBeanie, new SFML.Graphics.Font("Fonts/ReenieBeanie.ttf"));
        }

        /// <summary>
        /// Get the needed font from memory
        /// </summary>
        /// <param name="font">Font you wish to get</param>
        /// <returns>SFML.Graphics.Font or null</returns>
        public SFML.Graphics.Font getFont(Font font)
        {
            SFML.Graphics.Font result = null;

            lock (_Locker)
            {
                if (_Fonts == null)
                    initFonts();
                if (_Fonts.ContainsKey(font))
                {
                    result = _Fonts[font];
                }
            }

            return result;
        }
    }
}
