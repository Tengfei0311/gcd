﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using GCDConsoleLib.Internal;
using GCDConsoleLib.GCD;
using GCDConsoleLib.FIS;

namespace GCDConsoleLib.Internal.Operators
{

    public class FISRasterOp : CellByCellOperator<double>
    {
        FisFile _FISFile;
        RuleSet _RuleSet;

        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public FISRasterOp(Dictionary<string, Raster> rInputs, FileInfo fisFile, Raster rOutput) :
            base(rInputs.Values.ToList(), rOutput)
        {
            _FISFile = new FisFile(fisFile);
            _RuleSet = _FISFile.ruleset;
        }

        /// <summary>
        /// For our error rasters we actually don't need an output
        /// </summary>
        /// <param name="rInputs"></param>
        /// <param name="fisFile"></param>
        /// <param name="rOutput"></param>
        public FISRasterOp(Dictionary<string, Raster> rInputs, FileInfo fisFile) :
            base(rInputs.Values.ToList())
        {
            _FISFile = new FisFile(fisFile);
            _RuleSet = _FISFile.ruleset;
        }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override double CellOp(List<double[]> data, int id)
        {
            return FISCellOp(data, id);
        }

        /// <summary>
        /// Do the actual calculation
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public double FISCellOp(List<double[]> data, int id)
        {
            return _RuleSet.calculate(data, id, true, _rasternodatavals, OpNodataVal);
        }

    }
}