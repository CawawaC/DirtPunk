using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowRoots : MonoBehaviour
{

    public List<MeshRenderer> growRootsMeshes;
    public float timeToGrow = 5;
    public float refreshRate = 0.05f;
    [Range(0,1)]
    public float minGrow = 0f;
    [Range(0, 1)]
    public float maxGrow = 0.97f;
    // How far the model is between minGrow and maxGrow determines how much of the model has grown


    private List<Material> growRootsMaterials = new List<Material>();
    private bool fullyGrown;

   
    bool RootGrowHasRun = false;
    


    // Start is called before the first frame update
    void Start()
    {

       

        for (int i=0; i<growRootsMeshes.Count; i++)
        {
            for (int j=0; j<growRootsMeshes[i].materials.Length; j++)
                if(growRootsMeshes[i].materials[j].HasProperty("Grow_"))
            {
                    growRootsMeshes[i].materials[j].SetFloat("Grow_", minGrow);
                    growRootsMaterials.Add(growRootsMeshes[i].materials[j]);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // This is where we can trigger the growing 
        if (Input.GetKeyDown(KeyCode.Space))
         
        {
            for (int i = 0; i < growRootsMaterials.Count; i++)
            {
                StartCoroutine(GrowRootsGrow(growRootsMaterials[i]));
                RootGrowHasRun = true;
            }
        }
        
    }

    IEnumerator GrowRootsGrow (Material mat)
    {
        float growValue = mat.GetFloat("Grow_");

        if (!fullyGrown)
        {
            while(growValue < maxGrow)
            {
                growValue += 1 / (timeToGrow / refreshRate);
                mat.SetFloat("Grow_", growValue);

                yield return new WaitForSeconds(refreshRate);
    
            }
        }
        else
        {
            while (growValue > minGrow)
            {
            //    growValue -= 1 / (timeToGrow / refreshRate);
           //     mat.SetFloat("Grow_", growValue);
        
            //    yield return new WaitForSeconds(refreshRate);
            }
        }

        if (growValue >= maxGrow)
            fullyGrown = true;
        else
            fullyGrown = false;
    }
}
 