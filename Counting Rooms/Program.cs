using System.Text;

namespace Counting_Rooms;

internal class A : ISolve
{
    public void Solve(StreamReader reader, StreamWriter writer)
    {
        GetInput(reader, out var n, out var m, out var chars);
        ReadOnlySpan<(int, int)> dir = [(1, 0), (0, 1), (-1, 0), (0, -1)];
    }

    private static void FloodFill(
        in char[][] chars, in bool[][] visited,
        in int currentX, in int currentY,
        in int n, in int m,
        ReadOnlySpan<(int, int)> dir,
        char floorChar
    )
    {
        Queue<(int y, int x)> queue = new();
        queue.Enqueue((currentX, currentY));
        visited[currentX][currentY] = true;

        while (queue.Count > 0)
        {
            var (y, x) = queue.Dequeue();

            foreach (var (dy, dx) in dir)
            {
                var canEnqueue = CanEnqueue(chars, visited, n, m, floorChar, y, dy, x, dx, out var ny, out var nx);
                if (!canEnqueue) continue;
                queue.Enqueue((ny, nx));
                visited[ny][nx] = true;
            }
        }
    }

    private static bool CanEnqueue(
        char[][] chars, bool[][] visited, 
        int n, int m, char floorChar, 
        int y, int dy, int x,
        int dx, out int ny, out int nx
        )
    {
        ny = y + dy;
        nx = x + dx;
        if (!IsInBound(n, m, ny, nx)) return false;
        if (chars[ny][nx] != floorChar) return false;
        return visited[ny][nx];
    }

    private static bool IsInBound(int n, int m, int ny, int nx)
    {
        return ny >= 0 && ny < n && nx >= 0 && nx < m;
    }

    private static void GetInput(StreamReader reader, out int n, out int m, out char[][] chars)
    {
        (n, m) = reader.NextInt2();
        chars = new char[n][];
        for (int y = 0; y < n; y++)
        {
            var line = reader.NextToken();
            chars[y] = new char[line.Length];
            for (var x = 0; x < line.Length; x++) chars[y][x] = line[x];
        }
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
        out StreamWriter writer)
    {
        string problemFolder = Path.Combine(ContestDirectory, problem);
        _isLocalCasePresentAfterFirstCheck = ReaderTryGetLocalFileOrConsoleLog(
            caseNumber, problemFolder, InFileTextFileFormat, _isLocalCasePresentAfterFirstCheck,
            out reader);

        if (caseNumber > 1 && !_isLocalCasePresentAfterFirstCheck)
        {
            reader = null!;
            writer = null!;
            return false;
        }

        WriterTryGetLocalFileOrConsoleLog(
            caseNumber, problemFolder, OutFileTextFileFormat, _isLocalCasePresentAfterFirstCheck,
            out writer);

        return _isLocalCasePresentAfterFirstCheck || caseNumber == 1;
    }

    private static bool ReaderTryGetLocalFileOrConsoleLog(
        int caseNumber, string problemFolder, string inputCaseFormat,
        bool isLocalCasePresent, out StreamReader reader)
    {
        if (isLocalCasePresent)
        {
            string fileName = string.Format(inputCaseFormat, caseNumber);
            string inputFilePath = Path.Combine(problemFolder, fileName);
            if (File.Exists(inputFilePath))
            {
                reader = new StreamReader(inputFilePath, Encoding.UTF8, false, 1024 * 32);
                return true;
            }
        }

        reader = new StreamReader(Console.OpenStandardInput(1024 * 32), Encoding.ASCII, false, 1024 * 32);
        return false;
    }

    private static void WriterTryGetLocalFileOrConsoleLog(
        int caseNumber, string problemFolder, string outputCaseFormat,
        bool isLocalCasePresent, out StreamWriter writer)
    {
        if (isLocalCasePresent)
        {
            string fileName = string.Format(outputCaseFormat, caseNumber);
            string outputFilePath = Path.Combine(problemFolder, fileName);
            writer = new StreamWriter(outputFilePath, false, Encoding.UTF8, 1024 * 32);
            return;
        }

        writer = new StreamWriter(Console.OpenStandardOutput(1024 * 32), Encoding.ASCII, 1024 * 32);
    }

    #endregion
}

#region Fast I/O Extensions

public static class Reader
{
    // Fast integer reading
    public static int NextInt(this StreamReader reader)
    {
        int c, res = 0, sign = 1;
        do
        {
            c = reader.Read();
        } while (c < '-');

        if (c == '-')
        {
            sign = -1;
            c = reader.Read();
        }

        do
        {
            res = res * 10 + c - '0';
            c = reader.Read();
        } while (c >= '0' && c <= '9');

        return res * sign;
    }

