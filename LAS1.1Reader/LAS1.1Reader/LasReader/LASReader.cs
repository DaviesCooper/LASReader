using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAS
{
    public class LASReader
    {

        #region Fields

        /// <summary>
        /// The header meta-data associated with this LAS file
        /// </summary>
        public LASHeader HeaderFile;

        /// <summary>
        /// The binary reader used for reading points
        /// </summary>
        BinaryReader pointReader;

        /// <summary>
        /// If the binary reader has finished reading all the points
        /// </summary>
        public bool Finished { get; set; }

        /// <summary>
        /// The current count of points read
        /// </summary>
        public uint currentPointCount = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a Header meta data, and then assigns a new binary reader and positions it for point reading
        /// </summary>
        /// <param name="FilePath"></param>
        public LASReader(string FilePath)
        {
            HeaderFile = new LASHeader(FilePath);
            pointReader = new BinaryReader(File.OpenRead(FilePath));
            pointReader.BaseStream.Position = (long)HeaderFile.Offset_To_Point_Data.Value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads a point from the LAS file
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool ReadPoint(out LASPoint point)
        {
            point = null;
            //If we have somehow finished reading
            if (HeaderFile == null || Finished || currentPointCount > HeaderFile.Number_Of_Point_Records)
            {
                //Close the stream
                pointReader.Close();
                return false;
            }

            //If the format of the point is 0
            if (HeaderFile.Point_Data_Format_ID == 0)
            {
                //read format0
                point = ReadFormat0();
                if (point == null)
                {
                    Finished = true;
                    return false;
                }
                currentPointCount++;
                return true;
            }
            else
            {
                //else Read format1
                point = ReadFormat1();
                if (point == null)
                {
                    Finished = true;
                    return false;
                }
                currentPointCount++;
                return true;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Reads a point from the LAS file into point format 0
        /// </summary>
        /// <returns></returns>
        private LASPoint0 ReadFormat0()
        {
            try
            {
                //These calculations are described in the specifications. Casting to floats for use in unity
                float x = (float)((pointReader.ReadInt32() * HeaderFile.X_Scale_Factor) + HeaderFile.X_Offset);
                float y = (float)((pointReader.ReadInt32() * HeaderFile.Y_Scale_Factor) +HeaderFile.Y_Offset);
                float z = (float)((pointReader.ReadUInt32() * HeaderFile.Z_Scale_Factor) +HeaderFile.Z_Offset);
                ushort intensity = pointReader.ReadUInt16();
                byte rnse = pointReader.ReadByte();
                byte classification = pointReader.ReadByte();
                sbyte sangle = pointReader.ReadSByte();
                byte userData = pointReader.ReadByte();
                ushort sourceID = pointReader.ReadUInt16();
                return new LASPoint0(x, y, z, intensity, rnse, classification, sangle, userData, sourceID);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Reads a point from the LAS file in format 1
        /// </summary>
        /// <returns></returns>
        private LASPoint1 ReadFormat1()
        {
            //Because the only addition to format 1 is the gps time, we can piggy-back on its read method
            LASPoint0 point0 = ReadFormat0();
            if (point0 == null)
                return null;

            double gpsTIme = pointReader.ReadDouble();

            return new LASPoint1(point0, gpsTIme);

        }

        #endregion
    }
}
