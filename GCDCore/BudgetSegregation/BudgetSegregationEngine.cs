﻿using System.IO;
using System.Collections.Generic;
using System.Text;
using GCDConsoleLib;

namespace GCDCore.BudgetSegregation
{
    public class BudgetSegregationEngine : ChangeDetection.Engine
    {
        public BudgetSegregationEngine(DirectoryInfo folder)
            : base(folder)
        {
        }

        public BSResultSet Calculate(ref ChangeDetection.DoDResult dod, ref Vector polygonMask, string fieldName)
        {
            // Build the budget segregation result set object that will be returned. This determines paths
            BSResultSet resultSet = new BSResultSet(AnalysisFolder, fieldName);

            // Copy the budget segregation mask ShapeFile into the folder
            polygonMask.Copy(resultSet.PolygonMask);
            Vector Mask = new Vector(resultSet.PolygonMask);

            Raster rawDoD = new Raster(dod.RawDoD);
            Raster thrDoD = new Raster(dod.ThrDoD);

            // Retrieve the segregated statistics from the DoD rasters depending on the thresholding type used.
            Dictionary<string, GCDConsoleLib.GCD.DoDStats> results = null;
            if (dod is ChangeDetection.DoDResultMinLoD)
            {
                float threshold = ((ChangeDetection.DoDResultMinLoD)dod).Threshold;
                results = RasterOperators.GetStatsMinLoD(rawDoD, thrDoD, threshold, Mask, fieldName, Project.ProjectManagerBase.CellArea, Project.ProjectManagerBase.Units);
            }
            else
            {
                Raster propErr = new Raster(((ChangeDetection.DoDResultPropagated)dod).PropErrRaster);

                if (dod is ChangeDetection.DoDResultPropagated)
                {
                    results = RasterOperators.GetStatsPropagated(rawDoD, thrDoD, propErr, polygonMask, fieldName, Project.ProjectManagerBase.CellArea, Project.ProjectManagerBase.Units);
                }
                else
                {
                    results = RasterOperators.GetStatsProbalistic(rawDoD, thrDoD, propErr, polygonMask, fieldName, Project.ProjectManagerBase.CellArea, Project.ProjectManagerBase.Units);
                }
            }

            // Retrieve the histograms for all budget segregation classes
            Dictionary<string, Histogram> rawHistos = RasterOperators.BinRaster(ref rawDoD, DEFAULTHISTOGRAMNUMBER, ref Mask, fieldName);
            Dictionary<string, Histogram> thrHistos = RasterOperators.BinRaster(ref thrDoD, DEFAULTHISTOGRAMNUMBER, ref Mask, fieldName);

            // Make sure that the output folder and the folder for the figures exist
            AnalysisFolder.Create();
            FiguresFolder.Create();

            // Build the output necessary output files 
            int classIndex = 1;
            StringBuilder legendText = new StringBuilder("Class Index, Class Name");
            foreach (KeyValuePair<string, GCDConsoleLib.GCD.DoDStats> segClass in results)
            {
                legendText.AppendLine(string.Format("{0},{1}", classIndex, segClass.Key));

                BSResult classResult = new BSResult(AnalysisFolder, segClass.Key, classIndex, segClass.Value);
                resultSet.ClassResults[segClass.Key] = classResult;

                GenerateSummaryXML(segClass.Value, classResult.SummaryXMLPath);
                GenerateChangeBarGraphicFiles(segClass.Value, 600, 600, classResult.ClassFilePrefix);

                Histogram rawHisto = rawHistos[segClass.Key];
                Histogram thrHisto = thrHistos[segClass.Key];

                WriteHistogram(rawHisto, classResult.RawHistogramPath);
                WriteHistogram(thrHisto, classResult.ThrHistogramPath);

                classIndex++;
            }

            // Write the class legend to file          
            File.WriteAllText(resultSet.ClassLegendPath.FullName, legendText.ToString());

            return resultSet;
        }
    }
}