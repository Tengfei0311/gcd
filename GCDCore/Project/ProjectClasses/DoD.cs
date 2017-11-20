﻿using System;
using System.IO;
using System.Collections.Generic;
using GCDConsoleLib.GCD;
using System.Xml;

namespace GCDCore.Project
{
    public class DoD : GCDProjectItem
    {
        public enum ThresholdingMethods
        {
            MinLoD,
            Propagated,
            Probabilistic
        }

        public readonly DirectoryInfo Folder;

        public readonly DEMSurvey NewDEM;
        public readonly DEMSurvey OldDEM;

        public readonly ErrorSurface NewErrorSurface;
        public readonly ErrorSurface OldErrorSurface;

        public ProjectRaster RawDoD { get; set; }
        public ProjectRaster ThrDoD { get; set; }

        public FileInfo RawHistogram { get; set; }
        public FileInfo ThrHistogram { get; set; }
        public FileInfo SummaryXML { get; set; }

        public readonly double? Threshold;
        public readonly ThresholdingMethods ThresholdingMethod;

        public readonly DoDStats Statistics;
        public readonly ChangeDetection.CoherenceProperties SpatCoProperties;

        public Dictionary<string, BudgetSegregation> BudgetSegregations { get; internal set; }

        public DoD(string name, DirectoryInfo folder, DEMSurvey newDEM, DEMSurvey oldDEM,
            ErrorSurface newError, ErrorSurface oldError, double? threshold,
            ThresholdingMethods method, DoDStats stats, ChangeDetection.CoherenceProperties props)
            : base(name)
        {
            Folder = folder;
            NewDEM = newDEM;
            OldDEM = oldDEM;
            NewErrorSurface = newError;
            OldErrorSurface = oldError;
            Threshold = threshold;
            ThresholdingMethod = method;
            Statistics = stats;
            SpatCoProperties = props;

            BudgetSegregations = new Dictionary<string, BudgetSegregation>();
        }

        public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodDoD = nodParent.AppendChild(xmlDoc.CreateElement("DoD"));
            nodDoD.AppendChild(xmlDoc.CreateElement("Name")).InnerText = Name;
            nodDoD.AppendChild(xmlDoc.CreateElement("Folder")).InnerText = ProjectManagerBase.GetRelativePath(Folder.FullName);
            nodDoD.AppendChild(xmlDoc.CreateElement("NewDEM")).InnerText = NewDEM.Name;
            nodDoD.AppendChild(xmlDoc.CreateElement("OldDEM")).InnerText = OldDEM.Name;
            nodDoD.AppendChild(xmlDoc.CreateElement("NewErrorSurface")).InnerText = NewErrorSurface.Name;
            nodDoD.AppendChild(xmlDoc.CreateElement("OldErrorSurface")).InnerText = OldErrorSurface.Name;
            nodDoD.AppendChild(xmlDoc.CreateElement("RawDoD")).InnerText = ProjectManagerBase.GetRelativePath(RawDoD.RasterPath);
            nodDoD.AppendChild(xmlDoc.CreateElement("ThrDoD")).InnerText = ProjectManagerBase.GetRelativePath(ThrDoD.RasterPath);
            nodDoD.AppendChild(xmlDoc.CreateElement("RawHistogram")).InnerText = ProjectManagerBase.GetRelativePath(RawHistogram);
            nodDoD.AppendChild(xmlDoc.CreateElement("ThrHistogram")).InnerText = ProjectManagerBase.GetRelativePath(ThrHistogram);
            nodDoD.AppendChild(xmlDoc.CreateElement("SummaryXML")).InnerText = ProjectManagerBase.GetRelativePath(SummaryXML);
            nodDoD.AppendChild(xmlDoc.CreateElement("Threshold")).InnerText = Threshold.HasValue ? Threshold.ToString() : string.Empty;
            nodDoD.AppendChild(xmlDoc.CreateElement("ThresholdingMethod")).InnerText = ThresholdingMethod.ToString();

            SerializeDoDStatistics(xmlDoc, nodDoD.AppendChild(xmlDoc.CreateElement("Statistics")), Statistics);

            if (SpatCoProperties != null)
            {
                XmlNode nodSpatCo = nodDoD.AppendChild(xmlDoc.CreateElement("SpatialCoherence"));
                nodSpatCo.AppendChild(xmlDoc.CreateElement("WindowSize")).InnerText = SpatCoProperties.MovingWindowDimensions.ToString();
                nodSpatCo.AppendChild(xmlDoc.CreateElement("InflectionA")).InnerText = SpatCoProperties.InflectionA.ToString();
                nodSpatCo.AppendChild(xmlDoc.CreateElement("InflectionB")).InnerText = SpatCoProperties.InflectionB.ToString();
            }

