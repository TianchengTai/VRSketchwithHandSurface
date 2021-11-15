using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRPainting{
    public class CombineSurfaces : MonoBehaviour
    {
        public List<GameObject> surfaces;
        void Start()
        {
            
        }

        void Update()
        {
        
        }

        public void myTest(){
            if(surfaces.Count>=2){
                Combine(surfaces[0],surfaces[1]); 
            }
        }

        public void Combine(GameObject surface1,GameObject surface2){
            GameObject newSurface = new GameObject("combine_surface");
            DrawSurface.RemoveInteractionComponent(surface1);
            DrawSurface.RemoveInteractionComponent(surface2);
            DrawSurface.AddInteractionComponent(newSurface);
            newSurface.AddComponent<MeshRenderer>();
            newSurface.transform.SetParent(GameObject.Find("Draw Surface").transform);
            surface1.transform.SetParent(newSurface.transform);
            surface2.transform.SetParent(newSurface.transform);
        }
    }
}
