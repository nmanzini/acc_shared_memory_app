using System.IO.MemoryMappedFiles;

Console.WriteLine("ACC to Console!");

String MEMORY_LOCATION = "Local\\acpmf_physics";

//sources
// https://docs.microsoft.com/en-us/dotnet/standard/io/memory-mapped-files
//https://docs.microsoft.com/en-us/dotnet/api/system.io.memorymappedfiles.memorymappedviewstream?view=net-6.0


using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(MEMORY_LOCATION))
{
    runStream(mmf);
}

static void runStream(MemoryMappedFile aMemoryMappedFile)
{
    using (MemoryMappedViewStream myStream = aMemoryMappedFile.CreateViewStream())
    {
        BinaryReader myReader = new BinaryReader(myStream);
        Console.WriteLine("Shake and bake");
        int mySleepMillis = 100;
        int myPrevPacketId = 0;

        while (true)
        {
            int myPacketId = myReader.ReadInt32();
            singleReadAndWrite(myReader, myPacketId, mySleepMillis);

            myPrevPacketId = myPacketId;
            myStream.Position = 0;
            Thread.Sleep(mySleepMillis);
        }
    }
}

static void singleReadAndWrite(BinaryReader aReader, int aPacketId, int aSleepMillis)
{
    String myOutput = String.Format("packetId: {1} gas: {2:F3} brake: {3:F3} fuel: {4:F3} gear: {5} rpm: {6}   (sleep: {0})",
        aSleepMillis, aPacketId, aReader.ReadSingle(), aReader.ReadSingle(), aReader.ReadSingle(), aReader.ReadInt32(), aReader.ReadInt32());
    Console.WriteLine(myOutput);
}