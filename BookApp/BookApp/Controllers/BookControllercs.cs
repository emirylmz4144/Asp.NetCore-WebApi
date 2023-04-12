using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using BookApp.Data;
using BookApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class BookControllercs : ControllerBase
    {
        // Dependency Injection Kullanılarak Logging işlemi için logger oluşturulmuştur 
        public readonly ILogger<BookControllercs> logger;

        public BookControllercs(ILogger <BookControllercs> logger)
        {
            this.logger = logger;
        }


        [HttpGet]

        public IActionResult getAllBooks()
        {
            var result = ApplicationContextcs.Books;
            this.logger.LogWarning("Kitaplar listelendi");
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public IActionResult getBook([FromRoute(Name = "id")] int id)
        {
            var result = ApplicationContextcs
                .Books
                .Where(p=>p.Id==id)
                .SingleOrDefault();

            if (result is null)
            {
                this.logger.LogWarning("Geçersiz istek");
                return NotFound("Bu id'ye ait bir kitap yok");
            }

            else
            {
                this.logger.LogWarning("İlgili kitap bilgileri listelendi");
                return Ok(result);
            }
        }

        [HttpPost]
        public IActionResult addBook([FromBody] Book book)// [FromBody] gibi yapıları eklemek kod okunulurluğunu arttırır
        {
            try
            {
                if (book is null)
                {
                    this.logger.LogWarning("Geçersiz istek");
                    return BadRequest("Lütfen kaynağı özelliklerine göre giriniz");
                }
                else
                {
                    this.logger.LogWarning("Kitap eklendi");
                    ApplicationContextcs.Books.Add(book);
                    return StatusCode(201, book);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:int}")]// Metot isteklerine tanımlamalar yapılabilir
        public IActionResult updateBook([FromRoute(Name = "id")] int id, [FromBody] Book book)//Buradaki FromRoute kullanımı Solid'e %100 uyar
        {
            var result = ApplicationContextcs
                .Books
                .Find(p => p.Id == id);

            if (result is null)
            {
                this.logger.LogWarning("Geçersiz istek");
                return NotFound("Bu id'ye sahip bir kitap yok");
            }
                
            else if (id != book.Id)
            {
                this.logger.LogWarning("Geçersiz istek");
                return BadRequest("Gönderilen id ile kitap eşleşmiyor");
            }
                
            else
            {
                this.logger.LogWarning("Kitap güncellendi");
                ApplicationContextcs.Books.Remove(result);
                book.Id = result.Id;
                ApplicationContextcs.Books.Add(result);
                return StatusCode(200, book);
            }

        }

        [HttpDelete]
        public IActionResult deleteAllBooks()
        {
            this.logger.LogWarning("Tüm kitaplar silindi");
            ApplicationContextcs.Books.Clear();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult deleteBook([FromRoute(Name = "id")] int id)
        {
            var result = ApplicationContextcs
                .Books
                .Where(p => p.Id == id)
                .SingleOrDefault();

            if (result == null)
            {
                this.logger.LogWarning("Geçersiz istek");
                return NotFound("Böyle bir kitap sistemde kayıtlı değildir");
            }
            else
            {
                this.logger.LogWarning("ilgili kitap silindi ");
                ApplicationContextcs.Books.Remove(result);
                return StatusCode(200, "Kitap silindi");
            }
        }

        //NuGet'den özel paket indirilir bu metot için dependencies (NewtonSoftJson, JsonPatch)
        [HttpPatch("{id:int}")]
        public IActionResult partiallyUpdateBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            var result = ApplicationContextcs.Books.Find(p => p.Id == id);
            if (result is null)
            {
                this.logger.LogWarning("Geçersiz istek");
                return NotFound("Böyle bir kitap sistemde kayıtlı değildir.");
            }
            else
            {
                this.logger.LogWarning("İlgili kitap kismi güncellemesi yapıldı"); 
                bookPatch.ApplyTo(result);
                return NoContent();
            }
        }
        
    }
}
