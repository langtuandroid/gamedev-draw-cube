using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerSkins", menuName = "ScriptableObjects/PlayerSkins", order = 1)]
    public class PlayerSkinsScriptableObject : ScriptableObject
    {
        [SerializeField] private List<Material> _materials;
        [SerializeField] private List<Mesh> _skinMeshes;

        public Mesh GetMeshByIndex(int index)
        {
            if (_skinMeshes.Count < index || index < 0) throw new Exception("There is no player skin with index " + index + "!");
            
            return _skinMeshes[index];
        }

        public Material GetMaterialByIndex(int index)
        {
            if (_materials.Count < index || index < 0) throw new Exception("There is no player skin color with index " + index + "!");
            
            return _materials[index];
        }
    }
}