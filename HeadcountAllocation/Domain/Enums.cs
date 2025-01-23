namespace HeadcountAllocation.Domain{

    public class Enums{

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
