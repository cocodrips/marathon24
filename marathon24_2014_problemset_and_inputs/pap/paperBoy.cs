using System;
using System.Collections.Generic;
using System.Linq;
using Watch = System.Diagnostics.Stopwatch;
using StringBuilder = System.Text.StringBuilder;
using BitVector = System.Collections.Specialized.BitVector32;
class Solver
{
    Random r = new Random(0);
    int last;
    int m;
    void Solve()
    {
        var n = sc.Integer();
        m = sc.Integer();
        var map = new HashMap<int, int>();
        last = n * m;
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                map[i * m + j] = i * m + j;

        var sw = new Watch();
        var current = getScore(map);
        long end = 60000;
        long now = 0;
        int best=int.MaxValue;
        KeyValuePair<int, int>[] bestSeq = map.ToArray();
        var R=10000;
        sw.Start();
        while (now < end)
        {
            var p = r.Next() % last;
            var q = r.Next() % last;
            if (p == q)
                continue;
            var tmp = map[p];
            map[p] = map[q];
            map[q] = tmp;
            var next = getScore(map);
            var force = R*(end-now)>end*(r.Next()%R);
            if (next > current || force)
            {
                current = next;
            }
            else
            {
                map[q] = map[p];
                map[p] = tmp;
            }
            if (current < best)
            {
                best = current;
                bestSeq = map.ToArray();
            }
            now = sw.ElapsedMilliseconds;

        }
        //Printer.PrintLine(best);
        var ar = n.Enumerate(i => new int[m]);
        foreach (var x in bestSeq)
        {
            var p = x.Value / m;
            var q = x.Value % m;
            ar[p][q] = x.Key + 1;
        }
        foreach(var a in ar)
            Printer.PrintLine(a);
    }
    int getScore(HashMap<int, int> map)
    {
        var visited = new HashMap<int, int>();
        var sx = map[0] / m;
        var sy = map[0] % m;
        for (int i = 1; i < last; i++)
        {
            var nx = map[i] / m;
            var ny = map[i] % m;
            var dist = Math.Abs(sx - nx) + Math.Abs(sy - ny);
            sx = nx;
            sy = ny;
            visited[dist]++;
        }
        var max = -1;
        foreach (var x in visited)
        {
            max = Math.Max(max, x.Value);
        }
        return max;

    }
    #region Main and Settings
    static void Main()
    {
        const string txt = @"C:\Users\camiya\Dropbox\Programming\competition\marathon24\marathon24\marathon24_2014_problemset_and_inputs\pap\pap10";
#if DEBUG

        var eStream = new System.IO.FileStream("debug.txt", System.IO.FileMode.Create, System.IO.FileAccess.Write);
        var iStream = new System.IO.FileStream(txt + ".in", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
        Console.SetIn(new System.IO.StreamReader(iStream));
#if TEST
        var oWriter = new System.IO.StreamWriter(new System.IO.FileStream(txt + ".out", System.IO.FileMode.Create, System.IO.FileAccess.Write)) { NewLine = "\n", AutoFlush = true };
        Printer.SetOut(oWriter);
#endif

        System.Diagnostics.Debug.AutoFlush = true;
        System.Diagnostics.Debug.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(new System.IO.StreamWriter(eStream) { NewLine = "\n" }));
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        try
        {
#endif
            var solver = new Solver();
            solver.Solve();
#if DEBUG
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            Console.ReadKey(true);
            return;
        }
#endif
#if TEST
#elif DEBUG
        sw.Stop();

        Console.ForegroundColor = ConsoleColor.Green;
        Printer.PrintLine("Time:{0}ms", sw.ElapsedMilliseconds);
        Console.ReadKey(true);
#endif
    }
    _Scanner sc = new _Scanner();
    #endregion

}
#region IO Helper
static public class Printer
{
    static private System.IO.TextWriter writer;
    static readonly private System.Globalization.CultureInfo info;
    static string Separator { get; set; }
    static Printer()
    {
        SetOut(Console.Out);
        info = System.Globalization.CultureInfo.InvariantCulture;
        Separator = " ";
    }
    public static void SetOut(System.IO.TextWriter writer)
    {
        Printer.writer = writer;
        Printer.writer.NewLine = "\n";
    }
    static public void Print(int num) { writer.Write(num.ToString(info)); }
    static public void Print(int num, string format) { writer.Write(num.ToString(format, info)); }
    static public void Print(long num) { writer.Write(num.ToString(info)); }
    static public void Print(long num, string format) { writer.Write(num.ToString(format, info)); }
    static public void Print(double num) { writer.Write(num.ToString(info)); }
    static public void Print(double num, string format) { writer.Write(num.ToString(format, info)); }
    static public void Print(string str) { writer.Write(str); }
    static public void Print(string format, params object[] arg) { writer.Write(format, arg); }
    static public void Print<T>(string format, IEnumerable<T> sources) { writer.Write(format, sources.OfType<object>().ToArray()); }
    static public void Print(IEnumerable<char> sources) { writer.Write(sources.AsString()); }
    static public void Print(params object[] arg)
    {
        var res = new System.Text.StringBuilder();
        foreach (var x in arg)
        {
            res.AppendFormat(info, "{0}", x);
            if (!string.IsNullOrEmpty(Separator))
                res.Append(Separator);
        }
        writer.Write(res.ToString(0, res.Length - Separator.Length));
    }
    static public void Print<T>(IEnumerable<T> sources)
    {
        var res = new System.Text.StringBuilder();
        foreach (var x in sources)
        {
            res.AppendFormat(info, "{0}", x);
            if (string.IsNullOrEmpty(Separator))
                res.Append(Separator);
        }
        writer.Write(res.ToString(0, res.Length - Separator.Length));
    }
    static public void PrintLine(int num) { writer.WriteLine(num.ToString(info)); }
    static public void PrintLine(int num, string format) { writer.WriteLine(num.ToString(format, info)); }
    static public void PrintLine(long num) { writer.WriteLine(num.ToString(info)); }
    static public void PrintLine(long num, string format) { writer.WriteLine(num.ToString(format, info)); }
    static public void PrintLine(double num) { writer.WriteLine(num.ToString(info)); }
    static public void PrintLine(double num, string format) { writer.WriteLine(num.ToString(format, info)); }
    static public void PrintLine(string str) { writer.WriteLine(str); }
    static public void PrintLine(string format, params object[] arg) { writer.WriteLine(format, arg); }
    static public void PrintLine<T>(string format, IEnumerable<T> sources) { writer.WriteLine(format, sources.OfType<object>().ToArray()); }

