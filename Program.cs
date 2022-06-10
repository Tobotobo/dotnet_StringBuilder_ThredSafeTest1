//#define ENABLED_LOCK

using System.Text;

const int APPEND_LENGTH = 1000;
const int TRY_COUNT = 9;
var patterns = new[] { "a", "b", "c" };

Task append(StringBuilder sb, string s)
{
    return Task.Run(() =>
    {
        try
        {
#if ENABLED_LOCK
            lock (sb)
            {
#endif
            for (int i = 0; i < APPEND_LENGTH; i++)
                sb.Append(s);
#if ENABLED_LOCK
            }
#endif
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    {s} : {ex.ToString()}");
        }
    });
}

for (int i = 0; i < TRY_COUNT; i++)
{
    var sb = new StringBuilder();

    Task.WaitAll(patterns.Select(s => append(sb, s)).ToArray());

    Console.WriteLine($"#{i + 1} {(sb.Length == (patterns.Length * APPEND_LENGTH) ? "○" : $"× {sb.Length}")}");
}


