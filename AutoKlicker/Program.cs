using System;
using System.Runtime.InteropServices;
using System.Threading;

class AutoClicker
{
    // Windows API-Funktionen zum Senden von Maus-Events und Hooks
    [DllImport("user32.dll", SetLastError = true)]
    private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;

    // Variablen für den Auto-Klicker
    private static bool isRunning = false;
    private static Thread? clickerThread;

    // Funktion, um einen Linksklick zu simulieren
    static void LeftClick()
    {
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        Thread.Sleep(10);
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }

    static void StartClicker()
    {
        isRunning = true;
        clickerThread = new Thread(() =>
        {
            while (isRunning)
            {
                LeftClick();
                Thread.Sleep(0); // Intervall zwischen Klicks in Millisekunden
            }
        });
        clickerThread.Start();
        Console.WriteLine("AutoClicker gestartet.");
    }

    static void StopClicker()
    {
        isRunning = false;
        if (clickerThread != null && clickerThread.IsAlive)
        {
            clickerThread.Join();
        }
        Console.WriteLine("AutoClicker gestoppt.");
    }

    static void Main()
    {
        Console.WriteLine("Drücke 'S', um den Klicker zu starten und 'Q', um ihn zu beenden.");

        while (true)
        {
            // 'S'-Taste starten und 'Q'-Taste stoppen den Auto-Klicker
            if (GetAsyncKeyState(0x53) < 0 && !isRunning) // 0x53 ist der virtuelle Keycode für 'S'
            {
                StartClicker();
            }
            else if (GetAsyncKeyState(0x51) < 0 && isRunning) // 0x51 ist der virtuelle Keycode für 'Q'
            {
                StopClicker();
            }

            Thread.Sleep(50); // Kurze Pause, um die CPU-Auslastung zu senken
        }
    }
}
