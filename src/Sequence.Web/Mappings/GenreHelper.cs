using Sequence.Web.Domain.Enums;

namespace Sequence.Web.Mappings;

public static class GenreHelper
{
    public static string ToFarsi(Genre genre) => genre switch
    {
        Genre.Action => "اکشن",
        Genre.Adventure => "ماجراجویی",
        Genre.Animation => "انیمیشن",
        Genre.Comedy => "کمدی",
        Genre.Crime => "جنایی",
        Genre.Documentary => "مستند",
        Genre.Drama => "درام",
        Genre.Fantasy => "فانتزی",
        Genre.Horror => "ترسناک",
        Genre.Musical => "موزیکال",
        Genre.Mystery => "معمایی",
        Genre.Romance => "عاشقانه",
        Genre.SciFi => "علمی‌تخیلی",
        Genre.Thriller => "هیجان‌انگیز",
        Genre.Western => "وسترن",
        Genre.Historical => "تاریخی",
        Genre.Family => "خانوادگی",
        _ => "نامشخص"
    };

    public static Dictionary<int, string> GetAllFarsi() =>
        Enum.GetValues<Genre>()
            .ToDictionary(g => (int)g, ToFarsi);
}