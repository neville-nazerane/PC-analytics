using LibreHardwareMonitor.Hardware;
using LibreHardwareMonitor.Hardware.Motherboard;
using PcAnalytics.Models.Entities;
using Computer = LibreHardwareMonitor.Hardware.Computer;


namespace PcAnalytics.ClientApi.Service
{
    public class InfoService
    {

        public string? Serial { get; private set; }

        public void Init()
        {
            Console.WriteLine("Fetching computer info...");
            var computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsMotherboardEnabled = true,
                IsControllerEnabled = true
            };
            computer.Open();

            Serial = ((Motherboard)computer.Hardware.Single(h => h.HardwareType == HardwareType.Motherboard)).SMBios.Board.SerialNumber;
            Console.WriteLine("Done fetching computer info");
        }

    }
}
