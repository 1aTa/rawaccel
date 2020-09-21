﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace grapher.Models.Calculations
{
    public class AccelChartData
    {
        #region Constructors

        public AccelChartData()
        {
            AccelPoints = new SortedDictionary<double, double>();
            VelocityPoints = new SortedDictionary<double, double>();
            GainPoints = new SortedDictionary<double, double>();
            OrderedVelocityPointsList = new List<double>();
            OutVelocityToPoints = new Dictionary<double, (double, double, double)>();
        }

        #endregion Constructors

        #region Properties

        public SortedDictionary<double, double> AccelPoints { get; }

        public SortedDictionary<double, double> VelocityPoints { get; }

        public SortedDictionary<double, double> GainPoints { get; }

        public List<double> OrderedVelocityPointsList { get; }

        public Dictionary<double, (double, double, double)> OutVelocityToPoints { get; }

        #endregion Properties

        #region Methods

        public void Clear()
        {
            AccelPoints.Clear();
            VelocityPoints.Clear();
            GainPoints.Clear();
            OrderedVelocityPointsList.Clear();
            OutVelocityToPoints.Clear();
        }

        public (double, double, double) FindPointValuesFromOut(double outVelocityValue)
        {
            if (OutVelocityToPoints.TryGetValue(outVelocityValue, out var values))
            {
                return values;
            }
            else
            {
                var velIdx = GetVelocityIndex(outVelocityValue);

                velIdx = Math.Min(velIdx, VelocityPoints.Count - 1);
                values = (VelocityPoints.ElementAt(velIdx).Key, AccelPoints.ElementAt(velIdx).Value, GainPoints.ElementAt(velIdx).Value);
                OutVelocityToPoints.Add(outVelocityValue, values);
                return values;
            }
        }

        public (double, double, double) ValuesAtIndex(int index)
        {
            return (VelocityPoints.ElementAt(index).Value, AccelPoints.ElementAt(index).Value, GainPoints.ElementAt(index).Value);
        }

        public int GetVelocityIndex(double outVelocityValue)
        {
            var velIdx = OrderedVelocityPointsList.BinarySearch(outVelocityValue);

            if (velIdx < 0)
            {
                velIdx = ~velIdx;
            }

            return velIdx;
        }

        #endregion Methods
    }
}
