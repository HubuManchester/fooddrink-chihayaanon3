using TasteNote.Models;

namespace TasteNote.Services;

public class SeedDataService
{
    private const string SeedVersionKey = "seed_version";
    private const string CurrentSeedVersion = "2";

    private readonly TastingRepository _tastingRepo;
    private readonly BeverageRepository _beverageRepo;
    private readonly FavoritePlaceRepository _favoritePlaceRepo;
    private readonly TasteProfileRepository _tasteProfileRepo;
    private readonly UserPreferenceRepository _userPrefRepo;

    public SeedDataService(
        TastingRepository tastingRepo,
        BeverageRepository beverageRepo,
        FavoritePlaceRepository favoritePlaceRepo,
        TasteProfileRepository tasteProfileRepo,
        UserPreferenceRepository userPrefRepo)
    {
        _tastingRepo = tastingRepo;
        _beverageRepo = beverageRepo;
        _favoritePlaceRepo = favoritePlaceRepo;
        _tasteProfileRepo = tasteProfileRepo;
        _userPrefRepo = userPrefRepo;
    }

    public async Task InitializeAsync()
    {
        try
        {
            var version = await _userPrefRepo.GetAsync(SeedVersionKey);
            if (version == CurrentSeedVersion)
                return;

            await SeedTastingRecordsAsync();
            await SeedBeveragesAsync();
            await SeedFavoritePlacesAsync();
            await SeedTasteProfileAsync();
            await SeedUserPreferencesAsync();

            await _userPrefRepo.SetAsync(SeedVersionKey, CurrentSeedVersion);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[SeedDataService] 初始化种子数据失败: {ex.Message}");
        }
    }

