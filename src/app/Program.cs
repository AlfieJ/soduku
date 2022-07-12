using System;
using soduku;

internal class Program
{
    private static int Main(string[] args)
    {
        bool create = false;

        for (int i = 0; i < args.Length; ++i)
        {
            string arg = args[i];
            if (arg == "-create" || arg == "-c")
                create = true;
        }

        int status = -1;
        if (create)
            status = CreateBoard();
        else
            ShowUsage();

        return status;
    }

    static int CreateBoard()
    {
        Board board = new Board();
        bool success = board.Populate();

        if (success)
            Console.WriteLine(board.ToString());
        else
            Console.WriteLine("Error generating board");

        return success ? 0 : -1;
    }

    static void ShowUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("soduku [-create || -c]");
    }
}