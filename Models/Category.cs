using System.ComponentModel.DataAnnotations; // validações



namespace Shop.Models
{
  // [Table("Category")] // caso queira mudar o nome da tabela
  public class Category
  {
    [Key]
    // [Column("Cat_ID")] // caso queira mudar o nome da coluna
    // [DataType("nvarchar")] // caso queira mudar o tipo de dado
    public int Id { get; set; }

    [Required(ErrorMessage = "Este campo é obrigatório")]
    [MinLength(3, ErrorMessage = "Este campo deve ter entre 3 e 60 caracteres")]
    [MaxLength(60, ErrorMessage = "Este campo deve ter entre 3 e 60 caracteres")]
    public string Title { get; set; }
  }
}