    private async Task SeedTastingRecordsAsync()
    {
        var now = DateTime.Now;
        var baseDate = new DateTime(now.Year, now.Month, now.Day);

        var records = new List<TastingRecord>
        {
            new()
            {
                Title = "晨间手冲",
                Category = "咖啡",
                Rating = 4,
                FlavorTags = "[\"果香\",\"回甘\",\"丝滑\"]",
                ImagePath = "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?w=400",
                Notes = "清晨第一杯，明亮的果酸在口中绽放，回甘持久，开启美好的一天。",
                LocationName = "独立咖啡实验室",
                Latitude = 39.9120 + RandomOffset(),
                Longitude = 116.4570 + RandomOffset(),
                IsFavorite = false,
                CreatedAt = baseDate.AddDays(-13).AddHours(8).AddMinutes(30)
            },
            new()
            {
                Title = "抹茶拿铁",
                Category = "茶饮",
                Rating = 5,
                FlavorTags = "[\"草本\",\"蜜甜\",\"丝滑\"]",
                ImagePath = "https://images.unsplash.com/photo-1515823064-d6e0c04616a7?w=400",
                Notes = "浓郁的抹茶香气与绵密的奶泡完美融合，草本清香中带着自然的蜜甜。",
                LocationName = "隐泉茶室",
                Latitude = 39.9080 + RandomOffset(),
                Longitude = 116.4620 + RandomOffset(),
                IsFavorite = true,
                CreatedAt = baseDate.AddDays(-12).AddHours(14).AddMinutes(15)
            },
            new()
            {
                Title = "莫吉托",
                Category = "鸡尾酒",
                Rating = 4,
                FlavorTags = "[\"清爽\",\"薄荷\",\"气泡感\"]",
                ImagePath = "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=400",
                Notes = "薄荷与青柠的经典组合，气泡感十足，是夏夜最好的伴侣。",
                LocationName = "夜色酒吧",
                Latitude = 39.9150 + RandomOffset(),
                Longitude = 116.4500 + RandomOffset(),
                IsFavorite = false,
                CreatedAt = baseDate.AddDays(-10).AddHours(21).AddMinutes(45)
            },
            new()
            {
                Title = "提拉米苏",
                Category = "甜品",
                Rating = 5,
                FlavorTags = "[\"奶油\",\"微苦\",\"浓郁\"]",
                ImagePath = "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?w=400",
                Notes = "层次分明的口感，咖啡的微苦与马斯卡彭奶酪的浓郁完美平衡，每一口都是享受。",
                LocationName = "甜蜜时光甜品店",
                Latitude = 39.9100 + RandomOffset(),
                Longitude = 116.4650 + RandomOffset(),
                IsFavorite = true,
                CreatedAt = baseDate.AddDays(-9).AddHours(15).AddMinutes(20)
            },
            new()
            {
                Title = "牛肉拉面",
                Category = "主食",
                Rating = 4,
                FlavorTags = "[\"鲜味\",\"浓郁\",\"回甘\"]",
                ImagePath = "https://images.unsplash.com/photo-1555126634-323283e090fa?w=400",
                Notes = "手工拉面劲道弹牙，牛骨熬制的浓汤鲜美醇厚，一碗下肚暖到心底。",
                LocationName = "一兰拉面",
                Latitude = 39.9130 + RandomOffset(),
                Longitude = 116.4550 + RandomOffset(),
                IsFavorite = false,
                CreatedAt = baseDate.AddDays(-7).AddHours(12).AddMinutes(10)
            },
            new()
            {
                Title = "烤鸡翅",
                Category = "小吃",
                Rating = 3,
                FlavorTags = "[\"烟熏\",\"微辣\",\"焦糖\"]",
                ImagePath = "https://images.unsplash.com/photo-1527477396000-e27163b481c2?w=400",
                Notes = "炭火慢烤的鸡翅外焦里嫩，烟熏味配上微辣的酱汁，越吃越过瘾。",
                LocationName = "街角烧烤屋",
                Latitude = 39.9110 + RandomOffset(),
                Longitude = 116.4600 + RandomOffset(),
                IsFavorite = false,
                CreatedAt = baseDate.AddDays(-6).AddHours(19).AddMinutes(30)
            },
            new()
            {
                Title = "法式洋葱汤",
                Category = "汤品",
                Rating = 4,
                FlavorTags = "[\"鲜味\",\"浓郁\",\"焦糖\"]",
                ImagePath = "https://images.unsplash.com/photo-1547592166-23ac45744acd?w=400",
                Notes = "慢炖数小时的洋葱释放出自然的焦糖甜味，搭配烤得金黄的芝士面包，经典法式风味。",
                LocationName = "左岸法餐厅",
                Latitude = 39.9060 + RandomOffset(),
                Longitude = 116.4580 + RandomOffset(),
                IsFavorite = false,
                CreatedAt = baseDate.AddDays(-5).AddHours(18).AddMinutes(45)
            },
            new()
            {
                Title = "冰美式",
                Category = "咖啡",
                Rating = 3,
                FlavorTags = "[\"醇苦\",\"清爽\",\"轻盈\"]",
                ImagePath = "https://images.unsplash.com/photo-1461023058943-07fcbe16d735?w=400",
                Notes = "简洁纯粹的黑咖啡，醇厚的苦味在冰块的调和下变得清爽宜人。",
                LocationName = "星巴克",
                Latitude = 39.9140 + RandomOffset(),
                Longitude = 116.4530 + RandomOffset(),
                IsFavorite = false,
                CreatedAt = baseDate.AddDays(-4).AddHours(9).AddMinutes(0)
            },
            new()
            {
                Title = "铁观音",
                Category = "茶饮",
                Rating = 5,
                FlavorTags = "[\"花香\",\"回甘\",\"轻盈\"]",
                ImagePath = "https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=400",
                Notes = "传统的功夫茶泡法，兰花香气高雅悠远，七泡之后依然回甘绵长。",
                LocationName = "武夷山茶馆",
                Latitude = 39.9090 + RandomOffset(),
                Longitude = 116.4640 + RandomOffset(),
                IsFavorite = true,
                CreatedAt = baseDate.AddDays(-3).AddHours(10).AddMinutes(30)
            },
            new()
            {
                Title = "玛格丽特",
                Category = "鸡尾酒",
                Rating = 4,
                FlavorTags = "[\"酸甜\",\"清爽\",\"果香\"]",
                ImagePath = "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=400",
                Notes = "龙舌兰的烈性被青柠的酸和橙味利口酒的甜完美中和，杯口的盐边增添了层次。",
                LocationName = "龙舌兰之家",
                Latitude = 39.9090 + RandomOffset(),
                Longitude = 116.4630 + RandomOffset(),
                IsFavorite = false,
                CreatedAt = baseDate.AddDays(-2).AddHours(20).AddMinutes(15)
            },
            new()
            {
                Title = "马卡龙",
                Category = "甜品",
                Rating = 4,
                FlavorTags = "[\"蜜甜\",\"奶油\",\"轻盈\"]",
                ImagePath = "https://images.unsplash.com/photo-1569864358642-9d1684040f43?w=400",
                Notes = "外壳酥脆内里柔软，甜蜜的奶油夹心在口中化开，轻盈如云朵般的法式甜点。",
                LocationName = "拉杜丽",
                Latitude = 39.9105 + RandomOffset(),
                Longitude = 116.4610 + RandomOffset(),
                IsFavorite = false,
                CreatedAt = baseDate.AddDays(-1).AddHours(16).AddMinutes(0)
            },
            new()
            {
                Title = "春卷",
                Category = "小吃",
                Rating = 3,
                FlavorTags = "[\"清爽\",\"轻盈\",\"鲜味\"]",
                ImagePath = "https://images.unsplash.com/photo-1548507200-fcb8e498eef0?w=400",
                Notes = "透明的米皮包裹着新鲜的蔬菜和虾仁，蘸上鱼露清爽开胃，简单却让人回味。",
                LocationName = "越南小馆",
                Latitude = 39.9115 + RandomOffset(),
                Longitude = 116.4590 + RandomOffset(),
                IsFavorite = false,
                CreatedAt = baseDate.AddDays(0).AddHours(11).AddMinutes(30)
            }
        };

        foreach (var record in records)
        {
            await _tastingRepo.SaveAsync(record);
        }
    }