            XmlNode nodBS = nodParent.AppendChild(xmlDoc.CreateElement("BudgetSegregations"));
            foreach (BudgetSegregation bs in BudgetSegregations.Values)
                bs.Serialize(xmlDoc, nodBS);
        }

        public static void SerializeDoDStatistics(XmlDocument xmlDoc, XmlNode nodParent, DoDStats stats)
        {
            XmlNode nodErosion = nodParent.AppendChild(xmlDoc.CreateElement("Erosion"));
            SerializeAreaVolume(xmlDoc, nodErosion.AppendChild(xmlDoc.CreateElement("Raw")), stats.ErosionRaw, stats.StatsUnits, stats.CellArea);
            SerializeAreaVolume(xmlDoc, nodErosion.AppendChild(xmlDoc.CreateElement("Thresholded")), stats.ErosionThr, stats.StatsUnits, stats.CellArea);
            nodErosion.AppendChild(xmlDoc.CreateElement("Error")).AppendChild(xmlDoc.CreateElement("Volume")).InnerText =
                stats.ErosionErr.GetVolume(stats.CellArea, stats.StatsUnits.VertUnit).As(stats.StatsUnits.VolUnit).ToString();

            XmlNode nodDeposition = nodParent.AppendChild(xmlDoc.CreateElement("Deposition"));
            SerializeAreaVolume(xmlDoc, nodDeposition.AppendChild(xmlDoc.CreateElement("Raw")), stats.DepositionRaw, stats.StatsUnits, stats.CellArea);
            SerializeAreaVolume(xmlDoc, nodDeposition.AppendChild(xmlDoc.CreateElement("Thresholded")), stats.DepositionThr, stats.StatsUnits, stats.CellArea);
            nodDeposition.AppendChild(xmlDoc.CreateElement("Error")).AppendChild(xmlDoc.CreateElement("Volume")).InnerText =
                stats.DepositionErr.GetVolume(stats.CellArea, stats.StatsUnits.VertUnit).As(stats.StatsUnits.VolUnit).ToString();
        }

        private static void SerializeAreaVolume(XmlDocument xmlDoc, XmlNode nodParent, GCDAreaVolume areaVol, UnitGroup units, UnitsNet.Area cellArea)
        {
            nodParent.AppendChild(xmlDoc.CreateElement("Area")).InnerText = areaVol.GetArea(cellArea).As(units.ArUnit).ToString();
            nodParent.AppendChild(xmlDoc.CreateElement("Volume")).InnerText = areaVol.GetVolume(cellArea, units.VertUnit).As(units.VolUnit).ToString();
        }

        public static DoD Deserialize(XmlNode nodDoD, Dictionary<string, DEMSurvey> DEMs)
        {
            string name = nodDoD.SelectSingleNode("Name").InnerText;
            DirectoryInfo folder = ProjectManagerBase.GetAbsoluteDir(nodDoD.SelectSingleNode("Folder").InnerText);

            DEMSurvey newDEM, oldDEM;
            ErrorSurface newError, oldError;

            DeserializeDEM(nodDoD, DEMs, out newDEM, out newError, "NewDEM", "NewError");
            DeserializeDEM(nodDoD, DEMs, out oldDEM, out oldError, "OldDEM", "OldError");

            double? Threshold = new double?();
            if (!string.IsNullOrEmpty(nodDoD.SelectSingleNode("Threshold").InnerText))
                Threshold = double.Parse(nodDoD.SelectSingleNode("Threshold").InnerText);

            FileInfo rawDoD = ProjectManagerBase.GetAbsolutePath(nodDoD.SelectSingleNode("RawDoD").InnerText);
            FileInfo thrDoD = ProjectManagerBase.GetAbsolutePath(nodDoD.SelectSingleNode("ThrDoD").InnerText);
            FileInfo rawHis = ProjectManagerBase.GetAbsolutePath(nodDoD.SelectSingleNode("RawHistogram").InnerText);
            FileInfo thrHis = ProjectManagerBase.GetAbsolutePath(nodDoD.SelectSingleNode("ThrHistogram").InnerText);
            FileInfo summar = ProjectManagerBase.GetAbsolutePath(nodDoD.SelectSingleNode("SummaryXML").InnerText);

            ThresholdingMethods method = (ThresholdingMethods)Enum.Parse(typeof(ThresholdingMethods), nodDoD.SelectSingleNode("ThresholdingMethod").InnerText);

            DoDStats stats = DeserializeStatistics(nodDoD.SelectSingleNode("Statistics"), ProjectManagerBase.CellArea, ProjectManagerBase.Units);

            ChangeDetection.CoherenceProperties props = null;
            XmlNode nodSpatCo = nodDoD.SelectSingleNode("SpatialCoherence");
            if (nodSpatCo is XmlNode)
            {
                int windowSize = int.Parse(nodSpatCo.SelectSingleNode("WindowSize").InnerText);
                int inflectinA = int.Parse(nodSpatCo.SelectSingleNode("InflectionA").InnerText);
                int inflectinB = int.Parse(nodSpatCo.SelectSingleNode("InflectionB").InnerText);
                props = new ChangeDetection.CoherenceProperties(windowSize, inflectinA, inflectinB);
            }

            DoD dod = new DoD(name, folder, newDEM, oldDEM, newError, oldError, Threshold, method, stats, props);

            foreach(XmlNode nodBS in nodDoD.SelectNodes("BudgetSegregations/BudgetSegregation"))
            {
                BudgetSegregation bs = BudgetSegregation.Deserialize(nodBS, dod);
                dod.BudgetSegregations[bs.Name] = bs;
            }

            return dod;
        }

