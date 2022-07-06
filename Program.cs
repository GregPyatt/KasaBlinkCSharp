using TPLinkSmartDevices;
using TPLinkSmartDevices.Devices;

class Program
{
         
    static void Main(string[] args)
    {
        // See https://aka.ms/new-console-template for more information
        Console.WriteLine("Here we go, baby!");
        // Program prog = new Program();
        // prog.SetTimers();
        RandomTimeBlinks rtb = new RandomTimeBlinks();
        rtb.SetTimers();
    }
}

 
public class RandomTimeBlinks{
    bool powerState = true;
    Random timer = new Random();
    double displayTime = 0;
    int sleepTime = 3000;

    public RandomTimeBlinks(){
    }

       public void SetTimers(){
        for (int x=0; x<1000; x++){
            displayTime = (timer.NextDouble() * 15) + 1;
            sleepTime = (int)displayTime * 60000;
            FlipTheSwitch();
            Thread.Sleep(sleepTime);
            Console.WriteLine("x = " + x);
        }
    }
        public async void FlipTheSwitch(){
        //Bug 1 is 192.168.1.10
        //Family Room Lamp is 192.168.1.3
        powerState = powerState ? false : true;
        Console.WriteLine("Next switching event in " + Math.Round(displayTime, 2) + " minutes.");
        var smartPlug = await TPLinkSmartPlug.Create("192.168.1.10");
        await smartPlug.SetPoweredOn(powerState); // Turn on/off relay 
    }
}