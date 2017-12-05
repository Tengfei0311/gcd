﻿using System.IO;
using System.Xml;
using GCDConsoleLib;
using System;

namespace GCDCore.Project
{
    public class AssocSurface : GCDProjectRasterItem
    {
        public string AssocSurfaceType { get; set; }
        public readonly DEMSurvey DEM;
        
        /// <summary>
        /// Needed to support putting these items in a DataGridView combo column
        /// </summary>
        public AssocSurface This { get { return this; } }
        
        public AssocSurface(string name, FileInfo rasterPath, string sType, DEMSurvey dem)
            : base(name, rasterPath)
        {
            AssocSurfaceType = sType;
            DEM = dem;
        }

        public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodAssoc = nodParent.AppendChild(xmlDoc.CreateElement("AssociatedSurface"));
            nodAssoc.AppendChild(xmlDoc.CreateElement("Name")).InnerText = Name;
            nodAssoc.AppendChild(xmlDoc.CreateElement("Type")).InnerText = AssocSurfaceType.ToString();
            nodAssoc.AppendChild(xmlDoc.CreateElement("Path")).InnerText = ProjectManager.Project.GetRelativePath(Raster.GISFileInfo);
        }

        public static AssocSurface Deserialize(XmlNode nodAssoc, DEMSurvey dem)
        {
            string name = nodAssoc.SelectSingleNode("Name").InnerText;
            FileInfo path = ProjectManager.Project.GetAbsolutePath(nodAssoc.SelectSingleNode("Path").InnerText);
            string type = nodAssoc.SelectSingleNode("Type").InnerText;
            return new AssocSurface(name, path, type, dem);
        }     
    }
}
