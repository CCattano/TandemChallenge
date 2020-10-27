using AutoMapper;

namespace Tandem.Web.Apps.Trivia.Infrastructure
{
    public static class Extensions
    {
        public static void RegisterTranslator<TType1, TType2, TConverter>(this Profile mapperProfile) where TConverter : ITypeConverter<TType1, TType2>, ITypeConverter<TType2, TType1>
        {
            mapperProfile.CreateMap<TType1, TType2>().ConvertUsing<TConverter>();
            mapperProfile.CreateMap<TType2, TType1>().ConvertUsing<TConverter>();
        }
    }
}
