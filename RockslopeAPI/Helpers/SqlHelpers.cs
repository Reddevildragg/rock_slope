using Microsoft.AspNetCore.Http;

namespace RockslopeAPI.Helpers;

public static class SqlHelpers
{
    public static void PageValues(HttpRequest req, out int offset, out int limit)
    {
        limit = 100;
        if (req.Query.ContainsKey("limit"))
        {
            limit = int.Parse(req.Query["limit"]);
        }
        
        offset = 0;
        if (req.Query.ContainsKey("offset"))
        {
            offset = int.Parse(req.Query["offset"]);
        }
    }
    
    public static string OrderBy(this string query, string field)
    {
        return query + $" ORDER BY {field} ";
    }
    
    public static string Pageinate(this string query, int offset, int pageResults)
    {
        return query + $" OFFSET {offset} ROWS FETCH NEXT {pageResults} ROWS ONLY ";
    }
}