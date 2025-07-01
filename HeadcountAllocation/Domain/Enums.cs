using System.ComponentModel;

namespace HeadcountAllocation.Domain
{

    public class Enums
    {

        public enum TimeZones
        {
            Morning,
            Noon,
            Evening,
            Flexible

        }

        public enum Skills
        {
            Python,
            SQL,
            API,
            Java,
            UI

        }


        public enum Languages
        {
            English,
            Hebrew,
            Spanish
        }

        public enum Reasons
        {
            [Description("Reserve Duty")]
            ReserveDuty,
            [Description("Maternity / Paternity Leave")]
            MaternityPaternityLeave,
            [Description("Study Leave")]
            StudyLeave,
            [Description("Sick Leave")]
            SickLeave,
            [Description("Mourning Leave")]
            MourningLeave,
            [Description("Long Vacation")]
            LongVacation,
            [Description("Personal Leave")]
            PersonalLeave,
            [Description("Mission Abroad")]
            MissionAbroad,
            [Description("Other")]
            Other
        }

        public static int GetId<TEnum>(TEnum enumValue) where TEnum : Enum
        {
            return Convert.ToInt32(enumValue);
        }
        public static TEnum GetValueById<TEnum>(int id) where TEnum : Enum
        {
            if (Enum.IsDefined(typeof(TEnum), id))
            {
                return (TEnum)Enum.ToObject(typeof(TEnum), id);
            }
            throw new ArgumentException($"Invalid ID {id} for enum {typeof(TEnum).Name}");
        }

    }
}
