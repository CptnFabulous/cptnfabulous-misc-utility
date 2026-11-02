using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public static class MaterialUtility
    {
        public static void SetBool(this Material material, string propertyName, bool value)
        {
            material.SetInt(propertyName, value ? 1 : 0);
        }
    }
}
