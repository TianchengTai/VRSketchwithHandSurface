using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VRPainting{
    public class SaveScript : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject saveObj;
        int id = 1;

        public int load_id;

        public void Save(){
            bool res;
            PrefabUtility.SaveAsPrefabAsset(saveObj,"Assets/save_data/"+saveObj.name+id+".prefab",out res);
            id++;
            Debug.Log(res);
        }

        public void Load(int prefab_id){
            GameObject g = PrefabUtility.LoadPrefabContents("Assets/save_data/Draw Surface"+prefab_id+".prefab");
            Instantiate(g);
            foreach(MeshRenderer mesh in g.GetComponentsInChildren<MeshRenderer>()){
                GameObject sur = mesh.gameObject;
                SelectModel.SurfaceContainer.Add(sur);
            }
        }
    }

}
