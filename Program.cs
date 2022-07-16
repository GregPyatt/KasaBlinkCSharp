using TPLinkSmartDevices;
using TPLinkSmartDevices.Devices;

class Program
{
         
    static void Main(string[] args)
    {
        double maxWaitTime = 0;
        // DotNet recently got rid of much of the ceremony code in console apps, but I kick it old school.
        // See https://aka.ms/new-console-template for more information
        // Get args
        if (args.Length == 0)
        {
            // Display message to user to provide parameters.
            Console.WriteLine("Please enter maximum number of minutes between blinks.");
            if (!double.TryParse(Console.ReadLine(), out maxWaitTime)) {
                Console.WriteLine("There was an error while args.Length==0");
            }
        }else{
            // Loop through array to list args parameters.
            for (int i = 0; i < args.Length; i++)
            {
                Console.Write(args[i] + Environment.NewLine);
            }
            if (!double.TryParse(args[0], out maxWaitTime)) {
                Console.WriteLine("There was an error while args.Length!=0");
            }            
        }
        
        Console.WriteLine("Here we go, baby!");
        RandomTimeBlinks rtb = new RandomTimeBlinks();
        rtb.SetTimers(maxWaitTime);
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
        PlugIPs.Add("Bug 1", "192.168.1.10");
        PlugIPs.Add("Bug 2", "192.168.1.22");
        PlugIPs.Add("Family Room Lamp", "192.168.1.3");
        PlugIPs.Add("Bedroom Fan", "192.168.1.2");
        PlugIPs.Add("Bedroom Table Lamp", "192.168.1.9");
        PlugIPs.Add("Stereo & Bluetooth", "192.168.1.7");
        PlugIPs.Add("Bedroom Floor Lamp", "192.168.1.8");
        PlugIPs.Add("Bathroom Fan", "192.168.1.5");
    }

    public void SetTimers(double maximumWaitTime)
    {
        bool bugSwitch = true;
        while (true)
        {
            displayTime = (timer.NextDouble() * maximumWaitTime);
            //adding safety measure of 10 seconds, but should add error handlers later.
            sleepTime = (int)((displayTime * 60000) + 10000);  //60,000 is the number of miliseconds in a minute, naturally.
            bugSwitch = (bugSwitch) ? false : true;  //Let's just work with Bug 1 & 2 for now.
            FlipTheSwitch(bugSwitch);
            Thread.Sleep(sleepTime);
        }
    }

    public async void FlipTheSwitch(bool bugSwitch)
    {
        string plugName;
        bool powerState;

        switch (bugSwitch)
        {
            case true:
                plugName = "Bug 1";
                break;
            case false:
                plugName = "Bug 2";
                break;
        }
        var smartPlug = await TPLinkSmartPlug.Create(PlugIPs[plugName].ToString());
        powerState = (smartPlug.OutletPowered) ? false : true;
        Console.WriteLine("Switching plug " + plugName + " to " + powerState.ToString() + ".  Next switching event in " + sleepTime / 60000 + " minutes & " + (sleepTime % 60000) / 1000 + " seconds.");
        await smartPlug.SetPoweredOn(powerState); // Turn on/off relay 
    }
}