using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VIP
{
    public class Gun : MonoBehaviour
    {
        #region Variables
        //controls the range of the projectile
        [SerializeField] float range = 50;
        #endregion

        #region BuiltInMethods
        private void Update()
        {
            //input for the weapon
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        #endregion

        #region CustomMethods
        void Shoot()
        {
            RaycastHit hit;
            //sends out a raycast from the camera position and will print out the name of any object it hits
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);
            }
        }
        #endregion
    }
}
