using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// David, I saw some NaNs while moving so I brought in my vector utility stuff
namespace Utility
{
    /// <summary>
    /// Utility class for Vectors.
    /// </summary>
    public static class Vectors
    {
        public static Vector3 Identity = new Vector3(0f, 0f, 0f);

        /// <summary>
        /// Returns true if any of the Vector axes is NaN.
        /// </summary>
        /// <param name="vector">Vector3 to check for NaNs</param>
        /// <returns>bool</returns>
        public static bool HasNans(Vector3 vector)
        {
            return float.IsNaN(vector.x) || float.IsNaN(vector.y) || float.IsNaN(vector.z);
        }
        /// <summary>
        /// Returns true if any of the Vector axes is NaN.
        /// </summary>
        /// <param name="vector">Vector2 to check for NaNs</param>
        /// <returns>bool</returns>
        public static bool HasNans(Vector2 vector)
        {
            return float.IsNaN(vector.x) || float.IsNaN(vector.y);
        }
    }
}
