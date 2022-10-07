using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAS
{
    public abstract class LASPoint
    {

        #region Fields

        /// <summary>
        /// The X Coordinate
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// The Y Coordinate
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// The Z Coordinate
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// The ipulse return magnitude
        /// </summary>
        public ushort Intensity { get; set; }

        public byte R_N_S_E;

        /// <summary>
        /// Pulse return number for a given oulse
        /// </summary>
        public byte Return_Number
        {
            get
            {
                return (byte)((R_N_S_E & 224) >> 5);
            }
        }

        /// <summary>
        /// Total number of returns for a given pulse
        /// </summary>
        public byte Number_Of_Returns
        {
            get
            {
                return (byte)((R_N_S_E & 28) >> 2);
            }
        }

        /// <summary>
        /// Direction of scan. True is a positive scan direction, false is a negative scan direction.
        /// </summary>
        public bool Scan_Direction_Flag
        {
            get
            {
                return (R_N_S_E & 2) != 0;
            }
        }

        /// <summary>
        /// Value if the scan is at the end of a scan line. True is the edge.
        /// </summary>
        public bool Edge_Of_Flight_Line
        {
            get
            {
                return (R_N_S_E & 1) != 0;
            }
        }

        /// <summary>
        /// The classification of this point
        /// <para>Bit 0:4 -> Standard ASPRS classification</para>
        /// <para>Bit 5 -> Synthetic</para>
        /// <para>Bit 6 -> Key-Point</para>
        /// <para>Bit 7 -> Witheld</para>
        /// </summary>
        public byte Classification { get; set; }

        /// <summary>
        ///  The scan angle rank is the angle (rounded to the nearest integer in the absolute value sense) at which the laser point was output from the laser system
        /// </summary>
        public sbyte Scan_Angle_Rank { get; set; }

        /// <summary>
        /// Used at the users discretion
        /// </summary>
        public byte User_Data { get; set; }

        /// <summary>
        /// Corresponds to the File_Source_ID of the file this is read from
        /// </summary>
        public ushort Point_Source_ID { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for LASPoint
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="intensity"></param>
        /// <param name="r_N_S_E"></param>
        /// <param name="classification"></param>
        /// <param name="scan_Angle_Rank"></param>
        /// <param name="user_Data"></param>
        /// <param name="point_Source_ID"></param>
        public LASPoint(float x, float y, float z,
                            ushort intensity, byte r_N_S_E, byte classification,
                            sbyte scan_Angle_Rank, byte user_Data, ushort point_Source_ID)
        {
            X = x;
            Y = y;
            Z = z;
            Intensity = intensity;
            R_N_S_E = r_N_S_E;
            Classification = classification;
            Scan_Angle_Rank = scan_Angle_Rank;
            User_Data = user_Data;
            Point_Source_ID = point_Source_ID;
        }

        #endregion

    }

    public class LASPoint0 : LASPoint
    {
        #region Constructors

        public LASPoint0(float x, float y, float z,
            ushort intensity, byte r_N_S_E, byte classification,
            sbyte scan_Angle_Rank, byte user_Data, ushort point_Source_ID)
            : base(x, y, z,
                  intensity, r_N_S_E, classification,
                  scan_Angle_Rank, user_Data, point_Source_ID)
        {
        }

        #endregion
    }

    public class LASPoint1 : LASPoint
    {

        #region Constructors

        public LASPoint1(float x, float y, float z,
            ushort intensity, byte r_N_S_E, byte classification,
            sbyte scan_Angle_Rank, byte user_Data, ushort point_Source_ID,
            double GPS_Time)
            : base(x, y, z,
                  intensity, r_N_S_E, classification,
                  scan_Angle_Rank, user_Data, point_Source_ID)
        {
            this.GPS_Time = GPS_Time;
        }

        public LASPoint1(LASPoint0 p, double gpsTime) : base(p.X, p.Y, p.Z,
                  p.Intensity, p.R_N_S_E, p.Classification,
                  p.Scan_Angle_Rank, p.User_Data, p.Point_Source_ID)
        {

            GPS_Time = gpsTime;
        }

        #endregion

        #region Fields

        /// <summary>
        ///  GPS time is the double floating point time tag value at which the point was acquired.
        /// </summary>
        public double GPS_Time { get; set; }

        #endregion

    }
}