using TPLinkSmartDevices;
using TPLinkSmartDevices.Devices;

class Program
{
         
    static void Main(string[] args)
    {
        // DotNet recently got rid of much of the ceremony code in console apps, but I kick it old school.
        // See https://aka.ms/new-console-template for more information
        Console.WriteLine("Here we go, baby!");
        RandomTimeBlinks rtb = new RandomTimeBlinks();
        rtb.SetTimers();
    }
}

 
public class RandomTimeBlinks
{
    Random timer = new Random();
    double displayTime = 0;
    int sleepTime = 3000;
    Dictionary<string, string> PlugIPs = new Dictionary<string, string>();

    public RandomTimeBlinks(){
        // I just like having the constructor here.  It makes me feel better.
        // I may use this later for passing arguments into the console start.
        PlugIPs.Add("Bathroom Fan", "192.168.1.5");
        PlugIPs.Add("Bug 1", "192.168.1.10");
        PlugIPs.Add("Bug 2", "192.168.1.22");
        PlugIPs.Add("Family Room Lamp", "192.168.1.3");
        PlugIPs.Add("Bedroom Fan", "192.168.1.2");
        PlugIPs.Add("Bedroom Table Lamp", "192.168.1.9");
        PlugIPs.Add("Stereo & Bluetooth", "192.168.1.7");
        PlugIPs.Add("Bedroom Floor Lamp", "192.168.1.8");
    }

    public void SetTimers()
    {
        while (true)
        {
            displayTime = (timer.NextDouble() * 20) + 1;
            sleepTime = (int)displayTime * 60000;  //60,000 is the number of miliseconds in a minute, naturally.
            FlipTheSwitch();
            Thread.Sleep(sleepTime);
        }
    }

    public async void FlipTheSwitch()
    {
        //The TP-Link switch named "Bug 1" is at 192.168.1.10
        //The TP-Link switch named "Family Room Lamp" is at 192.168.1.3
        powerState = powerState ? false : true;
        Console.WriteLine("Switching to " + powerState.ToString() + " Next switching event in " + Math.Round(displayTime, 2) + " minutes.");
        var smartPlug = await TPLinkSmartPlug.Create("192.168.1.10");
        await smartPlug.SetPoweredOn(powerState); // Turn on/off relay 
    }
}