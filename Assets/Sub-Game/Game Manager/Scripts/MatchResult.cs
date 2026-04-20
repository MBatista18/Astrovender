using System.Collections.Generic;

// Data Structure to keep track of match information
public class MatchResult
{
    public Dictionary<NodeType, int> ClearedCounts = new();

    public void Add(NodeType type, int amount = 1)
    {
        if (!ClearedCounts.ContainsKey(type))
            ClearedCounts[type] = 0;

        ClearedCounts[type] += amount;
    }
}
