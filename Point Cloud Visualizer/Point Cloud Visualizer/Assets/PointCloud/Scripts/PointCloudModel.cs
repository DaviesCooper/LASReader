using UnityEngine;
using System.Collections;
using System.IO;
using LAS;
using Assets.PointCloud.Scripts;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

public static class PointCloudModel
{

    #region Fields

    /// <summary>
    /// The name of the LAS files we wish to create point clouds from
    /// </summary>
    public static string dataPath;

    /// <summary>
    /// The scale of the point clouds
    /// </summary>
    public static float scale = 1;

    /// <summary>
    /// Unity's XYZ has Y up and Z as depth. Most conventional 3D structures have this inverted, so this is to account for that
    /// </summary>
    public static bool invertYAndZ = false;

    /// <summary>
    /// The number of maximum points each "sub-mesh" contains
    /// </summary>
    public static int limitPoints = 65000;

    /// <summary>
    /// Calculated from a single point cloud file so that the others are generated relative to it
    /// </summary>
    public static Vector3? Origin;

    /// <summary>
    /// Same as the origin except y and z are inverted
    /// </summary>
    public static Vector3? InvertOrigin;

    #endregion

    #region Main Method

    /// <summary>
    /// Using the datapaths, will generate the point clouds
    /// </summary>
    public static Mesh[] CreateClouds(string file)
    {
        dataPath = file;
        //Create an offset from one of the files
        CreateOffset(dataPath);
        if (PointCloudModel.Origin == null)
            return null;


        Mesh[] meshes = CreateNewPointCloud(dataPath);
        return meshes;
    }

    #endregion

    #region Point Cloud Generation

    /// <summary>
    /// Will create a point cloud, and attach it to the concurrent dictionary when finished
    /// </summary>
    /// <param name="dataPath"></param>
    static Mesh[] CreateNewPointCloud(string dataPath)
    {
        if (Origin == null)
        {
            return null;
        }
        System.Random rng = new System.Random();
        Color c = new Color((rng.Next(0,255)*1.0f)/255, (rng.Next(0, 255) * 1.0f) / 255, (rng.Next(0, 255) * 1.0f)/ 255);
        PointCloudGenerator cloud = new PointCloudGenerator(dataPath, c);
        Mesh[] meshes;
        cloud.LoadLAS(out meshes);
        return meshes;
    }
    #endregion

    #region Helpers

    /// <summary>
    /// Attempts to create an offset variable from a LAS file. Returns true if successful
    /// </summary>
    /// <param name="dataPath"></param>
    /// <returns></returns>
    static bool CreateOffset(string dataPath)
    {
        if (Origin != null)
            return false;
        try
        {
            LASReader reader = new LASReader(dataPath);
            float middlex = (float)(((reader.HeaderFile.Max_X - reader.HeaderFile.Min_X) / 2) + reader.HeaderFile.Min_X);
            float middley = (float)(((reader.HeaderFile.Max_Y - reader.HeaderFile.Min_Y) / 2) + reader.HeaderFile.Min_Y);
            float middlez = (float)(((reader.HeaderFile.Max_Z - reader.HeaderFile.Min_Z) / 2) + reader.HeaderFile.Min_Z);
            Origin = new Vector3(middlex * scale, middley * scale, middlez * scale);
            InvertOrigin = new Vector3(middlex * scale, middlez * scale, middley * scale);
            return true;
        }
        catch
        {
            return false;
        }
    }

    #endregion

}
