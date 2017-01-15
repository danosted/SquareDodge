namespace Assets.Code.DataAccess.DTOs
{
    public class Wave
    {
        public bool IsStarted { get; set; }
        public int ObstacleCount { get; set; }
        public int ObstacleLevel { get; set; }
        public float WaveLengthSeconds { get; set; }
        public float WaveActiveTime { get; set; }
        public Wave NextWave { get; set; }
    }
}
