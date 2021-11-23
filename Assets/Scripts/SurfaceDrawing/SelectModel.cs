using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRPainting
{
    public class SelectModel : MonoBehaviour
    {
        public static List<GameObject> SurfaceContainer;
        public GameObject Target;

        public string planeName;

        public bool transparent = false;

        Material mt; //????

        public float SELECTED = 0.6f;  // ??��??????

        public float MIDDLE = 0.2f;    // ???????????????

        public float HIDDEN = 0.2f;    // ��??��??????

        string HiddenLayer = "hidden_surface";

        string SelectedLayer = "selected_surface";

        private void Start()
        {   
            SurfaceContainer = new List<GameObject>();
            Physics.queriesHitBackfaces = true;
            //initMaterials();
        }


        public void SetTarget(GameObject hitObject)
        {
            Target = hitObject;
            planeName = hitObject.name;
            ClearAllLayer();
            //hitObject.layer = LayerMask.NameToLayer(SelectedLayer);
        }

        public static void AddSurface(GameObject surface){
            //nitMaterial(surface);
            SurfaceContainer.Add(surface);
        }

        public void HighlightGameObject(GameObject go)
        {

            foreach(MeshRenderer mesh in go.GetComponentsInChildren<MeshRenderer>()){
                Material mt =mesh.material;
                mt.color = new Color(1, 1, 1, Target == go ? SELECTED + MIDDLE : HIDDEN + MIDDLE);
            }
            
        }

        public void ClearAllLayer()
        {
            foreach (GameObject surface in SurfaceContainer)
            {
                if(surface.activeSelf){
                    surface.layer = LayerMask.NameToLayer(HiddenLayer);
                }
            }
            Debug.Log(SurfaceContainer.Count);
        }


        public void ClearSelect()
        {
            Target = null;
            ClearAllLayer();
            SetMaterial();
        }

        public void SetMaterial()
        {
            foreach (GameObject surface in SurfaceContainer)
            {
                if(!surface.activeSelf){
                    continue;
                }
                // if (child.gameObject.name=="Default")
                // {
                //     continue;
                // }
                Material mat = surface.GetComponent<MeshRenderer>().material;
                if (transparent) {
                    mat.color = new Color(1, 1, 1, 0);
                    continue;
                }
                if (surface.layer == LayerMask.NameToLayer(HiddenLayer))
                {
                    mat.color = new Color(1, 1, 1, HIDDEN);
                }
            }
        }

        void initMaterial(GameObject surface){
            Material mat = new Material(Shader.Find("Standard"));
            SetMaterialRenderingMode(mat, RenderingMode.Fade);
            mat.color = new Color(1, 1, 1, HIDDEN);
            surface.GetComponent<MeshRenderer>().material = mat;
            surface.layer = LayerMask.NameToLayer(HiddenLayer);
        }

        // void initMaterials()
        // {
        //     foreach (Transform child in GameObject.Find("Draw Surface").transform)
        //     {
        //         if (child.gameObject.name != "Default")
        //         {
        //             Material mat = new Material(Shader.Find("Standard"));
        //             SetMaterialRenderingMode(mat, RenderingMode.Fade);
        //             mat.color = new Color(1, 1, 1, HIDDEN);
        //             child.gameObject.GetComponent<MeshRenderer>().material = mat;
        //             child.gameObject.layer = LayerMask.NameToLayer(HiddenLayer);
        //         }
        //     }
        // }

        public static void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
        {
            switch (renderingMode)
            {
                case RenderingMode.Opaque:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = -1;
                    break;
                case RenderingMode.Cutout:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.EnableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 2450;
                    break;
                case RenderingMode.Fade:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    break;
                case RenderingMode.Transparent:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    break;
            }
        }

    }

    
}


