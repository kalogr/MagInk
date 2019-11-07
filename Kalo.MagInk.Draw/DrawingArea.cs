using System;

namespace Kalo.MagInk.Draw
{
    public class DrawingArea
    {
        #region Constants

        public static readonly int ROTATE_0 = 0;
        public static readonly int ROTATE_90 = 1;
        public static readonly int ROTATE_180 = 2;
        public static readonly int ROTATE_270 = 3;

        #endregion

        #region Properties

        /** Electronic paper display width. */
        public int EpdWidth { get; set; }

        /** Electronic paper display height. */
        public int EpdHeight { get; set; }

        /** Current width for display. */
        public int CurrentWidth { get; set; }

        /** Current height for display. */
        public int CurrentHeight { get; set; }

        /** Current rotate for display. */
        public int CurrentRotate { get; set; }

        #endregion
    }
}