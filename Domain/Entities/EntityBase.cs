using System.ComponentModel.DataAnnotations;

namespace MyCompany.Domain.Entities
{
    public abstract class EntityBase
    {
        protected EntityBase() => DateAdded = DateTime.UtcNow;

        [Required]
        public Guid Id { get; set; }

        [Display(Name = "Название (заголовок)")]
        public virtual string? Title { get; set; }

        [Display(Name = "Краткое описание")]
        public virtual string? Subtitle { get; set; }

        [Display(Name = "Полное описание")]
        public virtual string? Text { get; set; }

        [Display(Name = "Титульная картинка")]
        public virtual string? TitleImagePath { get; set; }

        [Display(Name = "SEO метатег Title")]
        public virtual string? MetaTitle { get; set; }
        
        [Display(Name = "SEO метатег Description")]
        public virtual string? MetaDescription { get; set; }

        [Display(Name = "SEO метатег Keywords")]
        public virtual string? MetaKeywords { get; set; }

        [DataType(DataType.Time)]
        public DateTime? DateAdded { get; set; }
    }
}
