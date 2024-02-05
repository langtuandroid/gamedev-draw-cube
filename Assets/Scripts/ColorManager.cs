using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    //COLORING THE LEVELS ARE AUTOMATICALLY DONE WITH THIS SCRIPT

    public Material[] objectMaterials;
    public Material backgroundMaterial;
    public Color[] backgroundColors;
    public Color[] objectColors;
    public Image[] progressBarImages;

    private List<Color[]> objectColorArrays;
    private int nextColorIndex = 0;

    void Start()
    {
        if (PlayerPrefs.GetInt("Colorized" + PlayerPrefs.GetInt("Level", 0).ToString(), 0) == 0)      //If this is the first attempt on this level
        {
            PlayerPrefs.SetInt("Colorized" + PlayerPrefs.GetInt("Level", 0).ToString(), 1);
            ChangeMaterials();
            ChangeColors();
        }
        else
            ChangeColors();
    }

    private void ChangeColors()
    {
        //Changes background color
        backgroundMaterial.color = backgroundColors[PlayerPrefs.GetInt("BackgroundColor" + PlayerPrefs.GetInt("Level", 0).ToString(), 0)];

        //Changes progress bar colors
        int randomIndex = UnityEngine.Random.Range(0, objectMaterials.Length);
        for (int i = 0; i < progressBarImages.Length; i++)
            progressBarImages[i].color = objectMaterials[randomIndex].color;
    }

    private void ChangeMaterials()
    {
        PlayerPrefs.SetInt("BackgroundColor" + PlayerPrefs.GetInt("Level", 0).ToString(), UnityEngine.Random.Range(0, backgroundColors.Length));

        #region Creating List for Colors

        objectColorArrays = new List<Color[]>();

        for (int i = 0; i < objectColors.Length / 2; i++)
        {
            objectColorArrays.Add(new Color[2]);

            for (int j = 0; j < 2; j++)
            {
                objectColorArrays[i][j] = objectColors[nextColorIndex];
                nextColorIndex++;
            }
        }

        #endregion

        //Changes objectMaterial colors
        int randomIndex = UnityEngine.Random.Range(0, objectColorArrays.Count);
        for (int i = 0; i < objectMaterials.Length; i++)
            objectMaterials[i].color = objectColorArrays[randomIndex][i];
    }
}
