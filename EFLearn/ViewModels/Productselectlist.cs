using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EFLearn.ViewModels
{
    public class Productselectlist
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [Compare("Name",ErrorMessage ="نام مشابه نیست")]
        public string? ConfirmName { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public List<SelectListItem>? Category {  get; set; }
    }
}
