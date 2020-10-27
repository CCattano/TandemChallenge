namespace Tandem.Web.Apps.Trivia.Infrastructure
{
    public struct SystemConstants
    {
        public const string DefaultUser = "Web.Apps.Trivia";
        public struct AppSettings
        {
            public struct ConnStrings
            {
                public const string SectionName = "ConnectionStrings";
                public const string DataFilePath = "DataFilePath";
            }
        }
    }
}
