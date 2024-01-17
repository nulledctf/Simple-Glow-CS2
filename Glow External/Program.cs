using Swed64;

// init swed

Swed swed = new Swed("cs2");

// get client.dll
IntPtr client = swed.GetModuleBase("client.dll");

// offsets

int dwEntityList = 0x17C1950;
int m_hPlayerPawn = 0x7EC;
int m_flDetectedByEnemySensorTime = 0x13E4;

// glow loop

while (true)
{
    // entity list
    IntPtr entityList = swed.ReadPointer(client, dwEntityList);

    // first entry
    IntPtr listEntry = swed.ReadPointer(entityList, 0x10);

    for (int i = 0; i < 64;i++) // 64 controllers
    {
        if (listEntry == IntPtr.Zero)
            continue;

        // current controller
        IntPtr currentController = swed.ReadPointer(listEntry, i * 0x78);

        if (currentController == IntPtr.Zero)
            continue;

        // current pawn
        int pawnHandle = swed.ReadInt(currentController, m_hPlayerPawn);
        if (pawnHandle == 0)
            continue;

        // second entry
        IntPtr listEntry2 = swed.ReadPointer(entityList, 0x8 * ((pawnHandle & 0x7FFF) >> 9) + 0x10);

        IntPtr currentPawn = swed.ReadPointer(listEntry2, 0x78 * (pawnHandle & 0x1FF));

        // force the glow 

        swed.WriteFloat(currentPawn, m_flDetectedByEnemySensorTime, 86400); //86400 makes it glow


        Console.WriteLine($"{i}:  {currentPawn}");
    }
    Thread.Sleep(50);
    Console.Clear();

}