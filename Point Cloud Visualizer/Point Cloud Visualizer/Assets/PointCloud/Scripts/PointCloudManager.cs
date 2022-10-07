using System.Collections;
using System.IO;
using UnityEngine;

namespace Assets.PointCloud.Scripts
{
    public class PointCloudManager : MonoBehaviour
    {

        /// <summary>
        /// The name of the LAS files we wish to create point clouds from
        /// </summary>
        public string dataPath;

        /// <summary>
        /// The scale of the point clouds
        /// </summary>
        public float scale = 1;

        public Material matVertex;

        /// <summary>
        /// Unity's XYZ has Y up and Z as depth. Most conventional 3D structures have this inverted, so this is to account for that
        /// </summary>
        public bool invertYAndZ = false;

        /// <summary>
        /// The number of maximum points each "sub-mesh" contains
        /// </summary>
        public int limitPoints = 65000;

        private Mesh[] meshes;

        public void Start()
        {
            PointCloudModel.invertYAndZ = invertYAndZ;
            PointCloudModel.scale = scale;
            PointCloudModel.limitPoints = limitPoints;

            foreach(string file in Directory.GetFiles(dataPath))
            {
                StartCoroutine(DoTheThing(file));
            }
            
        }

        IEnumerator DoTheThing(string file)
        {
            meshes = PointCloudModel.CreateClouds(file);
            GameObject g = new GameObject(Path.GetFileName(file));
            for (int i = 0; i < meshes.Length; i++)
            {
                GameObject pointGroup = new GameObject(g.name + "/" + i);
                pointGroup.AddComponent<MeshFilter>();
                pointGroup.AddComponent<MeshRenderer>();
                pointGroup.GetComponent<Renderer>().material = matVertex;

                pointGroup.GetComponent<MeshFilter>().mesh = meshes[i];
                pointGroup.transform.parent = g.transform;
            }
            yield return null;
        }
    }
}
