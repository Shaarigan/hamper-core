// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

namespace Soe.Runtime
{
    #if EXPORT_HAMPER_CORE_RUNTIME_COMMANDS
    public
    #else
    internal
    #endif
    static class SpanExtension
    {
        public static bool TryFindOption(this ReadOnlySpan<Option> options, string name, out Option result)
        {
            if(options.Length > 0)
            {
                int lo = 0;
                int hi = options.Length;
                char first = name[0];

                while (lo < hi)
                {
                    int mid = (lo + ((hi - lo) >> 1));
                    if (options[mid].Name[0] < first)
                    {
                        lo = mid + 1;
                    }
                    else hi = mid;
                }

                hi = options.Length;
                int start = lo;

                while (lo < hi)
                {
                    int mid = (lo + ((hi - lo) >> 1));
                    if (options[mid].Name[0] <= first)
                    {
                        lo = mid + 1;
                    }
                    else hi = mid;
                }

                for (int i = start, length = name.Length; i < lo; i++)
                {
                    if (options[i].Name.Length == length && options[i].Name.SequenceEqual(name))
                    {
                        result = options[i];
                        return true;
                    }
                }
            }
            result = default;
            return false;
        }
    }
}