    private static double RandomOffset()
    {
        var random = new Random();
        return (random.NextDouble() - 0.5) * 0.002;
    }

    private async Task SeedBeveragesAsync()
    {
        var beverages = new List<Beverage>
        {
            // 咖啡
            new()
            {
                Name = "拿铁",
                Category = "咖啡",
                SubCategory = "意式咖啡",
                Description = "浓缩咖啡与蒸汽牛奶的经典组合，口感顺滑温和，适合咖啡入门者。",
                ImagePath = "https://images.unsplash.com/photo-1461023058943-07fcbe16d735?w=400",
                Origin = "意大利",
                FlavorProfile = "{\"sweet\":4,\"sour\":1,\"bitter\":2,\"salty\":0,\"umami\":1}",
                Story = "拿铁(Latte)源自意大利语中的\"牛奶\"一词。在20世纪80年代，西雅图的咖啡文化将其推广至全球。如今拿铁已成为世界上最受欢迎的咖啡饮品之一，也是拉花艺术的最佳画布。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"研磨18g咖啡豆至意式细度\"},{\"Index\":1,\"Text\":\"萃取25秒获得36ml浓缩咖啡\"},{\"Index\":2,\"Text\":\"将200ml牛奶加热至65°C并打出绵密奶泡\"},{\"Index\":3,\"Text\":\"将奶泡缓缓倒入浓缩咖啡中\"}]",
                Tags = "[\"经典\",\"奶味\",\"顺滑\",\"入门\"]"
            },
            new()
            {
                Name = "美式咖啡",
                Category = "咖啡",
                SubCategory = "意式咖啡",
                Description = "浓缩咖啡加热水稀释，保留了咖啡的风味但降低了强度，口感清爽。",
                ImagePath = "https://images.unsplash.com/photo-1509042239860-f550ce710b93?w=400",
                Origin = "美国",
                FlavorProfile = "{\"sweet\":1,\"sour\":2,\"bitter\":4,\"salty\":0,\"umami\":1}",
                Story = "美式咖啡据说起源于二战时期，驻扎在意大利的美国士兵觉得浓缩咖啡太浓烈，便加水稀释。这种喝法后来传回美国并风靡全球，成为黑咖啡爱好者的日常之选。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"研磨18g咖啡豆至意式细度\"},{\"Index\":1,\"Text\":\"萃取25秒获得36ml浓缩咖啡\"},{\"Index\":2,\"Text\":\"加入150ml热水(90°C)稀释\"},{\"Index\":3,\"Text\":\"轻轻搅拌均匀即可享用\"}]",
                Tags = "[\"经典\",\"清爽\",\"低卡\",\"日常\"]"
            },
            new()
            {
                Name = "卡布奇诺",
                Category = "咖啡",
                SubCategory = "意式咖啡",
                Description = "等比例的浓缩咖啡、蒸汽牛奶和奶泡三层结构，口感丰富且有层次。",
                ImagePath = "https://images.unsplash.com/photo-1572442388796-11668a67e53d?w=400",
                Origin = "意大利",
                FlavorProfile = "{\"sweet\":3,\"sour\":1,\"bitter\":3,\"salty\":0,\"umami\":1}",
                Story = "卡布奇诺得名于意大利嘉布遣修道士的棕色帽兜，其颜色与咖啡上奶泡的颜色相似。传统意大利人只在早晨饮用卡布奇诺，认为午后喝加奶的咖啡不利于消化。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"研磨18g咖啡豆至意式细度\"},{\"Index\":1,\"Text\":\"萃取25秒获得36ml浓缩咖啡\"},{\"Index\":2,\"Text\":\"将100ml牛奶打发至产生厚实奶泡\"},{\"Index\":3,\"Text\":\"依次倒入蒸汽牛奶和厚奶泡\"}]",
                Tags = "[\"经典\",\"浓郁\",\"奶泡\",\"意式\"]"
            },
            new()
            {
                Name = "手冲咖啡",
                Category = "咖啡",
                SubCategory = "单品咖啡",
                Description = "通过手动注水萃取的咖啡，能最大程度展现咖啡豆的风味特征，适合品鉴。",
                ImagePath = "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?w=400",
                Origin = "德国",
                FlavorProfile = "{\"sweet\":2,\"sour\":3,\"bitter\":2,\"salty\":0,\"umami\":1}",
                Story = "手冲咖啡起源于20世纪初的德国，Melitta Bentz夫人发明了滤纸冲泡法。如今手冲已成为精品咖啡运动的标志，每一位咖啡爱好者都追求通过水温、研磨度和注水手法来呈现咖啡的最佳风味。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"研磨15g咖啡豆至中细度\"},{\"Index\":1,\"Text\":\"用92°C热水湿润滤纸并预热器具\"},{\"Index\":2,\"Text\":\"注入30ml热水焖蒸30秒\"},{\"Index\":3,\"Text\":\"分三次缓慢注入剩余200ml热水\"},{\"Index\":4,\"Text\":\"等待滴滤完成，总时间约3分钟\"}]",
                Tags = "[\"精品\",\"花香\",\"果酸\",\"手艺\"]"
            },

            // 茶饮
            new()
            {
                Name = "抹茶",
                Category = "茶饮",
                SubCategory = "绿茶",
                Description = "将遮荫生长的茶叶研磨成极细的粉末，冲泡后饮用，富含茶氨酸和抗氧化物。",
                ImagePath = "https://images.unsplash.com/photo-1515823064-d6e0c04616a7?w=400",
                Origin = "日本",
                FlavorProfile = "{\"sweet\":3,\"sour\":0,\"bitter\":3,\"salty\":0,\"umami\":4}",
                Story = "抹茶起源于中国隋唐时期，后经日本僧人带回并发展成独特的茶道文化。日本宇治地区是最高品质抹茶的产地，遮荫栽培的方法使得茶叶产生浓郁的鲜味和独特的翠绿色泽。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"取2g抹茶粉放入茶碗\"},{\"Index\":1,\"Text\":\"加入80°C热水约60ml\"},{\"Index\":2,\"Text\":\"用茶筅快速击拂至起泡\"},{\"Index\":3,\"Text\":\"以W字形搅动直至表面产生细密泡沫\"}]",
                Tags = "[\"日式\",\"鲜味\",\"抗氧化\",\"仪式\"]"
            },
            new()
            {
                Name = "铁观音",
                Category = "茶饮",
                SubCategory = "乌龙茶",
                Description = "半发酵乌龙茶的代表，兼具绿茶的清香和红茶的醇厚，兰花香气独特。",
                ImagePath = "https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=400",
                Origin = "中国福建",
                FlavorProfile = "{\"sweet\":4,\"sour\":1,\"bitter\":1,\"salty\":0,\"umami\":3}",
                Story = "铁观音产于福建省安溪县，相传因观音菩萨托梦指引茶农发现此茶树而得名。其独特的\"观音韵\"和\"七泡有余香\"的特点，使其成为中国十大名茶之一。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"取7g铁观音茶叶放入盖碗\"},{\"Index\":1,\"Text\":\"注入95°C热水洗茶后倒掉\"},{\"Index\":2,\"Text\":\"再次注入热水浸泡15秒\"},{\"Index\":3,\"Text\":\"每泡递增5秒，可冲泡7-8次\"}]",
                Tags = "[\"中式\",\"兰花香\",\"回甘\",\"功夫茶\"]"
            },
            new()
            {
                Name = "伯爵茶",
                Category = "茶饮",
                SubCategory = "红茶",
                Description = "以红茶为基底，加入佛手柑精油熏制而成，带有独特的柑橘清香。",
                ImagePath = "https://images.unsplash.com/photo-1571934811356-5cc061b6821f?w=400",
                Origin = "英国",
                FlavorProfile = "{\"sweet\":2,\"sour\":2,\"bitter\":2,\"salty\":0,\"umami\":1}",
                Story = "伯爵茶得名于19世纪英国首相格雷伯爵二世。传说一位中国官员将这种加了佛手柑的红茶作为礼物送给他。Twinings茶公司随后将其商品化，成为英国下午茶的标志性饮品。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"取2g伯爵茶茶叶或茶包\"},{\"Index\":1,\"Text\":\"注入100°C热水约200ml\"},{\"Index\":2,\"Text\":\"浸泡3-5分钟\"},{\"Index\":3,\"Text\":\"可加牛奶或柠檬搭配饮用\"}]",
                Tags = "[\"英式\",\"柑橘\",\"下午茶\",\"优雅\"]"
            },

            // 鸡尾酒
            new()
            {
                Name = "莫吉托",
                Category = "鸡尾酒",
                SubCategory = "朗姆基酒",
                Description = "以白朗姆酒为基酒，搭配新鲜薄荷、青柠和苏打水，清爽宜人。",
                ImagePath = "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=400",
                Origin = "古巴",
                FlavorProfile = "{\"sweet\":3,\"sour\":3,\"bitter\":0,\"salty\":0,\"umami\":0}",
                Story = "莫吉托诞生于古巴哈瓦那，是海明威最爱的鸡尾酒之一。传统配方可以追溯到16世纪，当时水手们用朗姆酒、薄荷和青柠来预防坏血病和疾病。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"在杯中放入6片薄荷叶和半个青柠汁\"},{\"Index\":1,\"Text\":\"加入2茶匙糖轻轻捣碎\"},{\"Index\":2,\"Text\":\"倒入45ml白朗姆酒\"},{\"Index\":3,\"Text\":\"加满碎冰和苏打水，轻轻搅拌\"}]",
                Tags = "[\"清爽\",\"薄荷\",\"夏日\",\"经典\"]"
            },
            new()
            {
                Name = "玛格丽特",
                Category = "鸡尾酒",
                SubCategory = "龙舌兰基酒",
                Description = "龙舌兰酒搭配橙味利口酒和青柠汁，杯口盐边是标志性特征。",
                ImagePath = "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=400",
                Origin = "墨西哥",
                FlavorProfile = "{\"sweet\":2,\"sour\":4,\"bitter\":1,\"salty\":1,\"umami\":0}",
                Story = "玛格丽特的起源有多种说法，最广为流传的是1948年墨西哥调酒师为一位名叫玛格丽特的女士调制。杯口的盐边不仅增添风味，据说是为了中和龙舌兰的辛辣口感。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"用青柠片擦拭杯口后蘸上盐\"},{\"Index\":1,\"Text\":\"将50ml龙舌兰、25ml橙味利口酒和25ml青柠汁加冰摇匀\"},{\"Index\":2,\"Text\":\"滤入准备好的盐边杯中\"},{\"Index\":3,\"Text\":\"以青柠片装饰即可享用\"}]",
                Tags = "[\"酸甜\",\"盐边\",\"墨西哥\",\"派对\"]"
            },
            new()
            {
                Name = "长岛冰茶",
                Category = "鸡尾酒",
                SubCategory = "混合基酒",
                Description = "虽名为冰茶但不含茶叶，由四种基酒混合而成，酒精度较高。",
                ImagePath = "https://images.unsplash.com/photo-1536935338788-846bb9981813?w=400",
                Origin = "美国",
                FlavorProfile = "{\"sweet\":3,\"sour\":2,\"bitter\":1,\"salty\":0,\"umami\":0}",
                Story = "长岛冰茶发明于20世纪70年代纽约长岛的一家酒吧。调酒师为了避开禁酒令时期的限制，将多种烈酒混合并加入可乐，使其看起来像普通的冰茶。这款酒以\"外表温和、内力深厚\"著称。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"在杯中加满冰块\"},{\"Index\":1,\"Text\":\"各加入15ml伏特加、金酒、白朗姆和龙舌兰\"},{\"Index\":2,\"Text\":\"加入30ml橙味利口酒和柠檬汁\"},{\"Index\":3,\"Text\":\"最后加满可乐轻轻搅拌\"}]",
                Tags = "[\"烈酒\",\"可乐\",\"经典\",\"刺激\"]"
            },

            // 甜品
            new()
            {
                Name = "提拉米苏",
                Category = "甜品",
                SubCategory = "意式甜点",
                Description = "浸泡浓缩咖啡的手指饼干与马斯卡彭奶酪交替层叠，撒上可可粉。",
                ImagePath = "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?w=400",
                Origin = "意大利",
                FlavorProfile = "{\"sweet\":4,\"sour\":0,\"bitter\":2,\"salty\":0,\"umami\":1}",
                Story = "提拉米苏(Tiramisu)在意大利语中意为\"带我走\"或\"让我开心起来\"。它于20世纪60年代在意大利威尼托地区首次出现。这道甜点完美诠释了意大利人对生活的热爱——简单食材的精妙组合。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"将蛋黄与糖打至发白，加入马斯卡彭奶酪拌匀\"},{\"Index\":1,\"Text\":\"将蛋白打发后轻轻拌入奶酪糊\"},{\"Index\":2,\"Text\":\"手指饼干快速蘸取浓缩咖啡铺底\"},{\"Index\":3,\"Text\":\"交替铺上奶酪糊和饼干层，冷藏4小时\"},{\"Index\":4,\"Text\":\"食用前撒上一层可可粉\"}]",
                Tags = "[\"经典\",\"咖啡\",\"奶酪\",\"浪漫\"]"
            },
            new()
            {
                Name = "马卡龙",
                Category = "甜品",
                SubCategory = "法式甜点",
                Description = "由杏仁粉制成的法式圆饼夹心饼干，外壳酥脆内里柔软，色彩缤纷。",
                ImagePath = "https://images.unsplash.com/photo-1569864358642-9d1684040f43?w=400",
                Origin = "法国",
                FlavorProfile = "{\"sweet\":5,\"sour\":0,\"bitter\":0,\"salty\":0,\"umami\":0}",
                Story = "马卡龙虽以法式甜点闻名，但其原型可追溯到意大利文艺复兴时期，由凯瑟琳·德·美第奇带入法国。巴黎老字号Ladurée将其发展为双层夹心的现代形态，成为法式优雅的象征。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"将杏仁粉与糖粉过筛混合\"},{\"Index\":1,\"Text\":\"蛋白打发至硬性发泡，加入食用色素\"},{\"Index\":2,\"Text\":\"将粉类拌入蛋白霜，装入裱花袋挤成圆形\"},{\"Index\":3,\"Text\":\"室温晾干30分钟后以150°C烘烤12分钟\"},{\"Index\":4,\"Text\":\"冷却后夹入奶油馅料即可\"}]",
                Tags = "[\"法式\",\"精致\",\"缤纷\",\"甜蜜\"]"
            },
            new()
            {
                Name = "焦糖布丁",
                Category = "甜品",
                SubCategory = "西式甜点",
                Description = "丝滑的蛋奶布丁覆盖一层金黄焦糖，冷热交替的口感令人着迷。",
                ImagePath = "https://images.unsplash.com/photo-1488477181946-6428a0291777?w=400",
                Origin = "法国/西班牙",
                FlavorProfile = "{\"sweet\":5,\"sour\":0,\"bitter\":1,\"salty\":0,\"umami\":0}",
                Story = "焦糖布丁的历史可追溯至罗马时代，法国和西班牙都声称是其发源地。法式Crème brûlée以表面的焦糖脆壳著称，而西式Flan则以整体焦糖浸润为特色。两种风格各有千秋。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"将糖加热至焦糖色，倒入模具底部\"},{\"Index\":1,\"Text\":\"将蛋黄、牛奶和香草精混合均匀\"},{\"Index\":2,\"Text\":\"将蛋奶液过滤倒入焦糖模具中\"},{\"Index\":3,\"Text\":\"水浴法以160°C烘烤40分钟\"},{\"Index\":4,\"Text\":\"冷藏4小时后倒扣脱模\"}]",
                Tags = "[\"经典\",\"丝滑\",\"焦糖\",\"治愈\"]"
            },

            // 主食
            new()
            {
                Name = "意大利面",
                Category = "主食",
                SubCategory = "西式主食",
                Description = "以硬质小麦粉制成的面条，搭配各种酱汁，是意大利饮食文化的核心。",
                ImagePath = "https://images.unsplash.com/photo-1551183053-bf91a1d81141?w=400",
                Origin = "意大利",
                FlavorProfile = "{\"sweet\":1,\"sour\":2,\"bitter\":0,\"salty\":2,\"umami\":4}",
                Story = "虽然马可·波罗从中国带回面条的故事广为流传，但考古证据表明意大利面在古罗马时期就已存在。意大利有超过350种面条形状，每种都与特定的酱汁搭配，体现了意大利人对美食的极致追求。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"大锅水煮沸后加盐，放入意大利面\"},{\"Index\":1,\"Text\":\"按包装时间减1分钟煮至弹牙(Al Dente)\"},{\"Index\":2,\"Text\":\"保留一杯煮面水后沥干面条\"},{\"Index\":3,\"Text\":\"将面条放入酱汁锅中翻拌均匀\"},{\"Index\":4,\"Text\":\"用煮面水调节浓稠度，装盘即食\"}]",
                Tags = "[\"经典\",\"弹牙\",\"百搭\",\"日常\"]"
            },
            new()
            {
                Name = "寿司",
                Category = "主食",
                SubCategory = "日式主食",
                Description = "醋饭搭配新鲜鱼生或其他食材，简约而精致，是日本料理的代表。",
                ImagePath = "https://images.unsplash.com/photo-1579584425555-c3ce17fd4351?w=400",
                Origin = "日本",
                FlavorProfile = "{\"sweet\":1,\"sour\":2,\"bitter\":0,\"salty\":2,\"umami\":5}",
                Story = "寿司最初源于东南亚的鱼肉发酵保存法，后经日本改良为使用醋饭的握寿司。江户前寿司在19世纪的东京街头小摊诞生，如今已成为全球最受尊敬的美食之一，顶级寿司需要师傅十年以上的修炼。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"精选寿司米蒸煮后拌入寿司醋\"},{\"Index\":1,\"Text\":\"将米饭扇凉至体温温度\"},{\"Index\":2,\"Text\":\"取适量米饭用手握成椭圆形\"},{\"Index\":3,\"Text\":\"在鱼片上抹少许芥末后覆盖在饭团上\"}]",
                Tags = "[\"日式\",\"新鲜\",\"鲜味\",\"匠人\"]"
            },
            new()
            {
                Name = "牛肉面",
                Category = "主食",
                SubCategory = "中式主食",
                Description = "以牛骨熬制的浓汤配上劲道面条和软烂牛肉，中国各地的灵魂美食。",
                ImagePath = "https://images.unsplash.com/photo-1555126634-323283e090fa?w=400",
                Origin = "中国",
                FlavorProfile = "{\"sweet\":1,\"sour\":1,\"bitter\":0,\"salty\":3,\"umami\":5}",
                Story = "牛肉面是中国最具代表性的面食之一，各地做法不同：兰州牛肉拉面讲究\"一清二白三红四绿五黄\"，台湾牛肉面融合了川菜和眷村文化，而襄阳牛肉面则以浓厚的牛油汤底闻名。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"牛骨和牛肉焯水后加香料炖煮3小时\"},{\"Index\":1,\"Text\":\"面条煮熟后捞入碗中\"},{\"Index\":2,\"Text\":\"铺上切片的卤牛肉\"},{\"Index\":3,\"Text\":\"浇上滚烫的牛肉汤，撒上葱花香菜\"}]",
                Tags = "[\"中式\",\"浓郁\",\"温暖\",\"家常\"]"
            },

            // 小吃
            new()
            {
                Name = "春卷",
                Category = "小吃",
                SubCategory = "亚洲小吃",
                Description = "用薄面皮或米皮包裹蔬菜和肉类，可炸可鲜，是亚洲各地流行的开胃小食。",
                ImagePath = "https://images.unsplash.com/photo-1548507200-fcb8e498eef0?w=400",
                Origin = "中国/越南",
                FlavorProfile = "{\"sweet\":1,\"sour\":1,\"bitter\":0,\"salty\":2,\"umami\":3}",
                Story = "春卷在中国有着悠久的历史，最初是春节应节食品，象征春天和新的开始。越南春卷(Goi Cuon)则以透明米皮包裹鲜虾和蔬菜为特色，清爽健康，是越南菜的代表之一。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"准备蔬菜丝、虾仁或肉丝等馅料\"},{\"Index\":1,\"Text\":\"将米皮浸水软化后铺平\"},{\"Index\":2,\"Text\":\"放入馅料后紧紧卷起\"},{\"Index\":3,\"Text\":\"炸春卷需180°C炸至金黄，鲜春卷直接食用\"},{\"Index\":4,\"Text\":\"搭配鱼露或甜辣酱蘸食\"}]",
                Tags = "[\"清爽\",\"开胃\",\"传统\",\"健康\"]"
            },
            new()
            {
                Name = "炸鸡",
                Category = "小吃",
                SubCategory = "西式小吃",
                Description = "裹上调味面糊或裹粉后油炸的鸡肉，外酥里嫩，是风靡全球的街头美食。",
                ImagePath = "https://images.unsplash.com/photo-1626645738196-c2a7c87a8f58?w=400",
                Origin = "美国/韩国",
                FlavorProfile = "{\"sweet\":1,\"sour\":0,\"bitter\":0,\"salty\":3,\"umami\":3}",
                Story = "炸鸡的历史可追溯到苏格兰和西非的烹饪传统，在美国南方发展成熟。近年来韩式炸鸡风靡全球，双重炸制法和各种酱汁裹衣使其外酥内嫩。流行文化更是让炸鸡成为了\"灵魂美食\"的代名词。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"鸡肉用盐、胡椒和蒜粉腌制2小时\"},{\"Index\":1,\"Text\":\"准备面粉、淀粉和调味料混合的裹粉\"},{\"Index\":2,\"Text\":\"鸡肉裹粉后浸入冰水再次裹粉(双重裹法)\"},{\"Index\":3,\"Text\":\"170°C油炸6-8分钟至金黄酥脆\"},{\"Index\":4,\"Text\":\"韩式可刷上甜辣酱或蜂蜜黄油酱\"}]",
                Tags = "[\"酥脆\",\"满足\",\"派对\",\"灵魂美食\"]"
            },

            // 汤品
            new()
            {
                Name = "法式洋葱汤",
                Category = "汤品",
                SubCategory = "西式汤品",
                Description = "慢炖焦糖化的洋葱配以浓郁的牛骨汤底，覆上烤至金黄的芝士面包。",
                ImagePath = "https://images.unsplash.com/photo-1547592166-23ac45744acd?w=400",
                Origin = "法国",
                FlavorProfile = "{\"sweet\":2,\"sour\":0,\"bitter\":1,\"salty\":2,\"umami\":5}",
                Story = "法式洋葱汤起源于18世纪的法国，据说是里昂地区的工人美食。洋葱经过长时间慢炖产生的焦糖化反应赋予汤底深邃的甜味和鲜味，表面覆盖的焗烤芝士面包是这道汤的灵魂所在。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"将洋葱切丝，用黄油小火慢炒40分钟至焦糖色\"},{\"Index\":1,\"Text\":\"加入面粉炒匀后倒入白葡萄酒\"},{\"Index\":2,\"Text\":\"加入牛骨汤炖煮20分钟调味\"},{\"Index\":3,\"Text\":\"倒入耐热碗中，放上法棍面包片和芝士\"},{\"Index\":4,\"Text\":\"200°C烤至芝士金黄冒泡即可\"}]",
                Tags = "[\"经典\",\"浓郁\",\"温暖\",\"法式\"]"
            },
            new()
            {
                Name = "冬阴功汤",
                Category = "汤品",
                SubCategory = "东南亚汤品",
                Description = "泰国经典酸辣汤，以虾为主料，香茅、南姜和柠檬叶营造独特香气。",
                ImagePath = "https://images.unsplash.com/photo-1562565652-a0d8f0c59eb4?w=400",
                Origin = "泰国",
                FlavorProfile = "{\"sweet\":1,\"sour\":4,\"bitter\":1,\"salty\":2,\"umami\":4}",
                Story = "冬阴功汤是泰国的国汤，\"冬阴\"意为煮酸辣，\"功\"意为虾。这道汤完美体现了泰国菜追求酸、甜、咸、辣四味平衡的烹饪哲学。2004年的一份研究甚至发现它具有抗氧化功效。",
                BrewMethod = "[{\"Index\":0,\"Text\":\"将香茅、南姜和柠檬叶放入沸水中煮出香味\"},{\"Index\":1,\"Text\":\"加入冬阴功酱和椰奶搅匀\"},{\"Index\":2,\"Text\":\"放入虾和蘑菇煮熟\"},{\"Index\":3,\"Text\":\"加入鱼露和青柠汁调味\"},{\"Index\":4,\"Text\":\"撒上香菜和辣椒装饰即可\"}]",
                Tags = "[\"酸辣\",\"泰式\",\"鲜虾\",\"开胃\"]"
            }
        };

        foreach (var beverage in beverages)
        {
            await _beverageRepo.SaveAsync(beverage);
        }
    }

