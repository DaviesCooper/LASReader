using LAS;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.PointCloud.Scripts
{
    public class PointCloudGenerator
    {

        private string dataPath;
        private string filename;
        private int numPoints;
        private int numPointGroups;
        private Color temp_Color;
        private Color[] meshColors;
        private Vector3[] points;
        private Vector3 minValue;
        

        /// <summary>
        /// Constructor for the point cloud generator. If anything goes wrong, quietly exits, and nothing happens
        /// </summary>
        /// <param name="dPath">path to the LAS file</param>
        /// <param name="col">the color of this mesh (temporary)</param>
        /// <param name="toAdd">the dictionary to add this mesh to when generated</param>
        public PointCloudGenerator(string dPath, Color col)
        {
            dataPath = dPath;
            filename = Path.GetFileName(dataPath);
            temp_Color = col;
        }
        
        
        public bool LoadLAS(out Mesh[] Meshes)
        {
            Meshes = null;
            //Try to read the file, if it fails, simply return
            LASReader reader;
            try
            {
                reader = new LASReader(dataPath);
            }
            catch
            {   
                return false;
            }
            //Set-up init values
            numPoints = (int)reader.HeaderFile.Number_Of_Point_Records;
            meshColors = new Color[numPoints];
            points = new Vector3[numPoints];
            LASPoint p;
            //Read each las point and add it to point array
            while (reader.ReadPoint(out p))
            {
                if (!PointCloudModel.invertYAndZ)
                {
                    Vector3 pointLoc = new Vector3(p.X * PointCloudModel.scale, p.Y * PointCloudModel.scale, p.Z * PointCloudModel.scale) - PointCloudModel.Origin.Value;
                    points[reader.currentPointCount - 1] = pointLoc;
                }
                else
                {
                    Vector3 pointLoc = new Vector3(p.X * PointCloudModel.scale, p.Z * PointCloudModel.scale, p.Y * PointCloudModel.scale) - PointCloudModel.InvertOrigin.Value;
                    points[reader.currentPointCount - 1] = pointLoc;
                }
                meshColors[reader.currentPointCount - 1] = temp_Color;
            }

            // Instantiate Point Groups
            numPointGroups = Mathf.CeilToInt(numPoints * 1.0f / PointCloudModel.limitPoints * 1.0f);
            //We need point groups plus the remaining
            TempMesh[] retVal = new TempMesh[numPointGroups];


            //Create a mesh for each point group
            for (int i = 0; i < numPointGroups - 1; i++)
            {
                retVal[i] = CreateMesh(i, PointCloudModel.limitPoints);
            }
            //mesh the remaining points
            retVal[retVal.Length-1] = CreateMesh(numPointGroups - 1, numPoints - ((numPointGroups - 1) * PointCloudModel.limitPoints));

            Meshes = new Mesh[retVal.Length];
            for(int i = 0; i < retVal.Length; i++)
            {
                Meshes[i] = retVal[i].Convert();
            }
            return true;
        }


        TempMesh CreateMesh(int id, int nPoints)
        {
            TempMesh mesh = new TempMesh();
            Vector3[] myPoints = new Vector3[nPoints];
            int[] indecies = new int[nPoints];
            Color[] myColors = new Color[nPoints];
            //Assign a color and vertex for each point, in the mesh
            for (int i = 0; i < nPoints; ++i)
            {
                try
                {
                    myPoints[i] = points[id * PointCloudModel.limitPoints + i];
                    indecies[i] = i;
                    myColors[i] = meshColors[id * PointCloudModel.limitPoints + 1];
                }
                catch(Exception e)
                {
                    Debug.Log("i: " + i + " \nID: "+id+", \nmyPoints.Length: " + myPoints.Length +", \nPoints.Length: "+points.Length + ", \nid*limit+1: " + id * PointCloudModel.limitPoints + 1);
                    Debug.Log(e.StackTrace);
                    throw e;
                }
            }
            //Assign the values to the mesh
            mesh.vertices = myPoints;
            mesh.colors = myColors;
            mesh.indices = indecies;
            return mesh;
        }
    }
}
