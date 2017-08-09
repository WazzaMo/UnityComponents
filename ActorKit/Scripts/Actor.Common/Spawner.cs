/*
 * Spawner Unity Component
 * (c) Copyright 2017, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor.Common {

    public class Spawner : MonoBehaviour {
        [SerializeField]
        private GameObject CharacterPrefabToSpawn;

        private Transform _SpawnTransform;

        void Awake() {
            SetupAttachedGameObjectAsSpawnPoint();
        }

        public GameObject SpawnCharacter() {
            GameObject CharacterInWorld = MakeMainCharacter();
            Transform CharacterTransform = CharacterInWorld.GetComponent<Transform>();
            UpdateCharacterPosition(CharacterTransform);

            return CharacterInWorld;
        }

        private void SetupAttachedGameObjectAsSpawnPoint() {
            _SpawnTransform = GetComponent<Transform>();
        }

        private GameObject MakeMainCharacter() {
            return GameObject.Instantiate(CharacterPrefabToSpawn, _SpawnTransform.position, _SpawnTransform.rotation);
        }

        private void UpdateCharacterPosition(Transform characterTransform) {
            if (characterTransform != null) {
                characterTransform.position = _SpawnTransform.position;
                characterTransform.rotation = _SpawnTransform.rotation;
            }
        }
    }

}