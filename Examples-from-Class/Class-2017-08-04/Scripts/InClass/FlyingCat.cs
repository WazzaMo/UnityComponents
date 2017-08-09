/*
 * Written in class on 2017-08-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InClass {

    public class FlyingCat : MonoBehaviour {
        [SerializeField]
        private Color ColorToUse;

        private MeshRenderer _MeshRenderer;

        // Use this for initialization
        void Start() {
            _MeshRenderer = GetComponent<MeshRenderer>();
            
        }

        // Update is called once per frame
        void Update() {
            _MeshRenderer.material.color = ColorToUse;
        }
    }

}