// ***********************************************************************
// Assembly         : FiseiD
// Author           : Usuario
// Created          : 12-07-2017
//
// Last Modified By : Usuario
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="FPS_CALCULAR.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;


namespace EvaluacionErgonomica
{
    /// <summary>
    /// Class FPS_CALCULAR.
    /// </summary>
    internal class FPS_CALCULAR
    {
        /// <summary>
        /// The update perdiod
        /// </summary>
        private readonly double _updatePerdiod;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public double Value { get; private set; }

        /// <summary>
        /// The frame count
        /// </summary>
        private int _frameCount;
        /// <summary>
        /// The time last reset
        /// </summary>
        private DateTime _timeLastReset;

        /// <summary>
        /// Initializes a new instance of the <see cref="FPS_CALCULAR" /> class.
        /// </summary>
        /// <param name="updatePerdiod">The update perdiod.</param>
        public FPS_CALCULAR(double updatePerdiod)
        {
            _updatePerdiod = updatePerdiod;
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        public void Tick()
        {
            _frameCount++;
            var timeSinceLastReset = (DateTime.Now - _timeLastReset).TotalSeconds;
            if (timeSinceLastReset >= _updatePerdiod)
            {
                Value = _frameCount / timeSinceLastReset;
                Reset();
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            _frameCount = 0;
            _timeLastReset = DateTime.Now;
        }

    }
}