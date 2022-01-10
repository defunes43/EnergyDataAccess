namespace EnergyDataAccess.EnedisGateway.Response
{
    /// <summary>
    /// Enumerates the interval length offered by the Enedis API.
    /// </summary>
    internal enum IntervalLengthEnum
    {
        /// <summary>
        /// No interval defined.
        /// </summary>
        UNDEFINED,

        /// <summary>
        /// 30 minutes interval.
        /// </summary>
        PT30M,

        /// <summary>
        /// One day interval.
        /// </summary>
        P1D,
    }
}