        private static void DeserializeDEM(XmlNode nodDoD, Dictionary<string, DEMSurvey> DEMs, out DEMSurvey dem, out ErrorSurface error, string nodDEMName, string nodErrorName)
        {
            dem = null;
            error = null;
            string demName = nodDoD.SelectSingleNode(nodDEMName).InnerText;
            if (DEMs.ContainsKey(demName))
            {
                dem = DEMs[demName];
                string errorName = nodDoD.SelectSingleNode(nodErrorName).InnerText;
                if (dem.ErrorSurfaces.ContainsKey(errorName))
                {
                    error = dem.ErrorSurfaces[errorName];
                }
            }
        }

        public static DoDStats DeserializeStatistics(XmlNode nodStatistics, UnitsNet.Area cellArea, UnitGroup units)
        {
            UnitsNet.Area AreaErosion_Raw = UnitsNet.Area.From(double.Parse(nodStatistics.SelectSingleNode("Erosion/Raw/Area").InnerText), units.ArUnit);
            UnitsNet.Area AreaDeposit_Raw = UnitsNet.Area.From(double.Parse(nodStatistics.SelectSingleNode("Deposition/Raw/Area").InnerText), units.ArUnit);
            UnitsNet.Area AreaErosion_Thr = UnitsNet.Area.From(double.Parse(nodStatistics.SelectSingleNode("ErosionThr/Area").InnerText), units.ArUnit);
            UnitsNet.Area AreaDeposit_Thr = UnitsNet.Area.From(double.Parse(nodStatistics.SelectSingleNode("Deposition/Thresholded/Area").InnerText), units.ArUnit);

            UnitsNet.Volume VolErosion_Raw = UnitsNet.Volume.From(double.Parse(nodStatistics.SelectSingleNode("Erosion/Raw/Volume").InnerText), units.VolUnit);
            UnitsNet.Volume VolDeposit_Raw = UnitsNet.Volume.From(double.Parse(nodStatistics.SelectSingleNode("Deposition/Raw/Volume").InnerText), units.VolUnit);
            UnitsNet.Volume VolErosion_Thr = UnitsNet.Volume.From(double.Parse(nodStatistics.SelectSingleNode("Erosion/Thresholded/Volume").InnerText), units.VolUnit);
            UnitsNet.Volume VolDeposit_Thr = UnitsNet.Volume.From(double.Parse(nodStatistics.SelectSingleNode("Deposition/Thresholded/Volume").InnerText), units.VolUnit);

            UnitsNet.Volume VolErosion_Err = UnitsNet.Volume.From(double.Parse(nodStatistics.SelectSingleNode("Erosion/Error/Volume").InnerText), units.VolUnit);
            UnitsNet.Volume VolDeposit_Err = UnitsNet.Volume.From(double.Parse(nodStatistics.SelectSingleNode("Deposition/Error/Volume").InnerText), units.VolUnit);

            return new DoDStats(
                AreaErosion_Raw, AreaDeposit_Raw, AreaErosion_Thr, AreaDeposit_Thr,
                VolErosion_Raw, VolDeposit_Raw, VolErosion_Thr, VolDeposit_Thr,
                VolErosion_Err, VolDeposit_Err,
                cellArea, units);
        }

        private static UnitsNet.Area DeserializeArea(XmlNode nodParent, string nodName, UnitsNet.Units.AreaUnit unit)
        {
            double value = double.Parse(nodParent.SelectSingleNode(nodName).InnerText);
            return UnitsNet.Area.From(value, unit);
        }
    }
}