    // Fast long reading
    public static long NextLong(this StreamReader reader)
    {
        int c;
        long res = 0, sign = 1;
        do
        {
            c = reader.Read();
        } while (c < '-');

        if (c == '-')
        {
            sign = -1;
            c = reader.Read();
        }

        do
        {
            res = res * 10 + c - '0';
            c = reader.Read();
        } while (c >= '0' && c <= '9');

        return res * sign;
    }

    // Read double
    public static double NextDouble(this StreamReader reader)
    {
        return double.Parse(NextToken(reader));
    }

    // Read next token (word)
    public static string NextToken(this StreamReader reader)
    {
        int c;
        var sb = new StringBuilder();
        do
        {
            c = reader.Read();
        } while (c != -1 && char.IsWhiteSpace((char)c));

        while (c != -1 && !char.IsWhiteSpace((char)c))
        {
            sb.Append((char)c);
            c = reader.Read();
        }

        return sb.ToString();
    }

    // Read line
    public static string NextLine(this StreamReader reader)
    {
        return reader.ReadLine();
    }

    // Read array of integers
    public static int[] NextIntArray(this StreamReader reader, int n)
    {
        int[] arr = new int[n];
        for (int i = 0; i < n; i++)
            arr[i] = reader.NextInt();
        return arr;
    }

    // Read array of longs
    public static long[] NextLongArray(this StreamReader reader, int n)
    {
        long[] arr = new long[n];
        for (int i = 0; i < n; i++)
            arr[i] = reader.NextLong();
        return arr;
    }

    // Read 2D array
    public static int[][] Next2DIntArray(this StreamReader reader, int rows, int cols)
    {
        int[][] arr = new int[rows][];
        for (int i = 0; i < rows; i++)
            arr[i] = reader.NextIntArray(cols);
        return arr;
    }

    // Read multiple integers in one line
    public static (int, int) NextInt2(this StreamReader reader)
    {
        return (reader.NextInt(), reader.NextInt());
    }

    public static (int, int, int) NextInt3(this StreamReader reader)
    {
        return (reader.NextInt(), reader.NextInt(), reader.NextInt());
    }

    public static (long, long) NextLong2(this StreamReader reader)
    {
        return (reader.NextLong(), reader.NextLong());
    }
}

#endregion

#region Math Utilities

public static class MathUtils
{
    public const long MOD = 1_000_000_007;
    public const long MOD2 = 998_244_353;

    // GCD
    public static long GCD(long a, long b)
    {
        while (b != 0)
        {
            long t = b;
            b = a % b;
            a = t;
        }

        return a;
    }

    // LCM
    public static long LCM(long a, long b)
    {
        return a / GCD(a, b) * b;
    }

    // Modular exponentiation
    public static long ModPow(long baseNum, long exp, long mod = MOD)
    {
        long result = 1;
        baseNum %= mod;
        while (exp > 0)
        {
            if ((exp & 1) == 1)
                result = (result * baseNum) % mod;
            baseNum = (baseNum * baseNum) % mod;
            exp >>= 1;
        }

        return result;
    }

    // Modular inverse (using Fermat's little theorem, mod must be prime)
    public static long ModInv(long a, long mod = MOD)
    {
        return ModPow(a, mod - 2, mod);
    }

    // Check if prime
    public static bool IsPrime(long n)
    {
        if (n < 2) return false;
        if (n == 2) return true;
        if (n % 2 == 0) return false;
        for (long i = 3; i * i <= n; i += 2)
            if (n % i == 0)
                return false;
        return true;
    }

    // Sieve of Eratosthenes
    public static bool[] Sieve(int n)
    {
        bool[] prime = new bool[n + 1];
        Array.Fill(prime, true);
        prime[0] = prime[1] = false;
        for (int i = 2; i * i <= n; i++)
        {
            if (prime[i])
            {
                for (int j = i * i; j <= n; j += i)
                    prime[j] = false;
            }
        }

        return prime;
    }

    // Factorial with mod
    public static long[] GetFactorials(int n, long mod = MOD)
    {
        long[] fact = new long[n + 1];
        fact[0] = 1;
        for (int i = 1; i <= n; i++)
            fact[i] = (fact[i - 1] * i) % mod;
        return fact;
    }

    // nCr with mod
    public static long NCR(int n, int r, long mod = MOD)
    {
        if (r > n || r < 0) return 0;
        if (r == 0 || r == n) return 1;
        long[] fact = GetFactorials(n, mod);
        return (fact[n] * ModInv(fact[r], mod) % mod) * ModInv(fact[n - r], mod) % mod;
    }
}

#endregion

#region Array/Collection Utilities

public static class ArrayUtils
{
    // Binary search (returns index or -1)
    public static int BinarySearch<T>(T[] arr, T target) where T : IComparable<T>
    {
        int left = 0, right = arr.Length - 1;
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            int cmp = arr[mid].CompareTo(target);
            if (cmp == 0) return mid;
            if (cmp < 0) left = mid + 1;
            else right = mid - 1;
        }

