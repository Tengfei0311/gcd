﻿using System.IO;
using System.Collections.Generic;
using System.Text;
using GCDConsoleLib;
using GCDCore.Project;
using System;

namespace GCDCore.Engines
{
    public class BudgetSegregationEngine : EngineBase
    {
        public BudgetSegregationEngine(string name, DirectoryInfo folder)
            : base(name, folder)
        {
        }

        public BudgetSegregation Calculate(DoDBase dod, ref Vector polygonMask, string fieldName)
        {
            // Build the budget segregation result set object that will be returned. This determines paths
            BudgetSegregation bsResult = new BudgetSegregation(Name, AnalysisFolder, fieldName, dod);

            // Copy the budget segregation mask ShapeFile into the folder
            polygonMask.Copy(bsResult.PolygonMask);
            Vector Mask = new Vector(bsResult.PolygonMask);

            // Retrieve the segregated statistics from the DoD rasters depending on the thresholding type used.
            Dictionary<string, GCDConsoleLib.GCD.DoDStats> results = null;

            if (dod is DoDMinLoD)
            {
                results = RasterOperators.GetStatsMinLoD(dod.RawDoD.Raster, dod.RawDoD.Raster, ((DoDMinLoD)dod).Threshold, Mask, fieldName, ProjectManager.Project.CellArea, Project.ProjectManager.Project.Units);
            }
            else
            {
                Raster propErr = ((DoDPropagated)dod).PropagatedError.Raster;

                if (dod is DoDProbabilistic)
                {
                    results = RasterOperators.GetStatsProbalistic(dod.RawDoD.Raster, dod.ThrDoD.Raster, propErr, Mask, fieldName, ProjectManager.Project.CellArea, ProjectManager.Project.Units);
                }
                else
                {
                    results = RasterOperators.GetStatsPropagated(dod.RawDoD.Raster, dod.ThrDoD.Raster, propErr, Mask, fieldName, ProjectManager.Project.CellArea, ProjectManager.Project.Units);
                }
            }

            // Retrieve the histograms for all budget segregation classes
            Dictionary<string, Histogram> rawHistos = RasterOperators.BinRaster(dod.RawDoD.Raster, DEFAULTHISTOGRAMNUMBER, Mask, fieldName);
            Dictionary<string, Histogram> thrHistos = RasterOperators.BinRaster(dod.ThrDoD.Raster, DEFAULTHISTOGRAMNUMBER, Mask, fieldName);

            // Make sure that the output folder and the folder for the figures exist
            AnalysisFolder.Create();
            FiguresFolder.Create();

            // Build the output necessary output files 
            int classIndex = 1;
            StringBuilder legendText = new StringBuilder("Class Index, Class Name");
            foreach (KeyValuePair<string, GCDConsoleLib.GCD.DoDStats> segClass in results)
            {
                legendText.AppendLine(string.Format("{0},{1}", classIndex, segClass.Key));

                string filePrefix = string.Format("c{0:000}", classIndex);
                FileInfo sumaryXML = new FileInfo(Path.Combine(AnalysisFolder.FullName, string.Format("{0}_summary.xml", filePrefix)));
                FileInfo rawHstPth = new FileInfo(Path.Combine(AnalysisFolder.FullName, string.Format("{0}_raw.csv", filePrefix)));
                FileInfo thrHstPth = new FileInfo(Path.Combine(AnalysisFolder.FullName, string.Format("{0}_thr.csv", filePrefix)));

                GenerateSummaryXML(segClass.Value, sumaryXML);
                GenerateChangeBarGraphicFiles(segClass.Value, 600, 600, filePrefix);

                WriteHistogram(rawHistos[segClass.Key], rawHstPth);
                WriteHistogram(thrHistos[segClass.Key], thrHstPth);

                HistogramPair histograms = new HistogramPair(rawHistos[segClass.Key], rawHstPth, thrHistos[segClass.Key], thrHstPth);

                BudgetSegregationClass bsClass = new BudgetSegregationClass(segClass.Key, AnalysisFolder, filePrefix, segClass.Value, histograms, sumaryXML);
                bsResult.Classes[segClass.Key] = bsClass;

                classIndex++;
            }

            // Write the class legend to file          
            File.WriteAllText(bsResult.ClassLegend.FullName, legendText.ToString());

            return bsResult;
        }
    }
}