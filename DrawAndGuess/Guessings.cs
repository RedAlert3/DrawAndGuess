using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DrawAndGuess;

public class Guessings
{
    public List<string> GuessingsList { get; set; } = new()
    {
        "闭月羞花",
        "刻舟求剑",
        "亡羊补牢",
        "钢琴键",
        "井底之蛙",
        "顶天立地",
        "咖啡杯",
        "沙滩椅",
        "帆船",
        "太阳镜",
        "996",
        "弹幕",
        "火箭",
        "美食",
        "狐假虎威",
        "蓝瘦香菇",
        "你弄啥嘞",
        "笔记本电脑",
        "摩托车",
        "高楼大厦",
        "狗头人",
        "撒币",
        "棒球",
        "帆布鞋",
        "杀鸡取卵",
        "班门弄斧",
        "马到成功",
        "九牛一毛",
        "钻石戒指",
        "彩虹糖",
        "天鹅",
        "车轮战",
        "一石二鸟",
        "鸡飞蛋打",
        "卧槽",
        "抱歉，我错了",
        "咖啡豆",
        "画龙点睛",
        "珠光宝气",
        "杯弓蛇影",
        "手表",
        "猫咪",
        "科学实验",
        "心想事成",
        "望洋兴叹",
        "摄影机",
        "突然打架",
        "画蛇添足",
        "守株待兔",
        "闭门造车",
    };
}

public static class GuessingsExtensions
{
    public static Guessings? Load(this string path, Action<JsonSerializerOptions>? optioner = null)
    {
        path = Path.GetFullPath(path);

        if (Path.Exists(path))
        {
            var options = new JsonSerializerOptions()
            {

            };

            optioner?.Invoke(options);

            return JsonSerializer.Deserialize<Guessings>(File.ReadAllText(path), options);
        }
        else return null;
    }

    public static void SaveTo(
        this Guessings guessings,
        string path,
        Action<JsonSerializerOptions>? optioner = null)
    {
        var options = new JsonSerializerOptions()
        {
            IncludeFields = true,
            WriteIndented = true,
        };
        optioner?.Invoke(options);

        File.WriteAllText(path, JsonSerializer.Serialize(guessings));
    }
}