        return -1;
    }

    // Lower bound (first element >= target)
    public static int LowerBound<T>(T[] arr, T target) where T : IComparable<T>
    {
        int left = 0, right = arr.Length;
        while (left < right)
        {
            int mid = left + (right - left) / 2;
            if (arr[mid].CompareTo(target) < 0)
                left = mid + 1;
            else
                right = mid;
        }

        return left;
    }

    // Upper bound (first element > target)
    public static int UpperBound<T>(T[] arr, T target) where T : IComparable<T>
    {
        int left = 0, right = arr.Length;
        while (left < right)
        {
            int mid = left + (right - left) / 2;
            if (arr[mid].CompareTo(target) <= 0)
                left = mid + 1;
            else
                right = mid;
        }

        return left;
    }

    // Prefix sum
    public static long[] PrefixSum(int[] arr)
    {
        long[] prefix = new long[arr.Length + 1];
        for (int i = 0; i < arr.Length; i++)
            prefix[i + 1] = prefix[i] + arr[i];
        return prefix;
    }

    // 2D prefix sum
    public static long[,] PrefixSum2D(int[,] arr)
    {
        int n = arr.GetLength(0), m = arr.GetLength(1);
        long[,] prefix = new long[n + 1, m + 1];
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                prefix[i, j] = arr[i - 1, j - 1] + prefix[i - 1, j] +
                    prefix[i, j - 1] - prefix[i - 1, j - 1];
            }
        }

        return prefix;
    }

    // Reverse array
    public static void Reverse<T>(T[] arr)
    {
        Array.Reverse(arr);
    }

    // Sort with custom comparator
    public static void Sort<T>(T[] arr, Comparison<T> comparison)
    {
        Array.Sort(arr, comparison);
    }
}

#endregion

#region Graph Utilities

public static class GraphUtils
{
    // DFS
    public static void DFS(List<int>[] graph, int node, bool[] visited, Action<int> action = null)
    {
        visited[node] = true;
        action?.Invoke(node);
        foreach (int neighbor in graph[node])
        {
            if (!visited[neighbor])
                DFS(graph, neighbor, visited, action);
        }
    }

    // BFS
    public static void BFS(List<int>[] graph, int start, Action<int> action = null)
    {
        bool[] visited = new bool[graph.Length];
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(start);
        visited[start] = true;

        while (queue.Count > 0)
        {
            int node = queue.Dequeue();
            action?.Invoke(node);
            foreach (int neighbor in graph[node])
            {
                if (!visited[neighbor])
                {
                    visited[neighbor] = true;
                    queue.Enqueue(neighbor);
                }
            }
        }
    }

    // Dijkstra's algorithm
    public static long[] Dijkstra(List<(int, long)>[] graph, int start)
    {
        int n = graph.Length;
        long[] dist = new long[n];
        Array.Fill(dist, long.MaxValue);
        dist[start] = 0;

        var pq = new PriorityQueue<(int node, long dist), long>();
        pq.Enqueue((start, 0), 0);

        while (pq.Count > 0)
        {
            var (node, d) = pq.Dequeue();
            if (d > dist[node]) continue;

            foreach (var (neighbor, weight) in graph[node])
            {
                long newDist = dist[node] + weight;
                if (newDist < dist[neighbor])
                {
                    dist[neighbor] = newDist;
                    pq.Enqueue((neighbor, newDist), newDist);
                }
            }
        }

        return dist;
    }
}

#endregion

#region String Utilities

public static class StringUtils
{
    // Check palindrome
    public static bool IsPalindrome(string s)
    {
        int left = 0, right = s.Length - 1;
        while (left < right)
        {
            if (s[left++] != s[right--])
                return false;
        }

        return true;
    }

    // Count character frequency
    public static Dictionary<char, int> CharFrequency(string s)
    {
        var freq = new Dictionary<char, int>();
        foreach (char c in s)
        {
            if (!freq.ContainsKey(c))
                freq[c] = 0;
            freq[c]++;
        }

        return freq;
    }
}

#endregion

#region Debug Utilities

public static class Debug
{
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Print<T>(T value, string label = "")
    {
        Console.Error.WriteLine($"{label}{(label != "" ? ": " : "")}{value}");
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void PrintArray<T>(T[] arr, string label = "")
    {
        Console.Error.WriteLine($"{label}{(label != "" ? ": " : "")}{string.Join(" ", arr)}");
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void Print2DArray<T>(T[,] arr, string label = "")
    {
        if (label != "") Console.Error.WriteLine(label + ":");
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
                Console.Error.Write(arr[i, j] + " ");
            Console.Error.WriteLine();
        }
    }
}

#endregion