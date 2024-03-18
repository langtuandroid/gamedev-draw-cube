using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Managers
{
    public class DCColorManager : MonoBehaviour
    {
        [FormerlySerializedAs("objectMaterials")] [SerializeField] private Material[] _objectMaterials;
        [FormerlySerializedAs("backgroundMaterial")] [SerializeField] private Material _backgroundMaterial;
        [FormerlySerializedAs("backgroundColors")] [SerializeField] private Color[] _backgroundColors;
        [FormerlySerializedAs("objectColors")] [SerializeField] private Color[] _objectColors;
        [FormerlySerializedAs("progressBarImages")] [SerializeField] private Image[] _progressBarImages;

        private List<Color[]> _objectNewColors;
        private int _nextColorIndex;

        void Start()
        {
            if (PlayerPrefs.GetInt("Colorized" + PlayerPrefs.GetInt("Level", 0), 0) == 0)
            {
                PlayerPrefs.SetInt("Colorized" + PlayerPrefs.GetInt("Level", 0), 1);
                ChangeMaterials();
                ChangeEnvironmentColors();
            }
            else ChangeEnvironmentColors();
        }

        private void ChangeEnvironmentColors()
        {
            _backgroundMaterial.color = _backgroundColors[PlayerPrefs.GetInt("BackgroundColor" + PlayerPrefs.GetInt("Level", 0), 0)];

            int randomIndex = Random.Range(0, _objectMaterials.Length);
            foreach (var img in _progressBarImages) img.color = _objectMaterials[randomIndex].color;
        }

        private void ChangeMaterials()
        {
            PlayerPrefs.SetInt("BackgroundColor" + PlayerPrefs.GetInt("Level", 0), Random.Range(0, _backgroundColors.Length));

            _objectNewColors = new List<Color[]>();

            for (int i = 0; i < _objectColors.Length / 2; i++)
            {
                _objectNewColors.Add(new Color[2]);

                for (int j = 0; j < 2; j++)
                {
                    _objectNewColors[i][j] = _objectColors[_nextColorIndex];
                    _nextColorIndex++;
                }
            }

            int randomIndex = Random.Range(0, _objectNewColors.Count);
            for (int i = 0; i < _objectMaterials.Length; i++) _objectMaterials[i].color = _objectNewColors[randomIndex][i];
        }
    }
}
