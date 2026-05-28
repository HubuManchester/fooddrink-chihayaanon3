using TasteNote.Models;

namespace TasteNote.Services;

public class FlavorCalculator
{
    private readonly TastingRepository _tastingRepo;

    public FlavorCalculator(TastingRepository tastingRepo)
    {
        _tastingRepo = tastingRepo;
    }

    public async Task<FlavorSummary> CalculateSummaryAsync()
    {
        var records = await _tastingRepo.GetAllAsync();
        return CalculateFromRecords(records);
    }

    public FlavorSummary CalculateFromRecords(List<TastingRecord> records)
    {
        var summary = new FlavorSummary { TotalRecords = records.Count };
        if (records.Count == 0) return summary;

        var allTags = new List<string>();
        var categoryCount = new Dictionary<string, int>();

        foreach (var record in records)
        {
            if (!string.IsNullOrEmpty(record.FlavorTags))
            {
                try
                {
                    var tags = System.Text.Json.JsonSerializer.Deserialize<List<string>>(record.FlavorTags);
                    if (tags != null) allTags.AddRange(tags);
                }
                catch { }
            }

            if (!string.IsNullOrEmpty(record.Category))
            {
                if (categoryCount.ContainsKey(record.Category))
                    categoryCount[record.Category]++;
                else
                    categoryCount[record.Category] = 1;
            }
        }

        // Simple flavor scoring based on tag keywords
        summary.SweetAvg = CountTagMatches(allTags, new[] { "蜜甜", "果香", "奶油", "焦糖" }) / (double)records.Count * 5;
        summary.SourAvg = CountTagMatches(allTags, new[] { "酸甜", "果香" }) / (double)records.Count * 5;
        summary.BitterAvg = CountTagMatches(allTags, new[] { "醇苦", "微苦", "烟熏", "草本" }) / (double)records.Count * 5;
        summary.SaltyAvg = CountTagMatches(allTags, new[] { "微辣" }) / (double)records.Count * 5;
        summary.UmamiAvg = CountTagMatches(allTags, new[] { "鲜味", "浓郁", "回甘" }) / (double)records.Count * 5;

        // Clamp values
        summary.SweetAvg = Math.Min(5, Math.Round(summary.SweetAvg, 1));
        summary.SourAvg = Math.Min(5, Math.Round(summary.SourAvg, 1));
        summary.BitterAvg = Math.Min(5, Math.Round(summary.BitterAvg, 1));
        summary.SaltyAvg = Math.Min(5, Math.Round(summary.SaltyAvg, 1));
        summary.UmamiAvg = Math.Min(5, Math.Round(summary.UmamiAvg, 1));

        if (categoryCount.Count > 0)
            summary.FavoriteCategory = categoryCount.OrderByDescending(c => c.Value).First().Key;

        return summary;
    }

    private int CountTagMatches(List<string> tags, string[] keywords)
    {
        return tags.Count(t => keywords.Any(k => t.Contains(k)));
    }

    public List<FlavorPoint> ToFlavorPoints(FlavorSummary summary)
    {
        return new List<FlavorPoint>
        {
            new() { Dimension = "甜", Score = summary.SweetAvg },
            new() { Dimension = "酸", Score = summary.SourAvg },
            new() { Dimension = "苦", Score = summary.BitterAvg },
            new() { Dimension = "咸", Score = summary.SaltyAvg },
            new() { Dimension = "鲜", Score = summary.UmamiAvg },
        };
    }

    public async Task<List<MonthlyStat>> GetMonthlyStatsAsync(int months = 6)
    {
        var records = await _tastingRepo.GetAllAsync();
        var stats = new List<MonthlyStat>();
        var now = DateTime.Now;

        for (int i = months - 1; i >= 0; i--)
        {
            var date = now.AddMonths(-i);
            var monthStr = date.ToString("yyyy-MM");
            var monthLabel = date.ToString("M月");
            var monthRecords = records.Where(r => r.CreatedAt.ToString("yyyy-MM") == monthStr).ToList();

            stats.Add(new MonthlyStat
            {
                Month = monthLabel,
                Count = monthRecords.Count,
                Category = monthRecords.GroupBy(r => r.Category)
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault()?.Key ?? ""
            });
        }
        return stats;
    }
}
