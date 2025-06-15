using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTSolutions.IEC61034.Common.Setting
{
    public static class LightMeasurementUtils
    {
        /// <summary>
        /// 현재 측정값으로부터 투과율(0.0 ~ 1.0)을 계산합니다.
        /// </summary>
        public static double CalculateTransmittanceRatio(double voltage)
        {
            double zeroVoltage = DbChannel.AI_LIGHT_PHOTODIODE.MinVoltage;
            double spanVoltage = DbChannel.AI_LIGHT_PHOTODIODE.MaxVoltage;

            double lt = (voltage - zeroVoltage) / (spanVoltage - zeroVoltage);

            return Clamp(lt, 0.0, 1.0);
        }

        public static double CalculateTransmittancePercent(double voltage)
        {
            double gradient = DbChannel.AI_LIGHT_PHOTODIODE.GradientVoltage;
            double intercept = DbChannel.AI_LIGHT_PHOTODIODE.InterceptVoltage;

            double lt = gradient * voltage + intercept;
            return Math.Round(Clamp(lt, 0, 100), 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 투과율로부터 흡수율(Absorbance = -log₁₀(T))을 계산합니다.
        /// </summary>
        public static double CalculateAbsorbance(double transmittance)
        {
            if (transmittance <= 0 || transmittance > 1.0)
                throw new ArgumentException("투과율은 0보다 크고 1.0 이하이어야 합니다.");

            return -Math.Log10(transmittance);
        }

        /// <summary>
        /// 측정값으로부터 직접 흡수율을 계산합니다.
        /// </summary>
        public static double CalculateAbsorbanceFromVoltage(double voltage)
        {
            double lt = CalculateTransmittanceRatio(voltage);

            //return CalculateAbsorbance(lt);
            return Math.Round(CalculateAbsorbance(lt), 2, MidpointRounding.AwayFromZero);
        }

        private static double Clamp(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        /// <summary>
        /// Ac = (Am / toluenePercent) * (chamberVolume / lightPathLength)
        /// </summary>
        /// <param name="Am">측정된 최대 흡광도</param>
        /// <param name="toluenePercent">톨루엔 농도 (%)</param>
        /// <param name="chamberVolume">입방체의 용적 (m³)</param>
        /// <param name="lightPathLength">빛의 경로 길이 (m)</param>
        /// <returns>보정된 흡광도 Ac</returns>
        public static double CalculateCorrectedAbsorbance(double Am, double toluenePercent, double chamberVolume, double lightPathLength)
        {
            return (Am / toluenePercent) * (chamberVolume / lightPathLength);
        }


        // <summary>
        /// 전압값 리스트에서 최소 투과 전압을 기준으로 Am 계산
        /// </summary>
        /// <param name="initialVoltage">I₀: span 시 기준 전압 (예: 4.85V)</param>
        /// <param name="voltageReadings">1초마다 측정된 전압 리스트</param>
        /// <returns>최대 흡광도 Am</returns>
        public static double CalculateMaxAbsorbance(double initialVoltage, List<double> voltageReadings)
        {
            if (initialVoltage <= 0)
                throw new ArgumentException("Initial voltage (I₀) must be greater than 0.");

            if (voltageReadings == null || voltageReadings.Count == 0)
                throw new ArgumentException("Voltage readings list is empty.");

            // 0 로그 방지: 0 이하 값은 작은 값으로 대체
            double minVoltage = voltageReadings.Min(v => v <= 0 ? 0.0001 : v);

            return Math.Round(Math.Log10(initialVoltage / minVoltage), 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 투과율 (%)을 기반으로 Am 계산 (100이 I₀ 기준)
        /// </summary>
        public static double CalculateAmFromTransmissionPercent(double minTransmissionPercent)
        {
            if (minTransmissionPercent <= 0)
                throw new ArgumentException("Transmission percent must be greater than 0.");

            return Math.Log10(100.0 / minTransmissionPercent);
        }

        // 불꽃이 꺼지고 나서 5분 동안에 빛의 투과 감소가 없으면 시험이 종료된 것으로 간주하기 위해서 - 빛의 투과 감소 여부 체크
        public static bool CheckTransmissionStabilized(List<double> recentValues, double tolerance, double duration)
        {
            double StabilityToleranceRatio = tolerance;

            if (recentValues == null || recentValues.Count < duration * 60)
                return false;

            double max = recentValues.Max();
            double min = recentValues.Min();

            double ratioDiff = (max - min) / max;

            return ratioDiff <= StabilityToleranceRatio;
        }
    }
}
