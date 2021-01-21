using System;

namespace Temzit.Api
{
    public record TemzitActualState
    {
        public TemzitActualState(byte[] data)
        {
            State = BitConverter.ToInt16(data, 0);
            SchedulerNumber = BitConverter.ToInt16(data, 2);
            OutsideTemperature = (float) BitConverter.ToInt16(data, 4) / 10;
            InsideTemperature = (float) BitConverter.ToInt16(data, 6) / 10;
            OutHeatTemperature = (float) BitConverter.ToInt16(data, 8) / 10;
            InHeatTemperature = (float) BitConverter.ToInt16(data, 10) / 10;
            LiquidSpeed = BitConverter.ToInt16(data, 18);
            InputPower = (float) BitConverter.ToInt16(data, 28) / 10;
            HotWater = (float) BitConverter.ToInt16(data, 16) / 10;
        }

        public float HotWater { get; set; }

        public float InputPower { get; set; }

        public float LiquidSpeed { get; set; }

        public float OutHeatTemperature { get; set; }

        public float InHeatTemperature { get; set; }

        public float InsideTemperature { get; set; }

        public float OutsideTemperature { get; init; }

        public short State { get; set; }
        public short SchedulerNumber { get; set; }
        public float OutputPower => (OutHeatTemperature - InHeatTemperature) * 4200 * LiquidSpeed / 60 / 1000;
        public float? COP => InputPower == 0 ? null : OutputPower / InputPower;
    }
}