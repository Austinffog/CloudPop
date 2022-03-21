using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.cloudpop
{

    [CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
    public class Gun : ScriptableObject
    {
        public string weaponName;
        public float fireRate, bloom;
        public GameObject prefab;
        public int maxAmmo, clipSize;
        public float reload;

        private int currentAmmo, currentClip;

        public void Initialize()
        {
            currentAmmo = maxAmmo;
            currentClip = clipSize;
        }

        public bool FireBullet()
        {
            if (currentClip > 0)
            {
                currentClip -= 1;
                return true;
            }
            else 
                return false;

        }

        public void Reload()
        {
            currentAmmo += currentClip;
            currentClip = Mathf.Min(clipSize, currentAmmo);
            currentAmmo -= currentClip;
        }

        public int GetAmmo()
        {
            return maxAmmo;
        }

        public int GetClip()
        {
            return currentClip;
        }
    }
}
