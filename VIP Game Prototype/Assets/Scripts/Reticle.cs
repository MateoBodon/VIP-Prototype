using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VIP
{
    public class Reticle : MonoBehaviour
    {
        #region Variables
        [SerializeField] float restingSize = 75f;
        [SerializeField] float maxSize = 250f;
        [SerializeField] float rateOfChange = 2f;

         float currentSize;

        RectTransform reticle;
        #endregion

        #region BuiltInMethods
       
        private void Start()
        {
            reticle = GetComponent<RectTransform>();
        }

        private void Update()
        {
            //determines if the player is moving and changes the size by a fixed rate
            if (IsMoving)
            {
                currentSize = Mathf.Lerp(currentSize, maxSize, rateOfChange * Time.deltaTime);
            }
            else
            {
                currentSize = Mathf.Lerp(currentSize, restingSize, rateOfChange * Time.deltaTime);
            }

            //sets the size of the reticle
            reticle.sizeDelta = new Vector2(currentSize, currentSize);           
        }
        #endregion

        #region CustomMethods

        //determines if there is any movement
        bool IsMoving
        {
            get
            {
                if (
                    Input.GetAxis("Horizontal") != 0 ||
                    Input.GetAxis("Vertical") != 0 
                    //Input.GetAxis("Mouse X") != 0 ||
                    //Input.GetAxis("Mouse Y") != 0
                    )
                    return true;
                else
                    return false;
            }
        }
        #endregion
    }
}