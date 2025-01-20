using System.Text;

namespace Div._4_993;

internal class A : ISolve
{
    public void Solve(StreamReader reader, StreamWriter writer)
    {
        
        int 
        int t = int.Parse(reader.ReadLine()!);
        for (int i = 0; i < t; i++)
        {
            int n = int.Parse(reader.ReadLine()!);
            writer.WriteLine(n - 1);
        }
    }
}

internal class B : ISolve
{
    public void Solve(StreamReader reader, StreamWriter writer)
    {
        int t = int.Parse(reader.ReadLine()!);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < t; i++)
        {
            sb.Clear();
            string s = reader.ReadLine()!;
            for (var index = s.Length - 1; index >= 0; index--)
            {
                var c = s[index];
                if (c == 'q') sb.Append('p');
                else if (c == 'p') sb.Append('q');
                else sb.Append(c);
            }

            writer.WriteLine(sb);
        }
    }
}

internal class C : ISolve
{
    public void Solve(StreamReader reader, StreamWriter writer)
    {
        int t = int.Parse(reader.ReadLine()!);
        for (int i = 0; i < t; i++)
        {
            int m = reader.Next();
            int a = reader.Next();
            int b = reader.Next();
            int c = reader.Next();
            reader.ReadLine();

            int totalSeats = m * 2;

            int aSits = Math.Min(a, m);
            int bSits = Math.Min(b, m);
            int seatFilled = aSits + bSits;
            int seatsLeft = totalSeats - seatFilled;
            if (seatsLeft > 0)
            {
                int cSits = Math.Min(c, seatsLeft);
                seatFilled += cSits;
            }

            writer.WriteLine(seatFilled);
        }
    }
}

internal interface ISolve
{
    void Solve(StreamReader reader, StreamWriter writer);
}

internal abstract class Template
{
    private static readonly ISolve Solver = new D();

    #region Setup

    private const string ContestDirectory = @"D:\CSharpProblemSolve\Div.4 993";
    private const string InFileTextFileFormat = "in {0}.txt";
    private const string OutFileTextFileFormat = "out {0}.txt";
    private static bool _isLocalCasePresentAfterFirstCheck = true;

    public static void Main(string[] args)
    {
        int caseNumber = 1;
        string problem = Solver.GetType().Name;
        while (TryKeepGettingLocalCasesOrSingleConsoleSession(problem, caseNumber, out var reader, out var writer))
        {
            Solver.Solve(reader, writer);
            writer.Flush();
            caseNumber++;
        }
    }

    static bool TryKeepGettingLocalCasesOrSingleConsoleSession(
        string problem, int caseNumber,
        out StreamReader reader,
        out StreamWriter writer
    )
    {
        string problemFolder = Path.Combine(ContestDirectory, problem);
        _isLocalCasePresentAfterFirstCheck = ReaderTryGetLocalFileOrConsoleLog(
            caseNumber, problemFolder, InFileTextFileFormat, _isLocalCasePresentAfterFirstCheck,
            out reader
        );

        if (caseNumber > 1 && !_isLocalCasePresentAfterFirstCheck)
        {
            reader = null!;
            writer = null!;
            return false;
        }


        WriterTryGetLocalFileOrConsoleLog(
            caseNumber, problemFolder, OutFileTextFileFormat, _isLocalCasePresentAfterFirstCheck,
            out writer
        );
        return _isLocalCasePresentAfterFirstCheck || caseNumber == 1;
    }

    private static bool ReaderTryGetLocalFileOrConsoleLog(
        int caseNumber,
        string problemFolder,
        string inputCaseFormat,
        bool isLocalCasePresent,
        out StreamReader reader
    )
    {
        if (isLocalCasePresent)
        {
            string fileName = string.Format(inputCaseFormat, caseNumber);
            string inputFilePath = Path.Combine(problemFolder, fileName);
            if (File.Exists(inputFilePath))
            {
                reader = new StreamReader(inputFilePath, Encoding.UTF8, false, 1024 * 10);
                return true;
            }
        }

        reader = new StreamReader(
            Console.OpenStandardInput(1024 * 10),
            Encoding.ASCII, false, 1024 * 10
        );
        return false;
    }

    private static void WriterTryGetLocalFileOrConsoleLog(int caseNumber,
        string problemFolder,
        string outputCaseFormat,
        bool isLocalCasePresent,
        out StreamWriter writer)
    {
        if (isLocalCasePresent)
        {
            string fileName = string.Format(outputCaseFormat, caseNumber);
            string outputFilePath = Path.Combine(problemFolder, fileName);
            writer = new StreamWriter(outputFilePath, false, Encoding.UTF8, 1024 * 10);
            return;
        }

        writer = new StreamWriter(Console.OpenStandardOutput(1024 * 10), Encoding.ASCII, 1024 * 10);
    }

    #endregion
}

internal class D : ISolve
{
    public void Solve(StreamReader reader, StreamWriter writer)
    {
        int t = int.Parse(reader.ReadLine()!);
        for (int i = 0; i < t; i++)
        {
            int n = int.Parse(reader.ReadLine()!);
            string line = reader.ReadLine()!;
            
            writer.WriteLine();
        }
    }
}

public static class Reader
{
    public static int Next(this StreamReader reader)
    {
        int c;
        int m = 1;
        int res = 0;
        do
        {
            c = reader.Read();
            if (c == '-')
                m = -1;
        } while (c < '0' || c > '9');

        res = c - '0';
        while (true)
        {
            c = reader.Read();
            if (c < '0' || c > '9')
                return m * res;
            res *= 10;
            res += c - '0';
        }
    }
}