    static public void PrintLine(params object[] arg)
    {
        var res = new System.Text.StringBuilder();
        foreach (var x in arg)
        {
            res.AppendFormat(info, "{0}", x);
            if (!string.IsNullOrEmpty(Separator))
                res.Append(Separator);
        }
        writer.WriteLine(res.ToString(0, res.Length - Separator.Length));
    }
    static public void PrintLine<T>(IEnumerable<T> sources)
    {
        var res = new System.Text.StringBuilder();
        foreach (var x in sources)
        {
            res.AppendFormat(info, "{0}", x);
            if (!string.IsNullOrEmpty(Separator))
                res.Append(Separator);
        }
        writer.WriteLine(res.ToString(0, res.Length - Separator.Length));
    }
    static public void PrintLine(System.Linq.Expressions.Expression<Func<string, object>> ex)
    {
        var res = ex.Parameters[0];
        writer.WriteLine(res.Name);
    }
}
public class _Scanner
{
    readonly private System.Globalization.CultureInfo info;
    readonly System.IO.TextReader reader;
    string[] buffer = new string[0];
    int position;

    public char[] Separator { get; set; }
    public _Scanner(System.IO.TextReader reader = null, string separator = null, System.Globalization.CultureInfo info = null)
    {

        this.reader = reader ?? Console.In;
        if (string.IsNullOrEmpty(separator))
            separator = " ";
        this.Separator = separator.ToCharArray();
        this.info = info ?? System.Globalization.CultureInfo.InvariantCulture;
    }
    public string Scan()
    {
        if (this.position < this.buffer.Length)
            return this.buffer[this.position++];
        do this.buffer = this.reader.ReadLine().Split(this.Separator, StringSplitOptions.RemoveEmptyEntries);
        while (this.buffer.Length == 0);
        this.position = 0;
        return this.buffer[this.position++];
    }

    public string ScanLine()
    {
        if (this.position >= this.buffer.Length)
            return this.reader.ReadLine();
        else
        {
            var sb = new System.Text.StringBuilder();
            for (; this.position < buffer.Length; this.position++)
            {
                sb.Append(this.buffer[this.position]);
                sb.Append(' ');
            }
            return sb.ToString();
        }
    }
    public string[] ScanArray(int length)
    {
        var ar = new string[length];
        for (int i = 0; i < length; i++)
            ar[i] = this.Scan();
        return ar;
    }

    public int Integer()
    {
        return int.Parse(this.Scan(), info);
    }
    public long Long()
    {
        return long.Parse(this.Scan(), info);
    }
    public double Double()
    {
        return double.Parse(this.Scan(), info);
    }
    public double Double(string str)
    {
        return double.Parse(str, info);
    }

    public int[] IntArray(int length)
    {
        var a = new int[length];
        for (int i = 0; i < length; i++)
            a[i] = this.Integer();
        return a;
    }
    public long[] LongArray(int length)
    {
        var a = new long[length];
        for (int i = 0; i < length; i++)
            a[i] = this.Long();
        return a;
    }
    public double[] DoubleArray(int length)
    {
        var a = new double[length];
        for (int i = 0; i < length; i++)
            a[i] = this.Double();
        return a;
    }

}
static public partial class EnumerableEx
{
    static public string AsString(this IEnumerable<char> source)
    {
        return new string(source.ToArray());
    }
    static public T[] Enumerate<T>(this int n, Func<int, T> selector)
    {
        var res = new T[n];
        for (int i = 0; i < n; i++)
            res[i] = selector(i);
        return res;
    }
}
#endregion

#region HashMap
class HashMap<K, V> : Dictionary<K, V>
{
    new public V this[K i]
    {
        get
        {
            V v;
            return TryGetValue(i, out v) ? v : base[i] = default(V);
        }
        set { base[i] = value; }
    }
}
#endregion


