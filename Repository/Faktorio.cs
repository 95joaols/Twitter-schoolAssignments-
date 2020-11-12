namespace Repository
{
    public static class Faktorio
    {
        private static string connectionString = "";
        public static IFullControl GetRepository()
        {
            return new SQLAccess(connectionString);
        }
    }
}