    private async Task SeedFavoritePlacesAsync()
    {
        var places = new List<FavoritePlace>
        {
            new()
            {
                Name = "独立咖啡实验室",
                Category = "咖啡馆",
                Address = "创意园区A栋1楼",
                Rating = 5,
                Latitude = 39.9120,
                Longitude = 116.4570,
                Notes = "精品咖啡爱好者的天堂，老板对每一杯都很用心。",
                CreatedAt = DateTime.Now.AddDays(-10)
            },
            new()
            {
                Name = "隐泉茶室",
                Category = "茶馆",
                Address = "老街巷32号",
                Rating = 4,
                Latitude = 39.9080,
                Longitude = 116.4620,
                Notes = "闹中取静的隐世茶室，适合安静品茶和思考。",
                CreatedAt = DateTime.Now.AddDays(-10)
            },
            new()
            {
                Name = "夜色酒吧",
                Category = "酒吧",
                Address = "滨江大道88号",
                Rating = 4,
                Latitude = 39.9150,
                Longitude = 116.4500,
                Notes = "调酒师技术精湛，氛围感十足。",
                CreatedAt = DateTime.Now.AddDays(-10)
            },
            new()
            {
                Name = "甜蜜时光甜品店",
                Category = "甜品店",
                Address = "商业街56号",
                Rating = 5,
                Latitude = 39.9100,
                Longitude = 116.4650,
                Notes = "每一款甜品都是艺术品，味道和颜值双在线。",
                CreatedAt = DateTime.Now.AddDays(-10)
            },
            new()
            {
                Name = "左岸法餐厅",
                Category = "餐厅",
                Address = "湖滨路12号",
                Rating = 4,
                Latitude = 39.9060,
                Longitude = 116.4580,
                Notes = "正宗法式料理，洋葱汤和鹅肝值得一试。",
                CreatedAt = DateTime.Now.AddDays(-10)
            },
            new()
            {
                Name = "一兰拉面",
                Category = "餐厅",
                Address = "美食广场B区",
                Rating = 4,
                Latitude = 39.9130,
                Longitude = 116.4550,
                Notes = "汤头浓郁，拉面劲道，一人食也很舒适。",
                CreatedAt = DateTime.Now.AddDays(-10)
            },
            new()
            {
                Name = "街角烧烤屋",
                Category = "小吃摊",
                Address = "夜市C排8号",
                Rating = 3,
                Latitude = 39.9110,
                Longitude = 116.4600,
                Notes = "接地气的路边烧烤，鸡翅是招牌。",
                CreatedAt = DateTime.Now.AddDays(-10)
            },
            new()
            {
                Name = "龙舌兰之家",
                Category = "酒吧",
                Address = "酒吧街19号",
                Rating = 4,
                Latitude = 39.9090,
                Longitude = 116.4630,
                Notes = "专业龙舌兰酒吧，玛格丽特是必点。",
                CreatedAt = DateTime.Now.AddDays(-10)
            }
        };

        foreach (var place in places)
        {
            await _favoritePlaceRepo.SaveAsync(place);
        }
    }

    private async Task SeedTasteProfileAsync()
    {
        var today = DateTime.Now.ToString("yyyy-MM-dd");
        var profile = new TasteProfile
        {
            ProfileDate = today,
            SweetAvg = 3.5,
            SourAvg = 2.8,
            BitterAvg = 2.5,
            SaltyAvg = 1.2,
            UmamiAvg = 3.0,
            FavoriteCategory = "咖啡",
            TotalRecords = 3
        };

        await _tasteProfileRepo.SaveAsync(profile);
    }

    private async Task SeedUserPreferencesAsync()
    {
        await _userPrefRepo.SetAsync("theme", "Light");
        await _userPrefRepo.SetAsync("fontSize", "Medium");
        // Do NOT set first_run - leave it unset so onboarding shows on first launch
    }
}
