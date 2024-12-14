using System.Text;

namespace CSharpProblemSolve;

internal class A : ISolve
{
    public void Solve(StreamReader reader, StreamWriter writer)
    {
        
    }
}

internal interface ISolve
{
    void Solve(StreamReader reader, StreamWriter writer);
}

internal abstract class Template
{
    private static readonly ISolve Solver = new A();

    #region Setup

    private const string ContestDirectory = @"D:\CSharpProblemSolve\CSharpProblemSolve";
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