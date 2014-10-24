namespace Helpers {

    using System.Collections;

    using UnityEngine;

    public static class MathHelper {

        public static int RoundToWhole(float a) {
            return (int)(a + 0.5f);
        }
    }
}