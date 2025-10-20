using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;

namespace LaFokinCabra
{
    public partial class MainPage : ContentPage
    {
        // Constructor de la clase
        public MainPage()
        {
            InitializeComponent();
            // Mensajes iniciales
            AccelValue.Text = "—";
            CompassValue.Text = "—";
            BarometerValue.Text = "—";
            GyroValue.Text = "—";

            AccelDetail.Text = Accelerometer.Default.IsSupported ? "Compatible" : "No disponible en este dispositivo";
            CompassDetail.Text = Compass.Default.IsSupported ? "Compatible" : "No disponible en este dispositivo";
            BarometerDetail.Text = Barometer.Default.IsSupported ? "Compatible" : "No disponible en este dispositivo";
            GyroDetail.Text = Gyroscope.Default.IsSupported ? "Compatible" : "No disponible en este dispositivo";
        }

        // ====== Acelerómetro ======
        private void AccelSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (!Accelerometer.Default.IsSupported)
            {
                AccelSwitch.IsToggled = false; AccelDetail.Text = "No disponible"; return;
            }
            if (e.Value)
            {
                Accelerometer.Default.ReadingChanged += OnAccelReadingChanged;
                Accelerometer.Default.Start(SensorSpeed.UI);
                AccelDetail.Text = "Leyendo (UI)";
            }
            else
            {
                try { Accelerometer.Default.ReadingChanged -= OnAccelReadingChanged; } catch { }
                try { Accelerometer.Default.Stop(); } catch { }
                AccelDetail.Text = "Detenido";
                AccelValue.Text = "—";
            }
            UpdateStatus();
        }

        private void OnAccelReadingChanged(object? sender, AccelerometerChangedEventArgs e)
        {
            var v = e.Reading.Acceleration;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                AccelValue.Text = $"X {v.X,6:F2}  Y {v.Y,6:F2}  Z {v.Z,6:F2} g";
                AccelDetail.Text = "Muestreo UI (aprox 60 Hz)";
            });
        }

        // ====== Brújula ======
        private void CompassSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (!Compass.Default.IsSupported)
            {
                CompassSwitch.IsToggled = false; CompassDetail.Text = "No disponible"; return;
            }
            if (e.Value)
            {
                Compass.Default.ReadingChanged += OnCompassReadingChanged;
                Compass.Default.Start(SensorSpeed.UI);
                CompassDetail.Text = "Leyendo (UI)";
            }
            else
            {
                try { Compass.Default.ReadingChanged -= OnCompassReadingChanged; } catch { }
                try { Compass.Default.Stop(); } catch { }
                CompassDetail.Text = "Detenido";
                CompassValue.Text = "—";
            }
            UpdateStatus();
        }

        private void OnCompassReadingChanged(object? sender, CompassChangedEventArgs e)
        {
            var heading = e.Reading.HeadingMagneticNorth; // 0–360°
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CompassValue.Text = $"{heading:F0}°";
                CompassDetail.Text = "Respecto al norte magnético";
            });
        }

        // ====== Barómetro ======
        private void BarometerSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (!Barometer.Default.IsSupported)
            {
                BarometerSwitch.IsToggled = false; BarometerDetail.Text = "No disponible"; return;
            }
            if (e.Value)
            {
                Barometer.Default.ReadingChanged += OnBarometerReadingChanged;
                Barometer.Default.Start(SensorSpeed.UI);
                BarometerDetail.Text = "Leyendo (UI)";
            }
            else
            {
                try { Barometer.Default.ReadingChanged -= OnBarometerReadingChanged; } catch { }
                try { Barometer.Default.Stop(); } catch { }
                BarometerDetail.Text = "Detenido";
                BarometerValue.Text = "—";
            }
            UpdateStatus();
        }

        private void OnBarometerReadingChanged(object? sender, BarometerChangedEventArgs e)
        {
            var hPa = e.Reading.PressureInHectopascals;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                BarometerValue.Text = $"{hPa:F1} hPa";
                BarometerDetail.Text = "Presión atmosférica";
            });
        }

        // ====== Giroscopio ======
        private void GyroSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (!Gyroscope.Default.IsSupported)
            {
                GyroSwitch.IsToggled = false; GyroDetail.Text = "No disponible"; return;
            }
            if (e.Value)
            {
                Gyroscope.Default.ReadingChanged += OnGyroReadingChanged;
                Gyroscope.Default.Start(SensorSpeed.UI);
                GyroDetail.Text = "Leyendo (UI)";
            }
            else
            {
                try { Gyroscope.Default.ReadingChanged -= OnGyroReadingChanged; } catch { }
                try { Gyroscope.Default.Stop(); } catch { }
                GyroDetail.Text = "Detenido";
                GyroValue.Text = "—";
            }
            UpdateStatus();
        }

        private void OnGyroReadingChanged(object? sender, GyroscopeChangedEventArgs e)
        {
            var v = e.Reading.AngularVelocity; // rad/s
            MainThread.BeginInvokeOnMainThread(() =>
            {
                GyroValue.Text = $"X {v.X,6:F2}  Y {v.Y,6:F2}  Z {v.Z,6:F2} rad/s";
                GyroDetail.Text = "Velocidad angular";
            });
        }

        // ====== Botones globales ======
        private void StartAll_Clicked(object sender, EventArgs e)
        {
            if (Accelerometer.Default.IsSupported) AccelSwitch.IsToggled = true;
            if (Compass.Default.IsSupported) CompassSwitch.IsToggled = true;
            if (Barometer.Default.IsSupported) BarometerSwitch.IsToggled = true;
            if (Gyroscope.Default.IsSupported) GyroSwitch.IsToggled = true;
            UpdateStatus();
        }

        private void StopAll_Clicked(object sender, EventArgs e)
        {
            AccelSwitch.IsToggled = false;
            CompassSwitch.IsToggled = false;
            BarometerSwitch.IsToggled = false;
            GyroSwitch.IsToggled = false;
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            var active = (AccelSwitch.IsToggled ? 1 : 0)
                      + (CompassSwitch.IsToggled ? 1 : 0)
                      + (BarometerSwitch.IsToggled ? 1 : 0)
                      + (GyroSwitch.IsToggled ? 1 : 0);
            StatusLabel.Text = active > 0 ? $"Sensores activos: {active}" : "Listo";
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Asegurar liberación
            try { Accelerometer.Default.Stop(); } catch { }
            try { Compass.Default.Stop(); } catch { }
            try { Barometer.Default.Stop(); } catch { }
            try { Gyroscope.Default.Stop(); } catch { }
        }
    }

}