using Microsoft.Maui.Devices.Sensors;

namespace TasteNote.Services;

public class SensorService
{
    private bool _isMonitoring;
    private double _lastX, _lastY, _lastZ;
    private bool _hasLastReading;
    private const double ShakeThreshold = 1.5;
    private DateTime _lastShakeTime = DateTime.MinValue;
    private const int ShakeCooldownMs = 500;

    public event Action? ShakeDetected;

    public void StartShakeDetection()
    {
        try
        {
            if (_isMonitoring) return;

            if (!Accelerometer.IsSupported)
            {
                System.Diagnostics.Debug.WriteLine("[SensorService] 设备不支持加速度计");
                return;
            }

            if (!Accelerometer.Default.IsMonitoring)
            {
                Accelerometer.Default.ReadingChanged += OnAccelerationChanged;
                Accelerometer.Default.Start(SensorSpeed.Game);
            }

            _isMonitoring = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[SensorService] 启动摇晃检测失败: {ex.Message}");
        }
    }

    public void StopShakeDetection()
    {
        try
        {
            if (!_isMonitoring) return;

            if (Accelerometer.Default.IsMonitoring)
            {
                Accelerometer.Default.Stop();
                Accelerometer.Default.ReadingChanged -= OnAccelerationChanged;
            }

            _isMonitoring = false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[SensorService] 停止摇晃检测失败: {ex.Message}");
        }
    }

    private void OnAccelerationChanged(object? sender, AccelerometerChangedEventArgs e)
    {
        try
        {
            var current = e.Reading.Acceleration;

            if (_hasLastReading)
            {
                var deltaX = Math.Abs(current.X - _lastX);
                var deltaY = Math.Abs(current.Y - _lastY);
                var deltaZ = Math.Abs(current.Z - _lastZ);

                var totalDelta = deltaX + deltaY + deltaZ;

                if (totalDelta > ShakeThreshold)
                {
                    var now = DateTime.Now;
                    if ((now - _lastShakeTime).TotalMilliseconds > ShakeCooldownMs)
                    {
                        _lastShakeTime = now;
                        ShakeDetected?.Invoke();
                    }
                }
            }

            _lastX = current.X;
            _lastY = current.Y;
            _lastZ = current.Z;
            _hasLastReading = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[SensorService] 处理加速度数据失败: {ex.Message}");
        }
    }

    public void StartCompass(Action<double> onHeadingChanged)
    {
        try
        {
            if (!Compass.IsSupported)
            {
                System.Diagnostics.Debug.WriteLine("[SensorService] 设备不支持指南针");
                return;
            }

            if (!Compass.Default.IsMonitoring)
            {
                Compass.Default.ReadingChanged += (sender, e) =>
                {
                    try
                    {
                        onHeadingChanged(e.Reading.HeadingMagneticNorth);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[SensorService] 指南针回调失败: {ex.Message}");
                    }
                };
                Compass.Default.Start(SensorSpeed.Game);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[SensorService] 启动指南针失败: {ex.Message}");
        }
    }

    public void StopCompass()
    {
        try
        {
            if (Compass.Default.IsMonitoring)
            {
                Compass.Default.Stop();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[SensorService] 停止指南针失败: {ex.Message}");
        }
    }
}
