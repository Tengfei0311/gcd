﻿using GCDConsoleLib;
using GCDConsoleLib.GCD;
using System.IO;
using GCDCore.Project;

namespace GCDCore.Engines
{
    public class ChangeDetectionEnginePropProb : ChangeDetectionEngineBase
    {
        protected readonly ErrorSurface NewError;
        protected readonly ErrorSurface OldError;
        public Raster PropagatedErrRaster;

        public ChangeDetectionEnginePropProb(Surface newDEM, Surface oldDEM, ErrorSurface newError, ErrorSurface oldError, Project.Masks.AOIMask aoi,
            bool isAsync = false)
            : base(newDEM, oldDEM, aoi, isAsync)
        {
            NewError = newError;
            OldError = oldError;
        }

        protected override Raster ThresholdRawDoD(Raster rawDoD, FileInfo thrDoDPath)
        {
            GeneratePropagatedErrorRaster(thrDoDPath.Directory);
            Raster thrDoD = RasterOperators.AbsoluteSetNull(rawDoD, RasterOperators.ThresholdOps.GreaterThan, PropagatedErrRaster, thrDoDPath,
                OnProgressChangeDoD);
            return thrDoD;
        }

        protected override Raster GenerateErrorRaster(FileInfo errDoDPath)
        {
            PropagatedErrRaster.Copy(errDoDPath);
            return new Raster(errDoDPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawDoD"></param>
        /// <param name="thrDoD">NEVER USED IN THIS CASE</param>
        /// <param name="units"></param>
        /// <returns></returns>
        protected override DoDStats CalculateChangeStats(Raster rawDoD, Raster thrDoD,UnitGroup units)
        {
            return RasterOperators.GetStatsPropagated(rawDoD, PropagatedErrRaster, units, OnProgressChangeDoD);
        }

        protected override DoDBase GetDoDResult(string dodName, DoDStats changeStats, Raster rawDoD, Raster thrDoD, Raster thrErr, HistogramPair histograms, FileInfo summaryXML)
        {
            return new DoDPropagated(dodName, rawDoD.GISFileInfo.Directory, NewSurface, OldSurface, AOIMask, rawDoD, thrDoD, thrErr, histograms, summaryXML, NewError, OldError, PropagatedErrRaster, changeStats);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>Calculate the propograted error raster based on the two error surfaces. Then threshold the raw
        /// DoD removing any cells that have a value less than the propogated error.</remarks>
        protected void GeneratePropagatedErrorRaster(DirectoryInfo analysisFolder)
        {
            FileInfo propErrPath = BuildFilePath(analysisFolder, "PropErr", ProjectManager.RasterExtension);
            PropagatedErrRaster = RasterOperators.RootSumSquares(NewError.Raster, OldError.Raster, propErrPath, OnProgressChangeDoD);

            // Build Pyramids
            ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.PropagatedError, propErrPath);
        }
    }
}