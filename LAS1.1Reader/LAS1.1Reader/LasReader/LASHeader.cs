using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


/*
 * Formatted as specified here https://www.asprs.org/wp-content/uploads/2010/12/asprs_las_format_v11.pdf
 */


namespace LAS
{
    public class LASHeader
    {
        public LASHeader(string filePath)
        {
            BinaryReader read = new BinaryReader(File.OpenRead(filePath));
            ReadHeader(read);
            if (!ValidateFields())
            {
                throw new IOException("Invalid LAS format");
            }
            read.Close();
        }

        #region Fields

        /// <summary>
        /// MUST be "LASF". Used for type determination
        /// <para>REQUIRED</para>
        /// </summary>
        char[] File_Signature = new char[4];

        /// <summary>
        /// ID of source of data generation
        /// <para>REQUIRED</para>
        /// <para>Value: 1-65535</para>
        /// <para>0 is interpretted as an unassigned ID</para>
        /// </summary>
        public ushort? File_Source_ID = null;

        /// <summary>
        /// Reserved to always be Zero-Filled
        /// </summary>
        ushort? Reserved = null;

        /// <summary>
        /// Project ID for this given project. IDs containing the same GUID are considered to be the same project
        /// </summary>
        public ulong? Project_ID_GUID_Data_1 = null;

        /// <summary>
        /// Project ID for this given project. IDs containing the same GUID are considered to be the same project
        /// </summary>
        ushort? Project_ID_GUID_Data_2 = null;

        /// <summary>
        /// Project ID for this given project. IDs containing the same GUID are considered to be the same project
        /// </summary>
        ushort? Project_ID_GUID_Data_3 = null;

        /// <summary>
        /// Project ID for this given project. IDs containing the same GUID are considered to be the same project
        /// </summary>
        char[] Project_ID_GUID_Data_4 = new char[8];

        /// <summary>
        /// The Major part of the format number of the current specification itself
        /// <para>REQUIRED</para>
        /// </summary>
        byte? Version_Major = null;

        /// <summary>
        /// The Minor part of the format number of the current specification itself
        /// <para>REQUIRED</para>
        /// </summary>
        byte? Version_Minor = null;

        /// <summary>
        /// The identifier for how this file was created.
        /// <para>REQUIRED</para>
        /// <para>Sample Value: "MERGE" etc.</para>
        /// </summary>
        char[] System_Identifier = new char[32];

        /// <summary>
        /// The description of the generating software
        /// <para>REQUIRED</para>
        /// <para>Sample Value: "TerraScan V-10.8" etc.</para>
        /// </summary>
        char[] Generating_Software = new char[32];

        /// <summary>
        /// Day is computed as the Greenwich Mean Time (GMT) day. January 1 is considered day 1
        /// </summary>
        public ushort? File_Creation_Day_Of_Year = null;

        /// <summary>
        /// The year, expressed as a four digit number, in which the file was created
        /// </summary>
        public ushort? File_Creation_Year = null;

        /// <summary>
        ///  The size, in bytes, of the header block itself
        ///  <para>REQUIRED</para>
        /// </summary>
        ushort? Header_Size = null;

        /// <summary>
        /// Number of bytes from start of file to the first point field.
        /// <para>REQUIRED</para>
        /// </summary>
        public ulong? Offset_To_Point_Data = null;

        /// <summary>
        /// Number of variable length records appended onto the file
        /// <para>REQUIRED</para>
        /// </summary>
        ulong? Number_Of_Variable_Length_Records = null;

        /// <summary>
        /// Corresponds to te point data record format type
        /// <para>Sample Values: (0,1)</para>
        /// <para>REQUIRED</para>
        /// </summary>
        public byte? Point_Data_Format_ID = null;

        /// <summary>
        /// Description not found in documentation
        /// <para>REQUIRED</para>
        /// </summary>
        ushort? Point_Data_Record_Length = null;

        /// <summary>
        /// Totalt number of point records in the file
        /// <para>REQUIRED</para>
        /// </summary>
        public ulong? Number_Of_Point_Records = null;

        /// <summary>
        /// Array of total point record returns. First return index 1, second index 2, etc.
        /// <para>REQUIRED</para>
        /// </summary>
        ulong[] Number_Of_Points_By_Return = new ulong[5];

        /// <summary>
        /// Scale factor fields contain a double floating point value that is used to scale the corresponding X, Y, and Z long values within the point records. 
        /// <para>Xcoordinate = (Xrecord * Xscale) + Xoffset </para>
        /// <para>Ycoordinate = (Yrecord * Yscale) + Yoffset </para>
        /// <para>Zcoordinate = (Zrecord * Zscale) + Zoffset </para>
        /// <para>REQUIRED</para>
        /// </summary>
        public double? X_Scale_Factor = null;

        /// <summary>
        /// Scale factor fields contain a double floating point value that is used to scale the corresponding X, Y, and Z long values within the point records. 
        /// <para>Xcoordinate = (Xrecord * Xscale) + Xoffset </para>
        /// <para>Ycoordinate = (Yrecord * Yscale) + Yoffset </para>
        /// <para>Zcoordinate = (Zrecord * Zscale) + Zoffset </para>
        /// <para>REQUIRED</para>
        /// </summary>
        public double? Y_Scale_Factor = null;

        /// <summary>
        /// Scale factor fields contain a double floating point value that is used to scale the corresponding X, Y, and Z long values within the point records. 
        /// <para>Xcoordinate = (Xrecord * Xscale) + Xoffset </para>
        /// <para>Ycoordinate = (Yrecord * Yscale) + Yoffset </para>
        /// <para>Zcoordinate = (Zrecord * Zscale) + Zoffset </para>
        /// <para>REQUIRED</para>
        /// </summary>
        public double? Z_Scale_Factor = null;

