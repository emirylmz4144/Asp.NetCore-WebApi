using BookApp.Model; 

namespace BookApp.Data
{
    public static class ApplicationContextcs
    {
        public static List<Book> Books { get; set; }
        static ApplicationContextcs()
        {
            Books = new List<Book>()
            {
                new Book() { Id = 1, Price = 20, Title = "İçimizdeki Şeytan" },
                new Book() { Id = 2, Price = 25, Title = "1984" },
                new Book() { Id = 3, Price = 15, Title = "Hayvan Çiftliği" },
                new Book() { Id = 4, Price = 30, Title = "Kürk Mantolu Madonna" }
            };
        }
    }
}
