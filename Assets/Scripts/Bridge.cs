using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Bridge : MonoBehaviour
{
    public GameObject part;
    public Material[] partMaterials;
    public int countOfParts = 10;
    public float distanceBetweenParts = 1f, startHeight = 0.5f;
    public bool createNewBridge = false;

    private int lastChildIndex, tempMaterialIndex = 0;

    void Start()
    {
        CreateBridge();
    }

    void Update()
    {
        if (createNewBridge)
        {
            createNewBridge = false;
            CreateBridge();
        }
    }

    private void CreateBridge()
    {
        //Bridge needs 3 parts at least
        if (countOfParts < 3)
            countOfParts = 3;

        lastChildIndex = countOfParts - 1;
        tempMaterialIndex = 0;

        //Deleting parts
        for (int i = transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(transform.GetChild(i).gameObject);

        //Spawning and spacing the parts
        for (int i = 0; i < countOfParts; i++)
        {
            GameObject tempPart = Instantiate(part, transform);
            tempPart.transform.localPosition = new Vector2(i * distanceBetweenParts, 0f);
            tempPart.GetComponent<MeshRenderer>().material = partMaterials[tempMaterialIndex];
            if (tempMaterialIndex + 1 >= partMaterials.Length)
                tempMaterialIndex = 0;
            else
                tempMaterialIndex++;
        }

        //Setting up the middle parts
        for (int i = 1; i < lastChildIndex; i++)
        {
            if (transform.GetChild(i).gameObject.GetComponent<DistanceJoint2D>() != null)
                continue;

            transform.GetChild(i).gameObject.AddComponent<DistanceJoint2D>().connectedBody = transform.GetChild(i - 1).gameObject.GetComponent<Rigidbody2D>();
            transform.GetChild(i).gameObject.AddComponent<DistanceJoint2D>().connectedBody = transform.GetChild(i + 1).gameObject.GetComponent<Rigidbody2D>();
            transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        }

        //Setting up the first and last part
        transform.GetChild(0).localPosition = new Vector2(transform.GetChild(0).localPosition.x, startHeight);
        transform.GetChild(lastChildIndex).localPosition = new Vector2(transform.GetChild(lastChildIndex).localPosition.x, startHeight);
    }
}
