using System.Collections.ObjectModel;
using System.Windows;

namespace OpenTURZXStudio.Desktop
{
    public partial class MainWindow : Window
    {
        public static readonly ObservableCollection<string> SampleFiles = new()
        {
            "imagem1.jpg",
            "imagem2.png",
            "animacao.gif",
            "photo.bmp"
        };

        public static readonly ObservableCollection<string> SamplePorts = new()
        {
            "COM3 (USB Serial Port)",
            "COM4 (Arduino Uno)"
        };

        public static readonly ObservableCollection<string> SampleUsbDevices = new()
        {
            "TURZX Device v1.0",
            "USB Mass Storage Device"
        };

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}