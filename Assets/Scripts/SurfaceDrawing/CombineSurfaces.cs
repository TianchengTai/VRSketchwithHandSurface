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
            newSurface.transform.position = (surface1.transform.position + surface2.transform.position) / 2;
            DrawSurface.RemoveInteractionComponent(surface1);
            DrawSurface.RemoveInteractionComponent(surface2);
            DrawSurface.AddInteractionComponent(newSurface);            
            newSurface.transform.SetParent(DrawSurface.DrawingBoard.transform);
            surface1.transform.SetParent(newSurface.transform);
            surface2.transform.SetParent(newSurface.transform);
        }
    }
}