        /// <summary>
        /// Offset fields should be used to set the overall offset for the point records.
        /// <para>Xcoordinate = (Xrecord * Xscale) + Xoffset </para>
        /// <para>Ycoordinate = (Yrecord * Yscale) + Yoffset </para>
        /// <para>Zcoordinate = (Zrecord * Zscale) + Zoffset </para>
        ///<para>REQUIRED</para>
        /// </summary>
        public double? X_Offset = null;

        /// <summary>
        /// Offset fields should be used to set the overall offset for the point records.
        /// <para>Xcoordinate = (Xrecord * Xscale) + Xoffset </para>
        /// <para>Ycoordinate = (Yrecord * Yscale) + Yoffset </para>
        /// <para>Zcoordinate = (Zrecord * Zscale) + Zoffset </para>
        ///<para>REQUIRED</para>
        /// </summary>
        public double? Y_Offset = null;

        /// <summary>
        /// Offset fields should be used to set the overall offset for the point records.
        /// <para>Xcoordinate = (Xrecord * Xscale) + Xoffset </para>
        /// <para>Ycoordinate = (Yrecord * Yscale) + Yoffset </para>
        /// <para>Zcoordinate = (Zrecord * Zscale) + Zoffset </para>
        ///<para>REQUIRED</para>
        /// </summary>
        public double? Z_Offset = null;

        /// <summary>
        /// The max and min data fields are the actual file coordinate extents of the LAS point file data
        /// <para>REQUIRED</para>
        /// </summary>
        public double? Max_X = null;

        /// <summary>
        /// 
        /// <para>REQUIRED</para>
        /// </summary>
        public double? Min_X = null;

        /// <summary>
        /// The max and min data fields are the actual file coordinate extents of the LAS point file data 
        /// <para>REQUIRED</para>
        /// </summary>
        public double? Max_Y = null;

        /// <summary>
        /// The max and min data fields are the actual file coordinate extents of the LAS point file data 
        /// <para>REQUIRED</para>
        /// </summary>
        public double? Min_Y = null;

        /// <summary>
        /// The max and min data fields are the actual file coordinate extents of the LAS point file data 
        /// <para>REQUIRED</para>
        /// </summary>
        public double? Max_Z = null;

        /// <summary>
        /// The max and min data fields are the actual file coordinate extents of the LAS point file data 
        /// <para>REQUIRED</para>
        /// </summary>
        public double? Min_Z = null;

        #endregion

        private void ReadHeader(BinaryReader reader)
        {
            try
            {
                if (reader == null) { throw new Exception("BinaryReader not instantiated"); }
                File_Signature = Encoding.Default.GetString(reader.ReadBytes(4)).ToArray();
                File_Source_ID = reader.ReadUInt16();
                Reserved = reader.ReadUInt16();
                Project_ID_GUID_Data_1 = reader.ReadUInt32();
                Project_ID_GUID_Data_2 = reader.ReadUInt16();
                Project_ID_GUID_Data_3 = reader.ReadUInt16();
                Project_ID_GUID_Data_4 = Encoding.Default.GetString(reader.ReadBytes(8)).ToArray();
                Version_Major = reader.ReadByte();
                Version_Minor = reader.ReadByte();
                System_Identifier = Encoding.Default.GetString(reader.ReadBytes(32)).ToArray();
                Generating_Software = Encoding.Default.GetString(reader.ReadBytes(32)).ToArray();
                File_Creation_Day_Of_Year = reader.ReadUInt16();
                File_Creation_Year = reader.ReadUInt16();
                Header_Size = reader.ReadUInt16();
                Offset_To_Point_Data = reader.ReadUInt32();
                Number_Of_Variable_Length_Records = reader.ReadUInt32();
                Point_Data_Format_ID = reader.ReadByte();
                Point_Data_Record_Length = reader.ReadUInt16();
                Number_Of_Point_Records = reader.ReadUInt32();
                Number_Of_Points_By_Return[0] = reader.ReadUInt32();
                Number_Of_Points_By_Return[1] = reader.ReadUInt32();
                Number_Of_Points_By_Return[2] = reader.ReadUInt32();
                Number_Of_Points_By_Return[3] = reader.ReadUInt32();
                Number_Of_Points_By_Return[4] = reader.ReadUInt32();
                X_Scale_Factor = reader.ReadDouble();
                Y_Scale_Factor = reader.ReadDouble();
                Z_Scale_Factor = reader.ReadDouble();
                X_Offset = reader.ReadDouble();
                Y_Offset = reader.ReadDouble();
                Z_Offset = reader.ReadDouble();
                Max_X = reader.ReadDouble();
                Min_X = reader.ReadDouble();
                Max_Y = reader.ReadDouble();
                Min_Y = reader.ReadDouble();
                Max_Z = reader.ReadDouble();
                Min_Z = reader.ReadDouble();
            }
            catch (Exception)
            {
                throw new Exception("Error reading the LAS file");
            }
            reader.Close();
        }

        /// <summary>
        /// Checks if the LAS file maintains all the required fields
        /// </summary>
        /// <returns></returns>
        private bool ValidateFields()
        {
            if (new string(File_Signature) != "LASF")
                return false;
            if (Reserved != 0)
                return false;
            if (Offset_To_Point_Data < Header_Size)
                return false;
            return true;
        }
    }
}
