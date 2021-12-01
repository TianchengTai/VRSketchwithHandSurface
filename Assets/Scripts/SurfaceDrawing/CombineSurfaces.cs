using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRPainting{
    public class CombineSurfaces : MonoBehaviour
    {
        public List<GameObject> ToCombineSurfaces;
        void Start()
        {
            
        }

        void Update()
        {
        
        }

        public void myTest(){
            if(ToCombineSurfaces.Count==2){
                Combine(ToCombineSurfaces[0],ToCombineSurfaces[1]); 
            }
            else if(ToCombineSurfaces.Count>2){
                CombineAll(ToCombineSurfaces);
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

        public void CombineAll(List<GameObject> surfaces){
            GameObject newSurface = new GameObject("combine_surface");
            int count = 0;
            Vector3 newSurfacePosition = new Vector3(0,0,0);
            foreach(GameObject surface in surfaces){
                if(surface==null) continue;
                DrawSurface.RemoveInteractionComponent(surface);
                newSurfacePosition+=surface.transform.position;
                count++;
            }
            newSurface.transform.position = newSurfacePosition / count;
            newSurface.transform.SetParent(DrawSurface.DrawingBoard.transform);
            DrawSurface.AddInteractionComponent(newSurface); 
            foreach(GameObject surface in surfaces){
                surface.transform.SetParent(newSurface.transform);
            }  
        }

    }
}
