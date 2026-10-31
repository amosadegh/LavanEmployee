using System.Globalization;
using Employee.Entities.Models.Enums;

namespace Employee.Web.Helpers
{
    /// توابع مشترک تاریخ شمسی و عنوان وضعیت کارکرد (User و Representative)
    public static class TimeSheetHelper
    {
        private static readonly PersianCalendar Pc = new();

        public static readonly string[] MonthNames =
        {
            "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
            "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
        };

        public static (int y, int m) NormalizeYearMonth(int? year, int? month, DateTime now)
        {
            var y = year ?? Pc.GetYear(now);
            var m = month ?? Pc.GetMonth(now);
            if (y < 1300 || y > 1600) y = Pc.GetYear(now);
            if (m < 1 || m > 12) m = Pc.GetMonth(now);
            return (y, m);
        }

        public static (DateOnly Start, DateOnly End) GetMonthRange(int y, int m)
        {
            var start = DateOnly.FromDateTime(Pc.ToDateTime(y, m, 1, 0, 0, 0, 0));
            var end = start.AddDays(Pc.GetDaysInMonth(y, m) - 1);
            return (start, end);
        }

        public static string ToPersian(DateOnly d)
        {
            var dt = d.ToDateTime(TimeOnly.MinValue);
            return $"{Pc.GetYear(dt):0000}/{Pc.GetMonth(dt):00}/{Pc.GetDayOfMonth(dt):00}";
        }

        public static string StatusTitle(WorkStatusType s) => s switch
        {
            WorkStatusType.OnshoreWork => "کارکرد خشکی",
            WorkStatusType.OffshoreWork => "کارکرد سکو",
            WorkStatusType.Standby => "آماده‌به کار",
            WorkStatusType.Rest => "استراحت",
            WorkStatusType.Leave => "مرخصی",
            WorkStatusType.Sick => "استعلاجی",
            WorkStatusType.Absent => "غیبت",
            WorkStatusType.Mission => "ماموریت",
            _ => s.ToString()
        };

        /// تبدیل ارقام فارسی/عربی به لاتین
        public static string NormalizeDigits(string input)
        {
            var chars = input.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                var c = chars[i];
                if (c >= '۰' && c <= '۹') chars[i] = (char)('0' + (c - '۰'));
                else if (c >= '٠' && c <= '٩') chars[i] = (char)('0' + (c - '٠'));
            }
            return new string(chars);
        }

        public static bool TryParsePersian(string? input, out DateOnly date)
        {
            date = default;
            if (string.IsNullOrWhiteSpace(input)) return false;

            var parts = NormalizeDigits(input.Trim()).Split('/');
            if (parts.Length != 3 ||
                !int.TryParse(parts[0], out var y) ||
                !int.TryParse(parts[1], out var m) ||
                !int.TryParse(parts[2], out var d))
                return false;
            if (y < 1300 || y > 1600 || m < 1 || m > 12 || d < 1 || d > Pc.GetDaysInMonth(y, m))
                return false;

            date = DateOnly.FromDateTime(Pc.ToDateTime(y, m, d, 0, 0, 0, 0));
            return true;
        }
    }
}
